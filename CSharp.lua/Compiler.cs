/*
Copyright 2017 YANG Huan (sy.yanghuan@gmail.com).

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

  http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Cake.Incubator.Project;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpLua {
  public sealed class Compiler {
    private const string kDllSuffix = ".dll";
    private const char kLuaModuleSuffix = '!';

    private readonly bool isProject_;
    private readonly string input_;
    private readonly string output_;
    private readonly string[] libs_;
    private readonly string[] metas_;
    private readonly string[] cscArguments_;
    private readonly bool isClassic_;
    private readonly string[] attributes_;
    private readonly string[] enums_;

    public bool IsExportMetadata { get; set; }
    public bool IsModule { get; set; }
    public bool IsInlineSimpleProperty { get; set; }
    public bool IsPreventDebugObject { get; set; }
    public bool IsCommentsDisabled { get; set; }

    public bool IsNotConstantForEnum { get; set; }

    public Compiler(string input, string output, string lib, string meta, string csc, bool isClassic, string atts, string enums) {
      isProject_ = new FileInfo(input).Extension.ToLower() == ".csproj";
      input_ = input;
      output_ = output;
      libs_ = Utility.Split(lib);
      metas_ = Utility.Split(meta);
      cscArguments_ = string.IsNullOrEmpty(csc) ? Array.Empty<string>() : csc.Trim().Split(' ', '\t');
      isClassic_ = isClassic;
      if (atts != null) {
        attributes_ = Utility.Split(atts, false);
      }
      if (enums != null) {
        enums_ = Utility.Split(enums, false);
      }
    }

    private static IEnumerable<string> GetMetas(IEnumerable<string> additionalMetas) {
      return additionalMetas.ToList();
    }

    private static bool IsCorrectSystemDll(string path) {
      try {
        Assembly.LoadFile(path);
        return true;
      } catch (Exception) {
        return false;
      }
    }

    private static List<string> GetSystemLibs() {
      string privateCorePath = typeof(object).Assembly.Location;
      List<string> libs = new List<string>() { privateCorePath };

      string systemDir = Path.GetDirectoryName(privateCorePath);
      foreach (string path in Directory.EnumerateFiles(systemDir, "*.dll")) {
        if (IsCorrectSystemDll(path)) {
          libs.Add(path);
        }
      }

      return libs;
    }

    private static List<string> GetLibs(IEnumerable<string> additionalLibs, out List<string> luaModuleLibs) {
      luaModuleLibs = new List<string>();
      var libs = GetSystemLibs();
      var dlls = new HashSet<string>(libs.Select(lib => new FileInfo(lib).Name));
      if (additionalLibs != null) {
        foreach (string additionalLib in additionalLibs) {
          string lib = additionalLib;
          bool isLuaModule = false;
          if (lib.Last() == kLuaModuleSuffix) {
            lib = lib.TrimEnd(kLuaModuleSuffix);
            isLuaModule = true;
          } else {
            var dllName = new FileInfo(lib).Name;
            if (dlls.Contains(dllName)) {
              // Avoid duplicate dlls.
              continue;
            } else {
              dlls.Add(dllName);
            }
          }

          string path = lib.EndsWith(kDllSuffix) ? lib : lib + kDllSuffix;
          if (File.Exists(path)) {
            if (isLuaModule) {
              luaModuleLibs.Add(Path.GetFileNameWithoutExtension(path));
            }

            libs.Add(path);
          } else {
            throw new CmdArgumentException($"-l {path} is not found");
          }
        }
      }
      return libs;
    }

    public void Compile() {
      GetGenerator().Generate(output_);
    }

    public void CompileSingleFile(string fileName, IEnumerable<string> luaSystemLibs) {
      GetGenerator().GenerateSingleFile(fileName, output_, luaSystemLibs);
    }

    public void CompileSingleFile(Stream target, IEnumerable<string> luaSystemLibs) {
      GetGenerator().GenerateSingleFile(target, luaSystemLibs);
    }

    private LuaSyntaxGenerator GetGenerator() {
      const string configurationDebug = "Debug";
      const string configurationRelease = "Release";
      var mainProject = isProject_ ? ProjectHelper.ParseProject(input_, IsCompileDebug() ? configurationDebug : configurationRelease) : null;
      var projects = mainProject?.EnumerateProjects().ToArray();
      var packages = isProject_ ? PackageHelper.EnumeratePackages(mainProject.TargetFrameworkVersions.First(), projects.Select(project => project.project)) : null;
      var files = isProject_ ? GetSourceFiles(projects) : GetSourceFiles();
      var packageBaseFolders = new List<string>();
      if (packages != null) {
        foreach (var package in packages) {
          var packageFiles = PackageHelper.EnumerateSourceFiles(package, out var baseFolder).ToArray();
          if (packageFiles.Length > 0) {
            files = files.Concat(packageFiles);
            packageBaseFolders.Add(baseFolder);
          }
        }
      }
      var codes = files.Select(i => (File.ReadAllText(i), i));
      var libs = GetLibs(isProject_ ? libs_.Concat(packages.SelectMany(package => PackageHelper.EnumerateLibs(package))) : libs_, out var luaModuleLibs);
      var metas = GetMetas(isProject_ ? metas_.Concat(packages.SelectMany(package => PackageHelper.EnumerateMetas(package))) : metas_);
      var setting = new LuaSyntaxGenerator.SettingInfo() {
        IsClassic = isClassic_,
        IsExportMetadata = IsExportMetadata,
        Attributes = attributes_,
        Enums = enums_,
        LuaModuleLibs = new HashSet<string>(luaModuleLibs),
        IsModule = IsModule,
        IsInlineSimpleProperty = IsInlineSimpleProperty,
        IsPreventDebugObject = IsPreventDebugObject,
        IsCommentsDisabled = IsCommentsDisabled,
        IsNotConstantForEnum = IsNotConstantForEnum,
      };
      if (isProject_) {
        foreach (var folder in projects.Select(p => p.folder)) {
          setting.AddBaseFolder(folder, false);
        }
        foreach (var folder in packageBaseFolders) {
          setting.AddBaseFolder(folder, false);
        }
      } else if (Directory.Exists(input_)) {
        setting.AddBaseFolder(input_, false);
      } else if (files.Count() == 1) {
        setting.AddBaseFolder(new FileInfo(files.Single()).DirectoryName, true);
      } else {
        // throw new NotImplementedException("Unable to determine basefolder(s) when the input is a list of source files.");
      }
      return new LuaSyntaxGenerator(codes, libs, cscArguments_, metas, setting);
    }

    private IEnumerable<string> GetSourceFiles() {
      if (Directory.Exists(input_)) {
        return Directory.EnumerateFiles(input_, "*.cs", SearchOption.AllDirectories);
      }
      return Utility.Split(input_, true);
    }

    private IEnumerable<string> GetSourceFiles(IEnumerable<(string folder, CustomProjectParserResult project)> projects) {
      return projects.SelectMany(project => project.project.EnumerateSourceFiles(project.folder));
    }

    private bool IsCompileDebug() {
      foreach (var arg in cscArguments_) {
        if (arg.StartsWith("-debug")) {
          return true;
        }
      }
      return false;
    }

    public static string CompileSingleCode(string code) {
      var codes = new (string, string)[] { (code, "") };
      var generator = new LuaSyntaxGenerator(codes, GetSystemLibs(), null, GetMetas(null), new LuaSyntaxGenerator.SettingInfo());
      return generator.GenerateSingle();
    }
  }
}

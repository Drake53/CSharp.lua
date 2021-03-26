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
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CSharpLua.LuaAst {
  public sealed class GenericUsingDeclare : IComparable<GenericUsingDeclare> {
    public LuaInvocationExpressionSyntax InvocationExpression;
    public string NewName;
    public List<string> ArgumentTypeNames;
    public bool IsFromCode;
    public bool IsFromGlobal;

    public int CompareTo(GenericUsingDeclare other) {
      if (other.ArgumentTypeNames.Contains(NewName)) {
        return -1;
      }

      if (ArgumentTypeNames.Contains(other.NewName)) {
        return 1;
      }

      if (NewName.Length != other.NewName.Length) {
        return NewName.Length.CompareTo(other.NewName.Length);
      }

      return NewName.CompareTo(other.NewName);
    }
  }

  public sealed class UsingDeclare : IComparable<UsingDeclare> {
    public string Prefix;
    public string NewPrefix;
    public bool IsFromCode;

    public int CompareTo(UsingDeclare other) {
      return Prefix.CompareTo(other.Prefix);
    }
  }

  public sealed class LuaCompilationUnitSyntax : LuaSyntaxNode {
    public string FilePath { get; }
    public readonly LuaSyntaxList<LuaStatementSyntax> Statements = new LuaSyntaxList<LuaStatementSyntax>();
    private readonly LuaStatementListSyntax importAreaStatements = new LuaStatementListSyntax();
    private bool isImportLinq_;
    private int typeDeclarationCount_;
    internal readonly List<UsingDeclare> UsingDeclares = new List<UsingDeclare>();
    internal readonly List<GenericUsingDeclare> GenericUsingDeclares = new List<GenericUsingDeclare>();

    public LuaCompilationUnitSyntax(string filePath = "", bool hasGeneratedMark = true) {
      FilePath = filePath;
      if (hasGeneratedMark) {
        AddStatement(new LuaShortCommentStatement(GeneratedMarkString));
      }
      var system = LuaIdentifierNameSyntax.System;
      AddImport(system, system);
    }

    public static string GeneratedMarkString => $" Generated by {Assembly.GetExecutingAssembly().GetName().Name} Compiler";

    public void AddStatement(LuaStatementSyntax statement) {
      Statements.Add(statement);
    }

    public bool IsEmpty {
      get {
        return typeDeclarationCount_ == 0;
      }
    }

    public void ImportLinq() {
      if (!isImportLinq_) {
        AddImport(LuaIdentifierNameSyntax.Linq, LuaIdentifierNameSyntax.SystemLinqEnumerable);
        isImportLinq_ = true;
      }
    }

    private void AddImport(LuaIdentifierNameSyntax name, LuaExpressionSyntax value) {
      importAreaStatements.Statements.Add(new LuaLocalVariableDeclaratorSyntax(name, value));
    }

    internal void AddTypeDeclarationCount() {
      ++typeDeclarationCount_;
    }

    private void CheckUsingDeclares() {
      var imports = UsingDeclares.Where(i => !i.IsFromCode).ToList();
      if (imports.Count > 0) {
        imports.Sort();
        foreach (var import in imports) {
          AddImport(import.NewPrefix, import.Prefix);
        }
      }

      var genericImports = GenericUsingDeclares.Where(i => !i.IsFromCode).ToList();
      if (genericImports.Count > 0) {
        genericImports.Sort();
        foreach (var import in genericImports) {
          AddImport(import.NewName, import.InvocationExpression);
        }
      }

      var usingDeclares = UsingDeclares.Where(i => i.IsFromCode).ToList();
      var genericDeclares = GenericUsingDeclares.Where(i => i.IsFromCode).ToList();
      if (usingDeclares.Count > 0 || genericDeclares.Count > 0) {
        usingDeclares.Sort();
        genericDeclares.Sort();

        foreach (var usingDeclare in usingDeclares) {
          AddImport(usingDeclare.NewPrefix, null);
        }

        foreach (var usingDeclare in genericDeclares) {
          AddImport(usingDeclare.NewName, null);
        }

        var global = LuaIdentifierNameSyntax.Global;
        var functionExpression = new LuaFunctionExpressionSyntax();
        functionExpression.AddParameter(global);
        foreach (var usingDeclare in usingDeclares) {
          LuaIdentifierNameSyntax newPrefixIdentifier = usingDeclare.NewPrefix;
          if (usingDeclare.Prefix != usingDeclare.NewPrefix) {
            functionExpression.Body.AddStatement(newPrefixIdentifier.Assignment(usingDeclare.Prefix));
          } else {
            functionExpression.Body.AddStatement(newPrefixIdentifier.Assignment(global.MemberAccess(usingDeclare.Prefix)));
          }
        }

        foreach (var usingDeclare in genericDeclares) {
          functionExpression.Body.AddStatement(((LuaIdentifierNameSyntax)usingDeclare.NewName).Assignment(usingDeclare.InvocationExpression));
        }

        var invocationExpression = new LuaInvocationExpressionSyntax(LuaIdentifierNameSyntax.Import, functionExpression);
        importAreaStatements.Statements.Add(invocationExpression);
      }

      int index = Statements.FindIndex(i => i is LuaNamespaceDeclarationSyntax);
      if (index != -1) {
        Statements.Insert(index, importAreaStatements);
      }
    }

    internal bool IsUsingDeclareConflict(LuaInvocationExpressionSyntax generic) {
      if (generic.Expression is LuaIdentifierNameSyntax identifier) {
        int pos = identifier.ValueText.IndexOf('.');
        if (pos != -1) {
          string prefix = identifier.ValueText.Substring(0, pos);
          return UsingDeclares.Exists(i => i.NewPrefix == prefix);
        }
      }
      return false;
    }

    internal override void Render(LuaRenderer renderer) {
      CheckUsingDeclares();
      renderer.Render(this);
    }
  }
}

using Bridge.Test.NUnit;
using System;

namespace Bridge.ClientTest
{
    [Category(Constants.MODULE_ARRAY)]
    [TestFixture(TestNameFormat = "MemoryExtensions - {0}")]
    public class MemoryExtensionsTests
    {
        [Test]
        public void ContainsWorks()
        {
            var arr = new string[] { "x", "y" };
            Assert.True(MemoryExtensions.Contains(arr, "x"));
            Assert.False(MemoryExtensions.Contains(arr, "z"));
        }
    }
}

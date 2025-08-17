using Bridge.Test.NUnit;
using System.Collections.Generic;

namespace Bridge.ClientTest.BasicCSharp
{
    [Category(Constants.MODULE_BASIC_CSHARP)]
    [TestFixture(TestNameFormat = "For loop - {0}")]
    public class TestForLoop
    {
        [Test]
        public static void TestLiteralNoEqualStep1()
        {
            var list = new List<int>();
            for (var i = 0; i < 5; i++)
            {
                list.Add(i);
            }
            Assert.AreDeepEqual(new int[] { 0, 1, 2, 3, 4 }, list);
        }

        [Test]
        public static void TestLiteralNoEqualStepNeg1()
        {
            var list = new List<int>();
            for (var i = 5; i > 0; i--)
            {
                list.Add(i);
            }
            Assert.AreDeepEqual(new int[] { 5, 4, 3, 2, 1 }, list);
        }

        [Test]
        public static void TestLiteralEqualStep1()
        {
            var list = new List<int>();
            for (var i = 0; i <= 5; i++)
            {
                list.Add(i);
            }
            Assert.AreDeepEqual(new int[] { 0, 1, 2, 3, 4, 5 }, list);
        }

        [Test]
        public static void TestLiteralEqualStepNeg1()
        {
            var list = new List<int>();
            for (var i = 5; i >= 0; i--)
            {
                list.Add(i);
            }
            Assert.AreDeepEqual(new int[] { 5, 4, 3, 2, 1, 0 }, list);
        }

        [Test]
        public static void TestLiteralNoEqualStep2()
        {
            var list = new List<int>();
            for (var i = 0; i < 5; i += 2)
            {
                list.Add(i);
            }
            Assert.AreDeepEqual(new int[] { 0, 2, 4 }, list);
        }

        [Test]
        public static void TestLiteralNoEqualStepNeg2()
        {
            var list = new List<int>();
            for (var i = 5; i > 0; i -= 2)
            {
                list.Add(i);
            }
            Assert.AreDeepEqual(new int[] { 5, 3, 1 }, list);
        }

        [Test]
        public static void TestLiteralEqualStep2()
        {
            var list = new List<int>();
            for (var i = 0; i <= 5; i += 2)
            {
                list.Add(i);
            }
            Assert.AreDeepEqual(new int[] { 0, 2, 4 }, list);
        }

        [Test]
        public static void TestLiteralEqualStepNeg2()
        {
            var list = new List<int>();
            for (var i = 5; i >= 0; i -= 2)
            {
                list.Add(i);
            }
            Assert.AreDeepEqual(new int[] { 5, 3, 1 }, list);
        }

        [Test]
        public static void TestExprNoEqualStep1()
        {
            var list = new List<int>();
            for (var i = 0; i < Expr(5); i += Expr(1))
            {
                list.Add(i);
            }
            Assert.AreDeepEqual(new int[] { 0, 1, 2, 3, 4 }, list);
        }

        [Test]
        public static void TestExprNoEqualStepNeg1()
        {
            var list = new List<int>();
            for (var i = 5; i > Expr(0); i -= Expr(1))
            {
                list.Add(i);
            }
            Assert.AreDeepEqual(new int[] { 5, 4, 3, 2, 1 }, list);
        }

        [Test]
        public static void TestExprEqualStep1()
        {
            var list = new List<int>();
            for (var i = 0; i <= Expr(5); i += Expr(1))
            {
                list.Add(i);
            }
            Assert.AreDeepEqual(new int[] { 0, 1, 2, 3, 4, 5 }, list);
        }

        [Test]
        public static void TestExprEqualStepNeg1()
        {
            var list = new List<int>();
            for (var i = 5; i >= Expr(0); i -= Expr(1))
            {
                list.Add(i);
            }
            Assert.AreDeepEqual(new int[] { 5, 4, 3, 2, 1, 0 }, list);
        }

        [Test]
        public static void TestExprNoEqualStep2()
        {
            var list = new List<int>();
            for (var i = 0; i < Expr(5); i += Expr(2))
            {
                list.Add(i);
            }
            Assert.AreDeepEqual(new int[] { 0, 2, 4 }, list);
        }

        [Test]
        public static void TestExprNoEqualStepNeg2()
        {
            var list = new List<int>();
            for (var i = 5; i > Expr(0); i -= Expr(2))
            {
                list.Add(i);
            }
            Assert.AreDeepEqual(new int[] { 5, 3, 1 }, list);
        }

        [Test]
        public static void TestExprEqualStep2()
        {
            var list = new List<int>();
            for (var i = 0; i <= Expr(5); i += Expr(2))
            {
                list.Add(i);
            }
            Assert.AreDeepEqual(new int[] { 0, 2, 4 }, list);
        }

        [Test]
        public static void TestExprEqualStepNeg2()
        {
            var list = new List<int>();
            for (var i = 5; i >= Expr(0); i -= Expr(2))
            {
                list.Add(i);
            }
            Assert.AreDeepEqual(new int[] { 5, 3, 1 }, list);
        }

        [Test]
        public static void TestWithNonInt()
        {
            var list = new List<double>();
            for (double i = 0; i < 1; i += 0.2)
            {
                list.Add(i);
            }
            Assert.AreDeepEqual(new double[] { 0, 0.2, 0.4, 0.6, 0.8 }, list);
        }

        [Test]
        public static void TestWithModifying()
        {
            var list = new List<int>();
            for (var i = 0; i < 5; i += 2)
            {
                list.Add(i);
                i--;
            }
            Assert.AreDeepEqual(new int[] { 0, 1, 2, 3, 4 }, list);
        }

        [Test]
        public static void TestWithModifyingNoBlock()
        {
            var list = new List<int>();
            for (var i = 0; i < 5; i += 2)
                list.Add(i--);
            Assert.AreDeepEqual(new int[] { 0, 1, 2, 3, 4 }, list);
        }

        [Test]
        public static void TestWithOut()
        {
            var list = new List<int>();
            for (var i = 0; i < 5; i += 2)
            {
                list.Add(i);
                OutDec(i, out i);
            }
            Assert.AreDeepEqual(new int[] { 0, 1, 2, 3, 4 }, list);
        }

        [Test]
        public static void TestWithRef()
        {
            var list = new List<int>();
            for (var i = 0; i < 5; i += 2)
            {
                list.Add(i);
                RefDec(ref i);
            }
            Assert.AreDeepEqual(new int[] { 0, 1, 2, 3, 4 }, list);
        }

        public static int Expr(int number)
        {
            return number;
        }

        public static void OutDec(int numberIn, out int numberOut)
        {
            numberOut = numberIn - 1;
        }

        public static void RefDec(ref int number)
        {
            number--;
        }
    }
}

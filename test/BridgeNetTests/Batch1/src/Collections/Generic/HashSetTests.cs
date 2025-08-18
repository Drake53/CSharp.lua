using Bridge.Test.NUnit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bridge.ClientTest.Collections.Generic
{
    [Category(Constants.MODULE_LIST)]
    [TestFixture(TestNameFormat = "HashSet - {0}")]
    public class HashSetTests
    {
        private class C
        {
            public readonly int i;

            public C(int i)
            {
                this.i = i;
            }

            public override bool Equals(object o)
            {
                return o is C && i == ((C)o).i;
            }

            public override int GetHashCode()
            {
                return i;
            }
        }

        [Test]
        public void TypePropertiesAreCorrect()
        {
            Assert.True(typeof(HashSet<int>).IsClass, "IsClass should be true");
            object hashSet = new HashSet<int>();
            Assert.True(hashSet is HashSet<int>, "is int[] should be true");
            Assert.True(hashSet is ISet<int>, "is ISet<int> should be true");
            Assert.True(hashSet is ICollection<int>, "is ICollection<int> should be true");
            Assert.True(hashSet is IEnumerable<int>, "is IEnumerable<int> should be true");
            Assert.True(hashSet is IReadOnlyCollection<int>, "is IReadOnlyCollection<int> should be true");
            Assert.True(hashSet is IReadOnlySet<int>, "is IReadOnlySet<int> should be true");
        }

        [Test]
        public void DefaultConstructorWorks()
        {
            var hashSet = new HashSet<int>();
            Assert.AreEqual(0, hashSet.Count);
        }

        [Test]
        public void ConstructorWithCapacityWorks()
        {
            var hashSet = new HashSet<int>(12);
            Assert.AreEqual(0, hashSet.Count);
        }

        [Test]
        public void ParamArrayConstructorWorks()
        {
            var array = new[] { 1, 4, 7, 8 };
            var hashSet = new HashSet<int> { 1, 4, 7, 8 };
            Assert.AreDeepEqual(array, hashSet.OrderBy(x => x));
        }

        [Test]
        public void ConstructingFromArrayWorks()
        {
            var array = new[] { 1, 4, 7, 8 };
            var hashSet = new HashSet<int>(array);
            Assert.False(hashSet == (object)array);
            Assert.AreDeepEqual(array, hashSet.OrderBy(x => x));
        }

        [Test]
        public void ConstructingFromListWorks()
        {
            var list = new List<int>() { 1, 4, 7, 8 };
            var hashSet = new HashSet<int>(list);
            Assert.False(hashSet == (object)list);
            Assert.AreDeepEqual(new[] { 1, 4, 7, 8 }, hashSet.OrderBy(x => x));
        }

        [Test]
        public void ConstructingFromIEnumerableWorks()
        {
            var array = new[] { 1, 4, 7, 8 };
            var hashSet = new HashSet<int>(array.Select(x => x));
            Assert.False(hashSet == (object)array);
            Assert.AreDeepEqual(array, hashSet.OrderBy(x => x));
        }

        [Test]
        public void CountWorks()
        {
            Assert.AreEqual(0, new HashSet<string>().Count);
            Assert.AreEqual(1, new HashSet<string> { "x" }.Count);
            Assert.AreEqual(2, new HashSet<string> { "x", "y" }.Count);
        }

        [Test]
        public void ForeachWorks()
        {
            var result = 0;
            foreach (var s in new HashSet<int> { 1, 2, 3 })
            {
                result += s;
            }
            Assert.AreEqual(6, result);
        }

        [Test]
        public void GetEnumeratorWorks()
        {
            var e = new HashSet<string> { "x", "y" }.GetEnumerator();
            Assert.True(e.MoveNext());
            var first = e.Current;
            Assert.True(e.MoveNext());
            var second = e.Current;
            Assert.False(e.MoveNext());
            Assert.True(first == "x" || first == "y");
            Assert.True(second == "x" || second == "y");
            Assert.AreNotEqual(first, second);
        }

        [Test]
        public void AddWorks()
        {
            var hashSet = new HashSet<string>();
            Assert.True(hashSet.Add("x"));
            Assert.True(hashSet.Add("y"));
            Assert.True(hashSet.Add("z"));
            Assert.AreDeepEqual(new[] { "x", "y", "a" }, hashSet.OrderBy(x => x));
        }

        [Test]
        public void ClearWorks()
        {
            var hashSet = new HashSet<string> { "x", "y" };
            hashSet.Clear();
            Assert.AreEqual(hashSet.Count, 0);
        }

        [Test]
        public void ContainsWorks()
        {
            var hashSet = new HashSet<string> { "x", "y" };
            Assert.True(hashSet.Contains("x"));
            Assert.False(hashSet.Contains("z"));
        }

        [Test]
        public void ContainsUsesEqualsMethod()
        {
            var hashSet = new HashSet<C> { new(1), new(2), new(3) };
            Assert.True(hashSet.Contains(new C(2)));
            Assert.False(hashSet.Contains(new C(4)));
        }

        [Test]
        public void CopyToMethodSameBound()
        {
            var hashSet = new HashSet<string> { "0", "1", "2" };

            var array = new string[3];
            hashSet.CopyTo(array, 0);

            Assert.AreDeepEqual(array.OrderBy(x => x), hashSet.OrderBy(x => x));
        }

        [Test]
        public void CopyToMethodOffsetBound()
        {
            var hashSet = new HashSet<string> { "0", "1", "2" };

            var array = new string[5];
            hashSet.CopyTo(array, 1);

            Assert.AreEqual(null, array[0], "Element 0");
            Assert.True(hashSet.Contains(array[1]), "Element 1");
            Assert.True(hashSet.Contains(array[2]), "Element 2");
            Assert.True(hashSet.Contains(array[3]), "Element 3");
            Assert.AreEqual(null, array[4], "Element 4");
        }

        [Test]
        public void CopyToMethodIllegalBound()
        {
            var hashSet = new HashSet<string> { "0", "1", "2" };

            Assert.Throws<ArgumentNullException>(() => { hashSet.CopyTo(null, 0); }, "null");

            var array1 = new string[2];
            Assert.Throws<ArgumentException>(() => { hashSet.CopyTo(array1, 0); }, "Short array");

            var array2 = new string[3];
            Assert.Throws<ArgumentException>(() => { hashSet.CopyTo(array2, 1); }, "Start index 1");
            Assert.Throws<ArgumentOutOfRangeException>(() => { hashSet.CopyTo(array2, -1); }, "Negative start index");
            Assert.Throws<ArgumentException>(() => { hashSet.CopyTo(array2, 3); }, "Start index 3");
        }

        [Test]
        public void ForEachWithListItemCallbackWorks()
        {
            var result = 0;
            new HashSet<int> { 1, 2, 3 }.ForEach(x => result += x);
            Assert.AreEqual(6, result);
        }

        [Test]
        public void RemoveWorks()
        {
            var hashSet = new HashSet<string> { "a", "b", "c" };
            Assert.True(hashSet.Remove("a"));
            Assert.AreDeepEqual(new[] { "b", "c" }, hashSet.OrderBy(x => x));
            Assert.False(hashSet.Remove("a"));
        }

        [Test]
        public void RemoveReturnsFalseIfTheElementWasNotFound()
        {
            var hashSet = new HashSet<string> { "a", "b", "c", };
            Assert.False(hashSet.Remove("d"));
            Assert.AreDeepEqual(new[] { "a", "b", "c", }, hashSet.OrderBy(x => x));
        }

        [Test]
        public void RemoveCanRemoveNullItem()
        {
            var hashSet = new HashSet<string> { "a", null, "c" };
            Assert.AreDeepEqual(new[] { null, "a", "c" }, hashSet.OrderBy(x => x));
            Assert.True(hashSet.Remove(null));
            Assert.AreDeepEqual(new[] { "a", "c" }, hashSet.OrderBy(x => x));
            Assert.False(hashSet.Remove(null));
        }

        [Test]
        public void RemoveUsesEqualsMethod()
        {
            var hashSet = new HashSet<C> { new(1), new(2), new(3) };
            Assert.True(hashSet.Remove(new C(2)));
            Assert.AreEqual(2, hashSet.Count);
            Assert.False(hashSet.Remove(new C(2)));
        }

        [Test]
        public void RemoveWhereWorks()
        {
            var hashSet = new HashSet<string> { "a", "b", "c", "d" };
            _ = hashSet.RemoveWhere(x => x == "b" || x == "c");
            Assert.AreDeepEqual(new[] { "a", "d" }, hashSet.OrderBy(x => x));
        }

        [Test]
        public void ForeachWhenCastToIEnumerableWorks()
        {
            IEnumerable<int> enumerable = new HashSet<int> { 1, 2, 3 };
            var result = 0;
            foreach (var s in enumerable)
            {
                result += s;
            }
            Assert.AreEqual(6, result);
        }

        [Test]
        public void IEnumerableGetEnumeratorWorks()
        {
            var enumerable = (IEnumerable<string>)new HashSet<string> { "x", "y" };
            var e = enumerable.GetEnumerator();
            Assert.True(e.MoveNext());
            var first = e.Current;
            Assert.True(e.MoveNext());
            var second = e.Current;
            Assert.False(e.MoveNext());
            Assert.True(first == "x" || first == "y");
            Assert.True(second == "x" || second == "y");
            Assert.AreNotEqual(first, second);
        }

        [Test]
        public void ICollectionCountWorks()
        {
            ICollection<string> collection = new HashSet<string> { "x", "y", "z" };
            Assert.AreEqual(3, collection.Count);
        }

        [Test]
        public void ICollectionAddWorks()
        {
            ICollection<string> collection = new HashSet<string>
            {
                "x",
                "y",
                "z",
                "a"
            };
            Assert.AreDeepEqual(new[] { "x", "y", "z", "a" }, collection.OrderBy(x => x));
        }

        [Test]
        public void ICollectionClearWorks()
        {
            ICollection<string> collection = new HashSet<string> { "x", "y", "z" };
            collection.Clear();
            Assert.AreDeepEqual(Array.Empty<string>(), collection);
        }

        [Test]
        public void ICollectionContainsUsesEqualsMethod()
        {
            ICollection<C> collection = new HashSet<C> { new(1), new(2), new(3) };
            Assert.True(collection.Contains(new C(2)));
            Assert.False(collection.Contains(new C(4)));
        }

        [Test]
        public void ICollectionRemoveWorks()
        {
            ICollection<string> collection = new HashSet<string> { "x", "y", "z" };
            Assert.True(collection.Remove("y"));
            Assert.False(collection.Remove("a"));

            var hashSet = collection as HashSet<string>;
            Assert.AreDeepEqual(new[] { "x", "z" }, hashSet.OrderBy(x => x));
        }

        [Test]
        public void ISetRemoveCanRemoveNullItem()
        {
            ISet<string> set = new HashSet<string> { "a", null, "c" };
            Assert.AreDeepEqual(new[] { null, "a", "c" }, set.OrderBy(x => x));
            Assert.True(set.Remove(null));
            Assert.AreDeepEqual(new[] { "a", "c" }, set.OrderBy(x => x));
        }

        [Test]
        public void ISetRemoveUsesEqualsMethod()
        {
            ISet<C> set = new HashSet<C> { new(1), new(2), new(3) };
            Assert.AreEqual(3, set.Count);
            Assert.True(set.Remove(new C(2)));
            Assert.AreEqual(2, set.Count);
            Assert.False(set.Remove(new C(2)));
        }

        [Test]
        public void ISetContainsWorks()
        {
            ISet<string> set = new HashSet<string> { "x", "y", "z" };
            Assert.True(set.Contains("y"));
            Assert.False(set.Contains("a"));
        }

        [Test]
        public void ISetContainsUsesEqualsMethod()
        {
            ISet<C> set = new HashSet<C> { new(1), new(2), new(3) };
            Assert.True(set.Contains(new C(2)));
            Assert.False(set.Contains(new C(4)));
        }

        [Test]
        public void ISetAddWorks()
        {
            ISet<string> set = new HashSet<string> { "x", "y", "z" };
            Assert.True(set.Add("a"));
            Assert.AreDeepEqual(new[] { "a", "x", "y", "z" }, set.OrderBy(x => x));
            Assert.False(set.Add("a"));
        }

        [Test]
        public void ISetRemoveWorks()
        {
            ISet<string> set = new HashSet<string> { "x", "y", "z" };
            Assert.True(set.Remove("x"));
            Assert.AreDeepEqual(new[] { "x", "z" }, set.OrderBy(x => x));
            Assert.False(set.Remove("x"));
        }

        [Test]
        public void ToArrayWorks()
        {
            var hashSet = new HashSet<string>
            {
                "a",
                "b"
            };
            var actual = hashSet.OrderBy(x => x).ToArray();
            Assert.False(ReferenceEquals(hashSet, actual));
            Assert.True(actual is Array);
            Assert.AreDeepEqual(new[] { "a", "b" }, actual.OrderBy(x => x));
        }

        [Test]
        public void IReadOnlyCollectionCountWorks()
        {
            IReadOnlyCollection<string> readOnlyCollection = new HashSet<string> { "x", "y", "z" };
            Assert.AreEqual(3, readOnlyCollection.Count);
        }

        [Test]
        public void IReadOnlyCollectionGetEnumeratorWorks()
        {
            var readOnlyCollection = (IReadOnlyCollection<string>)new HashSet<string> { "x", "y" };
            var e = readOnlyCollection.GetEnumerator();
            Assert.True(e.MoveNext());
            var first = e.Current;
            Assert.True(e.MoveNext());
            var second = e.Current;
            Assert.False(e.MoveNext());
            Assert.True(first == "x" || first == "y");
            Assert.True(second == "x" || second == "y");
            Assert.AreNotEqual(first, second);
        }

        [Test]
        public void IReadOnlySetCountWorks()
        {
            IReadOnlySet<string> readOnlySet = new HashSet<string> { "x", "y", "z" };
            Assert.AreEqual(3, readOnlySet.Count);
        }

        [Test]
        public void IReadOnlySetGetEnumeratorWorks()
        {
            var readOnlySet = (IReadOnlySet<string>)new HashSet<string> { "x", "y" };
            var e = readOnlySet.GetEnumerator();
            Assert.True(e.MoveNext());
            var first = e.Current;
            Assert.True(e.MoveNext());
            var second = e.Current;
            Assert.False(e.MoveNext());
            Assert.True(first == "x" || first == "y");
            Assert.True(second == "x" || second == "y");
            Assert.AreNotEqual(first, second);
        }
    }
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.Linq;
using System.Collections;

namespace UnitTests
{
    public partial class Tests
    {
        [TestMethod]
        public void AnonymousComparer_CallsComparer()
        {
            int invokeCount = 0;
            IComparer<int> test = new AnonymousComparer<int> { Compare = (x, y) => { ++invokeCount; return 0; } };
            int result = test.Compare(13, 17);
            Assert.AreEqual(1, invokeCount, "AnonymousComparer should use Compare to implement IComparer.");
            Assert.AreEqual(0, result, "AnonymousComparer should use Compare to implement IComparer.");
        }

        [TestMethod]
        public void AnonymousEnumerable_CallsGetEnumerator()
        {
            int invokeCount = 0;
            IEnumerable<int> source = new[] { 13 };
            IEnumerable<int> test = new AnonymousEnumerable<int> { GetEnumerator = () => { ++invokeCount; return source.GetEnumerator(); } };
            Assert.IsTrue(test.SequenceEqual(source), "AnonymousEnumerable should use GetEnumerator to implement IEnumerable<T>.");
            Assert.AreEqual(1, invokeCount, "AnonymousEnumerable should use GetEnumerator to implement IEnumerable<T>.");
        }

        [TestMethod]
        public void AnonymousEnumerable_ViaNonGenericEnumerable_CallsGetEnumerator()
        {
            int invokeCount = 0;
            IEnumerable<int> source = new[] { 13 };
            IEnumerable test = new AnonymousEnumerable<int> { GetEnumerator = () => { ++invokeCount; return source.GetEnumerator(); } };
            Assert.IsTrue(test.Cast<int>().SequenceEqual(source), "AnonymousEnumerable should use GetEnumerator to implement IEnumerable.");
            Assert.AreEqual(1, invokeCount, "AnonymousEnumerable should use GetEnumerator to implement IEnumerable.");
        }
    }
}

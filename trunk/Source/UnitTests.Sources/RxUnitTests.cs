using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito;

namespace UnitTests
{
    /// <summary>
    /// Tests are added to this class as Utility functionality is subsumed by the Rx framework.
    /// </summary>
    [TestClass]
    public class RxUnitTests
    {
        [TestMethod]
        public void Zip_ZipsElements()
        {
            IEnumerable<int> source1 = new List<int> { 13, 7 };
            IEnumerable<int> source2 = new List<int> { 17, 23 };
            var result = source1.Zip(source2, (x, y) => x.ToString() + y.ToString());
            Assert.IsTrue(result.SequenceEqual(new[] { "1317", "723" }), "Zip should combine elements.");
        }

        [TestMethod]
        public void Zip_WithShorterFirstSource_IsSmaller()
        {
            IEnumerable<int> source1 = new List<int> { 13 };
            IEnumerable<int> source2 = new List<int> { 17, 23 };
            var result = source1.Zip(source2, (x, y) => x.ToString() + y.ToString());
            Assert.IsTrue(result.SequenceEqual(new[] { "1317" }), "Zip should ignore extra elements.");
        }

        [TestMethod]
        public void Zip_WithShorterSecondSource_IsSmaller()
        {
            IEnumerable<int> source1 = new List<int> { 13, 23 };
            IEnumerable<int> source2 = new List<int> { 17 };
            var result = source1.Zip(source2, (x, y) => x.ToString() + y.ToString());
            Assert.IsTrue(result.SequenceEqual(new[] { "1317" }), "Zip should ignore extra elements.");
        }

        [TestMethod]
        public void Repeat_EnumeratesRepeatedItems()
        {
            IEnumerable<int> source = new[] { 13, 15 };
            var result = source.Repeat(3);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 15, 13, 15, 13, 15 }), "Items should be repeated.");
        }

        [TestMethod]
        public void Repeat_NegativeTimes_EnumeratesEmptySequence()
        {
            IEnumerable<int> source = new[] { 13, 15 };
            var result = source.Repeat(-1);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Items should not be repeated.");
        }

        [TestMethod]
        public void Repeat_Infinitely_EnumeratesRepeatedItems()
        {
            IEnumerable<int> source = new[] { 13, 15 };
            var result = source.Repeat().Take(5);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 15, 13, 15, 13 }), "Items should be repeated.");
        }

        [TestMethod]
        public void Run_InvokesCallback()
        {
            IEnumerable<int> source = new[] { 13, 15 };
            List<int> result = new List<int>();
            source.Run(x => result.Add(x));
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 15 }), "Items should be passed to ForEach.");
        }

        [TestMethod]
        public void Do_InvokesCallbackAndReturnsSequence()
        {
            IEnumerable<int> source = new[] { 13, 15 };
            List<int> result = new List<int>();
            var original = source.Do(x => result.Add(x)).ToList();
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 15 }), "Items should be passed to Tee.");
            Assert.IsTrue(original.SequenceEqual(new[] { 13, 15 }), "Items should be passed through Tee.");
        }

        [TestMethod]
        public void Concat_ConcatenatesSequences()
        {
            IEnumerable<int> test1 = new[] { 1 };
            IEnumerable<int> test2 = new[] { 2, 3 };
            IEnumerable<int> test3 = new[] { 4 };
            var result = EnumerableExtensions.Concat(test1, test2, test3);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Concat should concatenate sequences.");
        }

        [TestMethod]
        public void Return_EnumeratesSingleItem()
        {
            int source = 13;
            var result = EnumerableExtensions.Return(source);
            Assert.IsTrue(result.SequenceEqual(new[] { 13 }), "Item should be enumerated.");
        }

        [TestMethod]
        public void RepeatValue_EnumeratesRepeatedItem()
        {
            int source = 13;
            var result = EnumerableExtensions.Repeat(source, 3);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 13, 13 }), "Item should be repeated.");
        }

        [TestMethod]
        public void RepeatValue_NegativeTimes_EnumeratesEmptySequence()
        {
            int source = 13;
            var result = EnumerableExtensions.Repeat(source, -1);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Item should not be repeated.");
        }

        [TestMethod]
        public void RepeatValue_Infinitely_EnumeratesRepeatedItem()
        {
            int source = 13;
            var result = EnumerableExtensions.Repeat(source).Take(3);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 13, 13 }), "Item should be repeated.");
        }
    }
}

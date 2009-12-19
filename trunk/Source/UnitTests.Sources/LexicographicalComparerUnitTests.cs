using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito;

namespace UnitTests
{
    [TestClass]
    public class LexicographicalComparerUnitTests
    {
        [TestMethod]
        public void Compare_EmptySequences_ReturnsEqual()
        {
            var x = new int[] { };
            var y = new int[] { };
            int result = new LexicographicalComparer<int>().Compare(x, y);
            Assert.IsTrue(result == 0, "Empty sequences should be equal");
        }

        [TestMethod]
        public void Compare_FirstSequenceEmpty_ReturnsLessThan()
        {
            var x = new int[] { };
            var y = new int[] { 1 };
            int result = new LexicographicalComparer<int>().Compare(x, y);
            Assert.IsTrue(result < 0, "Empty sequence should be less than non-empty sequence");
        }

        [TestMethod]
        public void Compare_SecondSequenceEmpty_ReturnsGreaterThan()
        {
            var x = new int[] { 1 };
            var y = new int[] { };
            int result = new LexicographicalComparer<int>().Compare(x, y);
            Assert.IsTrue(result > 0, "Empty sequence should be less than non-empty sequence");
        }

        [TestMethod]
        public void Compare_EqualSequences_ReturnsEqual()
        {
            var x = new int[] { 1, 2, 3 };
            var y = new int[] { 1, 2, 3 };
            int result = new LexicographicalComparer<int>().Compare(x, y);
            Assert.IsTrue(result == 0, "Equal sequences should be equal");
        }

        [TestMethod]
        public void Compare_FirstSequenceShorter_ReturnsLessThan()
        {
            var x = new int[] { 1, 2 };
            var y = new int[] { 1, 2, 3 };
            int result = new LexicographicalComparer<int>().Compare(x, y);
            Assert.IsTrue(result < 0, "Shorter sequence should be less than longer sequence");
        }

        [TestMethod]
        public void Compare_SecondSequenceShorter_ReturnsGreaterThan()
        {
            var x = new int[] { 1, 2, 3 };
            var y = new int[] { 1, 2 };
            int result = new LexicographicalComparer<int>().Compare(x, y);
            Assert.IsTrue(result > 0, "Shorter sequence should be less than longer sequence");
        }

        [TestMethod]
        public void Compare_FirstSequenceSmaller_ReturnsLessThan()
        {
            var x = new int[] { 1, 1, 3 };
            var y = new int[] { 1, 2, 3 };
            int result = new LexicographicalComparer<int>().Compare(x, y);
            Assert.IsTrue(result < 0, "Smaller sequence should be less than larger sequence");
        }

        [TestMethod]
        public void Compare_SecondSequenceSmaller_ReturnsGreaterThan()
        {
            var x = new int[] { 1, 1, 3 };
            var y = new int[] { 1, 0, 3 };
            int result = new LexicographicalComparer<int>().Compare(x, y);
            Assert.IsTrue(result > 0, "Smaller sequence should be less than larger sequence");
        }
    }
}

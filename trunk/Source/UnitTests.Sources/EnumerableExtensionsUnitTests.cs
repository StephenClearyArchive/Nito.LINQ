using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito;

namespace UnitTests
{
    public partial class Tests
    {
        [TestMethod]
        public void Enumerable_IndexOf_WithValidItem_FindsItem()
        {
            IEnumerable<int> source = new List<int> { 13 };
            int result = source.IndexOf(13);
            Assert.AreEqual(0, result, "Item should be found.");
        }

        [TestMethod]
        public void Enumerable_IndexOf_WithInvalidItem_DoesNotFindItem()
        {
            IEnumerable<int> source = new List<int> { 13 };
            int result = source.IndexOf(17);
            Assert.AreEqual(-1, result, "Item should not be found.");
        }

        [TestMethod]
        public void Enumerable_IndexOf_WithSimpleMatch_FindsItem()
        {
            IEnumerable<int> source = new List<int> { 13 };
            int result = source.IndexOf(_ => true);
            Assert.AreEqual(0, result, "Item should be found.");
        }

        [TestMethod]
        public void Enumerable_IndexOf_WithInvalidMatch_DoesNotFindItem()
        {
            IEnumerable<int> source = new List<int> { 13 };
            int result = source.IndexOf(_ => false);
            Assert.AreEqual(-1, result, "Item should not be found.");
        }

        [TestMethod]
        public void Enumerable_IndexOf_WithMultipleMatches_FindsFirstItem()
        {
            IEnumerable<int> source = new List<int> { 13, 15 };
            int result = source.IndexOf(_ => true);
            Assert.AreEqual(0, result, "Item should be found.");
        }

        [TestMethod]
        public void Enumerable_LastIndexOf_WithValidItem_FindsItem()
        {
            IEnumerable<int> source = new List<int> { 13 };
            int result = source.LastIndexOf(13);
            Assert.AreEqual(0, result, "Item should be found.");
        }

        [TestMethod]
        public void Enumerable_LastIndexOf_WithInvalidItem_DoesNotFindItem()
        {
            IEnumerable<int> source = new List<int> { 13 };
            int result = source.LastIndexOf(17);
            Assert.AreEqual(-1, result, "Item should not be found.");
        }

        [TestMethod]
        public void Enumerable_LastIndexOf_WithSimpleMatch_FindsItem()
        {
            IEnumerable<int> source = new List<int> { 13 };
            int result = source.LastIndexOf(_ => true);
            Assert.AreEqual(0, result, "Item should be found.");
        }

        [TestMethod]
        public void Enumerable_LastIndexOf_WithInvalidMatch_DoesNotFindItem()
        {
            IEnumerable<int> source = new List<int> { 13 };
            int result = source.LastIndexOf(_ => false);
            Assert.AreEqual(-1, result, "Item should not be found.");
        }

        [TestMethod]
        public void Enumerable_LastIndexOf_WithMultipleMatches_FindsLastItem()
        {
            IEnumerable<int> source = new List<int> { 13, 15 };
            int result = source.LastIndexOf(_ => true);
            Assert.AreEqual(1, result, "Item should be found.");
        }

        [TestMethod]
        public void Enumerable_IndexOfMin_WithEmptySequence_ReturnsNegativeOne()
        {
            IEnumerable<int> source = new List<int> { };
            int result = source.IndexOfMin();
            Assert.AreEqual(-1, result, "Minimum element of an empty sequence should not be found.");
        }

        [TestMethod]
        public void Enumerable_IndexOfMin_WithOrderedSequence_ReturnsFirstValue()
        {
            IEnumerable<int> source = new List<int> { 1, 2, 3 };
            int result = source.IndexOfMin();
            Assert.AreEqual(0, result, "Minimum element of a sequence should be found.");
        }

        [TestMethod]
        public void Enumerable_IndexOfMin_WithBackwardsOrderedSequence_ReturnsLastValue()
        {
            IEnumerable<int> source = new List<int> { 3, 2, 1 };
            int result = source.IndexOfMin();
            Assert.AreEqual(2, result, "Minimum element of a sequence should be found.");
        }

        [TestMethod]
        public void Enumerable_IndexOfMin_WithBackwardsOrderingFunction_ReturnsMaxValue()
        {
            IEnumerable<int> source = new List<int> { 3, 2, 1 };
            int result = source.IndexOfMin(new AnonymousComparer<int> { Compare = (x, y) => Comparer<int>.Default.Compare(y, x) });
            Assert.AreEqual(0, result, "Minimum element of a sequence should be found.");
        }

        [TestMethod]
        public void Enumerable_IndexOfMax_WithEmptySequence_ReturnsNegativeOne()
        {
            IEnumerable<int> source = new List<int> { };
            int result = source.IndexOfMax();
            Assert.AreEqual(-1, result, "Maximum element of an empty sequence should not be found.");
        }

        [TestMethod]
        public void Enumerable_IndexOfMax_WithOrderedSequence_ReturnsLastValue()
        {
            IEnumerable<int> source = new List<int> { 1, 2, 3 };
            int result = source.IndexOfMax();
            Assert.AreEqual(2, result, "Maximum element of a sequence should be found.");
        }

        [TestMethod]
        public void Enumerable_IndexOfMax_WithBackwardsOrderedSequence_ReturnsFirstValue()
        {
            IEnumerable<int> source = new List<int> { 3, 2, 1 };
            int result = source.IndexOfMax();
            Assert.AreEqual(0, result, "Maximum element of a sequence should be found.");
        }

        [TestMethod]
        public void Enumerable_IndexOfMax_WithBackwardsOrderingFunction_ReturnsMinValue()
        {
            IEnumerable<int> source = new List<int> { 3, 2, 1 };
            int result = source.IndexOfMax(new AnonymousComparer<int> { Compare = (x, y) => Comparer<int>.Default.Compare(y, x) });
            Assert.AreEqual(2, result, "Maximum element of a sequence should be found.");
        }

        [TestMethod]
        public void Enumerable_Min_WithEmptySequence_ReturnsDefaultValue()
        {
            IEnumerable<int> source = new List<int> { };
            int result = source.Min(Comparer<int>.Default);
            Assert.AreEqual(0, result, "Minimum element of an empty sequence should not be found.");
        }

        [TestMethod]
        public void Enumerable_Min_WithOrderedSequence_ReturnsFirstValue()
        {
            IEnumerable<int> source = new List<int> { 1, 2, 3 };
            int result = source.Min(Comparer<int>.Default);
            Assert.AreEqual(1, result, "Minimum element of a sequence should be found.");
        }

        [TestMethod]
        public void Enumerable_Min_WithBackwardsOrderedSequence_ReturnsLastValue()
        {
            IEnumerable<int> source = new List<int> { 3, 2, 1 };
            int result = source.Min(Comparer<int>.Default);
            Assert.AreEqual(1, result, "Minimum element of a sequence should be found.");
        }

        [TestMethod]
        public void Enumerable_Min_WithBackwardsOrderingFunction_ReturnsMaxValue()
        {
            IEnumerable<int> source = new List<int> { 3, 2, 1 };
            int result = source.Min(new AnonymousComparer<int> { Compare = (x, y) => Comparer<int>.Default.Compare(y, x) });
            Assert.AreEqual(3, result, "Minimum element of a sequence should be found.");
        }

        [TestMethod]
        public void Enumerable_Max_WithEmptySequence_ReturnsDefaultValue()
        {
            IEnumerable<int> source = new List<int> { };
            int result = source.Max(Comparer<int>.Default);
            Assert.AreEqual(0, result, "Maximum element of an empty sequence should not be found.");
        }

        [TestMethod]
        public void Enumerable_Max_WithOrderedSequence_ReturnsLastValue()
        {
            IEnumerable<int> source = new List<int> { 1, 2, 3 };
            int result = source.Max(Comparer<int>.Default);
            Assert.AreEqual(3, result, "Maximum element of a sequence should be found.");
        }

        [TestMethod]
        public void Enumerable_Max_WithBackwardsOrderedSequence_ReturnsFirstValue()
        {
            IEnumerable<int> source = new List<int> { 3, 2, 1 };
            int result = source.Max(Comparer<int>.Default);
            Assert.AreEqual(3, result, "Maximum element of a sequence should be found.");
        }

        [TestMethod]
        public void Enumerable_Max_WithBackwardsOrderingFunction_ReturnsMinValue()
        {
            IEnumerable<int> source = new List<int> { 3, 2, 1 };
            int result = source.Max(new AnonymousComparer<int> { Compare = (x, y) => Comparer<int>.Default.Compare(y, x) });
            Assert.AreEqual(1, result, "Maximum element of a sequence should be found.");
        }

        [TestMethod]
        public void Enumerable_Zip3_ZipsElements()
        {
            IEnumerable<int> source1 = new List<int> { 13, 7 };
            IEnumerable<int> source2 = new List<int> { 17, 23 };
            IEnumerable<int> source3 = new List<int> { 27, 29 };
            var result = source1.Zip(source2, source3, (x, y, z) => x.ToString() + y.ToString() + z.ToString());
            Assert.IsTrue(result.SequenceEqual(new[] { "131727", "72329" }), "Zip should combine elements.");
        }

        [TestMethod]
        public void Enumerable_Zip3_WithShorterFirstSource_IsSmaller()
        {
            IEnumerable<int> source1 = new List<int> { 13 };
            IEnumerable<int> source2 = new List<int> { 17, 23 };
            IEnumerable<int> source3 = new List<int> { 27, 29 };
            var result = source1.Zip(source2, source3, (x, y, z) => x.ToString() + y.ToString() + z.ToString());
            Assert.IsTrue(result.SequenceEqual(new[] { "131727" }), "Zip should ignore extra elements.");
        }

        [TestMethod]
        public void Enumerable_Zip3_WithShorterSecondSource_IsSmaller()
        {
            IEnumerable<int> source1 = new List<int> { 13, 23 };
            IEnumerable<int> source2 = new List<int> { 17 };
            IEnumerable<int> source3 = new List<int> { 27, 29 };
            var result = source1.Zip(source2, source3, (x, y, z) => x.ToString() + y.ToString() + z.ToString());
            Assert.IsTrue(result.SequenceEqual(new[] { "131727" }), "Zip should ignore extra elements.");
        }

        [TestMethod]
        public void Enumerable_Zip3_WithShorterThirdSource_IsSmaller()
        {
            IEnumerable<int> source1 = new List<int> { 13, 23 };
            IEnumerable<int> source2 = new List<int> { 17, 23 };
            IEnumerable<int> source3 = new List<int> { 29 };
            var result = source1.Zip(source2, source3, (x, y, z) => x.ToString() + y.ToString() + z.ToString());
            Assert.IsTrue(result.SequenceEqual(new[] { "131729" }), "Zip should ignore extra elements.");
        }

        [TestMethod]
        public void Enumerable_Flatten_FlattensSequences()
        {
            IEnumerable<IEnumerable<int>> test = new[] { new[] { 1 }, new[] { 2 }, new[] { 3, 4 } };
            var result = test.Flatten();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Flatten should flatten sequences.");
        }

        [TestMethod]
        public void Enumerable_Step_EmptySource_EnumeratesEmptySequence()
        {
            IEnumerable<int> source = new List<int> { };
            var result = source.Step(1);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Stepping an empty source should enumerate an empty sequence");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Step should reject 0 or negative step sizes")]
        public void Enumerable_Step_ZeroStep_IsRejected()
        {
            IEnumerable<int> source = new List<int> { 1, 2, 3, 4, 5, 6 };
            var result = source.Step(0);
        }

        [TestMethod]
        public void Enumerable_Step_SingleStep_EnumeratesItems()
        {
            IEnumerable<int> source = new List<int> { 1, 2, 3, 4, 5, 6 };
            var result = source.Step(1);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4, 5, 6 }), "Single-stepping should enumerate the entire source sequence");
        }

        [TestMethod]
        public void Enumerable_Step_By2_EnumeratesItems()
        {
            IEnumerable<int> source = new List<int> { 1, 2, 3, 4, 5, 6 };
            var result = source.Step(2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 3, 5 }), "Stepping should enumerate the requested items");
        }

        [TestMethod]
        public void Enumerable_Step_By3_EnumeratesItems()
        {
            IEnumerable<int> source = new List<int> { 1, 2, 3, 4, 5, 6 };
            var result = source.Step(3);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 4 }), "Stepping should enumerate the requested items");
        }

        [TestMethod]
        public void Enumerable_Rotate_EmptySequence_NegativeOffset_IsEmptySequence()
        {
            IEnumerable<int> source = new int[] { };
            var result = source.Rotate(-1);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Rotating an empty sequence should result in an empty sequence");
        }

        [TestMethod]
        public void Enumerable_Rotate_EmptySequence_ZeroOffset_IsEmptySequence()
        {
            IEnumerable<int> source = new int[] { };
            var result = source.Rotate(0);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Rotating an empty sequence should result in an empty sequence");
        }

        [TestMethod]
        public void Enumerable_Rotate_EmptySequence_PositiveOffset_IsEmptySequence()
        {
            IEnumerable<int> source = new int[] { };
            var result = source.Rotate(1);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Rotating an empty sequence should result in an empty sequence");
        }

        [TestMethod]
        public void Enumerable_Rotate_NegativeOffset_IsSameSequence()
        {
            IEnumerable<int> source = new[] { 1, 2, 3 };
            var result = source.Rotate(-1);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Rotating by negative offset should result in same sequence");
        }

        [TestMethod]
        public void Enumerable_Rotate_ZeroOffset_IsSameSequence()
        {
            IEnumerable<int> source = new[] { 1, 2, 3 };
            var result = source.Rotate(0);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Rotating by zero offset should result in same sequence");
        }

        [TestMethod]
        public void Enumerable_Rotate_RotatesSequence()
        {
            IEnumerable<int> source = new[] { 1, 2, 3 };
            var result = source.Rotate(1);
            Assert.IsTrue(result.SequenceEqual(new[] { 2, 3, 1 }), "Rotating should rotate sequence");
        }

        [TestMethod]
        public void Enumerable_Rotate_CountOffset_IsSameSequence()
        {
            IEnumerable<int> source = new[] { 1, 2, 3 };
            var result = source.Rotate(3);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Rotating by offset equal to count should result in same sequence");
        }

        [TestMethod]
        public void Enumerable_Rotate_GreaterThanCountOffset_IsSameSequence()
        {
            IEnumerable<int> source = new[] { 1, 2, 3 };
            var result = source.Rotate(4);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Rotating by offset greater than count should result in same sequence");
        }
    }
}

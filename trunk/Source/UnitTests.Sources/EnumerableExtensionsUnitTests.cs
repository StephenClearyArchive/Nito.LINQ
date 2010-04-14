using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.Linq;

namespace UnitTests
{
    [TestClass]
    public partial class Tests
    {
        [TestMethod]
        public void Enumerable_Empty_IsEmpty()
        {
            IEnumerable<int> result = EnumerableSource.Empty<int>();
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Empty should return an empty sequence.");
        }

        [TestMethod]
        public void Enumerable_Generate_WithInfiniteSimpleDelegate_GeneratesSequence()
        {
            int source = 13;
            IEnumerable<int> result = EnumerableSource.Generate(() => source++).Take(3);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 14, 15 }), "Generate should return the requested sequence.");
        }

        [TestMethod]
        public void Enumerable_Generate_WithInfiniteIndexedDelegate_GeneratesSequence()
        {
            IEnumerable<int> result = EnumerableSource.Generate((i) => i * 2).Take(3);
            Assert.IsTrue(result.SequenceEqual(new[] { 0, 2, 4 }), "Generate should return the requested sequence.");
        }

        [TestMethod]
        public void Enumerable_Generate_WithFiniteSimpleDelegate_GeneratesSequence()
        {
            int source = 13;
            IEnumerable<int> result = EnumerableSource.Generate(() => source++, 3);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 14, 15 }), "Generate should return the requested sequence.");
        }

        [TestMethod]
        public void Enumerable_Generate_WithFiniteIndexedDelegate_GeneratesSequence()
        {
            IEnumerable<int> result = EnumerableSource.Generate((i) => i * 2, 3);
            Assert.IsTrue(result.SequenceEqual(new[] { 0, 2, 4 }), "Generate should return the requested sequence.");
        }

        [TestMethod]
        public void Enumerable_Generate_WithFiniteSimpleDelegate_ZeroCount_GeneratesEmptySequence()
        {
            int source = 13;
            IEnumerable<int> result = EnumerableSource.Generate(() => source++, 0);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Generate should return an empty sequence.");
        }

        [TestMethod]
        public void Enumerable_Generate_WithFiniteIndexedDelegate_ZeroCount_GeneratesEmptySequence()
        {
            IEnumerable<int> result = EnumerableSource.Generate((i) => i * 2, 0);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Generate should return an empty sequence.");
        }

        [TestMethod]
        public void Enumerable_Generate_WithFiniteSimpleDelegate_NegativeCount_GeneratesEmptySequence()
        {
            int source = 13;
            IEnumerable<int> result = EnumerableSource.Generate(() => source++, -1);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Generate should return an empty sequence.");
        }

        [TestMethod]
        public void Enumerable_Generate_WithFiniteIndexedDelegate_NegativeCount_GeneratesEmptySequence()
        {
            IEnumerable<int> result = EnumerableSource.Generate((i) => i * 2, -1);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Generate should return an empty sequence.");
        }

        [TestMethod]
        public void Enumerable_Generate_WithInfiniteSingleValueDelegate_GeneratesSequence()
        {
            IEnumerable<int> result = EnumerableSource.Generate(0, i => i * 2, i => i + 1).Take(4);
            Assert.IsTrue(result.SequenceEqual(new[] { 0, 2, 4, 6 }), "Generate should return the requested sequence.");
        }

        [TestMethod]
        public void Enumerable_Generate_WithInfiniteMultiValueDelegate_GeneratesSequence()
        {
            IEnumerable<int> result = EnumerableSource.Generate(1, i => Enumerable.Range(1, i), i => i + 1).Take(6);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 1, 2, 1, 2, 3 }), "Generate should return the requested sequence.");
        }

        [TestMethod]
        public void Enumerable_Generate_WithFiniteSingleValueDelegate_GeneratesSequence()
        {
            IEnumerable<int> result = EnumerableSource.Generate(0, i => i < 4, i => i * 2, i => i + 1);
            Assert.IsTrue(result.SequenceEqual(new[] { 0, 2, 4, 6 }), "Generate should return the requested sequence.");
        }

        [TestMethod]
        public void Enumerable_Generate_WithFiniteMultiValueDelegate_GeneratesSequence()
        {
            IEnumerable<int> result = EnumerableSource.Generate(1, i => i <= 3, i => Enumerable.Range(1, i), i => i + 1);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 1, 2, 1, 2, 3 }), "Generate should return the requested sequence.");
        }

        [TestMethod]
        public void Enumerable_Range_GeneratesRange()
        {
            IEnumerable<int> result = EnumerableSource.Range(3, 4);
            Assert.IsTrue(result.SequenceEqual(new[] { 3, 4, 5, 6 }), "Range should return the requested range.");
        }

        [TestMethod]
        public void Enumerable_Range_ZeroCount_GeneratesEmptyRange()
        {
            IEnumerable<int> result = EnumerableSource.Range(3, 0);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Range should return an empty range.");
        }

        [TestMethod]
        public void Enumerable_Range_NegativeCount_GeneratesEmptyRange()
        {
            IEnumerable<int> result = EnumerableSource.Range(3, -1);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Range should return an empty range.");
        }

        [TestMethod]
        public void Enumerable_Defer_IsNotEvaluatedUntilEnumeration()
        {
            int invokeCount = 0;
            IEnumerable<int> result = EnumerableSource.Defer(() => { ++invokeCount; return new int[] { }; });
            Assert.AreEqual(0, invokeCount, "Defer should not be evaluated until it's enumerated.");
        }

        [TestMethod]
        public void Enumerable_Defer_IsReevaluatedEachEnumeration()
        {
            int invokeCount = 0;
            IEnumerable<int> result = EnumerableSource.Defer(() => { ++invokeCount; return new int[] { }; });
            result.Run();
            result.Run();
            Assert.AreEqual(2, invokeCount, "Defer should be evaluated each time it's enumerated.");
        }

        [TestMethod]
        public void Enumerable_Zip_ZipsElements()
        {
            IEnumerable<int> source1 = new List<int> { 13, 7 };
            IEnumerable<int> source2 = new List<int> { 17, 23 };
            var result = source1.Zip(source2, (x, y) => x.ToString() + y.ToString());
            Assert.IsTrue(result.SequenceEqual(new[] { "1317", "723" }), "Zip should combine elements.");
        }

        [TestMethod]
        public void Enumerable_Zip_WithShorterFirstSource_IsSmaller()
        {
            IEnumerable<int> source1 = new List<int> { 13 };
            IEnumerable<int> source2 = new List<int> { 17, 23 };
            var result = source1.Zip(source2, (x, y) => x.ToString() + y.ToString());
            Assert.IsTrue(result.SequenceEqual(new[] { "1317" }), "Zip should ignore extra elements.");
        }

        [TestMethod]
        public void Enumerable_Zip_WithShorterSecondSource_IsSmaller()
        {
            IEnumerable<int> source1 = new List<int> { 13, 23 };
            IEnumerable<int> source2 = new List<int> { 17 };
            var result = source1.Zip(source2, (x, y) => x.ToString() + y.ToString());
            Assert.IsTrue(result.SequenceEqual(new[] { "1317" }), "Zip should ignore extra elements.");
        }

        [TestMethod]
        public void Enumerable_Repeat_EnumeratesRepeatedItems()
        {
            IEnumerable<int> source = new[] { 13, 15 };
            var result = source.Repeat(3);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 15, 13, 15, 13, 15 }), "Items should be repeated.");
        }

        [TestMethod]
        public void Enumerable_Repeat_NegativeTimes_EnumeratesEmptySequence()
        {
            IEnumerable<int> source = new[] { 13, 15 };
            var result = source.Repeat(-1);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Items should not be repeated.");
        }

        [TestMethod]
        public void Enumerable_Repeat_Infinitely_EnumeratesRepeatedItems()
        {
            IEnumerable<int> source = new[] { 13, 15 };
            var result = source.Repeat().Take(5);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 15, 13, 15, 13 }), "Items should be repeated.");
        }

        [TestMethod]
        public void Enumerable_Run_InvokesCallback()
        {
            IEnumerable<int> source = new[] { 13, 15 };
            List<int> result = new List<int>();
            source.Run(x => result.Add(x));
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 15 }), "Items should be passed to Run.");
        }

        [TestMethod]
        public void Enumerable_Run_ExecutesSequence()
        {
            int value = 0;
            IEnumerable<int> source = EnumerableSource.Generate(0, i => i != 10, _ => ++value, i => i + 1);
            source.Run();
            Assert.AreEqual(10, value, "Run should evaluate the sequence.");
        }

        [TestMethod]
        public void Enumerable_Do_InvokesCallbackAndReturnsSequence()
        {
            IEnumerable<int> source = new[] { 13, 15 };
            List<int> result = new List<int>();
            var original = source.Do(x => result.Add(x)).ToList();
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 15 }), "Items should be passed to Do.");
            Assert.IsTrue(original.SequenceEqual(new[] { 13, 15 }), "Items should be passed through Do.");
        }

        [TestMethod]
        public void Enumerable_Concat_ConcatenatesSequences()
        {
            IEnumerable<int> test1 = new[] { 1 };
            IEnumerable<int> test2 = new[] { 2, 3 };
            IEnumerable<int> test3 = new[] { 4 };
            var result = EnumerableExtensions.Concat(test1, test2, test3);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Concat should concatenate sequences.");
        }

        [TestMethod]
        public void Enumerable_Return_EnumeratesSingleItem()
        {
            int source = 13;
            var result = EnumerableSource.Return(source);
            Assert.IsTrue(result.SequenceEqual(new[] { 13 }), "Item should be enumerated.");
        }

        [TestMethod]
        public void Enumerable_RepeatValue_EnumeratesRepeatedItem()
        {
            int source = 13;
            var result = EnumerableSource.Repeat(source, 3);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 13, 13 }), "Item should be repeated.");
        }

        [TestMethod]
        public void Enumerable_RepeatValue_NegativeTimes_EnumeratesEmptySequence()
        {
            int source = 13;
            var result = EnumerableSource.Repeat(source, -1);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Item should not be repeated.");
        }

        [TestMethod]
        public void Enumerable_RepeatValue_Infinitely_EnumeratesRepeatedItem()
        {
            int source = 13;
            var result = EnumerableSource.Repeat(source).Take(3);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 13, 13 }), "Item should be repeated.");
        }

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
            int result = source.IndexOfMin((x, y) => Comparer<int>.Default.Compare(y, x));
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
            int result = source.IndexOfMax((x, y) => Comparer<int>.Default.Compare(y, x));
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
            int result = source.Min((x, y) => Comparer<int>.Default.Compare(y, x));
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
            int result = source.Max((x, y) => Comparer<int>.Default.Compare(y, x));
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
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Stepping an empty source should enumerate an empty sequence.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Step should reject 0 or negative step sizes.")]
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
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4, 5, 6 }), "Single-stepping should enumerate the entire source sequence.");
        }

        [TestMethod]
        public void Enumerable_Step_By2_EnumeratesItems()
        {
            IEnumerable<int> source = new List<int> { 1, 2, 3, 4, 5, 6 };
            var result = source.Step(2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 3, 5 }), "Stepping should enumerate the requested items.");
        }

        [TestMethod]
        public void Enumerable_Step_By3_EnumeratesItems()
        {
            IEnumerable<int> source = new List<int> { 1, 2, 3, 4, 5, 6 };
            var result = source.Step(3);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 4 }), "Stepping should enumerate the requested items.");
        }

        [TestMethod]
        public void Enumerable_Rotate_EmptySequence_NegativeOffset_IsEmptySequence()
        {
            IEnumerable<int> source = new int[] { };
            var result = source.Rotate(-1);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Rotating an empty sequence should result in an empty sequence.");
        }

        [TestMethod]
        public void Enumerable_Rotate_EmptySequence_ZeroOffset_IsEmptySequence()
        {
            IEnumerable<int> source = new int[] { };
            var result = source.Rotate(0);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Rotating an empty sequence should result in an empty sequence.");
        }

        [TestMethod]
        public void Enumerable_Rotate_EmptySequence_PositiveOffset_IsEmptySequence()
        {
            IEnumerable<int> source = new int[] { };
            var result = source.Rotate(1);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Rotating an empty sequence should result in an empty sequence.");
        }

        [TestMethod]
        public void Enumerable_Rotate_NegativeOffset_IsSameSequence()
        {
            IEnumerable<int> source = new[] { 1, 2, 3 };
            var result = source.Rotate(-1);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Rotating by negative offset should result in same sequence.");
        }

        [TestMethod]
        public void Enumerable_Rotate_ZeroOffset_IsSameSequence()
        {
            IEnumerable<int> source = new[] { 1, 2, 3 };
            var result = source.Rotate(0);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Rotating by zero offset should result in same sequence.");
        }

        [TestMethod]
        public void Enumerable_Rotate_RotatesSequence()
        {
            IEnumerable<int> source = new[] { 1, 2, 3 };
            var result = source.Rotate(1);
            Assert.IsTrue(result.SequenceEqual(new[] { 2, 3, 1 }), "Rotating should rotate sequence.");
        }

        [TestMethod]
        public void Enumerable_Rotate_CountOffset_IsSameSequence()
        {
            IEnumerable<int> source = new[] { 1, 2, 3 };
            var result = source.Rotate(3);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Rotating by offset equal to count should result in same sequence.");
        }

        [TestMethod]
        public void Enumerable_Rotate_GreaterThanCountOffset_IsSameSequence()
        {
            IEnumerable<int> source = new[] { 1, 2, 3 };
            var result = source.Rotate(4);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Rotating by offset greater than count should result in same sequence.");
        }
    }
}

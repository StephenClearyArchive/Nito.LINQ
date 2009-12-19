using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito;
using System.Collections;

namespace UnitTests
{
    [TestClass]
    public class SortedEnumerableExtensionsUnitTests
    {
        [TestMethod]
        public void IsSorted_EmptySequence_IsTrue()
        {
            var test = new int[] { };
            bool result = test.IsSorted();
            Assert.IsTrue(result, "Empty sequences should be sorted");
        }

        [TestMethod]
        public void IsSorted_SingleElementSequence_IsTrue()
        {
            var test = new[] { 1 };
            bool result = test.IsSorted();
            Assert.IsTrue(result, "Single-element sequences should be sorted");
        }

        [TestMethod]
        public void IsSorted_SortedSequence_IsTrue()
        {
            var test = new[] { 1, 1, 3 };
            bool result = test.IsSorted();
            Assert.IsTrue(result, "Sorted sequences should be sorted");
        }

        [TestMethod]
        public void IsSorted_UnsortedSequence_IsFalse()
        {
            var test = new[] { 1, 1, 0 };
            bool result = test.IsSorted();
            Assert.IsFalse(result, "Unsorted sequences should not be sorted");
        }

        [TestMethod]
        public void IsSorted_ReverseSortedSequence_IsTrue()
        {
            var test = new[] { 1, 1, 0 };
            bool result = test.IsSorted(new AnonymousComparer<int> { Compare = (x, y) => Comparer<int>.Default.Compare(y, x) });
            Assert.IsTrue(result, "Reverse sorted sequences should be sorted (in reverse)");
        }

        [TestMethod]
        public void AsSortedEnumerable_ReturnsArgument()
        {
            var test = new[] { 1, 2, 3, 4 }.AsSorted();
            var result = test.AsSortedEnumerable();
            Assert.AreSame(test, result, "AsSortedEnumerable should return its argument");
        }

        [TestMethod]
        public void Empty_IsEmpty()
        {
            var sorted = SortedEnumerableExtensions.Empty<string>();
            Assert.IsTrue(sorted.SequenceEqual(new string[] { }), "Empty should be empty");
        }

        [TestMethod]
        public void Empty_RemembersComparer()
        {
            var sorted = SortedEnumerableExtensions.Empty<string>(StringComparer.InvariantCultureIgnoreCase);
            Assert.AreEqual(StringComparer.InvariantCultureIgnoreCase, sorted.Comparer, "Empty should remember its comparison object");
        }

        [TestMethod]
        public void AsSorted_RemembersComparer()
        {
            IEnumerable<string> source = new[] { "a", "b", "c" };
            var sorted = source.AsSorted(StringComparer.InvariantCultureIgnoreCase);
            Assert.AreEqual(StringComparer.InvariantCultureIgnoreCase, sorted.Comparer, "AsSorted should remember its comparison object");
        }

        [TestMethod]
        public void AsSorted_SupportsNongenericEnumeration()
        {
            IEnumerable<string> source = new[] { "a", "b", "c" };
            var sorted = source.AsSorted(StringComparer.InvariantCultureIgnoreCase);
            List<string> result = new List<string>();
            var iterator = ((IEnumerable)sorted).GetEnumerator();
            while (iterator.MoveNext())
            {
                result.Add((string)iterator.Current);
            }

            Assert.IsTrue(result.SequenceEqual(new[] { "a", "b", "c" }), "AsSorted should support non-generic enumeration");
        }

        [TestMethod]
        public void Return_EnumeratesSingleItem()
        {
            int source = 13;
            ISortedEnumerable<int> result = SortedEnumerableExtensions.Return(source);
            Assert.IsTrue(result.SequenceEqual(new[] { 13 }), "Item should be enumerated.");
        }

        [TestMethod]
        public void Repeat_EnumeratesRepeatedItem()
        {
            int source = 13;
            ISortedEnumerable<int> result = SortedEnumerableExtensions.Repeat(source, 3);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 13, 13 }), "Item should be repeated.");
        }

        [TestMethod]
        public void Repeat_NegativeTimes_EnumeratesEmptySequence()
        {
            int source = 13;
            var result = SortedEnumerableExtensions.Repeat(source, -1);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Item should not be repeated.");
        }

        [TestMethod]
        public void Repeat_Infinitely_EnumeratesRepeatedItem()
        {
            int source = 13;
            var result = SortedEnumerableExtensions.Repeat(source).Take(3);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 13, 13 }), "Item should be repeated.");
        }

        [TestMethod]
        public void LowerBound_ItemFound_ReturnsLowerBound()
        {
            IEnumerable<int> source = new[] { 1, 2, 2, 4 };
            var sorted = source.AsSorted();
            int result = sorted.LowerBound(2);
            Assert.AreEqual(1, result, "LowerBound should return the lower bound");
        }

        [TestMethod]
        public void LowerBound_ItemNotFound_ReturnsBitwiseComplement()
        {
            IEnumerable<int> source = new[] { 1, 2, 2, 4 };
            var sorted = source.AsSorted();
            int result = sorted.LowerBound(3);
            Assert.AreEqual(~3, result, "LowerBound should return the bitwise complement if not found");
        }

        [TestMethod]
        public void LowerBound_ItemNotFoundInEmptySequence_ReturnsBitwiseComplementOf0()
        {
            IEnumerable<int> source = new int[] { };
            var sorted = source.AsSorted();
            int result = sorted.LowerBound(3);
            Assert.AreEqual(~0, result, "LowerBound should return the bitwise complement if not found");
        }

        [TestMethod]
        public void LowerBound_ItemNotFoundPastSequence_ReturnsBitwiseComplement()
        {
            IEnumerable<int> source = new[] { 1, 2, 2, 4 };
            var sorted = source.AsSorted();
            int result = sorted.LowerBound(5);
            Assert.AreEqual(~4, result, "LowerBound should return the bitwise complement if not found");
        }

        [TestMethod]
        public void UpperBound_ItemFound_ReturnsUpperBound()
        {
            IEnumerable<int> source = new[] { 1, 2, 2, 4 };
            var sorted = source.AsSorted();
            int result = sorted.UpperBound(2);
            Assert.AreEqual(3, result, "UpperBound should return the upper bound");
        }

        [TestMethod]
        public void UpperBound_ItemNotFound_ReturnsBitwiseComplement()
        {
            IEnumerable<int> source = new[] { 1, 2, 2, 4 };
            var sorted = source.AsSorted();
            int result = sorted.UpperBound(3);
            Assert.AreEqual(~3, result, "UpperBound should return the bitwise complement if not found");
        }

        [TestMethod]
        public void UpperBound_ItemNotFoundInEmptySequence_ReturnsBitwiseComplementOf0()
        {
            IEnumerable<int> source = new int[] { };
            var sorted = source.AsSorted();
            int result = sorted.UpperBound(3);
            Assert.AreEqual(~0, result, "LowerBound should return the bitwise complement if not found");
        }

        [TestMethod]
        public void UpperBound_ItemNotFoundPastSequence_ReturnsBitwiseComplement()
        {
            IEnumerable<int> source = new[] { 1, 2, 2, 4 };
            var sorted = source.AsSorted();
            int result = sorted.UpperBound(5);
            Assert.AreEqual(~4, result, "LowerBound should return the bitwise complement if not found");
        }

        [TestMethod]
        public void EqualRange_RunOf0_AtBeginning()
        {
            IEnumerable<int> source = new[] { 1, 3 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(0, out begin, out end);
            Assert.AreEqual(0, begin);
            Assert.AreEqual(0, end);
        }

        [TestMethod]
        public void EqualRange_RunOf0_InMiddle()
        {
            IEnumerable<int> source = new[] { 1, 3 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(2, out begin, out end);
            Assert.AreEqual(1, begin);
            Assert.AreEqual(1, end);
        }

        [TestMethod]
        public void EqualRange_RunOf0_AtEnd()
        {
            IEnumerable<int> source = new[] { 1, 3 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(4, out begin, out end);
            Assert.AreEqual(2, begin);
            Assert.AreEqual(2, end);
        }

        [TestMethod]
        public void EqualRange_RunOf1_AtBeginning()
        {
            IEnumerable<int> source = new[] { 1, 3 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(1, out begin, out end);
            Assert.AreEqual(0, begin);
            Assert.AreEqual(1, end);
        }

        [TestMethod]
        public void EqualRange_RunOf1_InMiddle()
        {
            IEnumerable<int> source = new[] { 1, 3, 5, 7 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(3, out begin, out end);
            Assert.AreEqual(1, begin);
            Assert.AreEqual(2, end);
        }

        [TestMethod]
        public void EqualRange_RunOf1_InSecondMiddle()
        {
            IEnumerable<int> source = new[] { 1, 3, 5, 7 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(5, out begin, out end);
            Assert.AreEqual(2, begin);
            Assert.AreEqual(3, end);
        }

        [TestMethod]
        public void EqualRange_RunOf1_AtEnd()
        {
            IEnumerable<int> source = new[] { 1, 3 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(3, out begin, out end);
            Assert.AreEqual(1, begin);
            Assert.AreEqual(2, end);
        }

        [TestMethod]
        public void EqualRange_RunOf2_AtBeginning()
        {
            IEnumerable<int> source = new[] { 1, 1, 3, 4 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(1, out begin, out end);
            Assert.AreEqual(0, begin);
            Assert.AreEqual(2, end);
        }

        [TestMethod]
        public void EqualRange_RunOf2_AtMiddle()
        {
            IEnumerable<int> source = new[] { 1, 2, 3, 3, 4, 5 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(3, out begin, out end);
            Assert.AreEqual(2, begin);
            Assert.AreEqual(4, end);
        }

        [TestMethod]
        public void EqualRange_RunOf2_AtEnd()
        {
            IEnumerable<int> source = new[] { 1, 2, 3, 3 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(3, out begin, out end);
            Assert.AreEqual(2, begin);
            Assert.AreEqual(4, end);
        }

        [TestMethod]
        public void EqualRange_RunOf3_AtBeginning()
        {
            IEnumerable<int> source = new[] { 1, 1, 1, 4 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(1, out begin, out end);
            Assert.AreEqual(0, begin);
            Assert.AreEqual(3, end);
        }

        [TestMethod]
        public void EqualRange_RunOf3_AtMiddle()
        {
            IEnumerable<int> source = new[] { 0, 1, 1, 1, 4, 4 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(1, out begin, out end);
            Assert.AreEqual(1, begin);
            Assert.AreEqual(4, end);
        }

        [TestMethod]
        public void EqualRange_RunOf3_AtSecondMiddle()
        {
            IEnumerable<int> source = new[] { 0, 1, 4, 4, 4, 7 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(4, out begin, out end);
            Assert.AreEqual(2, begin);
            Assert.AreEqual(5, end);
        }

        [TestMethod]
        public void EqualRange_RunOf3_AtEnd()
        {
            IEnumerable<int> source = new[] { 0, 1, 1, 4, 4, 4 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(4, out begin, out end);
            Assert.AreEqual(3, begin);
            Assert.AreEqual(6, end);
        }

        [TestMethod]
        public void Contains_WithValidItemValue_FindsItem()
        {
            IEnumerable<string> source = new[] { "a", "b", "c" };
            var sorted = source.AsSorted();
            bool result = sorted.Contains("b");
            Assert.IsTrue(result, "Item should be found.");
        }

        [TestMethod]
        public void Contains_WithInvalidItemValue_DoesNotFindItem()
        {
            IEnumerable<string> source = new[] { "a", "b", "c" };
            var sorted = source.AsSorted();
            bool result = sorted.Contains("0");
            Assert.IsFalse(result, "Item should not be found.");
        }

        [TestMethod]
        public void Contains_WithValidItem_FindsItem()
        {
            IEnumerable<string> source = new[] { "a", "b", "c" };
            var sorted = source.AsSorted();
            bool result = sorted.Contains(x => StringComparer.InvariantCultureIgnoreCase.Compare("b", x));
            Assert.IsTrue(result, "Item should be found.");
        }

        [TestMethod]
        public void Contains_WithInvalidItem_DoesNotFindItem()
        {
            IEnumerable<string> source = new[] { "a", "b", "c" };
            var sorted = source.AsSorted();
            bool result = sorted.Contains(x => StringComparer.InvariantCultureIgnoreCase.Compare("0", x));
            Assert.IsFalse(result, "Item should not be found.");
        }

        [TestMethod]
        public void IndexOf_WithValidItem_FindsItem()
        {
            IEnumerable<string> source = new[] { "a", "a", "c" };
            var sorted = source.AsSorted();
            int result = sorted.IndexOf(x => StringComparer.InvariantCultureIgnoreCase.Compare("a", x));
            Assert.AreEqual(0, result, "Item should be found.");
        }

        [TestMethod]
        public void IndexOf_WithInvalidItem_DoesNotFindItem()
        {
            IEnumerable<string> source = new[] { "a", "a", "c" };
            var sorted = source.AsSorted();
            int result = sorted.IndexOf(x => StringComparer.InvariantCultureIgnoreCase.Compare("b", x));
            Assert.AreEqual(-1, result, "Item should not be found.");
        }

        [TestMethod]
        public void IndexOf_WithValidItemValue_FindsItem()
        {
            IEnumerable<string> source = new[] { "a", "a", "c" };
            var sorted = source.AsSorted();
            int result = sorted.IndexOf("a");
            Assert.AreEqual(0, result, "Item should be found.");
        }

        [TestMethod]
        public void IndexOf_WithInvalidItemValue_DoesNotFindItem()
        {
            IEnumerable<string> source = new[] { "a", "a", "c" };
            var sorted = source.AsSorted();
            int result = sorted.IndexOf("b");
            Assert.AreEqual(-1, result, "Item should not be found.");
        }

        [TestMethod]
        public void LastIndexOf_WithValidItem_FindsItem()
        {
            IEnumerable<string> source = new[] { "a", "a", "c" };
            var sorted = source.AsSorted();
            int result = sorted.LastIndexOf(x => StringComparer.InvariantCultureIgnoreCase.Compare("a", x));
            Assert.AreEqual(1, result, "Item should be found.");
        }

        [TestMethod]
        public void LastIndexOf_WithInvalidItem_DoesNotFindItem()
        {
            IEnumerable<string> source = new[] { "a", "a", "c" };
            var sorted = source.AsSorted();
            int result = sorted.LastIndexOf(x => StringComparer.InvariantCultureIgnoreCase.Compare("d", x));
            Assert.AreEqual(-1, result, "Item should not be found.");
        }

        [TestMethod]
        public void LastIndexOf_WithValidItemValue_FindsItem()
        {
            IEnumerable<string> source = new[] { "a", "a", "c" };
            var sorted = source.AsSorted();
            int result = sorted.LastIndexOf("a");
            Assert.AreEqual(1, result, "Item should be found.");
        }

        [TestMethod]
        public void LastIndexOf_WithInvalidItemValue_DoesNotFindItem()
        {
            IEnumerable<string> source = new[] { "a", "a", "c" };
            var sorted = source.AsSorted();
            int result = sorted.LastIndexOf("A");
            Assert.AreEqual(-1, result, "Item should not be found.");
        }

        [TestMethod]
        public void Skip_EnumeratesItems()
        {
            ISortedEnumerable<int> test = new[] { 1, 2, 3, 4 }.AsEnumerable().AsSorted();
            ISortedEnumerable<int> result = test.Skip(1);
            Assert.IsTrue(result.SequenceEqual(new[] { 2, 3, 4 }), "Skip should skip the requested items");
        }

        [TestMethod]
        public void Step_By2_EnumeratesItems()
        {
            ISortedEnumerable<int> source = new List<int> { 1, 2, 3, 4, 5, 6 }.AsEnumerable().AsSorted();
            ISortedEnumerable<int> result = source.Step(2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 3, 5 }), "Stepping should enumerate the requested items");
        }

        [TestMethod]
        public void Take_EnumeratesItems()
        {
            ISortedEnumerable<int> test = new[] { 1, 2, 3, 4 }.AsEnumerable().AsSorted();
            ISortedEnumerable<int> result = test.Take(2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2 }), "Take should take only the requested items");
        }

        [TestMethod]
        public void Merge_OnEmptySequences_IsEmptySequence()
        {
            ISortedEnumerable<int> source1 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            var result = source1.MergeSorted(source2);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Merging of empty sequences should be an empty sequence");
        }

        [TestMethod]
        public void MergeViaParams_OnEmptySequences_IsEmptySequence()
        {
            ISortedEnumerable<int> source1 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            var result = SortedEnumerableExtensions.MergeSorted(source1, source2);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Merging of empty sequences should be an empty sequence");
        }

        [TestMethod]
        public void MergeViaSequencesExtension_OnEmptySequences_IsEmptySequence()
        {
            ISortedEnumerable<int> source1 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            var sources = new[] { source1, source2 };
            var result = sources.MergeSorted();
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Merging of empty sequences should be an empty sequence");
        }

        [TestMethod]
        public void Merge_OnNonEmptySequences_IsMerge()
        {
            ISortedEnumerable<int> source1 = new int[] { 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source3 = new int[] { 1, 3 }.AsSorted();
            ISortedEnumerable<int> source4 = new int[] { 0, 1, 2 }.AsSorted();
            var result = source1.MergeSorted(source2, source3, source4);
            Assert.IsTrue(result.SequenceEqual(new int[] { 0, 1, 1, 1, 2, 3 }), "Merging of sequences should be their merging");
        }

        [TestMethod]
        public void MergeViaValues_IsMerge()
        {
            ISortedEnumerable<int> source1 = new int[] { 1 }.AsSorted();
            var result = source1.MergeSorted(1, 3, 0, 1, 2);
            Assert.IsTrue(result.SequenceEqual(new int[] { 0, 1, 1, 1, 2, 3 }), "Merging of values should be their merging");
        }

        [TestMethod]
        public void Merge_WithDuplicates_IsMerge()
        {
            ISortedEnumerable<int> source1 = new[] { 1, 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new[] { 1, 2 }.AsSorted();
            var result = source1.MergeSorted(source2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 1, 1, 2 }), "Merging should keep duplicates");
        }
        
        [TestMethod]
        public void Union_OnEmptySequences_IsEmptySequence()
        {
            ISortedEnumerable<int> source1 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            var result = source1.UnionWithDuplicates(source2);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Union of empty sequences should be an empty sequence");
        }

        [TestMethod]
        public void UnionViaParams_OnEmptySequences_IsEmptySequence()
        {
            ISortedEnumerable<int> source1 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            var result = SortedEnumerableExtensions.UnionWithDuplicates(source1, source2);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Union of empty sequences should be an empty sequence");
        }

        [TestMethod]
        public void UnionViaSequencesExtension_OnEmptySequences_IsEmptySequence()
        {
            ISortedEnumerable<int> source1 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            var sources = new[] { source1, source2 };
            var result = sources.UnionWithDuplicates();
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Union of empty sequences should be an empty sequence");
        }

        [TestMethod]
        public void Union_OnNonEmptySequences_IsUnion()
        {
            ISortedEnumerable<int> source1 = new int[] { 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source3 = new int[] { 1, 3 }.AsSorted();
            ISortedEnumerable<int> source4 = new int[] { 0, 1, 2 }.AsSorted();
            var result = source1.UnionWithDuplicates(source2, source3, source4);
            Assert.IsTrue(result.SequenceEqual(new int[] { 0, 1, 2, 3 }), "Union of sequences should be their union");
        }

        [TestMethod]
        public void UnionViaValues_IsUnion()
        {
            ISortedEnumerable<int> source1 = new int[] { 1 }.AsSorted();
            var result = source1.UnionWithDuplicates(1, 3, 0, 1, 2);
            Assert.IsTrue(result.SequenceEqual(new int[] { 0, 1, 2, 3 }), "Union of values should be their union");
        }

        [TestMethod]
        public void Union_WithDuplicates_IsUnion()
        {
            ISortedEnumerable<int> source1 = new[] { 1, 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new[] { 1, 2 }.AsSorted();
            var result = source1.UnionWithDuplicates(source2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 1, 2 }), "Union should keep duplicates");
        }

        [TestMethod]
        public void UnionDistinct_WithDuplicates_IsDistinctUnion()
        {
            ISortedEnumerable<int> source1 = new[] { 1, 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new[] { 1, 2 }.AsSorted();
            var result = source1.Union(source2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2 }), "Union should drop duplicates");
        }

        [TestMethod]
        public void UnionDistinct_ViaSequencesExtension_IsDistinctUnion()
        {
            ISortedEnumerable<int> source1 = new[] { 1, 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new[] { 1, 2 }.AsSorted();
            var source = new[] { source1, source2 };
            var result = source.Union();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2 }), "Union should drop duplicates");
        }

        [TestMethod]
        public void UnionDistinct_ViaValues_IsDistinctUnion()
        {
            ISortedEnumerable<int> source1 = new[] { 1, 1 }.AsSorted();
            var result = source1.Union(1, 2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2 }), "Union should drop duplicates");
        }

        [TestMethod]
        public void Intersect_OnEmptySequences_IsEmptySequence()
        {
            ISortedEnumerable<int> source1 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            var result = source1.IntersectWithDuplicates(source2);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Intersect of empty sequences should be an empty sequence");
        }

        [TestMethod]
        public void IntersectViaParams_OnEmptySequences_IsEmptySequence()
        {
            ISortedEnumerable<int> source1 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            var result = SortedEnumerableExtensions.IntersectWithDuplicates(source1, source2);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Intersect of empty sequences should be an empty sequence");
        }

        [TestMethod]
        public void IntersectViaSequencesExtension_OnEmptySequences_IsEmptySequence()
        {
            ISortedEnumerable<int> source1 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            var sources = new[] { source1, source2 };
            var result = sources.IntersectWithDuplicates();
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Intersect of empty sequences should be an empty sequence");
        }

        [TestMethod]
        public void Intersect_WithDuplicateValues_PreservesDuplicates()
        {
            ISortedEnumerable<int> source1 = new int[] { 1, 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { 0, 1, 1 }.AsSorted();
            ISortedEnumerable<int> source3 = new int[] { 1, 1, 3 }.AsSorted();
            ISortedEnumerable<int> source4 = new int[] { 0, 1, 1, 2 }.AsSorted();
            var result = source1.IntersectWithDuplicates(source2, source3, source4);
            Assert.IsTrue(result.SequenceEqual(new int[] { 1, 1 }), "Intersect of sequences should be their intersection");
        }

        [TestMethod]
        public void Intersect_WithSmallValuesEnding_IsIntersection()
        {
            ISortedEnumerable<int> source1 = new int[] { 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { 2, 2 }.AsSorted();
            var result = source1.IntersectWithDuplicates(source2);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Intersect of sequences should be their intersection");
        }

        [TestMethod]
        public void Intersect_WithLargeValuesEnding_IsIntersection()
        {
            ISortedEnumerable<int> source1 = new int[] { 2, 2 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { 1, 1, 1 }.AsSorted();
            var result = source1.IntersectWithDuplicates(source2);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Intersect of sequences should be their intersection");
        }

        [TestMethod]
        public void IntersectDistinct_WithDuplicateValues_IsDistinctIntersection()
        {
            ISortedEnumerable<int> source1 = new int[] { 1, 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { 1, 1, 2 }.AsSorted();
            var result = source1.Intersect(source2);
            Assert.IsTrue(result.SequenceEqual(new int[] { 1 }), "Intersect of sequences should be their intersection");
        }

        [TestMethod]
        public void IntersectDistinct_ViaSequencesExtension_IsDistinctIntersection()
        {
            ISortedEnumerable<int> source1 = new int[] { 1, 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { 1, 1, 2 }.AsSorted();
            var source = new[] { source1, source2 };
            var result = source.Intersect();
            Assert.IsTrue(result.SequenceEqual(new int[] { 1 }), "Intersect of sequences should be their intersection");
        }

        [TestMethod]
        public void Except_WithEmptySourceSequence_IsEmpty()
        {
            ISortedEnumerable<int> source1 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { 1 }.AsSorted();
            var result = source1.ExceptWithDuplicates(source2);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Except of sequences should be their difference");
        }

        [TestMethod]
        public void Except_WithEmptyOtherSequence_IsEmpty()
        {
            ISortedEnumerable<int> source1 = new int[] { 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            var result = source1.ExceptWithDuplicates(source2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1 }), "Except of sequences should be their difference");
        }

        [TestMethod]
        public void Except_WithOtherSequence_IsDifference()
        {
            ISortedEnumerable<int> source1 = new int[] { 1, 2, 3 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { 0, 2 }.AsSorted();
            var result = source1.ExceptWithDuplicates(source2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 3 }), "Except of sequences should be their difference");
        }

        [TestMethod]
        public void Except_WithDuplicateValues_IsDifference()
        {
            ISortedEnumerable<int> source1 = new int[] { 1, 2, 2, 2, 3 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { 2, 2 }.AsSorted();
            var result = source1.ExceptWithDuplicates(source2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Except of sequences should be their difference");
        }

        [TestMethod]
        public void ExceptDistinct_WithDuplicateValues_IsDistinctDifference()
        {
            ISortedEnumerable<int> source1 = new int[] { 1, 2, 2, 2, 3, 3 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { 2 }.AsSorted();
            var result = source1.Except(source2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 3 }), "Except of sequences should be their difference");
        }

        [TestMethod]
        public void Distinct_WithEmptySource_IsEmpty()
        {
            ISortedEnumerable<int> source = new int[] { }.AsSorted();
            var result = source.Distinct();
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Distinct should remove duplicates");
        }

        [TestMethod]
        public void Distinct_WithUniqueSourceValue_IsEqualToSource()
        {
            ISortedEnumerable<int> source = new[] { 1, 2, 3, 4 }.AsSorted();
            var result = source.Distinct();
            Assert.IsTrue(result.SequenceEqual(source), "Distinct should remove duplicates");
        }

        [TestMethod]
        public void Distinct_WithDuplicateSourceValues_RemovesDuplicates()
        {
            ISortedEnumerable<int> source = new[] { 1, 2, 2, 2, 3, 4, 4, 5 }.AsSorted();
            var result = source.Distinct();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4, 5 }), "Distinct should remove duplicates");
        }
    }
}

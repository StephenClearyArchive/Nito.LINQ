﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.Linq;
using System.Collections;

namespace UnitTests
{
    [TestClass]
    public class SortedEnumerableUnitTests
    {
        [TestMethod]
        public void Enumerable_IsSorted_EmptySequence_IsTrue()
        {
            IEnumerable<int> test = new int[] { };
            bool result = test.IsSorted();
            Assert.IsTrue(result, "Empty sequences should be sorted.");
        }

        [TestMethod]
        public void Enumerable_IsSorted_SingleElementSequence_IsTrue()
        {
            IEnumerable<int> test = new[] { 1 };
            bool result = test.IsSorted();
            Assert.IsTrue(result, "Single-element sequences should be sorted.");
        }

        [TestMethod]
        public void Enumerable_IsSorted_SortedSequence_IsTrue()
        {
            IEnumerable<int> test = new[] { 1, 1, 3 };
            bool result = test.IsSorted();
            Assert.IsTrue(result, "Sorted sequences should be sorted.");
        }

        [TestMethod]
        public void Enumerable_IsSorted_UnsortedSequence_IsFalse()
        {
            IEnumerable<int> test = new[] { 1, 1, 0 };
            bool result = test.IsSorted();
            Assert.IsFalse(result, "Unsorted sequences should not be sorted.");
        }

        [TestMethod]
        public void Enumerable_IsSorted_ReverseSortedSequence_IsTrue()
        {
            IEnumerable<int> test = new[] { 1, 1, 0 };
            bool result = test.IsSorted((x, y) => Comparer<int>.Default.Compare(y, x));
            Assert.IsTrue(result, "Reverse sorted sequences should be sorted (in reverse).");
        }

        [TestMethod]
        public void Enumerable_AsSortedEnumerable_ReturnsArgument()
        {
            ISortedEnumerable<int> test = new[] { 1, 2, 3, 4 }.AsSorted();
            var result = test.AsSortedEnumerable();
            Assert.AreSame(test, result, "AsSortedEnumerable should return its argument.");
        }

        [TestMethod]
        public void SortedEnumerable_Empty_IsEmpty()
        {
            ISortedEnumerable<string> sorted = SortedEnumerableSource.Empty<string>();
            Assert.IsTrue(sorted.SequenceEqual(new string[] { }), "Empty should be empty.");
        }

        [TestMethod]
        public void SortedEnumerable_Empty_RemembersComparer()
        {
            ISortedEnumerable<string> sorted = SortedEnumerableSource.Empty<string>(StringComparer.InvariantCultureIgnoreCase);
            Assert.AreEqual(StringComparer.InvariantCultureIgnoreCase, sorted.Comparer, "Empty should remember its comparison object.");
        }

        [TestMethod]
        public void SortedEnumerable_EmptyWithDelegate_IsEmpty()
        {
            ISortedEnumerable<string> sorted = SortedEnumerableSource.Empty<string>((x, y) => StringComparer.InvariantCultureIgnoreCase.Compare(y, x));
            Assert.IsTrue(sorted.SequenceEqual(new string[] { }), "Empty should be empty.");
        }

        [TestMethod]
        public void Enumerable_AsSorted_RemembersComparer()
        {
            IEnumerable<string> source = new[] { "a", "b", "c" };
            var sorted = source.AsSorted(StringComparer.InvariantCultureIgnoreCase);
            Assert.AreEqual(StringComparer.InvariantCultureIgnoreCase, sorted.Comparer, "AsSorted should remember its comparison object.");
        }

        [TestMethod]
        public void Enumerable_AsSorted_SupportsNongenericEnumeration()
        {
            IEnumerable<string> source = new[] { "a", "b", "c" };
            var sorted = source.AsSorted(StringComparer.InvariantCultureIgnoreCase);
            List<string> result = new List<string>();
            var iterator = ((IEnumerable)sorted).GetEnumerator();
            while (iterator.MoveNext())
            {
                result.Add((string)iterator.Current);
            }

            Assert.IsTrue(result.SequenceEqual(new[] { "a", "b", "c" }), "AsSorted should support non-generic enumeration.");
        }

        [TestMethod]
        public void Enumerable_AsSortedWithDelegate_EnumeratesIdenticalSequence()
        {
            IEnumerable<string> source = new[] { "a", "b", "c" };
            var result = source.AsSorted((x, y) => StringComparer.InvariantCultureIgnoreCase.Compare(x, y));
            Assert.IsTrue(result.SequenceEqual(new[] { "a", "b", "c" }), "AsSorted should not change the sequence.");
        }

        [TestMethod]
        public void SortedList_AsSorted_EnumeratesIdenticalSequence()
        {
            SortedList<int, string> source = new SortedList<int, string>();
            source.Add(0, "a");
            source.Add(1, "c");
            source.Add(2, "b");
            ISortedEnumerable<KeyValuePair<int, string>> result = source.AsSorted();
            Assert.IsTrue(result.SequenceEqual(new[] { new KeyValuePair<int, string>(0, "a"), new KeyValuePair<int, string>(1, "c"), new KeyValuePair<int, string>(2, "b") }), "AsSorted should not change the sequence.");
            Assert.AreEqual(0, result.IndexOf(new KeyValuePair<int, string>(0, "a")), "Could not find item in sorted list.");
        }

        [TestMethod]
        public void SortedDictionary_AsSorted_EnumeratesIdenticalSequence()
        {
            SortedDictionary<int, string> source = new SortedDictionary<int, string>();
            source.Add(0, "a");
            source.Add(1, "c");
            source.Add(2, "b");
            ISortedEnumerable<KeyValuePair<int, string>> result = source.AsSorted();
            Assert.IsTrue(result.SequenceEqual(new[] { new KeyValuePair<int, string>(0, "a"), new KeyValuePair<int, string>(1, "c"), new KeyValuePair<int, string>(2, "b") }), "AsSorted should not change the sequence.");
            Assert.AreEqual(0, result.IndexOf(new KeyValuePair<int, string>(0, "a")), "Could not find item in sorted dictionary.");
        }

        [TestMethod]
        public void SortedEnumerable_Return_EnumeratesSingleItem()
        {
            int source = 13;
            ISortedEnumerable<int> result = SortedEnumerableSource.Return(source);
            Assert.IsTrue(result.SequenceEqual(new[] { 13 }), "Item should be enumerated.");
        }

        [TestMethod]
        public void SortedEnumerable_ReturnWithComparer_EnumeratesSingleItem()
        {
            int source = 13;
            ISortedEnumerable<int> result = SortedEnumerableSource.Return(source, Comparer<int>.Default);
            Assert.IsTrue(result.SequenceEqual(new[] { 13 }), "Item should be enumerated.");
        }

        [TestMethod]
        public void SortedEnumerable_ReturnWithComparer_RemembersComparer()
        {
            var comparer = Comparer<int>.Default;
            int source = 13;
            ISortedEnumerable<int> result = SortedEnumerableSource.Return(source, Comparer<int>.Default);
            Assert.AreEqual(comparer, result.Comparer, "Comparer should be remembered.");
        }

        [TestMethod]
        public void SortedEnumerable_ReturnWithDelegate_EnumeratesSingleItem()
        {
            int source = 13;
            ISortedEnumerable<int> result = SortedEnumerableSource.Return(source, (x, y) => Comparer<int>.Default.Compare(y, x));
            Assert.IsTrue(result.SequenceEqual(new[] { 13 }), "Item should be enumerated.");
        }

        [TestMethod]
        public void SortedEnumerable_Repeat_EnumeratesRepeatedItem()
        {
            int source = 13;
            ISortedEnumerable<int> result = SortedEnumerableSource.Repeat(source, 3);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 13, 13 }), "Item should be repeated.");
        }

        [TestMethod]
        public void SortedEnumerable_RepeatWithComparer_EnumeratesRepeatedItem()
        {
            int source = 13;
            ISortedEnumerable<int> result = SortedEnumerableSource.Repeat(source, Comparer<int>.Default, 3);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 13, 13 }), "Item should be repeated.");
        }

        [TestMethod]
        public void SortedEnumerable_RepeatWithComparer_RemembersComparer()
        {
            var comparer = Comparer<int>.Default;
            int source = 13;
            ISortedEnumerable<int> result = SortedEnumerableSource.Repeat(source, comparer, 3);
            Assert.AreEqual(comparer, result.Comparer, "Comparer should be remembered.");
        }

        [TestMethod]
        public void SortedEnumerable_RepeatWithDelegate_EnumeratesRepeatedItem()
        {
            int source = 13;
            ISortedEnumerable<int> result = SortedEnumerableSource.Repeat(source, (x, y) => Comparer<int>.Default.Compare(y, x), 3);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 13, 13 }), "Item should be repeated.");
        }

        [TestMethod]
        public void SortedEnumerable_Repeat_NegativeTimes_EnumeratesEmptySequence()
        {
            int source = 13;
            var result = SortedEnumerableSource.Repeat(source, -1);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Item should not be repeated.");
        }

        [TestMethod]
        public void SortedEnumerable_Repeat_Infinitely_EnumeratesRepeatedItem()
        {
            int source = 13;
            var result = SortedEnumerableSource.Repeat(source).Take(3);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 13, 13 }), "Item should be repeated.");
        }

        [TestMethod]
        public void SortedEnumerable_RepeatWithComparer_Infinitely_EnumeratesRepeatedItem()
        {
            int source = 13;
            var result = SortedEnumerableSource.Repeat(source, Comparer<int>.Default).Take(3);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 13, 13 }), "Item should be repeated.");
        }

        [TestMethod]
        public void SortedEnumerable_RepeatWithComparer_Infinitely_RemembersComparer()
        {
            var comparer = Comparer<int>.Default;
            int source = 13;
            var result = SortedEnumerableSource.Repeat(source, Comparer<int>.Default).Take(3);
            Assert.AreEqual(comparer, result.Comparer, "Comparer should be remembered.");
        }

        [TestMethod]
        public void SortedEnumerable_RepeatWithDelegate_Infinitely_EnumeratesRepeatedItem()
        {
            int source = 13;
            var result = SortedEnumerableSource.Repeat(source, (x, y) => Comparer<int>.Default.Compare(y, x)).Take(3);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 13, 13 }), "Item should be repeated.");
        }

        [TestMethod]
        public void SortedEnumerable_LowerBound_ItemFound_ReturnsLowerBound()
        {
            IEnumerable<int> source = new[] { 1, 2, 2, 4 };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int result = sorted.LowerBound(2);
            Assert.AreEqual(1, result, "LowerBound should return the lower bound.");
        }

        [TestMethod]
        public void SortedEnumerable_LowerBound_ItemNotFound_ReturnsBitwiseComplement()
        {
            IEnumerable<int> source = new[] { 1, 2, 2, 4 };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int result = sorted.LowerBound(3);
            Assert.AreEqual(~3, result, "LowerBound should return the bitwise complement if not found.");
        }

        [TestMethod]
        public void SortedEnumerable_LowerBound_ItemNotFoundInEmptySequence_ReturnsBitwiseComplementOf0()
        {
            IEnumerable<int> source = new int[] { };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int result = sorted.LowerBound(3);
            Assert.AreEqual(~0, result, "LowerBound should return the bitwise complement if not found.");
        }

        [TestMethod]
        public void SortedEnumerable_LowerBound_ItemNotFoundPastSequence_ReturnsBitwiseComplement()
        {
            IEnumerable<int> source = new[] { 1, 2, 2, 4 };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int result = sorted.LowerBound(5);
            Assert.AreEqual(~4, result, "LowerBound should return the bitwise complement if not found.");
        }

        [TestMethod]
        public void SortedEnumerable_UpperBound_ItemFound_ReturnsUpperBound()
        {
            IEnumerable<int> source = new[] { 1, 2, 2, 4 };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int result = sorted.UpperBound(2);
            Assert.AreEqual(3, result, "UpperBound should return the upper bound.");
        }

        [TestMethod]
        public void SortedEnumerable_UpperBound_ItemNotFound_ReturnsBitwiseComplement()
        {
            IEnumerable<int> source = new[] { 1, 2, 2, 4 };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int result = sorted.UpperBound(3);
            Assert.AreEqual(~3, result, "UpperBound should return the bitwise complement if not found.");
        }

        [TestMethod]
        public void SortedEnumerable_UpperBound_ItemNotFoundInEmptySequence_ReturnsBitwiseComplementOf0()
        {
            IEnumerable<int> source = new int[] { };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int result = sorted.UpperBound(3);
            Assert.AreEqual(~0, result, "LowerBound should return the bitwise complement if not found.");
        }

        [TestMethod]
        public void SortedEnumerable_UpperBound_ItemNotFoundPastSequence_ReturnsBitwiseComplement()
        {
            IEnumerable<int> source = new[] { 1, 2, 2, 4 };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int result = sorted.UpperBound(5);
            Assert.AreEqual(~4, result, "LowerBound should return the bitwise complement if not found.");
        }

        [TestMethod]
        public void SortedEnumerable_EqualRange_RunOf0_AtBeginning()
        {
            IEnumerable<int> source = new[] { 1, 3 };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(0, out begin, out end);
            Assert.AreEqual(0, begin);
            Assert.AreEqual(0, end);
        }

        [TestMethod]
        public void SortedEnumerable_EqualRange_RunOf0_InMiddle()
        {
            IEnumerable<int> source = new[] { 1, 3 };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(2, out begin, out end);
            Assert.AreEqual(1, begin);
            Assert.AreEqual(1, end);
        }

        [TestMethod]
        public void SortedEnumerable_EqualRange_RunOf0_AtEnd()
        {
            IEnumerable<int> source = new[] { 1, 3 };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(4, out begin, out end);
            Assert.AreEqual(2, begin);
            Assert.AreEqual(2, end);
        }

        [TestMethod]
        public void SortedEnumerable_EqualRange_RunOf1_AtBeginning()
        {
            IEnumerable<int> source = new[] { 1, 3 };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(1, out begin, out end);
            Assert.AreEqual(0, begin);
            Assert.AreEqual(1, end);
        }

        [TestMethod]
        public void SortedEnumerable_EqualRange_RunOf1_InMiddle()
        {
            IEnumerable<int> source = new[] { 1, 3, 5, 7 };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(3, out begin, out end);
            Assert.AreEqual(1, begin);
            Assert.AreEqual(2, end);
        }

        [TestMethod]
        public void SortedEnumerable_EqualRange_RunOf1_InSecondMiddle()
        {
            IEnumerable<int> source = new[] { 1, 3, 5, 7 };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(5, out begin, out end);
            Assert.AreEqual(2, begin);
            Assert.AreEqual(3, end);
        }

        [TestMethod]
        public void SortedEnumerable_EqualRange_RunOf1_AtEnd()
        {
            IEnumerable<int> source = new[] { 1, 3 };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(3, out begin, out end);
            Assert.AreEqual(1, begin);
            Assert.AreEqual(2, end);
        }

        [TestMethod]
        public void SortedEnumerable_EqualRange_RunOf2_AtBeginning()
        {
            IEnumerable<int> source = new[] { 1, 1, 3, 4 };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(1, out begin, out end);
            Assert.AreEqual(0, begin);
            Assert.AreEqual(2, end);
        }

        [TestMethod]
        public void SortedEnumerable_EqualRange_RunOf2_AtMiddle()
        {
            IEnumerable<int> source = new[] { 1, 2, 3, 3, 4, 5 };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(3, out begin, out end);
            Assert.AreEqual(2, begin);
            Assert.AreEqual(4, end);
        }

        [TestMethod]
        public void SortedEnumerable_EqualRange_RunOf2_AtEnd()
        {
            IEnumerable<int> source = new[] { 1, 2, 3, 3 };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(3, out begin, out end);
            Assert.AreEqual(2, begin);
            Assert.AreEqual(4, end);
        }

        [TestMethod]
        public void SortedEnumerable_EqualRange_RunOf3_AtBeginning()
        {
            IEnumerable<int> source = new[] { 1, 1, 1, 4 };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(1, out begin, out end);
            Assert.AreEqual(0, begin);
            Assert.AreEqual(3, end);
        }

        [TestMethod]
        public void SortedEnumerable_EqualRange_RunOf3_AtMiddle()
        {
            IEnumerable<int> source = new[] { 0, 1, 1, 1, 4, 4 };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(1, out begin, out end);
            Assert.AreEqual(1, begin);
            Assert.AreEqual(4, end);
        }

        [TestMethod]
        public void SortedEnumerable_EqualRange_RunOf3_AtSecondMiddle()
        {
            IEnumerable<int> source = new[] { 0, 1, 4, 4, 4, 7 };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(4, out begin, out end);
            Assert.AreEqual(2, begin);
            Assert.AreEqual(5, end);
        }

        [TestMethod]
        public void SortedEnumerable_EqualRange_RunOf3_AtEnd()
        {
            IEnumerable<int> source = new[] { 0, 1, 1, 4, 4, 4 };
            ISortedEnumerable<int> sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(4, out begin, out end);
            Assert.AreEqual(3, begin);
            Assert.AreEqual(6, end);
        }

        [TestMethod]
        public void SortedEnumerable_Contains_WithValidItemValue_FindsItem()
        {
            IEnumerable<string> source = new[] { "a", "b", "c" };
            ISortedEnumerable<string> sorted = source.AsSorted();
            bool result = sorted.Contains("b");
            Assert.IsTrue(result, "Item should be found.");
        }

        [TestMethod]
        public void SortedEnumerable_Contains_WithInvalidItemValue_DoesNotFindItem()
        {
            IEnumerable<string> source = new[] { "a", "b", "c" };
            ISortedEnumerable<string> sorted = source.AsSorted();
            bool result = sorted.Contains("0");
            Assert.IsFalse(result, "Item should not be found.");
        }

        [TestMethod]
        public void SortedEnumerable_Contains_WithValidItem_FindsItem()
        {
            IEnumerable<string> source = new[] { "a", "b", "c" };
            ISortedEnumerable<string> sorted = source.AsSorted();
            bool result = sorted.Contains(x => StringComparer.InvariantCultureIgnoreCase.Compare("b", x));
            Assert.IsTrue(result, "Item should be found.");
        }

        [TestMethod]
        public void SortedEnumerable_Contains_WithInvalidItem_DoesNotFindItem()
        {
            IEnumerable<string> source = new[] { "a", "b", "c" };
            ISortedEnumerable<string> sorted = source.AsSorted();
            bool result = sorted.Contains(x => StringComparer.InvariantCultureIgnoreCase.Compare("0", x));
            Assert.IsFalse(result, "Item should not be found.");
        }

        [TestMethod]
        public void SortedEnumerable_IndexOf_WithValidItem_FindsItem()
        {
            IEnumerable<string> source = new[] { "a", "a", "c" };
            ISortedEnumerable<string> sorted = source.AsSorted();
            int result = sorted.IndexOf(x => StringComparer.InvariantCultureIgnoreCase.Compare("a", x));
            Assert.AreEqual(0, result, "Item should be found.");
        }

        [TestMethod]
        public void SortedEnumerable_IndexOf_WithInvalidItem_DoesNotFindItem()
        {
            IEnumerable<string> source = new[] { "a", "a", "c" };
            ISortedEnumerable<string> sorted = source.AsSorted();
            int result = sorted.IndexOf(x => StringComparer.InvariantCultureIgnoreCase.Compare("b", x));
            Assert.AreEqual(-1, result, "Item should not be found.");
        }

        [TestMethod]
        public void SortedEnumerable_IndexOf_WithValidItemValue_FindsItem()
        {
            IEnumerable<string> source = new[] { "a", "a", "c" };
            ISortedEnumerable<string> sorted = source.AsSorted();
            int result = sorted.IndexOf("a");
            Assert.AreEqual(0, result, "Item should be found.");
        }

        [TestMethod]
        public void SortedEnumerable_IndexOf_WithInvalidItemValue_DoesNotFindItem()
        {
            IEnumerable<string> source = new[] { "a", "a", "c" };
            ISortedEnumerable<string> sorted = source.AsSorted();
            int result = sorted.IndexOf("b");
            Assert.AreEqual(-1, result, "Item should not be found.");
        }

        [TestMethod]
        public void SortedEnumerable_LastIndexOf_WithValidItem_FindsItem()
        {
            IEnumerable<string> source = new[] { "a", "a", "c" };
            ISortedEnumerable<string> sorted = source.AsSorted();
            int result = sorted.LastIndexOf(x => StringComparer.InvariantCultureIgnoreCase.Compare("a", x));
            Assert.AreEqual(1, result, "Item should be found.");
        }

        [TestMethod]
        public void SortedEnumerable_LastIndexOf_WithInvalidItem_DoesNotFindItem()
        {
            IEnumerable<string> source = new[] { "a", "a", "c" };
            ISortedEnumerable<string> sorted = source.AsSorted();
            int result = sorted.LastIndexOf(x => StringComparer.InvariantCultureIgnoreCase.Compare("d", x));
            Assert.AreEqual(-1, result, "Item should not be found.");
        }

        [TestMethod]
        public void SortedEnumerable_LastIndexOf_WithValidItemValue_FindsItem()
        {
            IEnumerable<string> source = new[] { "a", "a", "c" };
            ISortedEnumerable<string> sorted = source.AsSorted();
            int result = sorted.LastIndexOf("a");
            Assert.AreEqual(1, result, "Item should be found.");
        }

        [TestMethod]
        public void SortedEnumerable_LastIndexOf_WithInvalidItemValue_DoesNotFindItem()
        {
            IEnumerable<string> source = new[] { "a", "a", "c" };
            ISortedEnumerable<string> sorted = source.AsSorted();
            int result = sorted.LastIndexOf("A");
            Assert.AreEqual(-1, result, "Item should not be found.");
        }

        [TestMethod]
        public void SortedEnumerable_Skip_EnumeratesItems()
        {
            ISortedEnumerable<int> test = new[] { 1, 2, 3, 4 }.AsEnumerable().AsSorted();
            var result = test.Skip(1);
            Assert.IsTrue(result.SequenceEqual(new[] { 2, 3, 4 }), "Skip should skip the requested items.");
        }

        [TestMethod]
        public void SortedEnumerable_Step_By2_EnumeratesItems()
        {
            ISortedEnumerable<int> source = new List<int> { 1, 2, 3, 4, 5, 6 }.AsEnumerable().AsSorted();
            var result = source.Step(2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 3, 5 }), "Stepping should enumerate the requested items.");
        }

        [TestMethod]
        public void SortedEnumerable_Take_EnumeratesItems()
        {
            ISortedEnumerable<int> test = new[] { 1, 2, 3, 4 }.AsEnumerable().AsSorted();
            var result = test.Take(2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2 }), "Take should take only the requested items.");
        }

        [TestMethod]
        public void SortedEnumerable_MergeSorted_OnEmptySequences_IsEmptySequence()
        {
            ISortedEnumerable<int> source1 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            var result = source1.MergeSorted(source2);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Merging of empty sequences should be an empty sequence.");
        }

        [TestMethod]
        public void SortedEnumerable_MergeSortedViaParams_OnEmptySequences_IsEmptySequence()
        {
            ISortedEnumerable<int> source1 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            var result = SortedEnumerableExtensions.MergeSorted(source1, source2);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Merging of empty sequences should be an empty sequence.");
        }

        [TestMethod]
        public void SortedEnumerable_MergeSortedViaSequencesExtension_OnEmptySequences_IsEmptySequence()
        {
            ISortedEnumerable<int> source1 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            IEnumerable<ISortedEnumerable<int>> sources = new[] { source1, source2 };
            var result = sources.MergeSorted();
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Merging of empty sequences should be an empty sequence.");
        }

        [TestMethod]
        public void SortedEnumerable_MergeSorted_OnNonEmptySequences_IsMerge()
        {
            ISortedEnumerable<int> source1 = new int[] { 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source3 = new int[] { 1, 3 }.AsSorted();
            ISortedEnumerable<int> source4 = new int[] { 0, 1, 2 }.AsSorted();
            var result = source1.MergeSorted(source2, source3, source4);
            Assert.IsTrue(result.SequenceEqual(new int[] { 0, 1, 1, 1, 2, 3 }), "Merging of sequences should be their merging.");
        }

        [TestMethod]
        public void SortedEnumerable_MergeSortedViaValues_IsMerge()
        {
            ISortedEnumerable<int> source1 = new int[] { 1 }.AsSorted();
            var result = source1.MergeSorted(1, 3, 0, 1, 2);
            Assert.IsTrue(result.SequenceEqual(new int[] { 0, 1, 1, 1, 2, 3 }), "Merging of values should be their merging.");
        }

        [TestMethod]
        public void SortedEnumerable_MergeSorted_WithDuplicates_IsMerge()
        {
            ISortedEnumerable<int> source1 = new[] { 1, 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new[] { 1, 2 }.AsSorted();
            var result = source1.MergeSorted(source2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 1, 1, 2 }), "Merging should keep duplicates.");
        }
        
        [TestMethod]
        public void SortedEnumerable_UnionWithDuplicates_OnEmptySequences_IsEmptySequence()
        {
            ISortedEnumerable<int> source1 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            var result = source1.UnionWithDuplicates(source2);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Union of empty sequences should be an empty sequence.");
        }

        [TestMethod]
        public void SortedEnumerable_UnionWithDuplicatesViaParams_OnEmptySequences_IsEmptySequence()
        {
            ISortedEnumerable<int> source1 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            var result = SortedEnumerableExtensions.UnionWithDuplicates(source1, source2);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Union of empty sequences should be an empty sequence.");
        }

        [TestMethod]
        public void SortedEnumerable_UnionWithDuplicatesViaSequencesExtension_OnEmptySequences_IsEmptySequence()
        {
            ISortedEnumerable<int> source1 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            IEnumerable<ISortedEnumerable<int>> sources = new[] { source1, source2 };
            var result = sources.UnionWithDuplicates();
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Union of empty sequences should be an empty sequence.");
        }

        [TestMethod]
        public void SortedEnumerable_UnionWithDuplicates_OnNonEmptySequences_IsUnion()
        {
            ISortedEnumerable<int> source1 = new int[] { 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source3 = new int[] { 1, 3 }.AsSorted();
            ISortedEnumerable<int> source4 = new int[] { 0, 1, 2 }.AsSorted();
            var result = source1.UnionWithDuplicates(source2, source3, source4);
            Assert.IsTrue(result.SequenceEqual(new int[] { 0, 1, 2, 3 }), "Union of sequences should be their union.");
        }

        [TestMethod]
        public void SortedEnumerable_UnionWithDuplicatesViaValues_IsUnion()
        {
            ISortedEnumerable<int> source1 = new int[] { 1 }.AsSorted();
            var result = source1.UnionWithDuplicates(1, 3, 0, 1, 2);
            Assert.IsTrue(result.SequenceEqual(new int[] { 0, 1, 2, 3 }), "Union of values should be their union.");
        }

        [TestMethod]
        public void SortedEnumerable_UnionWithDuplicates_WithDuplicates_IsUnion()
        {
            ISortedEnumerable<int> source1 = new[] { 1, 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new[] { 1, 2 }.AsSorted();
            var result = source1.UnionWithDuplicates(source2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 1, 2 }), "Union should keep duplicates.");
        }

        [TestMethod]
        public void SortedEnumerable_Union_WithDuplicates_IsDistinctUnion()
        {
            ISortedEnumerable<int> source1 = new[] { 1, 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new[] { 1, 2 }.AsSorted();
            var result = source1.Union(source2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2 }), "Union should drop duplicates.");
        }

        [TestMethod]
        public void SortedEnumerable_Union_ViaSequencesExtension_IsDistinctUnion()
        {
            ISortedEnumerable<int> source1 = new[] { 1, 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new[] { 1, 2 }.AsSorted();
            IEnumerable<ISortedEnumerable<int>> source = new[] { source1, source2 };
            var result = source.Union();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2 }), "Union should drop duplicates.");
        }

        [TestMethod]
        public void SortedEnumerable_Union_ViaValues_IsDistinctUnion()
        {
            ISortedEnumerable<int> source1 = new[] { 1, 1 }.AsSorted();
            var result = source1.Union(1, 2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2 }), "Union should drop duplicates.");
        }

        [TestMethod]
        public void SortedEnumerable_IntersectWithDuplicates_OnEmptySequences_IsEmptySequence()
        {
            ISortedEnumerable<int> source1 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            var result = source1.IntersectWithDuplicates(source2);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Intersect of empty sequences should be an empty sequence.");
        }

        [TestMethod]
        public void SortedEnumerable_IntersectWithDuplicatesViaParams_OnEmptySequences_IsEmptySequence()
        {
            ISortedEnumerable<int> source1 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            var result = SortedEnumerableExtensions.IntersectWithDuplicates(source1, source2);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Intersect of empty sequences should be an empty sequence.");
        }

        [TestMethod]
        public void SortedEnumerable_IntersectWithDuplicatesViaSequencesExtension_OnEmptySequences_IsEmptySequence()
        {
            ISortedEnumerable<int> source1 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            IEnumerable<ISortedEnumerable<int>> sources = new[] { source1, source2 };
            var result = sources.IntersectWithDuplicates();
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Intersect of empty sequences should be an empty sequence.");
        }

        [TestMethod]
        public void SortedEnumerable_IntersectWithDuplicates_WithDuplicateValues_PreservesDuplicates()
        {
            ISortedEnumerable<int> source1 = new int[] { 1, 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { 0, 1, 1 }.AsSorted();
            ISortedEnumerable<int> source3 = new int[] { 1, 1, 3 }.AsSorted();
            ISortedEnumerable<int> source4 = new int[] { 0, 1, 1, 2 }.AsSorted();
            var result = source1.IntersectWithDuplicates(source2, source3, source4);
            Assert.IsTrue(result.SequenceEqual(new int[] { 1, 1 }), "Intersect of sequences should be their intersection.");
        }

        [TestMethod]
        public void SortedEnumerable_IntersectWithDuplicates_WithSmallValuesEnding_IsIntersection()
        {
            ISortedEnumerable<int> source1 = new int[] { 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { 2, 2 }.AsSorted();
            var result = source1.IntersectWithDuplicates(source2);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Intersect of sequences should be their intersection.");
        }

        [TestMethod]
        public void SortedEnumerable_IntersectWithDuplicates_WithLargeValuesEnding_IsIntersection()
        {
            ISortedEnumerable<int> source1 = new int[] { 2, 2 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { 1, 1, 1 }.AsSorted();
            var result = source1.IntersectWithDuplicates(source2);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Intersect of sequences should be their intersection.");
        }

        [TestMethod]
        public void SortedEnumerable_Intersect_WithDuplicateValues_IsDistinctIntersection()
        {
            ISortedEnumerable<int> source1 = new int[] { 1, 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { 1, 1, 2 }.AsSorted();
            var result = source1.Intersect(source2);
            Assert.IsTrue(result.SequenceEqual(new int[] { 1 }), "Intersect of sequences should be their intersection.");
        }

        [TestMethod]
        public void SortedEnumerable_Intersect_ViaSequencesExtension_IsDistinctIntersection()
        {
            ISortedEnumerable<int> source1 = new int[] { 1, 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { 1, 1, 2 }.AsSorted();
            IEnumerable<ISortedEnumerable<int>> source = new[] { source1, source2 };
            var result = source.Intersect();
            Assert.IsTrue(result.SequenceEqual(new int[] { 1 }), "Intersect of sequences should be their intersection.");
        }

        [TestMethod]
        public void SortedEnumerable_ExceptWithDuplicates_WithEmptySourceSequence_IsEmpty()
        {
            ISortedEnumerable<int> source1 = new int[] { }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { 1 }.AsSorted();
            var result = source1.ExceptWithDuplicates(source2);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Except of sequences should be their difference.");
        }

        [TestMethod]
        public void SortedEnumerable_ExceptWithDuplicates_WithEmptyOtherSequence_IsEmpty()
        {
            ISortedEnumerable<int> source1 = new int[] { 1 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { }.AsSorted();
            var result = source1.ExceptWithDuplicates(source2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1 }), "Except of sequences should be their difference.");
        }

        [TestMethod]
        public void SortedEnumerable_ExceptWithDuplicates_WithOtherSequence_IsDifference()
        {
            ISortedEnumerable<int> source1 = new int[] { 1, 2, 3 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { 0, 2 }.AsSorted();
            var result = source1.ExceptWithDuplicates(source2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 3 }), "Except of sequences should be their difference.");
        }

        [TestMethod]
        public void SortedEnumerable_ExceptWithDuplicates_WithDuplicateValues_IsDifference()
        {
            ISortedEnumerable<int> source1 = new int[] { 1, 2, 2, 2, 3 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { 2, 2 }.AsSorted();
            var result = source1.ExceptWithDuplicates(source2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Except of sequences should be their difference.");
        }

        [TestMethod]
        public void SortedEnumerable_Except_WithDuplicateValues_IsDistinctDifference()
        {
            ISortedEnumerable<int> source1 = new int[] { 1, 2, 2, 2, 3, 3 }.AsSorted();
            ISortedEnumerable<int> source2 = new int[] { 2 }.AsSorted();
            var result = source1.Except(source2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 3 }), "Except of sequences should be their difference.");
        }

        [TestMethod]
        public void SortedEnumerable_Distinct_WithEmptySource_IsEmpty()
        {
            ISortedEnumerable<int> source = new int[] { }.AsSorted();
            var result = source.Distinct();
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Distinct should remove duplicates.");
        }

        [TestMethod]
        public void SortedEnumerable_Distinct_WithUniqueSourceValue_IsEqualToSource()
        {
            ISortedEnumerable<int> source = new[] { 1, 2, 3, 4 }.AsSorted();
            var result = source.Distinct();
            Assert.IsTrue(result.SequenceEqual(source), "Distinct should remove duplicates.");
        }

        [TestMethod]
        public void SortedEnumerable_Distinct_WithDuplicateSourceValues_RemovesDuplicates()
        {
            ISortedEnumerable<int> source = new[] { 1, 2, 2, 2, 3, 4, 4, 5 }.AsSorted();
            var result = source.Distinct();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4, 5 }), "Distinct should remove duplicates.");
        }
    }
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito;

namespace UnitTests
{
    [TestClass]
    public class SortedListExtensionsUnitTests
    {
        [TestMethod]
        public void ToSortedList_RemembersComparer()
        {
            var source = new[] { "a", "b", "c" }.AsEnumerable().AsSorted(StringComparer.InvariantCultureIgnoreCase);
            var result = source.ToSortedList();
            Assert.AreEqual(StringComparer.InvariantCultureIgnoreCase, result.Comparer, "ToSortedList should remember its comparison object");
        }

        [TestMethod]
        public void Empty_RemembersComparer()
        {
            var result = SortedListExtensions.Empty<string>(StringComparer.InvariantCultureIgnoreCase);
            Assert.AreEqual(StringComparer.InvariantCultureIgnoreCase, result.Comparer, "Empty should remember its comparison object");
        }

        [TestMethod]
        public void Empty_IsEmpty()
        {
            var result = SortedListExtensions.Empty<string>();
            Assert.IsTrue(result.SequenceEqual(new string[] { }), "Empty should be empty");
        }

        [TestMethod]
        public void AsSortedList_ReturnsArgument()
        {
            var test = new[] { 1, 2, 3, 4 }.AsSorted();
            var result = test.AsSortedList();
            Assert.AreSame(test, result, "AsSortedList should return its argument");
        }

        [TestMethod]
        public void AsSorted_RemembersComparer()
        {
            var source = new[] { "a", "b", "c" };
            var sorted = source.AsSorted(StringComparer.InvariantCultureIgnoreCase);
            Assert.AreEqual(StringComparer.InvariantCultureIgnoreCase, sorted.Comparer, "AsSorted should remember its comparison object");
        }

        [TestMethod]
        public void AsSorted_OverArray_IsReadOnly()
        {
            var source = new[] { 1, 2, 3 };
            var sorted = source.AsSorted();
            Assert.IsTrue(sorted.IsReadOnly, "AsSorted over an array should be read-only");
        }

        [TestMethod]
        public void AsSorted_OverList_IsNotReadOnly()
        {
            var source = new List<int> { 1, 2, 3 };
            var sorted = source.AsSorted();
            Assert.IsFalse(sorted.IsReadOnly, "AsSorted over an array should be read-only");
        }

        [TestMethod]
        public void AsSorted_Clear_ClearsSource()
        {
            var source = new List<int> { 1, 2, 3 };
            var sorted = source.AsSorted();
            sorted.Clear();
            Assert.IsTrue(source.SequenceEqual(new int[] {}), "Sorted Clear should clear source");
        }

        [TestMethod]
        public void AsSorted_SetItem_UpdatesSource()
        {
            var source = new List<int> { 1, 2, 4 };
            var sorted = source.AsSorted();
            sorted[1] = 3;
            Assert.IsTrue(source.SequenceEqual(new int[] { 1, 3, 4 }), "Sorted updates should update source");
        }

        [TestMethod]
        public void AsSorted_Insert_UpdatesSource()
        {
            var source = new List<int> { 1, 2, 4 };
            var sorted = source.AsSorted();
            sorted.Insert(1, 2);
            Assert.IsTrue(source.SequenceEqual(new int[] { 1, 2, 2, 4 }), "Sorted inserts should update source");
        }

        [TestMethod]
        public void AsSorted_Remove_UpdatesSource()
        {
            var source = new List<int> { 1, 2, 4 };
            var sorted = source.AsSorted();
            sorted.RemoveAt(1);
            Assert.IsTrue(source.SequenceEqual(new int[] { 1, 4 }), "Sorted removes should update source");
        }

        [TestMethod]
        public void AsSortedIndexOf_WithValidItem_FindsItem()
        {
            var source = new List<int> { 13 };
            var sorted = source.AsSorted();
            int result = sorted.IndexOf(13);
            Assert.AreEqual(0, result, "Item should be found.");
        }

        [TestMethod]
        public void AsSortedIndexOf_WithInvalidItem_DoesNotFindItem()
        {
            IList<int> source = new List<int> { 13 };
            var sorted = source.AsSorted();
            int result = sorted.IndexOf(17);
            Assert.AreEqual(-1, result, "Item should not be found.");
        }

        [TestMethod]
        public void AsSortedContains_WithValidItem_FindsItem()
        {
            var source = new List<int> { 13 };
            var sorted = source.AsSorted();
            bool result = sorted.Contains(13);
            Assert.IsTrue(result, "Item should be found.");
        }

        [TestMethod]
        public void AsSortedContains_WithInvalidItem_DoesNotFindItem()
        {
            IList<int> source = new List<int> { 13 };
            var sorted = source.AsSorted();
            bool result = sorted.Contains(17);
            Assert.IsFalse(result, "Item should not be found.");
        }

        [TestMethod]
        public void Contains_WithValidItem_FindsItem()
        {
            var source = new[] { "a", "b", "c" };
            var sorted = source.AsSorted();
            bool result = sorted.Contains(x => StringComparer.InvariantCultureIgnoreCase.Compare("b", x));
            Assert.IsTrue(result, "Item should be found.");
        }

        [TestMethod]
        public void Contains_WithInvalidItem_DoesNotFindItem()
        {
            var source = new[] { "a", "b", "c" };
            var sorted = source.AsSorted();
            bool result = sorted.Contains(x => StringComparer.InvariantCultureIgnoreCase.Compare("0", x));
            Assert.IsFalse(result, "Item should not be found.");
        }

        [TestMethod]
        public void IndexOf_WithValidItem_FindsItem()
        {
            var source = new[] { "a", "a", "c" };
            var sorted = source.AsSorted();
            int result = sorted.IndexOf(x => StringComparer.InvariantCultureIgnoreCase.Compare("a", x));
            Assert.AreEqual(0, result, "Item should be found.");
        }

        [TestMethod]
        public void IndexOf_WithInvalidItem_DoesNotFindItem()
        {
            var source = new[] { "a", "a", "c" };
            var sorted = source.AsSorted();
            int result = sorted.IndexOf(x => StringComparer.InvariantCultureIgnoreCase.Compare("b", x));
            Assert.AreEqual(-1, result, "Item should not be found.");
        }

        [TestMethod]
        public void LastIndexOf_WithValidItem_FindsItem()
        {
            var source = new[] { "a", "a", "c" };
            var sorted = source.AsSorted();
            int result = sorted.LastIndexOf(x => StringComparer.InvariantCultureIgnoreCase.Compare("a", x));
            Assert.AreEqual(1, result, "Item should be found.");
        }

        [TestMethod]
        public void LastIndexOf_WithInvalidItem_DoesNotFindItem()
        {
            var source = new[] { "a", "a", "c" };
            var sorted = source.AsSorted();
            int result = sorted.LastIndexOf(x => StringComparer.InvariantCultureIgnoreCase.Compare("d", x));
            Assert.AreEqual(-1, result, "Item should not be found.");
        }

        [TestMethod]
        public void LastIndexOf_WithValidItemValue_FindsItem()
        {
            var source = new[] { "a", "a", "c" };
            var sorted = source.AsSorted();
            int result = sorted.LastIndexOf("a");
            Assert.AreEqual(1, result, "Item should be found.");
        }

        [TestMethod]
        public void LastIndexOf_WithInvalidItemValue_DoesNotFindItem()
        {
            var source = new[] { "a", "a", "c" };
            var sorted = source.AsSorted();
            int result = sorted.LastIndexOf("A");
            Assert.AreEqual(-1, result, "Item should not be found.");
        }

        [TestMethod]
        public void BinarySearchOn3Items_ItemAt0_FindsItem()
        {
            var source = new[] { 1, 3, 5 };
            var sorted = source.AsSorted();
            int result = sorted.BinarySearch(1);
            Assert.AreEqual(0, result, "BinarySearch should find first item in 3-item list");
        }

        [TestMethod]
        public void BinarySearchOn3Items_ItemAt1_FindsItem()
        {
            var source = new[] { 1, 3, 5 };
            var sorted = source.AsSorted();
            int result = sorted.BinarySearch(3);
            Assert.AreEqual(1, result, "BinarySearch should find second item in 3-item list");
        }

        [TestMethod]
        public void BinarySearchOn3Items_ItemAt2_FindsItem()
        {
            var source = new[] { 1, 3, 5 };
            var sorted = source.AsSorted();
            int result = sorted.BinarySearch(5);
            Assert.AreEqual(2, result, "BinarySearch should find third item in 3-item list");
        }

        [TestMethod]
        public void BinarySearchOn3Items_ItemBefore0_FindsIndex()
        {
            var source = new[] { 1, 3, 5 };
            var sorted = source.AsSorted();
            int result = sorted.BinarySearch(0);
            Assert.AreEqual(~0, result, "BinarySearch should return index before 0 in 3-item list");
        }

        [TestMethod]
        public void BinarySearchOn3Items_ItemBefore1_FindsIndex()
        {
            var source = new[] { 1, 3, 5 };
            var sorted = source.AsSorted();
            int result = sorted.BinarySearch(2);
            Assert.AreEqual(~1, result, "BinarySearch should return index before 1 in 3-item list");
        }

        [TestMethod]
        public void BinarySearchOn3Items_ItemBefore2_FindsIndex()
        {
            var source = new[] { 1, 3, 5 };
            var sorted = source.AsSorted();
            int result = sorted.BinarySearch(4);
            Assert.AreEqual(~2, result, "BinarySearch should return index before 2 in 3-item list");
        }

        [TestMethod]
        public void BinarySearchOn3Items_ItemBefore3_FindsIndex()
        {
            var source = new[] { 1, 3, 5 };
            var sorted = source.AsSorted();
            int result = sorted.BinarySearch(6);
            Assert.AreEqual(~3, result, "BinarySearch should return index before 3 in 3-item list");
        }

        [TestMethod]
        public void BinarySearchOn4Items_ItemAt0_FindsItem()
        {
            var source = new[] { 2, 4, 6, 8 };
            var sorted = source.AsSorted();
            int result = sorted.BinarySearch(2);
            Assert.AreEqual(0, result, "BinarySearch should find first item in a 4-item list");
        }

        [TestMethod]
        public void BinarySearchOn4Items_ItemAt1_FindsItem()
        {
            var source = new[] { 2, 4, 6, 8 };
            var sorted = source.AsSorted();
            int result = sorted.BinarySearch(4);
            Assert.AreEqual(1, result, "BinarySearch should find second item in a 4-item list");
        }

        [TestMethod]
        public void BinarySearchOn4Items_ItemAt2_FindsItem()
        {
            var source = new[] { 2, 4, 6, 8 };
            var sorted = source.AsSorted();
            int result = sorted.BinarySearch(6);
            Assert.AreEqual(2, result, "BinarySearch should find third item in a 4-item list");
        }

        [TestMethod]
        public void BinarySearchOn4Items_ItemAt3_FindsItem()
        {
            var source = new[] { 2, 4, 6, 8 };
            var sorted = source.AsSorted();
            int result = sorted.BinarySearch(8);
            Assert.AreEqual(3, result, "BinarySearch should find fourth item in a 4-item list");
        }

        [TestMethod]
        public void BinarySearchOn4Items_ItemBefore0_FindsIndex()
        {
            var source = new[] { 2, 4, 6, 8 };
            var sorted = source.AsSorted();
            int result = sorted.BinarySearch(1);
            Assert.AreEqual(~0, result, "BinarySearch should return index before 0 in 4-item list");
        }

        [TestMethod]
        public void BinarySearchOn4Items_ItemBefore1_FindsIndex()
        {
            var source = new[] { 2, 4, 6, 8 };
            var sorted = source.AsSorted();
            int result = sorted.BinarySearch(3);
            Assert.AreEqual(~1, result, "BinarySearch should return index before 1 in 4-item list");
        }

        [TestMethod]
        public void BinarySearchOn4Items_ItemBefore2_FindsIndex()
        {
            var source = new[] { 2, 4, 6, 8 };
            var sorted = source.AsSorted();
            int result = sorted.BinarySearch(5);
            Assert.AreEqual(~2, result, "BinarySearch should return index before 2 in 4-item list");
        }

        [TestMethod]
        public void BinarySearchOn4Items_ItemBefore3_FindsIndex()
        {
            var source = new[] { 2, 4, 6, 8 };
            var sorted = source.AsSorted();
            int result = sorted.BinarySearch(7);
            Assert.AreEqual(~3, result, "BinarySearch should return index before 3 in 4-item list");
        }

        [TestMethod]
        public void BinarySearchOn4Items_ItemBefore4_FindsIndex()
        {
            var source = new[] { 2, 4, 6, 8 };
            var sorted = source.AsSorted();
            int result = sorted.BinarySearch(9);
            Assert.AreEqual(~4, result, "BinarySearch should return index before 4 in 4-item list");
        }

        [TestMethod]
        public void EqualRange_RunOf0_AtBeginning_EvenCount()
        {
            var source = new[] { 1, 3 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(0, out begin, out end);
            Assert.AreEqual(0, begin);
            Assert.AreEqual(0, end);
        }

        [TestMethod]
        public void EqualRange_RunOf0_AtBeginning_OddCount()
        {
            var source = new[] { 1, 3, 5 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(0, out begin, out end);
            Assert.AreEqual(0, begin);
            Assert.AreEqual(0, end);
        }

        [TestMethod]
        public void EqualRange_RunOf0_InMiddle_EvenCount()
        {
            var source = new[] { 1, 3 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(2, out begin, out end);
            Assert.AreEqual(1, begin);
            Assert.AreEqual(1, end);
        }

        [TestMethod]
        public void EqualRange_RunOf0_InMiddle_OddCount()
        {
            var source = new[] { 1, 3, 5 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(2, out begin, out end);
            Assert.AreEqual(1, begin);
            Assert.AreEqual(1, end);
        }

        [TestMethod]
        public void EqualRange_RunOf0_InSecondMiddle_OddCount()
        {
            var source = new[] { 1, 3, 5 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(4, out begin, out end);
            Assert.AreEqual(2, begin);
            Assert.AreEqual(2, end);
        }

        [TestMethod]
        public void EqualRange_RunOf0_AtEnd_EvenCount()
        {
            var source = new[] { 1, 3 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(4, out begin, out end);
            Assert.AreEqual(2, begin);
            Assert.AreEqual(2, end);
        }

        [TestMethod]
        public void EqualRange_RunOf0_AtEnd_OddCount()
        {
            var source = new[] { 1, 3, 5 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(6, out begin, out end);
            Assert.AreEqual(3, begin);
            Assert.AreEqual(3, end);
        }

        [TestMethod]
        public void EqualRange_RunOf1_AtBeginning_EvenCount()
        {
            var source = new[] { 1, 3 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(1, out begin, out end);
            Assert.AreEqual(0, begin);
            Assert.AreEqual(1, end);
        }

        [TestMethod]
        public void EqualRange_RunOf1_InMiddle_EvenCount()
        {
            var source = new[] { 1, 3, 5, 7 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(3, out begin, out end);
            Assert.AreEqual(1, begin);
            Assert.AreEqual(2, end);
        }

        [TestMethod]
        public void EqualRange_RunOf1_InSecondMiddle_EvenCount()
        {
            var source = new[] { 1, 3, 5, 7 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(5, out begin, out end);
            Assert.AreEqual(2, begin);
            Assert.AreEqual(3, end);
        }

        [TestMethod]
        public void EqualRange_RunOf1_AtEnd_EvenCount()
        {
            var source = new[] { 1, 3 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(3, out begin, out end);
            Assert.AreEqual(1, begin);
            Assert.AreEqual(2, end);
        }

        [TestMethod]
        public void EqualRange_RunOf1_AtBeginning_OddCount()
        {
            var source = new[] { 1, 3, 5 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(1, out begin, out end);
            Assert.AreEqual(0, begin);
            Assert.AreEqual(1, end);
        }

        [TestMethod]
        public void EqualRange_RunOf1_InMiddle_OddCount()
        {
            var source = new[] { 1, 3, 5 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(3, out begin, out end);
            Assert.AreEqual(1, begin);
            Assert.AreEqual(2, end);
        }

        [TestMethod]
        public void EqualRange_RunOf1_AtEnd_OddCount()
        {
            var source = new[] { 1, 3, 5 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(5, out begin, out end);
            Assert.AreEqual(2, begin);
            Assert.AreEqual(3, end);
        }

        [TestMethod]
        public void EqualRange_RunOf2_AtBeginning_EvenCount()
        {
            var source = new[] { 1, 1, 3, 4 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(1, out begin, out end);
            Assert.AreEqual(0, begin);
            Assert.AreEqual(2, end);
        }

        [TestMethod]
        public void EqualRange_RunOf2_AtMiddle_EvenCount()
        {
            var source = new[] { 1, 2, 3, 3, 4, 5 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(3, out begin, out end);
            Assert.AreEqual(2, begin);
            Assert.AreEqual(4, end);
        }

        [TestMethod]
        public void EqualRange_RunOf2_AtEnd_EvenCount()
        {
            var source = new[] { 1, 2, 3, 3 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(3, out begin, out end);
            Assert.AreEqual(2, begin);
            Assert.AreEqual(4, end);
        }

        [TestMethod]
        public void EqualRange_RunOf2_AtBeginning_OddCount()
        {
            var source = new[] { 1, 1, 3, 3, 4, 5, 7 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(1, out begin, out end);
            Assert.AreEqual(0, begin);
            Assert.AreEqual(2, end);
        }

        [TestMethod]
        public void EqualRange_RunOf2_AtMiddle_OddCount()
        {
            var source = new[] { 1, 1, 3, 3, 4, 5, 7 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(3, out begin, out end);
            Assert.AreEqual(2, begin);
            Assert.AreEqual(4, end);
        }

        [TestMethod]
        public void EqualRange_RunOf2_AtSecondMiddle_OddCount()
        {
            var source = new[] { 1, 1, 3, 4, 4, 5, 7 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(4, out begin, out end);
            Assert.AreEqual(3, begin);
            Assert.AreEqual(5, end);
        }

        [TestMethod]
        public void EqualRange_RunOf2_AtThirdMiddle_OddCount()
        {
            var source = new[] { 1, 1, 3, 4, 5, 5, 7 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(5, out begin, out end);
            Assert.AreEqual(4, begin);
            Assert.AreEqual(6, end);
        }

        [TestMethod]
        public void EqualRange_RunOf2_AtEnd_OddCount()
        {
            var source = new[] { 1, 1, 3, 4, 5, 7, 7 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(7, out begin, out end);
            Assert.AreEqual(5, begin);
            Assert.AreEqual(7, end);
        }

        [TestMethod]
        public void EqualRange_RunOf3_AtBeginning_EvenCount()
        {
            var source = new[] { 1, 1, 1, 4 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(1, out begin, out end);
            Assert.AreEqual(0, begin);
            Assert.AreEqual(3, end);
        }

        [TestMethod]
        public void EqualRange_RunOf3_AtMiddle_EvenCount()
        {
            var source = new[] { 0, 1, 1, 1, 4, 4 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(1, out begin, out end);
            Assert.AreEqual(1, begin);
            Assert.AreEqual(4, end);
        }

        [TestMethod]
        public void EqualRange_RunOf3_AtSecondMiddle_EvenCount()
        {
            var source = new[] { 0, 1, 4, 4, 4, 7 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(4, out begin, out end);
            Assert.AreEqual(2, begin);
            Assert.AreEqual(5, end);
        }

        [TestMethod]
        public void EqualRange_RunOf3_AtEnd_EvenCount()
        {
            var source = new[] { 0, 1, 1, 4, 4, 4 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(4, out begin, out end);
            Assert.AreEqual(3, begin);
            Assert.AreEqual(6, end);
        }

        [TestMethod]
        public void EqualRange_RunOf3_AtBeginning_OddCount()
        {
            var source = new[] { 0, 0, 0, 1, 4 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(0, out begin, out end);
            Assert.AreEqual(0, begin);
            Assert.AreEqual(3, end);
        }

        [TestMethod]
        public void EqualRange_RunOf3_InMiddle_OddCount()
        {
            var source = new[] { -1, 0, 0, 0, 1 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(0, out begin, out end);
            Assert.AreEqual(1, begin);
            Assert.AreEqual(4, end);
        }

        [TestMethod]
        public void EqualRange_RunOf3_AtEnd_OddCount()
        {
            var source = new[] { -1, -1, 0, 0, 0 };
            var sorted = source.AsSorted();
            int begin;
            int end;
            sorted.EqualRange(0, out begin, out end);
            Assert.AreEqual(2, begin);
            Assert.AreEqual(5, end);
        }

        [TestMethod]
        public void LowerBound_ItemFound_ReturnsLowerBound()
        {
            var source = new[] { 1, 2, 2, 4 };
            var sorted = source.AsSorted();
            int result = sorted.LowerBound(2);
            Assert.AreEqual(1, result, "LowerBound should return the lower bound");
        }

        [TestMethod]
        public void LowerBound_ItemNotFound_ReturnsBitwiseComplement()
        {
            var source = new[] { 1, 2, 2, 4 };
            var sorted = source.AsSorted();
            int result = sorted.LowerBound(3);
            Assert.AreEqual(~3, result, "LowerBound should return the bitwise complement if not found");
        }

        [TestMethod]
        public void LowerBound_ItemNotFoundInEmptySequence_ReturnsBitwiseComplementOf0()
        {
            var source = new int[] { };
            var sorted = source.AsSorted();
            int result = sorted.LowerBound(3);
            Assert.AreEqual(~0, result, "LowerBound should return the bitwise complement if not found");
        }

        [TestMethod]
        public void LowerBound_ItemNotFoundPastSequence_ReturnsBitwiseComplement()
        {
            var source = new[] { 1, 2, 2, 4 };
            var sorted = source.AsSorted();
            int result = sorted.LowerBound(5);
            Assert.AreEqual(~4, result, "LowerBound should return the bitwise complement if not found");
        }

        [TestMethod]
        public void UpperBound_ItemFound_ReturnsUpperBound()
        {
            var source = new[] { 1, 2, 2, 4 };
            var sorted = source.AsSorted();
            int result = sorted.UpperBound(2);
            Assert.AreEqual(3, result, "UpperBound should return the upper bound");
        }

        [TestMethod]
        public void UpperBound_ItemNotFound_ReturnsBitwiseComplement()
        {
            var source = new[] { 1, 2, 2, 4 };
            var sorted = source.AsSorted();
            int result = sorted.UpperBound(3);
            Assert.AreEqual(~3, result, "UpperBound should return the bitwise complement if not found");
        }

        [TestMethod]
        public void UpperBound_ItemNotFoundInEmptySequence_ReturnsBitwiseComplementOf0()
        {
            var source = new int[] { };
            var sorted = source.AsSorted();
            int result = sorted.UpperBound(3);
            Assert.AreEqual(~0, result, "LowerBound should return the bitwise complement if not found");
        }

        [TestMethod]
        public void UpperBound_ItemNotFoundPastSequence_ReturnsBitwiseComplement()
        {
            var source = new[] { 1, 2, 2, 4 };
            var sorted = source.AsSorted();
            int result = sorted.UpperBound(5);
            Assert.AreEqual(~4, result, "LowerBound should return the bitwise complement if not found");
        }

        [TestMethod]
        public void EmptyList_InsertSorted_ContainsSingleValue()
        {
            var list = new List<int>().AsSorted();
            list.Insert(13);
            Assert.IsTrue(list.SequenceEqual(new[] { 13 }), "Inserting into an empty list should result in a single-element list");
        }

        [TestMethod]
        public void SingleElementList_InsertSmall_ContainsSortedValues()
        {
            var list = new List<int> { 13 }.AsSorted();
            list.Insert(7);
            Assert.IsTrue(list.SequenceEqual(new[] { 7, 13 }), "Inserting into the beginning of a list should result in a sorted list");
        }

        [TestMethod]
        public void SingleElementList_InsertSame_ContainsSortedValues()
        {
            var list = new List<int> { 13 }.AsSorted();
            list.Insert(13);
            Assert.IsTrue(list.SequenceEqual(new[] { 13, 13 }), "Inserting into the middle of a list should result in a sorted list");
        }

        [TestMethod]
        public void SingleElementList_InsertLarge_ContainsSortedValues()
        {
            var list = new List<int> { 13 }.AsSorted();
            list.Insert(17);
            Assert.IsTrue(list.SequenceEqual(new[] { 13, 17 }), "Inserting into the end of a list should result in a sorted list");
        }

        [TestMethod]
        public void EmptyList_InsertSorted_ReturnsItemIndex()
        {
            var list = new List<int>().AsSorted();
            int result = list.Insert(13);
            Assert.AreEqual(0, result, "Inserting into an empty list should result in a single-element list");
        }

        [TestMethod]
        public void SingleElementList_InsertSmall_ReturnsItemIndex()
        {
            var list = new List<int> { 13 }.AsSorted();
            int result = list.Insert(7);
            Assert.AreEqual(0, result, "Inserting into the beginning of a list should result in a sorted list");
        }

        [TestMethod]
        public void SingleElementList_InsertSame_ReturnsItemIndex()
        {
            var list = new List<int> { 13 }.AsSorted();
            int result = list.Insert(13);
            Assert.IsTrue(result == 0 || result == 1, "Inserting into the middle of a list should result in a sorted list");
        }

        [TestMethod]
        public void SingleElementList_InsertLarge_ReturnsItemIndex()
        {
            var list = new List<int> { 13 }.AsSorted();
            int result = list.Insert(17);
            Assert.AreEqual(1, result, "Inserting into the end of a list should result in a sorted list");
        }

        [TestMethod]
        public void Sort_EmptyList_SortsList()
        {
            IList<int> source = new List<int>();
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Sorting an empty list should not fail");
        }

        [TestMethod]
        public void Sort_SingleElementList_SortsList()
        {
            IList<int> source = new List<int> { 1 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1 }), "Sorting a single-element list should not fail");
        }

        [TestMethod]
        public void Sort_2ElementList_Permutation1_SortsList()
        {
            IList<int> source = new List<int> { 1, 2 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_2ElementList_Permutation2_SortsList()
        {
            IList<int> source = new List<int> { 2, 1 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_3ElementList_Permutation1_SortsList()
        {
            IList<int> source = new List<int> { 1, 2, 3 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_3ElementList_Permutation2_SortsList()
        {
            IList<int> source = new List<int> { 1, 3, 2 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_3ElementList_Permutation3_SortsList()
        {
            IList<int> source = new List<int> { 2, 1, 3 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_3ElementList_Permutation4_SortsList()
        {
            IList<int> source = new List<int> { 3, 1, 2 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_3ElementList_Permutation5_SortsList()
        {
            IList<int> source = new List<int> { 2, 3, 1 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_3ElementList_Permutation6_SortsList()
        {
            IList<int> source = new List<int> { 3, 2, 1 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation1_SortsList()
        {
            IList<int> source = new List<int> { 1, 2, 3, 4 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation2_SortsList()
        {
            IList<int> source = new List<int> { 1, 2, 4, 3 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation3_SortsList()
        {
            IList<int> source = new List<int> { 1, 3, 2, 4 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation4_SortsList()
        {
            IList<int> source = new List<int> { 1, 4, 2, 3 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation5_SortsList()
        {
            IList<int> source = new List<int> { 1, 3, 4, 2 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation6_SortsList()
        {
            IList<int> source = new List<int> { 1, 4, 3, 2 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation7_SortsList()
        {
            IList<int> source = new List<int> { 2, 1, 3, 4 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation8_SortsList()
        {
            IList<int> source = new List<int> { 2, 1, 4, 3 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation9_SortsList()
        {
            IList<int> source = new List<int> { 2, 3, 1, 4 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation10_SortsList()
        {
            IList<int> source = new List<int> { 2, 4, 1, 3 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation11_SortsList()
        {
            IList<int> source = new List<int> { 2, 3, 4, 1 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation12_SortsList()
        {
            IList<int> source = new List<int> { 2, 4, 3, 1 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation13_SortsList()
        {
            IList<int> source = new List<int> { 3, 1, 2, 4 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation14_SortsList()
        {
            IList<int> source = new List<int> { 3, 1, 4, 2 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation15_SortsList()
        {
            IList<int> source = new List<int> { 3, 2, 1, 4 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation16_SortsList()
        {
            IList<int> source = new List<int> { 3, 4, 1, 2 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation17_SortsList()
        {
            IList<int> source = new List<int> { 3, 2, 4, 1 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation18_SortsList()
        {
            IList<int> source = new List<int> { 3, 4, 2, 1 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation19_SortsList()
        {
            IList<int> source = new List<int> { 4, 1, 2, 3 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation20_SortsList()
        {
            IList<int> source = new List<int> { 4, 1, 3, 2 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation21_SortsList()
        {
            IList<int> source = new List<int> { 4, 2, 1, 3 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation22_SortsList()
        {
            IList<int> source = new List<int> { 4, 3, 1, 2 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation23_SortsList()
        {
            IList<int> source = new List<int> { 4, 2, 3, 1 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_4ElementList_Permutation24_SortsList()
        {
            IList<int> source = new List<int> { 4, 3, 2, 1 };
            var result = source.Sort();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should sort the list");
        }

        [TestMethod]
        public void Sort_UpdatesSource()
        {
            IList<int> source = new List<int> { 4, 3, 2, 1 };
            var result = source.Sort();
            Assert.IsTrue(source.SequenceEqual(new[] { 1, 2, 3, 4 }), "Sorting a list should update the source");
        }

        [TestMethod]
        public void Sort_SyncsOtherList()
        {
            IList<int> source = new List<int> { 4, 2, 1, 3 };
            var other = new List<string> { "A", "B", "C", "D" };
            var result = source.Sort(other.AsSwappable());
            Assert.IsTrue(other.SequenceEqual(new[] { "C", "B", "D", "A" }), "Sorting a list should keep other lists in sync");
        }

        [TestMethod]
        public void Sort_SyncsTwoOtherLists()
        {
            IList<int> source = new List<int> { 3, 2, 1, 4 };
            var other1 = new List<string> { "A", "B", "C", "D" };
            var other2 = new List<string> { "Third", "Second", "First", "Fourth" };
            var result = source.Sort(other1.AsSwappable(), other2.AsSwappable());
            Assert.IsTrue(other1.SequenceEqual(new[] { "C", "B", "A", "D" }), "Sorting a list should keep other lists in sync");
            Assert.IsTrue(other2.SequenceEqual(new[] { "First", "Second", "Third", "Fourth" }), "Sorting a list should keep other lists in sync");
        }

        [TestMethod]
        public void Sort_SyncsThreeOtherLists()
        {
            IList<int> source = new List<int> { 2, 3, 1, 4 };
            var other1 = new List<string> { "B", "A", "C", "D" };
            var other2 = new List<string> { "Second", "Third", "First", "Fourth" };
            var other3 = new List<int> { 12, 13, 11, 14 };
            var result = source.Sort(other1.AsSwappable(), other2.AsSwappable(), other3.AsSwappable());
            Assert.IsTrue(other1.SequenceEqual(new[] { "C", "B", "A", "D" }), "Sorting a list should keep other lists in sync");
            Assert.IsTrue(other2.SequenceEqual(new[] { "First", "Second", "Third", "Fourth" }), "Sorting a list should keep other lists in sync");
            Assert.IsTrue(other3.SequenceEqual(new[] { 11, 12, 13, 14 }), "Sorting a list should keep other lists in sync");
        }

        [TestMethod]
        public void Sort_InReverse_DoesReverseSort()
        {
            IList<int> source = new List<int> { 4, 2, 1, 3 };
            IComparer<int> comparer = new ReverseSort<int> { Comparer = Comparer<int>.Default };
            var result = source.Sort(comparer);
            Assert.IsTrue(source.SequenceEqual(new[] { 4, 3, 2, 1 }), "Sorting a list should honor the comparer object");
        }

        [TestMethod]
        public void Sort_InReverse_SyncsOtherList()
        {
            IList<int> source = new List<int> { 4, 2, 1, 3 };
            IComparer<int> comparer = new ReverseSort<int> { Comparer = Comparer<int>.Default };
            var other = new List<string> { "A", "B", "C", "D" };
            var result = source.Sort(comparer, other.AsSwappable());
            Assert.IsTrue(other.SequenceEqual(new[] { "A", "D", "B", "C" }), "Sorting a list should keep other lists in sync");
        }

        [TestMethod]
        public void Sort_InReverse_SyncsTwoOtherLists()
        {
            IList<int> source = new List<int> { 3, 2, 1, 4 };
            IComparer<int> comparer = new ReverseSort<int> { Comparer = Comparer<int>.Default };
            var other1 = new List<string> { "A", "B", "C", "D" };
            var other2 = new List<string> { "Third", "Second", "First", "Fourth" };
            var result = source.Sort(comparer, other1.AsSwappable(), other2.AsSwappable());
            Assert.IsTrue(other1.SequenceEqual(new[] { "D", "A", "B", "C" }), "Sorting a list should keep other lists in sync");
            Assert.IsTrue(other2.SequenceEqual(new[] { "Fourth", "Third", "Second", "First" }), "Sorting a list should keep other lists in sync");
        }

        [TestMethod]
        public void Sort_InReverse_SyncsThreeOtherLists()
        {
            IList<int> source = new List<int> { 2, 3, 1, 4 };
            IComparer<int> comparer = new ReverseSort<int> { Comparer = Comparer<int>.Default };
            var other1 = new List<string> { "B", "A", "C", "D" };
            var other2 = new List<string> { "Second", "Third", "First", "Fourth" };
            var other3 = new List<int> { 12, 13, 11, 14 };
            var result = source.Sort(comparer, other1.AsSwappable(), other2.AsSwappable(), other3.AsSwappable());
            Assert.IsTrue(other1.SequenceEqual(new[] { "D", "A", "B", "C" }), "Sorting a list should keep other lists in sync");
            Assert.IsTrue(other2.SequenceEqual(new[] { "Fourth", "Third", "Second", "First" }), "Sorting a list should keep other lists in sync");
            Assert.IsTrue(other3.SequenceEqual(new[] { 14, 13, 12, 11 }), "Sorting a list should keep other lists in sync");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Sort should reject sync lists that are too small")]
        public void Sort_SyncingListTooSmall_IsRejected()
        {
            IList<int> source = new List<int> { 4, 2, 1, 3 };
            var other = new List<string> { "A", "B", "C" };
            var result = source.Sort(other.AsSwappable());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Sort should reject sync lists that are too small")]
        public void Sort_SyncingTwoOtherLists_FirstTooSmall_IsRejected()
        {
            IList<int> source = new List<int> { 4, 2, 1, 3 };
            var other = new List<string> { "A", "B", "C" };
            var other2 = new List<string> { "Third", "Second", "First", "Fourth" };
            var result = source.Sort(other.AsSwappable(), other2.AsSwappable());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Sort should reject sync lists that are too small")]
        public void Sort_SyncingTwoOtherLists_SecondTooSmall_IsRejected()
        {
            IList<int> source = new List<int> { 4, 2, 1, 3 };
            var other = new List<string> { "A", "B", "C", "D" };
            var other2 = new List<string> { "Third", "Second", "First" };
            var result = source.Sort(other.AsSwappable(), other2.AsSwappable());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Sort should reject sync lists that are too small")]
        public void Sort_SyncingThreeOtherLists_FirstTooSmall_IsRejected()
        {
            IList<int> source = new List<int> { 4, 2, 1, 3 };
            var other = new List<string> { "A", "B", "C" };
            var other2 = new List<string> { "Third", "Second", "First", "Fourth" };
            var other3 = new List<int> { 12, 13, 11, 14 };
            var result = source.Sort(other.AsSwappable(), other2.AsSwappable(), other3.AsSwappable());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Sort should reject sync lists that are too small")]
        public void Sort_SyncingThreeOtherLists_SecondTooSmall_IsRejected()
        {
            IList<int> source = new List<int> { 4, 2, 1, 3 };
            var other = new List<string> { "A", "B", "C", "D" };
            var other2 = new List<string> { "Third", "Second", "First" };
            var other3 = new List<int> { 12, 13, 11, 14 };
            var result = source.Sort(other.AsSwappable(), other2.AsSwappable(), other3.AsSwappable());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Sort should reject sync lists that are too small")]
        public void Sort_SyncingThreeOtherLists_ThirdTooSmall_IsRejected()
        {
            IList<int> source = new List<int> { 4, 2, 1, 3 };
            var other = new List<string> { "A", "B", "C", "D" };
            var other2 = new List<string> { "Third", "Second", "First", "Fourth" };
            var other3 = new List<int> { 12, 13, 11 };
            var result = source.Sort(other.AsSwappable(), other2.AsSwappable(), other3.AsSwappable());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Sort should reject sync lists that are too small")]
        public void Sort_InReverse_SyncingListTooSmall_IsRejected()
        {
            IList<int> source = new List<int> { 4, 2, 1, 3 };
            IComparer<int> comparer = new ReverseSort<int> { Comparer = Comparer<int>.Default };
            var other = new List<string> { "A", "B", "C" };
            var result = source.Sort(comparer, other.AsSwappable());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Sort should reject sync lists that are too small")]
        public void Sort_InReverse_SyncingTwoOtherLists_FirstTooSmall_IsRejected()
        {
            IList<int> source = new List<int> { 4, 2, 1, 3 };
            IComparer<int> comparer = new ReverseSort<int> { Comparer = Comparer<int>.Default };
            var other = new List<string> { "A", "B", "C" };
            var other2 = new List<string> { "Third", "Second", "First", "Fourth" };
            var result = source.Sort(comparer, other.AsSwappable(), other2.AsSwappable());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Sort should reject sync lists that are too small")]
        public void Sort_InReverse_SyncingTwoOtherLists_SecondTooSmall_IsRejected()
        {
            IList<int> source = new List<int> { 4, 2, 1, 3 };
            IComparer<int> comparer = new ReverseSort<int> { Comparer = Comparer<int>.Default };
            var other = new List<string> { "A", "B", "C", "D" };
            var other2 = new List<string> { "Third", "Second", "First" };
            var result = source.Sort(comparer, other.AsSwappable(), other2.AsSwappable());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Sort should reject sync lists that are too small")]
        public void Sort_InReverse_SyncingThreeOtherLists_FirstTooSmall_IsRejected()
        {
            IList<int> source = new List<int> { 4, 2, 1, 3 };
            IComparer<int> comparer = new ReverseSort<int> { Comparer = Comparer<int>.Default };
            var other = new List<string> { "A", "B", "C" };
            var other2 = new List<string> { "Third", "Second", "First", "Fourth" };
            var other3 = new List<int> { 12, 13, 11, 14 };
            var result = source.Sort(comparer, other.AsSwappable(), other2.AsSwappable(), other3.AsSwappable());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Sort should reject sync lists that are too small")]
        public void Sort_InReverse_SyncingThreeOtherLists_SecondTooSmall_IsRejected()
        {
            IList<int> source = new List<int> { 4, 2, 1, 3 };
            IComparer<int> comparer = new ReverseSort<int> { Comparer = Comparer<int>.Default };
            var other = new List<string> { "A", "B", "C", "D" };
            var other2 = new List<string> { "Third", "Second", "First" };
            var other3 = new List<int> { 12, 13, 11, 14 };
            var result = source.Sort(comparer, other.AsSwappable(), other2.AsSwappable(), other3.AsSwappable());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Sort should reject sync lists that are too small")]
        public void Sort_InReverse_SyncingThreeOtherLists_ThirdTooSmall_IsRejected()
        {
            IList<int> source = new List<int> { 4, 2, 1, 3 };
            IComparer<int> comparer = new ReverseSort<int> { Comparer = Comparer<int>.Default };
            var other = new List<string> { "A", "B", "C", "D" };
            var other2 = new List<string> { "Third", "Second", "First", "Fourth" };
            var other3 = new List<int> { 12, 13, 11 };
            var result = source.Sort(comparer, other.AsSwappable(), other2.AsSwappable(), other3.AsSwappable());
        }

        [TestMethod]
        public void Sort_WithDuplicates_SortsList()
        {
            IList<string> source = new List<string> { "This", "is", "only", "a", "test", "and", "is", "not", "a", "proof", "of", "correctness" };
            var result = source.Sort(StringComparer.InvariantCultureIgnoreCase);
            Assert.IsTrue(result.SequenceEqual(new[] { "a", "a", "and", "correctness", "is", "is", "not", "of", "only", "proof", "test", "This" }), "Sort should work with duplicates");
        }

        [TestMethod]
        public void Reverse_EnumeratesInReverse()
        {
            var source = new List<int> { 1, 2, 3, 4 }.AsSorted();
            var result = source.Reverse();
            Assert.IsTrue(result.SequenceEqual(new[] { 4, 3, 2, 1 }), "Reverse should reverse the list");
        }

        [TestMethod]
        public void Reverse_InsertItem_UpdatesSource()
        {
            var source = new List<int> { 1, 2, 3, 4 }.AsSorted();
            var result = source.Reverse();
            result.Insert(3);
            Assert.IsTrue(result.SequenceEqual(new[] { 4, 3, 3, 2, 1 }), "Reverse should allow setting items");
            Assert.IsTrue(source.SequenceEqual(new[] { 1, 2, 3, 3, 4 }), "Reverse should update the source list");
        }

        [TestMethod]
        public void Return_EnumeratesSingleItem()
        {
            int source = 13;
            ISortedList<int> result = SortedListExtensions.Return(source);
            Assert.IsTrue(result.SequenceEqual(new[] { 13 }), "Item should be enumerated.");
        }

        [TestMethod]
        public void Repeat_EnumeratesRepeatedItem()
        {
            int source = 13;
            ISortedList<int> result = SortedListExtensions.Repeat(source, 3);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 13, 13 }), "Item should be repeated.");
        }

        [TestMethod]
        public void Memoize_EnumeratesSource()
        {
            int seed = 0;
            ISortedList<int> source = ListExtensions.Generate(3, () => seed++).AsSorted();
            ISortedList<int> result = source.Memoize();
            Assert.IsTrue(result.SequenceEqual(new[] { 0, 1, 2 }), "Memoize should enumerate its source");
        }

        [TestMethod]
        public void MemoizeAll_EnumeratesSource()
        {
            int seed = 0;
            ISortedList<int> source = ListExtensions.Generate(3, () => seed++).AsSorted();
            ISortedList<int> result = source.MemoizeAll();
            Assert.IsTrue(result.SequenceEqual(new[] { 0, 1, 2 }), "MemoizeAll should enumerate its source");
        }

        [TestMethod]
        public void Skip_EnumeratesItems()
        {
            ISortedList<int> test = new[] { 1, 2, 3, 4 }.AsSorted();
            ISortedList<int> result = test.Skip(1);
            Assert.AreEqual(3, result.Count, "Skip should have a correct count");
            Assert.IsTrue(result.SequenceEqual(new[] { 2, 3, 4 }), "Skip should skip the requested items");
        }

        [TestMethod]
        public void Slice_EnumeratesItems()
        {
            ISortedList<int> test = new[] { 1, 2, 3, 4 }.AsSorted();
            ISortedList<int> slice = test.Slice(1, 2);
            Assert.AreEqual(2, slice.Count, "Slice should have a correct count");
            Assert.IsTrue(slice.SequenceEqual(new[] { 2, 3 }), "Slice should equal the sliced sequence");
        }

        [TestMethod]
        public void Step_By2_EnumeratesItems()
        {
            ISortedList<int> source = new List<int> { 1, 2, 3, 4, 5, 6 }.AsSorted();
            ISortedList<int> result = source.Step(2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 3, 5 }), "Stepping should enumerate the requested items");
        }

        [TestMethod]
        public void Take_EnumeratesItems()
        {
            ISortedList<int> test = new[] { 1, 2, 3, 4 }.AsSorted();
            ISortedList<int> result = test.Take(2);
            Assert.AreEqual(2, result.Count, "Take should have a correct count");
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2 }), "Take should take only the requested items");
        }

        private class ReverseSort<T> : IComparer<T>
        {
            public IComparer<T> Comparer { get; set; }

            public int Compare(T x, T y)
            {
                return this.Comparer.Compare(y, x);
            }
        }
    }
}

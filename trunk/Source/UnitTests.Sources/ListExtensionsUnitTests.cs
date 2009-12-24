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
        public void AsList_ReturnsArgument()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            var result = test.AsList();
            Assert.AreSame(test, result, "AsList should return its argument.");
        }

        [TestMethod]
        public void List_Empty_IsEmpty()
        {
            var result = ListSource.Empty<int>();
            Assert.AreEqual(0, result.Count, "Empty should be empty.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void List_Slice_RejectsNegativeOffset()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            test.Slice(-1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void List_Slice_RejectsOffsetOverflow()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            test.Slice(5, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void List_Slice_RejectsNegativeCount()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            test.Slice(1, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void List_Slice_RejectsCountOverflow()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            test.Slice(4, 1);
        }

        [TestMethod]
        public void List_Slice_OverArray_IsReadOnly()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            var slice = test.Slice(2, 2);
            bool result = slice.IsReadOnly;
            Assert.IsTrue(result, "Slices of arrays should be read-only.");
        }

        [TestMethod]
        public void List_SliceEmptyAtBeginningOfSource_EnumeratesNothing()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            var slice = test.Slice(0, 0);
            Assert.AreEqual(0, slice.Count, "Empty slice should have count of 0.");
            Assert.IsTrue(slice.SequenceEqual(new int[] { }), "Empty slice should equal empty sequence.");
        }

        [TestMethod]
        public void List_SliceEmptyAtEndOfSource_EnumeratesNothing()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            var slice = test.Slice(4, 0);
            Assert.AreEqual(0, slice.Count, "Empty slice should have count of 0.");
            Assert.IsTrue(slice.SequenceEqual(new int[] { }), "Empty slice should equal empty sequence.");
        }

        [TestMethod]
        public void List_SliceEmptyInMiddleOfSource_EnumeratesNothing()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            var slice = test.Slice(2, 0);
            Assert.AreEqual(0, slice.Count, "Empty slice should have count of 0.");
            Assert.IsTrue(slice.SequenceEqual(new int[] { }), "Empty slice should equal empty sequence.");
        }

        [TestMethod]
        public void List_SliceNonEmptyAtBeginningOfSource_EnumeratesItems()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            var slice = test.Slice(0, 2);
            Assert.AreEqual(2, slice.Count, "Slice should have a correct count.");
            Assert.IsTrue(slice.SequenceEqual(new[] { 1, 2 }), "Slice should equal the sliced sequence.");
        }

        [TestMethod]
        public void List_SliceNonEmptyInMiddleOfSource_EnumeratesItems()
        {
            int[] test = new[] { 1, 2, 3, 4, 5 };
            var slice = test.Slice(2, 2);
            Assert.AreEqual(2, slice.Count, "Slice should have a correct count.");
            Assert.IsTrue(slice.SequenceEqual(new[] { 3, 4 }), "Slice should equal the sliced sequence.");
        }

        [TestMethod]
        public void List_SliceNonEmptyAtEndOfSource_EnumeratesItems()
        {
            int[] test = new[] { 1, 2, 3, 4, 5 };
            var slice = test.Slice(3, 2);
            Assert.AreEqual(2, slice.Count, "Slice should have a correct count.");
            Assert.IsTrue(slice.SequenceEqual(new[] { 4, 5 }), "Slice should equal the sliced sequence.");
        }

        [TestMethod]
        public void List_Slice_AddItem_UpdatesSource()
        {
            List<int> test = new List<int> { 1, 2, 3, 4 };
            var slice = test.Slice(4, 0);
            slice.Add(5);
            Assert.IsTrue(slice.SequenceEqual(new[] { 5 }), "Slice should contain added elements.");
            Assert.IsTrue(test.SequenceEqual(new[] { 1, 2, 3, 4, 5 }), "Slice should update source list.");
        }

        [TestMethod]
        public void List_Slice_InsertItem_UpdatesSource()
        {
            List<int> test = new List<int> { 1, 2, 3, 4 };
            var slice = test.Slice(2, 2);
            slice.Insert(1, 5);
            Assert.IsTrue(slice.SequenceEqual(new[] { 3, 5, 4 }), "Slice should contain added elements.");
            Assert.IsTrue(test.SequenceEqual(new[] { 1, 2, 3, 5, 4 }), "Slice should update source list.");
        }

        [TestMethod]
        public void List_Slice_RemoveAtItem_UpdatesSource()
        {
            List<int> test = new List<int> { 1, 2, 3, 4 };
            var slice = test.Slice(2, 2);
            slice.RemoveAt(0);
            Assert.IsTrue(slice.SequenceEqual(new[] { 4 }), "Slice should not contain removed elements.");
            Assert.IsTrue(test.SequenceEqual(new[] { 1, 2, 4 }), "Slice should update source list.");
        }

        [TestMethod]
        public void List_Slice_RemoveItem_UpdatesSource()
        {
            List<int> test = new List<int> { 1, 2, 3, 4 };
            var slice = test.Slice(2, 2);
            bool result = slice.Remove(4);
            Assert.IsTrue(result, "Slice should have found the element to remove.");
            Assert.IsTrue(slice.SequenceEqual(new[] { 3 }), "Slice should not contain removed elements.");
            Assert.IsTrue(test.SequenceEqual(new[] { 1, 2, 3 }), "Slice should update source list.");
        }

        [TestMethod]
        public void List_Slice_RemoveInvalidItem_DoesNothing()
        {
            List<int> test = new List<int> { 1, 2, 3, 4 };
            var slice = test.Slice(2, 2);
            bool result = slice.Remove(5);
            Assert.IsFalse(result, "Slice should not have found the element to remove.");
            Assert.IsTrue(slice.SequenceEqual(new[] { 3, 4 }), "Slice should not be modified.");
            Assert.IsTrue(test.SequenceEqual(new[] { 1, 2, 3, 4 }), "Slice should not have updated source list.");
        }

        [TestMethod]
        public void List_Slice_Clear_UpdatesSource()
        {
            List<int> test = new List<int> { 1, 2, 3, 4 };
            var slice = test.Slice(1, 2);
            slice.Clear();
            Assert.IsTrue(slice.SequenceEqual(new int[] { }), "Slice should not contain any elements.");
            Assert.IsTrue(test.SequenceEqual(new[] { 1, 4 }), "Slice should update source list.");
        }

        [TestMethod]
        public void List_SliceGetItem_AdjustsIndex()
        {
            List<int> test = new List<int> { 1, 2, 3, 4 };
            var slice = test.Slice(1, 2);
            int value = slice[1];
            Assert.AreEqual(3, value, "Slice should adjust index values.");
        }

        [TestMethod]
        public void List_SliceSetItem_UpdatesSource()
        {
            List<int> test = new List<int> { 1, 2, 3, 4 };
            var slice = test.Slice(1, 2);
            slice[1] = 5;
            Assert.IsTrue(slice.SequenceEqual(new[] { 2, 5 }), "Slice should be modified.");
            Assert.IsTrue(test.SequenceEqual(new[] { 1, 2, 5, 4 }), "Slice should have updated source list.");
        }

        [TestMethod]
        public void List_Take_EnumeratesItems()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            var result = test.Take(2);
            Assert.AreEqual(2, result.Count, "Take should have a correct count.");
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2 }), "Take should take only the requested items.");
        }

        [TestMethod]
        public void List_Take_NegativeCount_EnumeratesEmpty()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            var result = test.Take(-1);
            Assert.AreEqual(0, result.Count, "Take should have a correct count.");
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Take should take only the requested items.");
        }

        [TestMethod]
        public void List_Take_ZeroCount_EnumeratesEmpty()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            var result = test.Take(0);
            Assert.AreEqual(0, result.Count, "Take should have a correct count.");
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Take should take only the requested items.");
        }

        [TestMethod]
        public void List_Take_ListCount_EnumeratesItems()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            var result = test.Take(4);
            Assert.AreEqual(4, result.Count, "Take should have a correct count.");
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Take should take only the requested items.");
        }

        [TestMethod]
        public void List_Take_GreaterThanListCount_EnumeratesItems()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            var result = test.Take(5);
            Assert.AreEqual(4, result.Count, "Take should have a correct count.");
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Take should take only the requested items.");
        }

        [TestMethod]
        public void List_Skip_EnumeratesItems()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            var result = test.Skip(1);
            Assert.AreEqual(3, result.Count, "Skip should have a correct count.");
            Assert.IsTrue(result.SequenceEqual(new[] { 2, 3, 4 }), "Skip should skip the requested items.");
        }

        [TestMethod]
        public void List_Skip_NegativeCount_EnumeratesItems()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            var result = test.Skip(-1);
            Assert.AreEqual(4, result.Count, "Skip should have a correct count.");
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Skip should skip the requested items.");
        }

        [TestMethod]
        public void List_Skip_ZeroCount_EnumeratesItems()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            var result = test.Skip(0);
            Assert.AreEqual(4, result.Count, "Skip should have a correct count.");
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Skip should skip the requested items.");
        }

        [TestMethod]
        public void List_Skip_ListCount_EnumeratesEmpty()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            var result = test.Skip(4);
            Assert.AreEqual(0, result.Count, "Skip should have a correct count.");
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Skip should skip the requested items.");
        }

        [TestMethod]
        public void List_Skip_GreaterThanListCount_EnumeratesEmpty()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            var result = test.Skip(5);
            Assert.AreEqual(0, result.Count, "Skip should have a correct count.");
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Skip should skip the requested items.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Step should reject 0 or negative step sizes.")]
        public void List_Step_ZeroStep_IsRejected()
        {
            List<int> source = new List<int> { 1, 2, 3, 4, 5, 6 };
            var result = source.Step(0);
        }

        [TestMethod]
        public void List_Step_EmptySource_EnumeratesEmptySequence()
        {
            List<int> source = new List<int> { };
            var result = source.Step(1);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Stepping an empty source should enumerate an empty list.");
        }

        [TestMethod]
        public void List_Step_SingleStep_EnumeratesItems()
        {
            List<int> source = new List<int> { 1, 2, 3, 4, 5, 6 };
            var result = source.Step(1);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4, 5, 6 }), "Single-stepping should enumerate the entire source list.");
        }

        [TestMethod]
        public void List_Step_By2_EnumeratesItems()
        {
            List<int> source = new List<int> { 1, 2, 3, 4, 5, 6 };
            var result = source.Step(2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 3, 5 }), "Stepping should enumerate the requested items.");
        }

        [TestMethod]
        public void List_Step_By3_EnumeratesItems()
        {
            List<int> source = new List<int> { 1, 2, 3, 4, 5, 6 };
            var result = source.Step(3);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 4 }), "Stepping should enumerate the requested items.");
        }

        [TestMethod]
        public void List_Step_By2_SourceLength0_CountIs0()
        {
            List<int> source = new List<int> { };
            var result = source.Step(2);
            Assert.AreEqual(result.Count, 0, "Stepping an empty list should have count of 0.");
        }

        [TestMethod]
        public void List_Step_By2_SourceLength1_CountIs1()
        {
            List<int> source = new List<int> { 1 };
            var result = source.Step(2);
            Assert.AreEqual(result.Count, 1, "Stepping by two over a source list of count 1 should have count of 1.");
        }

        [TestMethod]
        public void List_Step_By2_SourceLength2_CountIs1()
        {
            List<int> source = new List<int> { 1, 2 };
            var result = source.Step(2);
            Assert.AreEqual(result.Count, 1, "Stepping by two over a source list of count 2 should have count of 1.");
        }

        [TestMethod]
        public void List_Step_By2_SourceLength3_CountIs2()
        {
            List<int> source = new List<int> { 1, 2, 3 };
            var result = source.Step(2);
            Assert.AreEqual(result.Count, 2, "Stepping by two over a source list of count 3 should have count of 2.");
        }

        [TestMethod]
        public void List_Step_By2_SourceLength4_CountIs2()
        {
            List<int> source = new List<int> { 1, 2, 3, 4 };
            var result = source.Step(2);
            Assert.AreEqual(result.Count, 2, "Stepping by two over a source list of count 4 should have count of 2.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Clearing a step list should not be allowed.")]
        public void List_Step_Clear_IsRejected()
        {
            List<int> source = new List<int> { 1, 2, 3, 4, 5, 6 };
            var result = source.Step(3);
            result.Clear();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Removing elements from a step list should not be allowed.")]
        public void List_Step_RemoveAt_IsRejected()
        {
            List<int> source = new List<int> { 1, 2, 3, 4, 5, 6 };
            var result = source.Step(3);
            result.RemoveAt(0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Inserting elements into a step list should not be allowed.")]
        public void List_Step_Insert_IsRejected()
        {
            List<int> source = new List<int> { 1, 2, 3, 4, 5, 6 };
            var result = source.Step(3);
            result.Insert(0, 13);
        }

        [TestMethod]
        public void List_Step_SetItem_UpdatesSource()
        {
            List<int> source = new List<int> { 1, 2, 3, 4, 5, 6 };
            var result = source.Step(3);
            result[1] = 13;
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 13 }), "Setting an item in a step list should update the step list.");
            Assert.IsTrue(source.SequenceEqual(new[] { 1, 2, 3, 13, 5, 6 }), "Setting an item in a step list should update the source list.");
        }

        [TestMethod]
        public void List_Step_OnArray_IsReadOnly()
        {
            int[] source = new int[] { 1, 2, 3, 4, 5, 6 };
            var result = source.Step(3);
            Assert.IsTrue(result.IsReadOnly, "A step list over an array should be read-only.");
        }

        [TestMethod]
        public void List_SelectWithIndex_ProjectsSequenceSameAsEnumerableSelect()
        {
            List<int> test = new List<int> { 1, 2, 3, 4 };
            var result = test.Select((x, i) => new { x, i });
            var orig = ((IEnumerable<int>)test).Select((x, i) => new { x, i });
            Assert.IsTrue(result.SequenceEqual(orig), "IList<T>.Select (with index) should work identically as IEnumerable<T>.Select (with index).");
        }

        [TestMethod]
        public void List_SelectWithoutIndex_ProjectsSequenceSameAsEnumerableSelect()
        {
            List<int> test = new List<int> { 1, 2, 3, 4 };
            var result = test.Select(x => new { x });
            var orig = ((IEnumerable<int>)test).Select(x => new { x });
            Assert.IsTrue(result.SequenceEqual(orig), "IList<T>.Select (without index) should work identically as IEnumerable<T>.Select (without index).");
        }

        [TestMethod]
        public void List_SelectWriteable_ProjectsSequence()
        {
            List<int> test = new List<int> { 1, 2, 3, 4 };
            var result = test.Select(x => new { value = x * 2 }, x => x.value / 2);
            Assert.IsTrue(result.SequenceEqual(new[] { new { value = 2 }, new { value = 4 }, new { value = 6 }, new { value = 8 } }), "Writeable IList<T>.Select should project its source sequence.");
        }

        [TestMethod]
        public void List_SelectWriteable_OverArray_IsReadOnly()
        {
            int[] test = new[] { 1, 2, 3, 4 };
            var result = test.Select(x => x * 2, x => x / 2);
            Assert.IsTrue(result.IsReadOnly, "Writeable IList<T>.Select over array should be read-only.");
        }

        [TestMethod]
        public void List_SelectWriteable_AddItem_UpdatesSource()
        {
            List<int> test = new List<int> { 1, 2, 3, 4 };
            var result = test.Select(x => x * 2, x => x / 2);
            result.Add(10);
            Assert.IsTrue(result.SequenceEqual(new[] { 2, 4, 6, 8, 10 }), "Writeable IList<T>.Select should contain added elements.");
            Assert.IsTrue(test.SequenceEqual(new[] { 1, 2, 3, 4, 5 }), "Writeable IList<T>.Select should update source list.");
        }

        [TestMethod]
        public void List_SelectWriteable_InsertItem_UpdatesSource()
        {
            List<int> test = new List<int> { 1, 2, 3, 4 };
            var result = test.Select(x => x * 2, x => x / 2);
            result.Insert(1, 10);
            Assert.IsTrue(result.SequenceEqual(new[] { 2, 10, 4, 6, 8 }), "Writeable IList<T>.Select should contain added elements.");
            Assert.IsTrue(test.SequenceEqual(new[] { 1, 5, 2, 3, 4 }), "Writeable IList<T>.Select should update source list.");
        }

        [TestMethod]
        public void List_SelectWriteable_RemoveAtItem_UpdatesSource()
        {
            List<int> test = new List<int> { 1, 2, 3, 4 };
            var result = test.Select(x => x * 2, x => x / 2);
            result.RemoveAt(1);
            Assert.IsTrue(result.SequenceEqual(new[] { 2, 6, 8 }), "Writeable IList<T>.Select should not contain removed elements.");
            Assert.IsTrue(test.SequenceEqual(new[] { 1, 3, 4 }), "Writeable IList<T>.Select should update source list.");
        }

        [TestMethod]
        public void List_SelectWriteable_RemoveItem_UpdatesSource()
        {
            List<int> test = new List<int> { 1, 2, 3, 4 };
            var result = test.Select(x => x * 2, x => x / 2);
            bool removed = result.Remove(4);
            Assert.IsTrue(removed, "Writeable IList<T>.Select should have found the element to remove.");
            Assert.IsTrue(result.SequenceEqual(new[] { 2, 6, 8 }), "Writeable IList<T>.Select should not contain removed elements.");
            Assert.IsTrue(test.SequenceEqual(new[] { 1, 3, 4 }), "Writeable IList<T>.Select should update source list.");
        }

        [TestMethod]
        public void List_SelectWriteable_RemoveInvalidItem_DoesNothing()
        {
            List<int> test = new List<int> { 1, 2, 3, 4 };
            var result = test.Select(x => x * 2, x => x / 2);
            bool removed = result.Remove(3);
            Assert.IsFalse(removed, "Slice should not have found the element to remove.");
            Assert.IsTrue(result.SequenceEqual(new[] { 2, 4, 6, 8 }), "Slice should not be modified.");
            Assert.IsTrue(test.SequenceEqual(new[] { 1, 2, 3, 4 }), "Slice should not have updated source list.");
        }

        [TestMethod]
        public void List_SelectWriteable_Clear_UpdatesSource()
        {
            List<int> test = new List<int> { 1, 2, 3, 4 };
            var result = test.Select(x => x * 2, x => x / 2);
            result.Clear();
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Writeable IList<T>.Select should not contain any elements.");
            Assert.IsTrue(test.SequenceEqual(new int[] { }), "Writeable IList<T>.Select should update source list.");
        }

        [TestMethod]
        public void List_SelectWriteable_SetItem_UpdatesSource()
        {
            List<int> test = new List<int> { 1, 2, 3, 4 };
            var result = test.Select(x => x * 2, x => x / 2);
            result[1] = 10;
            Assert.IsTrue(result.SequenceEqual(new[] { 2, 10, 6, 8 }), "Writeable IList<T>.Select should be modified.");
            Assert.IsTrue(test.SequenceEqual(new[] { 1, 5, 3, 4 }), "Writeable IList<T>.Select should have updated source list.");
        }

        [TestMethod]
        public void List_CopyTo_CopyingEntireList_CopiesElements()
        {
            IList<int> source = new List<int> { 1, 2, 3, 4 };
            IList<int> dest = new List<int> { 5, 6, 7, 8 };
            source.CopyTo(dest);
            Assert.IsTrue(dest.SequenceEqual(new[] { 1, 2, 3, 4 }), "CopyTo should have copied the elements.");
        }

        [TestMethod]
        public void List_CopyTo_CopyingWithDestIndex_CopiesElements()
        {
            IList<int> source = new List<int> { 1, 2, 3, 4 };
            IList<int> dest = new List<int> { 5, 6, 7, 8, 9, 10 };
            source.CopyTo(dest, 2);
            Assert.IsTrue(dest.SequenceEqual(new[] { 5, 6, 1, 2, 3, 4 }), "CopyTo should have copied the elements.");
        }

        [TestMethod]
        public void List_CopyTo_CopyingWithDestAndSourceIndexes_CopiesElements()
        {
            IList<int> source = new List<int> { 1, 2, 3, 4 };
            IList<int> dest = new List<int> { 5, 6, 7, 8, 9, 10 };
            source.CopyTo(1, dest, 2, 2);
            Assert.IsTrue(dest.SequenceEqual(new[] { 5, 6, 2, 3, 9, 10 }), "CopyTo should have copied the elements.");
        }

        [TestMethod]
        public void List_CopyTo_CopiesForward()
        {
            IList<int> list = new List<int> { 1, 2, 3, 4 };
            list.CopyTo(0, list, 1, 3);
            Assert.IsTrue(list.SequenceEqual(new[] { 1, 1, 1, 1 }), "CopyTo should have copied the elements in a forward direction.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "CopyTo with a source index too large should be rejected.")]
        public void List_CopyTo_CopyingWithSourceIndexTooLarge_IsRejected()
        {
            IList<int> source = new List<int> { 1, 2, 3, 4 };
            IList<int> dest = new List<int> { 5, 6, 7, 8, 9, 10 };
            source.CopyTo(4, dest, 2, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "CopyTo with a source index too small should be rejected.")]
        public void List_CopyTo_CopyingWithSourceIndexTooSmall_IsRejected()
        {
            IList<int> source = new List<int> { 1, 2, 3, 4 };
            IList<int> dest = new List<int> { 5, 6, 7, 8, 9, 10 };
            source.CopyTo(-1, dest, 2, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "CopyTo with a dest index too large should be rejected.")]
        public void List_CopyTo_CopyingWithDestIndexTooLarge_IsRejected()
        {
            IList<int> source = new List<int> { 1, 2, 3, 4 };
            IList<int> dest = new List<int> { 5, 6, 7, 8 };
            source.CopyTo(0, dest, 4, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "CopyTo with a dest index too small should be rejected.")]
        public void List_CopyTo_CopyingWithDestIndexTooSmall_IsRejected()
        {
            IList<int> source = new List<int> { 1, 2, 3, 4 };
            IList<int> dest = new List<int> { 5, 6, 7, 8, 9, 10 };
            source.CopyTo(0, dest, -1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "CopyTo with a count larger than source should be rejected.")]
        public void List_CopyTo_CopyingWithCountTooLargeForSource_IsRejected()
        {
            IList<int> source = new List<int> { 1, 2 };
            IList<int> dest = new List<int> { 5, 6, 7, 8 };
            source.CopyTo(0, dest, 0, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "CopyTo with a count larger than dest should be rejected.")]
        public void List_CopyTo_CopyingWithCountTooLargeForDest_IsRejected()
        {
            IList<int> source = new List<int> { 1, 2, 3, 4 };
            IList<int> dest = new List<int> { 5, 6 };
            source.CopyTo(0, dest, 0, 3);
        }

        [TestMethod]
        public void List_CopyToBackward_CopiesBackward()
        {
            IList<int> list = new List<int> { 1, 2, 3, 4 };
            list.CopyBackward(0, list, 1, 3);
            Assert.IsTrue(list.SequenceEqual(new[] { 1, 1, 2, 3 }), "CopyBackward should have copied the elements in a backward direction.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "CopyBackward with a source index too large should be rejected.")]
        public void List_CopyBackward_CopyingWithSourceIndexTooLarge_IsRejected()
        {
            IList<int> source = new List<int> { 1, 2, 3, 4 };
            IList<int> dest = new List<int> { 5, 6, 7, 8, 9, 10 };
            source.CopyBackward(4, dest, 2, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "CopyBackward with a source index too small should be rejected.")]
        public void List_CopyBackward_CopyingWithSourceIndexTooSmall_IsRejected()
        {
            IList<int> source = new List<int> { 1, 2, 3, 4 };
            IList<int> dest = new List<int> { 5, 6, 7, 8, 9, 10 };
            source.CopyBackward(-1, dest, 2, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "CopyBackward with a dest index too large should be rejected.")]
        public void List_CopyBackward_CopyingWithDestIndexTooLarge_IsRejected()
        {
            IList<int> source = new List<int> { 1, 2, 3, 4 };
            IList<int> dest = new List<int> { 5, 6, 7, 8 };
            source.CopyBackward(0, dest, 4, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "CopyBackward with a dest index too small should be rejected.")]
        public void List_CopyBackward_CopyingWithDestIndexTooSmall_IsRejected()
        {
            IList<int> source = new List<int> { 1, 2, 3, 4 };
            IList<int> dest = new List<int> { 5, 6, 7, 8, 9, 10 };
            source.CopyBackward(0, dest, -1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "CopyBackward with a count larger than source should be rejected.")]
        public void List_CopyBackward_CopyingWithCountTooLargeForSource_IsRejected()
        {
            IList<int> source = new List<int> { 1, 2 };
            IList<int> dest = new List<int> { 5, 6, 7, 8 };
            source.CopyBackward(0, dest, 0, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "CopyBackward with a count larger than dest should be rejected.")]
        public void List_CopyBackward_CopyingWithCountTooLargeForDest_IsRejected()
        {
            IList<int> source = new List<int> { 1, 2, 3, 4 };
            IList<int> dest = new List<int> { 5, 6 };
            source.CopyBackward(0, dest, 0, 3);
        }

        [TestMethod]
        public void List_Reverse_OverList_IsNotReadOnly()
        {
            IList<int> source = new List<int> { 1, 2, 3, 4 };
            var result = source.Reverse();
            Assert.IsFalse(result.IsReadOnly, "List should not be read-only.");
        }

        [TestMethod]
        public void List_Reverse_OverArray_IsReadOnly()
        {
            IList<int> source = new[] { 1, 2, 3, 4 };
            var result = source.Reverse();
            Assert.IsTrue(result.IsReadOnly, "List should be read-only.");
        }

        [TestMethod]
        public void List_Reverse_EnumeratesInReverse()
        {
            IList<int> source = new List<int> { 1, 2, 3, 4 };
            var result = source.Reverse();
            Assert.IsTrue(result.SequenceEqual(new[] { 4, 3, 2, 1 }), "Reverse should reverse the list.");
        }

        [TestMethod]
        public void List_Reverse_SetItem_UpdatesSource()
        {
            IList<int> source = new List<int> { 1, 2, 3, 4 };
            var result = source.Reverse();
            result[1] = 7;
            Assert.IsTrue(result.SequenceEqual(new[] { 4, 7, 2, 1 }), "Reverse should allow setting items.");
            Assert.IsTrue(source.SequenceEqual(new[] { 1, 2, 7, 4 }), "Reverse should update the source list.");
        }

        [TestMethod]
        public void List_Reverse_RemoveAt_UpdatesSource()
        {
            IList<int> source = new List<int> { 1, 2, 3, 4 };
            var result = source.Reverse();
            result.RemoveAt(1);
            Assert.IsTrue(result.SequenceEqual(new[] { 4, 2, 1 }), "Reverse should allow removing items.");
            Assert.IsTrue(source.SequenceEqual(new[] { 1, 2, 4 }), "Reverse should update the source list.");
        }

        [TestMethod]
        public void List_Reverse_Clear_UpdatesSource()
        {
            IList<int> source = new List<int> { 1, 2, 3, 4 };
            var result = source.Reverse();
            result.Clear();
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Reverse should allow clearing items.");
            Assert.IsTrue(source.SequenceEqual(new int[] { }), "Reverse should update the source list.");
        }

        [TestMethod]
        public void List_Reverse_InsertItem_UpdatesSource()
        {
            IList<int> source = new List<int> { 1, 2, 3, 4 };
            var result = source.Reverse();
            result.Insert(1, 7);
            Assert.IsTrue(result.SequenceEqual(new[] { 4, 7, 3, 2, 1 }), "Reverse should allow setting items.");
            Assert.IsTrue(source.SequenceEqual(new[] { 1, 2, 3, 7, 4 }), "Reverse should update the source list.");
        }

        [TestMethod]
        public void List_Return_EnumeratesSingleItem()
        {
            int source = 13;
            var result = ListSource.Return(source);
            Assert.IsTrue(result.SequenceEqual(new[] { 13 }), "Item should be enumerated.");
        }

        [TestMethod]
        public void List_Return_IsReadOnly()
        {
            int source = 13;
            var result = ListSource.Return(source);
            Assert.IsTrue(result.IsReadOnly, "List should be read-only.");
        }

        [TestMethod]
        public void List_Repeat_IsReadOnly()
        {
            int source = 13;
            var result = ListSource.Repeat(source, 3);
            Assert.IsTrue(result.IsReadOnly, "List should be read-only.");
        }

        [TestMethod]
        public void List_Repeat_EnumeratesRepeatedItem()
        {
            int source = 13;
            var result = ListSource.Repeat(source, 3);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 13, 13 }), "Item should be repeated.");
        }

        [TestMethod]
        public void List_RepeatSingleValue_NegativeTimes_EnumeratesEmptySequence()
        {
            int source = 13;
            var result = ListSource.Repeat(source, -1);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Item should not be repeated.");
        }

        [TestMethod]
        public void List_Repeat_EnumeratesRepeatedItems()
        {
            IList<int> source = new[] { 13, 15 };
            var result = source.Repeat(3);
            Assert.IsTrue(result.SequenceEqual(new[] { 13, 15, 13, 15, 13, 15 }), "Items should be repeated.");
        }

        [TestMethod]
        public void List_Repeat_NegativeTimes_EnumeratesEmptySequence()
        {
            IList<int> source = new[] { 13, 15 };
            var result = source.Repeat(-1);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Items should not be repeated.");
        }

        [TestMethod]
        public void List_Concat_WithNoArguments_ReturnsEmptyList()
        {
            IList<IList<int>> source = new List<IList<int>>();
            var result = source.Concat();
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Concat with a no parameters should be an empty list.");
        }

        [TestMethod]
        public void List_Concat_WithOneArgument_ReturnsList()
        {
            IList<int> test1 = new[] { 1 };
            var result = ListExtensions.Concat(test1);
            Assert.IsTrue(result.SequenceEqual(new[] { 1 }), "Concat with a single parameter should concatenate lists.");
        }

        [TestMethod]
        public void List_Concat_WithTwoArguments_ConcatenatesLists()
        {
            IList<int> test1 = new[] { 1 };
            IList<int> test2 = new[] { 2, 3 };
            var result = ListExtensions.Concat(test1, test2);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Concat with a single parameter should concatenate lists.");
        }

        [TestMethod]
        public void List_Concat_WithMultipleArguments_ConcatenatesLists()
        {
            IList<int> test1 = new[] { 1 };
            IList<int> test2 = new[] { 2, 3 };
            IList<int> test3 = new[] { 4 };
            var result = ListExtensions.Concat(test1, test2, test3);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Concat should concatenate lists.");
        }

        [TestMethod]
        public void List_Concat_WithEmptySource_ConcatenatesLists()
        {
            IList<int> test1 = new[] { 1 };
            IList<int> test2 = new int[] { };
            IList<int> test3 = new[] { 4 };
            var result = ListExtensions.Concat(test1, test2, test3);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 4 }), "Concat should concatenate lists.");
        }

        [TestMethod]
        public void List_Concat_WithAtLeastOneReadOnlySource_IsReadOnly()
        {
            IList<int> test1 = new List<int> { 1 };
            IList<int> test2 = new[] { 2, 3 };
            IList<int> test3 = new List<int> { 4 };
            var result = ListExtensions.Concat(test1, test2, test3);
            Assert.IsTrue(result.IsReadOnly, "List should be read-only.");
        }

        [TestMethod]
        public void List_Concat_WithNoReadOnlySources_IsNotReadOnly()
        {
            IList<int> test1 = new List<int> { 1 };
            IList<int> test2 = new List<int> { 2, 3 };
            IList<int> test3 = new List<int> { 4 };
            var result = ListExtensions.Concat(test1, test2, test3);
            Assert.IsFalse(result.IsReadOnly, "List should not be read-only.");
        }

        [TestMethod]
        public void List_Concat_SetItem_UpdatesSource()
        {
            IList<int> test1 = new List<int> { 1 };
            IList<int> test2 = new List<int> { 2, 3 };
            IList<int> test3 = new List<int> { 4 };
            var result = ListExtensions.Concat(test1, test2, test3);
            result[1] = 7;
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 7, 3, 4 }), "Concat should allow setting items.");
            Assert.IsTrue(test2.SequenceEqual(new[] { 7, 3 }), "Concat should update the source list.");
        }

        [TestMethod]
        public void List_Concat_RemoveAt_UpdatesSource()
        {
            IList<int> test1 = new List<int> { 1 };
            IList<int> test2 = new List<int> { 2, 3 };
            IList<int> test3 = new List<int> { 4 };
            var result = ListExtensions.Concat(test1, test2, test3);
            result.RemoveAt(3);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Concat should allow removing items.");
            Assert.IsTrue(test3.SequenceEqual(new int[] { }), "Concat should update the source list.");
        }

        [TestMethod]
        public void List_Concat_Clear_UpdatesSource()
        {
            IList<int> test1 = new List<int> { 1 };
            IList<int> test2 = new List<int> { 2, 3 };
            IList<int> test3 = new List<int> { 4 };
            var result = ListExtensions.Concat(test1, test2, test3);
            result.Clear();
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Concat should allow clearing items.");
            Assert.IsTrue(test1.SequenceEqual(new int[] { }), "Concat should update the source list.");
            Assert.IsTrue(test2.SequenceEqual(new int[] { }), "Concat should update the source list.");
            Assert.IsTrue(test3.SequenceEqual(new int[] { }), "Concat should update the source list.");
        }

        [TestMethod]
        public void List_Concat_InsertItem_UpdatesSource()
        {
            IList<int> test1 = new List<int> { 1 };
            IList<int> test2 = new List<int> { 2, 3 };
            IList<int> test3 = new List<int> { 4 };
            var result = ListExtensions.Concat(test1, test2, test3);
            result.Insert(2, 7);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 7, 3, 4 }), "Concat should allow setting items.");
            Assert.IsTrue(test2.SequenceEqual(new[] { 2, 7, 3 }), "Concat should update the source list.");
        }

        [TestMethod]
        public void List_Concat_InsertItemAtEndOfList_UpdatesSource()
        {
            IList<int> test1 = new List<int> { 1 };
            IList<int> test2 = new List<int> { 2, 3 };
            IList<int> test3 = new List<int> { 4 };
            var result = ListExtensions.Concat(test1, test2, test3);
            result.Insert(1, 7);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 7, 2, 3, 4 }), "Concat should allow setting items.");
            Assert.IsTrue(test1.SequenceEqual(new[] { 1, 7 }), "Concat should update the source list.");
        }

        [TestMethod]
        public void List_Concat_InsertItemAtEmptyList_UpdatesPreviousSource()
        {
            IList<int> test1 = new List<int> { 1 };
            IList<int> test2 = new List<int>();
            IList<int> test3 = new List<int> { 4 };
            var result = ListExtensions.Concat(test1, test2, test3);
            result.Insert(1, 7);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 7, 4 }), "Concat should allow setting items.");
            Assert.IsTrue(test1.SequenceEqual(new[] { 1, 7 }), "Concat should update the source list.");
        }

        [TestMethod]
        public void List_Concat_InsertItemAtEmptyList_UpdatesFirstSource()
        {
            IList<int> test1 = new List<int>();
            IList<int> test2 = new List<int>();
            IList<int> test3 = new List<int> { 4 };
            var result = ListExtensions.Concat(test1, test2, test3);
            result.Insert(0, 7);
            Assert.IsTrue(result.SequenceEqual(new[] { 7, 4 }), "Concat should allow setting items.");
            Assert.IsTrue(test1.SequenceEqual(new[] { 7 }), "Concat should update the source list.");
        }

        [TestMethod]
        public void List_ConcatLists_WithTwoLists_ConcatenatesLists()
        {
            IList<int> test1 = new[] { 1 };
            IList<int> test2 = new[] { 2, 3 };
            var result = ListExtensions.Concat(new List<IList<int>>() { test1, test2 });
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Concat with a single parameter should concatenate lists.");
        }

        [TestMethod]
        public void List_Flatten_FlattensLists()
        {
            IList<int> test1 = new[] { 1 };
            IList<int> test2 = new[] { 2, 3 };
            IList<int> test3 = new[] { 4 };
            IList<IList<int>> test = new[] { test1, test2, test3 };
            var result = test.FlattenList();
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "Flatten should flatten lists.");
        }

        [TestMethod]
        public void List_Zip_ZipsElements()
        {
            IList<int> source1 = new List<int> { 13, 7 };
            IList<int> source2 = new List<int> { 17, 23 };
            var result = source1.Zip(source2, (x, y) => x.ToString() + y.ToString());
            Assert.IsTrue(result.SequenceEqual(new[] { "1317", "723" }), "Zip should combine elements.");
        }

        [TestMethod]
        public void List_Zip_WithShorterFirstSource_IsSmaller()
        {
            IList<int> source1 = new List<int> { 13 };
            IList<int> source2 = new List<int> { 17, 23 };
            var result = source1.Zip(source2, (x, y) => x.ToString() + y.ToString());
            Assert.IsTrue(result.SequenceEqual(new[] { "1317" }), "Zip should ignore extra elements.");
        }

        [TestMethod]
        public void List_Zip_WithShorterSecondSource_IsSmaller()
        {
            IList<int> source1 = new List<int> { 13, 23 };
            IList<int> source2 = new List<int> { 17 };
            var result = source1.Zip(source2, (x, y) => x.ToString() + y.ToString());
            Assert.IsTrue(result.SequenceEqual(new[] { "1317" }), "Zip should ignore extra elements.");
        }

        [TestMethod]
        public void List_Zip3_ZipsElements()
        {
            IList<int> source1 = new List<int> { 13, 7 };
            IList<int> source2 = new List<int> { 17, 23 };
            IList<int> source3 = new List<int> { 27, 29 };
            var result = source1.Zip(source2, source3, (x, y, z) => x.ToString() + y.ToString() + z.ToString());
            Assert.IsTrue(result.SequenceEqual(new[] { "131727", "72329" }), "Zip should combine elements.");
        }

        [TestMethod]
        public void List_Zip3_WithShorterFirstSource_IsSmaller()
        {
            IList<int> source1 = new List<int> { 13 };
            IList<int> source2 = new List<int> { 17, 23 };
            IList<int> source3 = new List<int> { 27, 29 };
            var result = source1.Zip(source2, source3, (x, y, z) => x.ToString() + y.ToString() + z.ToString());
            Assert.IsTrue(result.SequenceEqual(new[] { "131727" }), "Zip should ignore extra elements.");
        }

        [TestMethod]
        public void List_Zip3_WithShorterSecondSource_IsSmaller()
        {
            IList<int> source1 = new List<int> { 13, 23 };
            IList<int> source2 = new List<int> { 17 };
            IList<int> source3 = new List<int> { 27, 29 };
            var result = source1.Zip(source2, source3, (x, y, z) => x.ToString() + y.ToString() + z.ToString());
            Assert.IsTrue(result.SequenceEqual(new[] { "131727" }), "Zip should ignore extra elements.");
        }

        [TestMethod]
        public void List_Zip3_WithShorterThirdSource_IsSmaller()
        {
            IList<int> source1 = new List<int> { 13, 23 };
            IList<int> source2 = new List<int> { 17, 23 };
            IList<int> source3 = new List<int> { 29 };
            var result = source1.Zip(source2, source3, (x, y, z) => x.ToString() + y.ToString() + z.ToString());
            Assert.IsTrue(result.SequenceEqual(new[] { "131729" }), "Zip should ignore extra elements.");
        }

        [TestMethod]
        public void List_LastIndexOf_WithValidItem_FindsItem()
        {
            IList<int> source = new List<int> { 13 };
            int result = source.LastIndexOf(13);
            Assert.AreEqual(0, result, "Item should be found.");
        }

        [TestMethod]
        public void List_LastIndexOf_WithInvalidItem_DoesNotFindItem()
        {
            IList<int> source = new List<int> { 13 };
            int result = source.LastIndexOf(17);
            Assert.AreEqual(-1, result, "Item should not be found.");
        }

        [TestMethod]
        public void List_LastIndexOf_WithSimpleMatch_FindsItem()
        {
            IList<int> source = new List<int> { 13 };
            int result = source.LastIndexOf(_ => true);
            Assert.AreEqual(0, result, "Item should be found.");
        }

        [TestMethod]
        public void List_LastIndexOf_WithInvalidMatch_DoesNotFindItem()
        {
            IList<int> source = new List<int> { 13 };
            int result = source.LastIndexOf(_ => false);
            Assert.AreEqual(-1, result, "Item should not be found.");
        }

        [TestMethod]
        public void List_LastIndexOf_WithMultipleMatches_FindsLastItem()
        {
            IList<int> source = new List<int> { 13, 15 };
            int result = source.LastIndexOf(_ => true);
            Assert.AreEqual(1, result, "Item should be found.");
        }

        [TestMethod]
        public void List_Last_WithSimpleMatch_FindsItem()
        {
            IList<int> source = new List<int> { 13 };
            int result = source.Last(_ => true);
            Assert.AreEqual(13, result, "Item should be found.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Item should not be found.")]
        public void List_Last_WithInvalidMatch_DoesNotFindItem()
        {
            IList<int> source = new List<int> { 13 };
            int result = source.Last(_ => false);
        }

        [TestMethod]
        public void List_Last_WithMultipleMatches_FindsLastItem()
        {
            IList<int> source = new List<int> { 13, 15 };
            int result = source.Last(_ => true);
            Assert.AreEqual(15, result, "Item should be found.");
        }

        [TestMethod]
        public void List_LastOrDefault_WithSimpleMatch_FindsItem()
        {
            IList<int> source = new List<int> { 13 };
            int result = source.LastOrDefault(_ => true);
            Assert.AreEqual(13, result, "Item should be found.");
        }

        [TestMethod]
        public void List_LastOrDefault_WithInvalidMatch_DoesNotFindItem()
        {
            IList<int> source = new List<int> { 13 };
            int result = source.LastOrDefault(_ => false);
            Assert.AreEqual(0, result, "Item should not be found.");
        }

        [TestMethod]
        public void List_LastOrDefault_WithMultipleMatches_FindsLastItem()
        {
            IList<int> source = new List<int> { 13, 15 };
            int result = source.LastOrDefault(_ => true);
            Assert.AreEqual(15, result, "Item should be found.");
        }

        [TestMethod]
        public void List_AsReadOnly_IsReadOnly()
        {
            IList<int> source = new List<int> { 13 };
            var result = source.AsReadOnly();
            Assert.IsTrue(result.IsReadOnly, "Read-only list should be read-only.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Read-only list should reject inserts.")]
        public void List_AsReadOnly_Insert_IsRejected()
        {
            IList<int> source = new List<int> { 13 };
            var result = source.AsReadOnly();
            result.Insert(0, 13);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Read-only list should reject adds.")]
        public void List_AsReadOnly_Add_IsRejected()
        {
            IList<int> source = new List<int> { 13 };
            var result = source.AsReadOnly();
            result.Add(13);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Read-only list should reject removes.")]
        public void List_AsReadOnly_Remove_IsRejected()
        {
            IList<int> source = new List<int> { 13 };
            var result = source.AsReadOnly();
            result.Remove(12);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Read-only list should reject RemoveAt.")]
        public void List_AsReadOnly_RemoveAt_IsRejected()
        {
            IList<int> source = new List<int> { 13 };
            var result = source.AsReadOnly();
            result.RemoveAt(0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Read-only list should reject Clear.")]
        public void List_AsReadOnly_Clear_IsRejected()
        {
            IList<int> source = new List<int> { 13 };
            var result = source.AsReadOnly();
            result.Clear();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Read-only list should reject setting items.")]
        public void List_AsReadOnly_SetItem_IsRejected()
        {
            IList<int> source = new List<int> { 13 };
            var result = source.AsReadOnly();
            result[0] = 17;
        }

        [TestMethod]
        public void List_GenerateWithIndex_GeneratesItems()
        {
            IList<int> source = ListSource.Generate(i => i, 3);
            Assert.IsTrue(source.SequenceEqual(new[] { 0, 1, 2 }), "Generate should generate a sequence.");
        }

        [TestMethod]
        public void List_GenerateWithIndex_WithZeroCount_GeneratesEmptySequence()
        {
            IList<int> source = ListSource.Generate(i => i, 0);
            Assert.IsTrue(source.SequenceEqual(new int[] { }), "Generate should generate an empty sequence.");
        }

        [TestMethod]
        public void List_GenerateWithIndex_WithNegativeCount_IsRejected()
        {
            IList<int> source = ListSource.Generate(i => i, -1);
            Assert.IsTrue(source.SequenceEqual(new int[] { }), "Generate should generate an empty sequence.");
        }

        [TestMethod]
        public void List_GenerateWithoutIndex_GeneratesItems()
        {
            IList<int> source = ListSource.Generate(() => 13, 3);
            Assert.IsTrue(source.SequenceEqual(new[] { 13, 13, 13 }), "Generate should generate a sequence.");
        }

        [TestMethod]
        public void List_GenerateWithoutIndex_WithZeroCount_GeneratesEmptySequence()
        {
            IList<int> source = ListSource.Generate(() => 13, 0);
            Assert.IsTrue(source.SequenceEqual(new int[] { }), "Generate should generate an empty sequence.");
        }

        [TestMethod]
        public void List_GenerateWithoutIndex_WithNegativeCount_IsRejected()
        {
            IList<int> source = ListSource.Generate(() => 13, -1);
            Assert.IsTrue(source.SequenceEqual(new int[] { }), "Generate should generate an empty sequence.");
        }

        [TestMethod]
        public void List_Rotate_EmptySequence_NegativeOffset_IsEmptySequence()
        {
            IList<int> source = new int[] { };
            var result = source.Rotate(-1);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Rotating an empty sequence should result in an empty sequence.");
        }

        [TestMethod]
        public void List_Rotate_EmptySequence_ZeroOffset_IsEmptySequence()
        {
            IList<int> source = new int[] { };
            var result = source.Rotate(0);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Rotating an empty sequence should result in an empty sequence.");
        }

        [TestMethod]
        public void List_Rotate_EmptySequence_PositiveOffset_IsEmptySequence()
        {
            IList<int> source = new int[] { };
            var result = source.Rotate(1);
            Assert.IsTrue(result.SequenceEqual(new int[] { }), "Rotating an empty sequence should result in an empty sequence.");
        }

        [TestMethod]
        public void List_Rotate_NegativeOffset_IsSameSequence()
        {
            IList<int> source = new[] { 1, 2, 3 };
            var result = source.Rotate(-1);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Rotating by negative offset should result in same sequence.");
        }

        [TestMethod]
        public void List_Rotate_ZeroOffset_IsSameSequence()
        {
            IList<int> source = new[] { 1, 2, 3 };
            var result = source.Rotate(0);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Rotating by zero offset should result in same sequence.");
        }

        [TestMethod]
        public void List_Rotate_RotatesSequence()
        {
            IList<int> source = new[] { 1, 2, 3 };
            var result = source.Rotate(1);
            Assert.IsTrue(result.SequenceEqual(new[] { 2, 3, 1 }), "Rotating should rotate sequence.");
        }

        [TestMethod]
        public void List_Rotate_CountOffset_IsSameSequence()
        {
            IList<int> source = new[] { 1, 2, 3 };
            var result = source.Rotate(3);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Rotating by offset equal to count should result in same sequence.");
        }

        [TestMethod]
        public void List_Rotate_GreaterThanCountOffset_IsSameSequence()
        {
            IList<int> source = new[] { 1, 2, 3 };
            var result = source.Rotate(4);
            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3 }), "Rotating by offset greater than count should result in same sequence.");
        }

        [TestMethod]
        public void List_SequenceEqual_DifferentSourceCounts_AreNotEqual()
        {
            IList<int> source1 = new[] { 1, 2 };
            IList<int> source2 = new[] { 1, 2, 3 };
            var result = source1.SequenceEqual(source2);
            Assert.IsFalse(result, "Sources with different counts should not be equal.");
        }

        [TestMethod]
        public void List_SequenceEqual_SameSourceCountsDifferentValues_AreNotEqual()
        {
            IList<int> source1 = new[] { 1, 2, 3 };
            IList<int> source2 = new[] { 1, 2, 5 };
            var result = source1.SequenceEqual(source2);
            Assert.IsFalse(result, "Sources with different values should not be equal.");
        }

        [TestMethod]
        public void List_SequenceEqual_IdenticalSources_AreEqual()
        {
            IList<int> source1 = new[] { 1, 2, 3 };
            IList<int> source2 = new[] { 1, 2, 3 };
            var result = source1.SequenceEqual(source2);
            Assert.IsTrue(result, "Identical sources should be equal.");
        }
    }
}

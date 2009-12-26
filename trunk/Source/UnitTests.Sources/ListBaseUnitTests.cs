using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.Linq.Implementation;

namespace UnitTests
{
    public partial class Tests
    {
        [TestMethod]
        public void ReadWriteList_IsNotReadOnly()
        {
            var list = new ReadWriteList<int>(new List<int> { 13 });
            Assert.IsFalse(list.IsReadOnly, "Read/write lists should not be read-only.");
        }

        [TestMethod]
        public void ReadOnlyList_IsReadOnly()
        {
            var list = new ReadOnlyList<int>(new List<int> { 13 });
            Assert.IsTrue(list.IsReadOnly, "Read-only lists should be read-only.");
        }

        [TestMethod]
        public void WriteableReadOnlyList_IsReadOnly()
        {
            var list = new WriteableReadOnlyList<int>(new List<int> { 13 });
            Assert.IsTrue(list.IsReadOnly, "Writeable read-only lists should be read-only.");
        }

        [TestMethod]
        public void List_GetItem_WithValidIndex_RetrievesItem()
        {
            var list = new ReadWriteList<int>(new List<int> { 13 });
            int result = list[0];
            Assert.AreEqual(13, result, "Item at valid index should be retrieved.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Retrieving items with a negative index should be rejected.")]
        public void List_GetItem_WithNegativeIndex_IsRejected()
        {
            var list = new ReadWriteList<int>(new List<int> { 13 });
            int result = list[-1];
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Retrieving items with an invalid index should be rejected.")]
        public void List_GetItem_WithInvalidIndex_IsRejected()
        {
            var list = new ReadWriteList<int>(new List<int> { 13 });
            int result = list[1];
        }

        [TestMethod]
        public void List_SetItem_WithValidIndex_SetsItem()
        {
            var source = new List<int> { 13 };
            var list = new ReadWriteList<int>(source);
            list[0] = 17;
            Assert.AreEqual(17, source[0], "Item at valid index should be set.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Setting items with a negative index should be rejected.")]
        public void List_SetItem_WithNegativeIndex_IsRejected()
        {
            var list = new ReadWriteList<int>(new List<int> { 13 });
            list[-1] = 17;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Setting items with an invalid index should be rejected.")]
        public void List_SetItem_WithInvalidIndex_IsRejected()
        {
            var list = new ReadWriteList<int>(new List<int> { 13 });
            list[1] = 17;
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Setting items on a read-only list should be rejected.")]
        public void List_SetItem_OnReadOnlyList_IsRejected()
        {
            var list = new ReadOnlyList<int>(new List<int> { 13 });
            list[0] = 17;
        }

        [TestMethod]
        public void List_SetItem_OnWriteableReadOnlyList_SetsItem()
        {
            var source = new List<int> { 13 };
            var list = new WriteableReadOnlyList<int>(source);
            list[0] = 17;
            Assert.AreEqual(17, source[0], "Item at valid index should be set.");
        }

        [TestMethod]
        public void List_AddItem_AddsItem()
        {
            var source = new List<int> { 13 };
            var list = new ReadWriteList<int>(source);
            list.Add(17);
            Assert.IsTrue(source.SequenceEqual(new [] { 13, 17 }), "Item should be added.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Adding items on a read-only list should be rejected.")]
        public void List_AddItem_OnReadOnlyList_IsRejected()
        {
            var source = new List<int> { 13 };
            var list = new ReadOnlyList<int>(source);
            list.Add(17);
        }

        [TestMethod]
        public void List_AddItem_OnWriteableReadOnlyList_AddsItem()
        {
            var source = new List<int> { 13 };
            var list = new WriteableReadOnlyList<int>(source);
            list.Add(17);
            Assert.IsTrue(source.SequenceEqual(new[] { 13, 17 }), "Item should be added.");
        }

        [TestMethod]
        public void List_Clear_ClearsItems()
        {
            var source = new List<int> { 13 };
            var list = new ReadWriteList<int>(source);
            list.Clear();
            Assert.IsTrue(source.SequenceEqual(new int[] { }), "Items should be cleared.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Clearing items on a read-only list should be rejected.")]
        public void List_Clear_OnReadOnlyList_IsRejected()
        {
            var source = new List<int> { 13 };
            var list = new ReadOnlyList<int>(source);
            list.Clear();
        }

        [TestMethod]
        public void List_Clear_OnWriteableReadOnlyList_ClearsItems()
        {
            var source = new List<int> { 13 };
            var list = new WriteableReadOnlyList<int>(source);
            list.Clear();
            Assert.IsTrue(source.SequenceEqual(new int[] { }), "Items should be cleared.");
        }

        [TestMethod]
        public void List_Contains_WithValidItem_FindsItem()
        {
            var source = new List<int> { 13 };
            var list = new ReadWriteList<int>(source);
            bool result = list.Contains(13);
            Assert.IsTrue(result, "Item should be found.");
        }

        [TestMethod]
        public void List_Contains_WithInvalidItem_DoesNotFindItem()
        {
            var source = new List<int> { 13 };
            var list = new ReadWriteList<int>(source);
            bool result = list.Contains(17);
            Assert.IsFalse(result, "Item should not be found.");
        }

        [TestMethod]
        public void List_CopyToArray_CopiesSequence()
        {
            var source = new List<int> { 1, 2, 3, 4 };
            var list = new ReadWriteList<int>(source);
            int[] array = new int[5];
            list.CopyTo(array, 1);
            Assert.IsTrue(array.SequenceEqual(new[] { 0, 1, 2, 3, 4 }), "List should copy to an array.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void List_CopyTo_NullArray_IsRejected()
        {
            var source = new List<int> { 1, 2, 3, 4 };
            var list = new ReadWriteList<int>(source);
            list.CopyTo(null, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void List_CopyToArray_WithNegativeIndex_IsRejected()
        {
            var source = new List<int> { 1, 2, 3, 4 };
            var list = new ReadWriteList<int>(source);
            int[] array = new int[5];
            list.CopyTo(array, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void List_CopyToArray_WithInvalidIndex_IsRejected()
        {
            var source = new List<int> { 1, 2, 3, 4 };
            var list = new ReadWriteList<int>(source);
            int[] array = new int[5];
            list.CopyTo(array, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void List_CopyTo_TooSmallArray_IsRejected()
        {
            var source = new List<int> { 1, 2, 3, 4 };
            var list = new ReadWriteList<int>(source);
            int[] array = new int[3];
            list.CopyTo(array, 0);
        }

        [TestMethod]
        public void List_SupportsGenericEnumerator()
        {
            var source = new List<int> { 1, 2, 3, 4 };
            var list = new ReadWriteList<int>(source);
            List<int> result = new List<int>((IEnumerable<int>)list);

            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "List should enumerate.");
        }

        [TestMethod]
        public void List_SupportsOldEnumerator()
        {
            var source = new List<int> { 1, 2, 3, 4 };
            var list = new ReadWriteList<int>(source);
            List<int> result = new List<int>();
            var enumerator = (list as System.Collections.IEnumerable).GetEnumerator();
            while (enumerator.MoveNext())
            {
                result.Add((int)enumerator.Current);
            }

            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }), "List should enumerate.");
        }

        [TestMethod]
        public void List_IndexOf_WithValidItem_FindsItem()
        {
            var source = new List<int> { 13 };
            var list = new ReadWriteList<int>(source);
            int result = list.IndexOf(13);
            Assert.AreEqual(0, result, "Item should be found.");
        }

        [TestMethod]
        public void List_IndexOf_WithInvalidItem_DoesNotFindItem()
        {
            var source = new List<int> { 13 };
            var list = new ReadWriteList<int>(source);
            int result = list.IndexOf(17);
            Assert.AreEqual(-1, result, "Item should not be found.");
        }

        [TestMethod]
        public void List_Insert_InsertsItem()
        {
            var source = new List<int> { 13 };
            var list = new ReadWriteList<int>(source);
            list.Insert(0, 17);
            Assert.IsTrue(source.SequenceEqual(new[] { 17, 13}), "Item should be inserted.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void List_Insert_WithNegativeIndex_IsRejected()
        {
            var source = new List<int> { 13 };
            var list = new ReadWriteList<int>(source);
            list.Insert(-1, 17);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void List_Insert_WithInvalidIndex_IsRejected()
        {
            var source = new List<int> { 13 };
            var list = new ReadWriteList<int>(source);
            list.Insert(2, 17);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void List_Insert_OnReadOnlyList_IsRejected()
        {
            var source = new List<int> { 13 };
            var list = new ReadOnlyList<int>(source);
            list.Insert(0, 17);
        }

        [TestMethod]
        public void List_Insert_OnWriteableReadOnlyList_InsertsItem()
        {
            var source = new List<int> { 13 };
            var list = new WriteableReadOnlyList<int>(source);
            list.Insert(0, 17);
            Assert.IsTrue(source.SequenceEqual(new[] { 17, 13 }), "Item should be inserted.");
        }

        [TestMethod]
        public void List_Remove_WithValidItem_RemovesItem()
        {
            var source = new List<int> { 13 };
            var list = new ReadWriteList<int>(source);
            bool result = list.Remove(13);
            Assert.IsTrue(result, "Item should be found.");
            Assert.IsTrue(source.SequenceEqual(new int[] { }), "Item should be removed.");
        }

        [TestMethod]
        public void List_Remove_WithInvalidItem_DoesNotFindItem()
        {
            var source = new List<int> { 13 };
            var list = new ReadWriteList<int>(source);
            bool result = list.Remove(17);
            Assert.IsFalse(result, "Item should not be found.");
            Assert.IsTrue(source.SequenceEqual(new[] { 13 }), "Item should not be removed.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void List_Remove_OnReadOnlyList_IsRejected()
        {
            var source = new List<int> { 13 };
            var list = new ReadOnlyList<int>(source);
            list.Remove(13);
        }

        [TestMethod]
        public void List_Remove_OnWriteableReadOnlyList_RemovesItem()
        {
            var source = new List<int> { 13 };
            var list = new WriteableReadOnlyList<int>(source);
            bool result = list.Remove(13);
            Assert.IsTrue(result, "Item should be found.");
            Assert.IsTrue(source.SequenceEqual(new int[] { }), "Item should be removed.");
        }

        [TestMethod]
        public void List_RemoveAt_RemovesItem()
        {
            var source = new List<int> { 13 };
            var list = new ReadWriteList<int>(source);
            list.RemoveAt(0);
            Assert.IsTrue(source.SequenceEqual(new int[] { }), "Item should be removed.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void List_RemoveAt_WithNegativeIndex_IsRejected()
        {
            var source = new List<int> { 13 };
            var list = new ReadWriteList<int>(source);
            list.RemoveAt(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void List_RemoveAt_WithInvalidIndex_IsRejected()
        {
            var source = new List<int> { 13 };
            var list = new ReadWriteList<int>(source);
            list.RemoveAt(1);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void List_RemoveAt_OnReadOnlyList_IsRejected()
        {
            var source = new List<int> { 13 };
            var list = new ReadOnlyList<int>(source);
            list.RemoveAt(0);
        }

        [TestMethod]
        public void List_RemoveAt_OnWriteableReadOnlyList_RemovesItem()
        {
            var source = new List<int> { 13 };
            var list = new WriteableReadOnlyList<int>(source);
            list.RemoveAt(0);
            Assert.IsTrue(source.SequenceEqual(new int[] { }), "Item should be removed.");
        }

        /// <summary>
        /// A standard read-only list, deriving from ReadOnlyListBase. This is a normal, expected usage of the base class.
        /// </summary>
        private sealed class ReadOnlyList<T> : ReadOnlyListBase<T>
        {
            List<T> list;

            public ReadOnlyList(List<T> list)
            {
                this.list = list;
            }

            public override int Count
            {
                get { return this.list.Count; }
            }

            protected override T DoGetItem(int index)
            {
                return this.list[index];
            }
        }

        /// <summary>
        /// A "read-only" list that allows some modifications. This is an advanced usage of the base class.
        /// </summary>
        private sealed class WriteableReadOnlyList<T> : ReadOnlyListBase<T>
        {
            List<T> list;

            public WriteableReadOnlyList(List<T> list)
            {
                this.list = list;
            }

            public override int Count
            {
                get { return this.list.Count; }
            }

            protected override T DoGetItem(int index)
            {
                return this.list[index];
            }

            protected override void DoSetItem(int index, T item)
            {
                this.list[index] = item;
            }

            protected override void DoInsert(int index, T item)
            {
                this.list.Insert(index, item);
            }

            protected override void DoRemoveAt(int index)
            {
                this.list.RemoveAt(index);
            }

            public override void Clear()
            {
                this.list.Clear();
            }
        }

        /// <summary>
        /// A read/write list, deriving from ListBase. This is a normal, expected usage of the base class.
        /// </summary>
        private sealed class ReadWriteList<T> : ListBase<T>
        {
            private List<T> list;

            public ReadWriteList(List<T> list)
            {
                this.list = list;
            }

            public override int Count
            {
                get { return this.list.Count; }
            }

            protected override T DoGetItem(int index)
            {
                return this.list[index];
            }

            protected override void DoSetItem(int index, T item)
            {
                this.list[index] = item;
            }

            protected override void DoInsert(int index, T item)
            {
                this.list.Insert(index, item);
            }

            protected override void DoRemoveAt(int index)
            {
                this.list.RemoveAt(index);
            }

            public override void Clear()
            {
                this.list.Clear();
            }
        }
    }
}

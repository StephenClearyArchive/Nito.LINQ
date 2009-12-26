// <copyright file="Implementation.SortedListWrapper.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito.Implementation
{
    using System.Collections.Generic;

    /// <summary>
    /// Wraps a source list and comparison object.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    internal sealed class SortedListWrapper<T> : Implementation.ListBase<T>, ISortedList<T>
    {
        /// <summary>
        /// The source list.
        /// </summary>
        private IList<T> source;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedListWrapper&lt;T&gt;"/> class with the specified source list and comparison object.
        /// </summary>
        /// <param name="source">The source list.</param>
        /// <param name="comparer">The comparison object.</param>
        public SortedListWrapper(IList<T> source, IComparer<T> comparer)
        {
            this.source = source;
            this.Comparer = comparer;
        }

        /// <summary>
        /// Gets a comparison object that defines how this list is sorted.
        /// </summary>
        public IComparer<T> Comparer { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this list is read-only. This list is read-only iff its source list is read-only.
        /// </summary>
        /// <returns>true if this list is read-only; otherwise, false.</returns>
        public override bool IsReadOnly
        {
            get { return this.source.IsReadOnly; }
        }

        /// <summary>
        /// Gets the number of elements contained in this list.
        /// </summary>
        /// <returns>The number of elements contained in this list.</returns>
        public override int Count
        {
            get { return this.source.Count; }
        }

        /// <summary>
        /// Removes all items from this list.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        /// This list is read-only.
        /// </exception>
        public override void Clear()
        {
            this.source.Clear();
        }

        /// <summary>
        /// Determines the index of a specific item in this list.
        /// </summary>
        /// <param name="item">The object to locate in this list.</param>
        /// <returns>
        /// The index of <paramref name="item"/> if found in this list; otherwise, -1.
        /// </returns>
        public override int IndexOf(T item)
        {
            int ret = this.LowerBound(item);
            if (ret >= 0)
            {
                return ret;
            }

            return -1;
        }

        /// <summary>
        /// Determines whether this list contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in this list.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in this list; otherwise, false.
        /// </returns>
        public override bool Contains(T item)
        {
            return this.BinarySearch(item) >= 0;
        }

        /// <summary>
        /// Gets an element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
        /// <returns>The element at the specified index.</returns>
        protected override T DoGetItem(int index)
        {
            return this.source[index];
        }

        /// <summary>
        /// Sets an element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
        /// <param name="item">The element to store in the list.</param>
        protected override void DoSetItem(int index, T item)
        {
            this.source[index] = item;
        }

        /// <summary>
        /// Inserts an element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the element should be inserted. This index is guaranteed to be valid.</param>
        /// <param name="item">The element to store in the list.</param>
        protected override void DoInsert(int index, T item)
        {
            this.source.Insert(index, item);
        }

        /// <summary>
        /// Removes an element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove. This index is guaranteed to be valid.</param>
        protected override void DoRemoveAt(int index)
        {
            this.source.RemoveAt(index);
        }
    }
}

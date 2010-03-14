// <copyright file="IndirectList.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito.Linq.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// An indirect list, which provides a layer of indirection for the index values of a source list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public sealed class IndirectList<T> : ReadOnlyListBase<T>
    {
        /// <summary>
        /// The redirected index values.
        /// </summary>
        private int[] indices;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndirectList&lt;T&gt;"/> class for the given source list.
        /// </summary>
        /// <param name="source">The source list. The number of elements in the source list may not change as long as this <see cref="IndirectList{T}"/> is reachable.</param>
        public IndirectList(IList<T> source)
        {
            this.Source = source;
            this.indices = new int[source.Count];
        }

        /// <summary>
        /// Gets the source list.
        /// </summary>
        public IList<T> Source { get; private set; }

        /// <summary>
        /// Gets the redirected index values. Elements in this list may be set, but not inserted or removed.
        /// </summary>
        public IList<int> Indices
        {
            get { return this.indices; }
        }

        /// <summary>
        /// Gets the number of elements contained in this list.
        /// </summary>
        public override int Count
        {
            get
            {
                if (this.Source.Count != this.indices.Length)
                {
                    throw new InvalidOperationException("Source list count changed when the source is being indirectly referenced");
                }

                return this.indices.Length;
            }
        }

        /// <summary>
        /// Returns an indirect comparer which may be used to sort or compare elements in <see cref="Indices"/>, based on the specified source comparer.
        /// </summary>
        /// <param name="comparer">The source comparer.</param>
        /// <returns>The indirect comparer.</returns>
        public IComparer<int> GetComparer(IComparer<T> comparer)
        {
            return new AnonymousComparer<int> { Compare = (x, y) => comparer.Compare(this[x], this[y]) };
        }

        /// <summary>
        /// Returns an indirect comparer which may be used to sort or compare elements in <see cref="Indices"/>, based on the default source comparer.
        /// </summary>
        /// <returns>The indirect comparer.</returns>
        public IComparer<int> GetComparer()
        {
            return this.GetComparer(Comparer<T>.Default);
        }

        /// <summary>
        /// Gets an element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
        /// <returns>The element at the specified index.</returns>
        protected override T DoGetItem(int index)
        {
            return this.Source[this.indices[index]];
        }

        /// <summary>
        /// Sets an element at the specified index. This implementation always throws <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
        /// <param name="item">The element to store in the list.</param>
        protected override void DoSetItem(int index, T item)
        {
            this.Source[this.indices[index]] = item;
        }
    }
}

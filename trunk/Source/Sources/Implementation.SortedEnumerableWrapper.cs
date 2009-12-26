// <copyright file="Implementation.SortedEnumerableWrapper.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito.Linq.Implementation
{
    using System.Collections.Generic;

    /// <summary>
    /// Wraps a source sequence and comparison object.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    internal sealed class SortedEnumerableWrapper<T> : ISortedEnumerable<T>
    {
        /// <summary>
        /// The source sequence.
        /// </summary>
        private IEnumerable<T> source;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedEnumerableWrapper&lt;T&gt;"/> class with the specified source sequence and comparison object.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="comparer">The comparison object.</param>
        public SortedEnumerableWrapper(IEnumerable<T> source, IComparer<T> comparer)
        {
            this.source = source;
            this.Comparer = comparer;
        }

        /// <summary>
        /// Gets a comparison object that defines how this sequence is sorted.
        /// </summary>
        public IComparer<T> Comparer { get; private set; }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.source.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

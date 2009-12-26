// <copyright file="ListExtensions.ConcatList.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito.Linq
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides useful extension methods for the <see cref="List{T}"/> class.
    /// </summary>
    public static partial class ListExtensions
    {
        /// <summary>
        /// Concatenates a list of source lists into a single list.
        /// </summary>
        /// <typeparam name="T">The type of object contained in the list.</typeparam>
        private sealed class ConcatList<T> : Implementation.ListBase<T>
        {
            /// <summary>
            /// The list of source lists.
            /// </summary>
            private readonly IList<IList<T>> sources;

            /// <summary>
            /// Initializes a new instance of the <see cref="ConcatList&lt;T&gt;"/> class with the specified source lists.
            /// </summary>
            /// <param name="sources">The source lists to concatenate.</param>
            public ConcatList(IList<IList<T>> sources)
            {
                this.sources = sources;
            }

            /// <summary>
            /// Gets a value indicating whether this list is read-only. This list is read-only if any of its source lists are read-only.
            /// </summary>
            /// <returns>true if this list is read-only; otherwise, false.</returns>
            public override bool IsReadOnly
            {
                get { return this.sources.Any(x => x.IsReadOnly); }
            }

            /// <summary>
            /// Gets the number of elements contained in this list.
            /// </summary>
            /// <returns>The number of elements contained in this list.</returns>
            public override int Count
            {
                get { return this.sources.Sum(x => x.Count); }
            }

            /// <summary>
            /// Removes all elements from the list.
            /// </summary>
            public override void Clear()
            {
                foreach (IList<T> source in this.sources)
                {
                    source.Clear();
                }
            }

            /// <summary>
            /// Gets an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
            /// <returns>The element at the specified index.</returns>
            protected override T DoGetItem(int index)
            {
                IList<T> source;
                int sourceIndex;
                this.FindExistingIndex(index, out source, out sourceIndex);
                return source[sourceIndex];
            }

            /// <summary>
            /// Sets an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
            /// <param name="item">The element to store in the list.</param>
            protected override void DoSetItem(int index, T item)
            {
                IList<T> source;
                int sourceIndex;
                this.FindExistingIndex(index, out source, out sourceIndex);
                source[sourceIndex] = item;
            }

            /// <summary>
            /// Inserts an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index at which the element should be inserted. This index is guaranteed to be valid.</param>
            /// <param name="item">The element to store in the list.</param>
            protected override void DoInsert(int index, T item)
            {
                IList<T> source;
                int sourceIndex;
                this.FindNewIndex(index, out source, out sourceIndex);
                source.Insert(sourceIndex, item);
            }

            /// <summary>
            /// Removes an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the element to remove. This index is guaranteed to be valid.</param>
            protected override void DoRemoveAt(int index)
            {
                IList<T> source;
                int sourceIndex;
                this.FindExistingIndex(index, out source, out sourceIndex);
                source.RemoveAt(sourceIndex);
            }

            /// <summary>
            /// Finds the source list and its index for accessing a specified concatenated index.
            /// </summary>
            /// <param name="concatIndex">The concatenated index.</param>
            /// <param name="source">On return, holds the source list corresponding to the concatenated index.</param>
            /// <param name="sourceIndex">On return, holds the source list index corresponding to the concatenated index.</param>
            private void FindExistingIndex(int concatIndex, out IList<T> source, out int sourceIndex)
            {
                source = null;
                sourceIndex = concatIndex;
                foreach (IList<T> sourceList in this.sources)
                {
                    if (sourceIndex < sourceList.Count)
                    {
                        source = sourceList;
                        return;
                    }

                    sourceIndex -= sourceList.Count;
                }
            }

            /// <summary>
            /// Finds the source list and its index for inserting at a specified concatenated index.
            /// </summary>
            /// <param name="concatIndex">The concatenated index at which to insert.</param>
            /// <param name="source">On return, holds the source list corresponding to the concatenated index.</param>
            /// <param name="sourceIndex">On return, holds the source list index corresponding to the concatenated index.</param>
            private void FindNewIndex(int concatIndex, out IList<T> source, out int sourceIndex)
            {
                source = null;
                sourceIndex = concatIndex;
                foreach (IList<T> sourceList in this.sources)
                {
                    if (sourceIndex <= sourceList.Count)
                    {
                        source = sourceList;
                        return;
                    }

                    sourceIndex -= sourceList.Count;
                }
            }
        }
    }
}

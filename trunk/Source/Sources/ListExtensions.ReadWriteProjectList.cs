// <copyright file="ListExtensions.ReadWriteProjectList.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides useful extension methods for the <see cref="List{T}"/> class.
    /// </summary>
    public static partial class ListExtensions
    {
        /// <summary>
        /// Projects a source list to a result list, and projects the result list back to the source list.
        /// </summary>
        /// <typeparam name="TSource">The type of object contained in the source list.</typeparam>
        /// <typeparam name="TResult">The type of object contained in the resulting list.</typeparam>
        private sealed class ReadWriteProjectList<TSource, TResult> : Implementation.ListBase<TResult>
        {
            /// <summary>
            /// The source list.
            /// </summary>
            private readonly IList<TSource> source;

            /// <summary>
            /// The projection function from source to result.
            /// </summary>
            private readonly Func<TSource, TResult> selector;

            /// <summary>
            /// The projection function from result to source.
            /// </summary>
            private readonly Func<TResult, TSource> reverseSelector;

            /// <summary>
            /// Initializes a new instance of the <see cref="ReadWriteProjectList{TSource,TResult}"/> class.
            /// </summary>
            /// <param name="source">The source list.</param>
            /// <param name="selector">The projection function from source to result.</param>
            /// <param name="reverseSelector">The projection function from result to source.</param>
            public ReadWriteProjectList(IList<TSource> source, Func<TSource, TResult> selector, Func<TResult, TSource> reverseSelector)
            {
                this.source = source;
                this.selector = selector;
                this.reverseSelector = reverseSelector;
            }

            /// <summary>
            /// Gets a value indicating whether this list is read-only. This list is read-only if its source list is read-only.
            /// </summary>
            /// <value></value>
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
            /// Removes all elements from the list.
            /// </summary>
            public override void Clear()
            {
                this.source.Clear();
            }

            /// <summary>
            /// Gets an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
            /// <returns>The element at the specified index.</returns>
            protected override TResult DoGetItem(int index)
            {
                return this.selector(this.source[index]);
            }

            /// <summary>
            /// Sets an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
            /// <param name="item">The element to store in the list.</param>
            protected override void DoSetItem(int index, TResult item)
            {
                this.source[index] = this.reverseSelector(item);
            }

            /// <summary>
            /// Inserts an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index at which the element should be inserted. This index is guaranteed to be valid.</param>
            /// <param name="item">The element to store in the list.</param>
            protected override void DoInsert(int index, TResult item)
            {
                this.source.Insert(index, this.reverseSelector(item));
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
}

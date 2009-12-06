// <copyright file="ListExtensions.ProjectList.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito
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
        /// Projects a source list.
        /// </summary>
        /// <typeparam name="TSource">The type of object contained in the source list.</typeparam>
        /// <typeparam name="TResult">The type of object contained in the resulting list.</typeparam>
        private sealed class ProjectList<TSource, TResult> : Implementation.ReadOnlyListBase<TResult>
        {
            /// <summary>
            /// The source list.
            /// </summary>
            private readonly IList<TSource> source;

            /// <summary>
            /// The projection function.
            /// </summary>
            private readonly Func<TSource, int, TResult> selector;

            /// <summary>
            /// Initializes a new instance of the <see cref="ProjectList{TSource,TResult}"/> class.
            /// </summary>
            /// <param name="source">The source list.</param>
            /// <param name="selector">The projection function.</param>
            public ProjectList(IList<TSource> source, Func<TSource, int, TResult> selector)
            {
                this.source = source;
                this.selector = selector;
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
            /// Gets an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
            /// <returns>The element at the specified index.</returns>
            protected override TResult DoGetItem(int index)
            {
                return this.selector(this.source[index], index);
            }
        }
    }
}

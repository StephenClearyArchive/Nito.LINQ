// <copyright file="ListExtensions.RepeatList.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides useful extension methods for the <see cref="List{T}"/> class.
    /// </summary>
    public static partial class ListExtensions
    {
        /// <summary>
        /// Repeats a source list a specified number of times.
        /// </summary>
        /// <typeparam name="T">The type of object contained in the list.</typeparam>
        private sealed class RepeatList<T> : Implementation.ReadOnlyListBase<T>
        {
            /// <summary>
            /// The source list.
            /// </summary>
            private readonly IList<T> source;

            /// <summary>
            /// The number of times to repeat the source list.
            /// </summary>
            private readonly int repeatCount;

            /// <summary>
            /// Initializes a new instance of the <see cref="RepeatList&lt;T&gt;"/> class.
            /// </summary>
            /// <param name="source">The source list.</param>
            /// <param name="count">The number of times to repeat the source list.</param>
            /// <exception cref="OverflowException">If the number of elements in the source multiplied by the number of times to repeat the source would overflow a 32-bit signed integer.</exception>
            public RepeatList(IList<T> source, int count)
            {
                this.source = source;
                this.repeatCount = Math.Max(0, count);
                int limitCheck = checked(this.repeatCount * this.source.Count);
            }

            /// <summary>
            /// Gets the number of elements contained in this list.
            /// </summary>
            /// <value></value>
            /// <returns>The number of elements contained in this list.</returns>
            public override int Count
            {
                get { return this.repeatCount * this.source.Count; }
            }

            /// <summary>
            /// Gets an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
            /// <returns>The element at the specified index.</returns>
            protected override T DoGetItem(int index)
            {
                return this.source[index % this.source.Count];
            }
        }
    }
}

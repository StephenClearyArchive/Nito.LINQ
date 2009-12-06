// <copyright file="ListExtensions.GenerateList.cs" company="Nito Programs">
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
        /// A list that generates its elements on demand.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        private sealed class GenerateList<T> : Implementation.ReadOnlyListBase<T>
        {
            /// <summary>
            /// The number of elements in this list.
            /// </summary>
            private readonly int count;

            /// <summary>
            /// The delegate that is used to generate the elements.
            /// </summary>
            private readonly Func<int, T> generator;

            /// <summary>
            /// Initializes a new instance of the <see cref="GenerateList&lt;T&gt;"/> class.
            /// </summary>
            /// <param name="count">The number of elements in this list.</param>
            /// <param name="generator">The delegate that is used to generate the elements.</param>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
            public GenerateList(int count, Func<int, T> generator)
            {
                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException("count", "Generated list must have a count of at least 0");
                }

                this.count = count;
                this.generator = generator;
            }

            /// <summary>
            /// Gets the number of elements contained in this list.
            /// </summary>
            /// <value></value>
            /// <returns>The number of elements contained in this list.</returns>
            public override int Count
            {
                get { return this.count; }
            }

            /// <summary>
            /// Gets an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
            /// <returns>The element at the specified index.</returns>
            protected override T DoGetItem(int index)
            {
                return this.generator(index);
            }
        }
    }
}

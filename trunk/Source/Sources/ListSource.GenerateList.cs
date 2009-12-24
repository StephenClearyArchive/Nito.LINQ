// <copyright file="ListSource.GenerateList.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// List sources.
    /// </summary>
    public static partial class ListSource
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
            /// <param name="generator">The delegate that is used to generate the elements. May be <c>null</c> if <paramref name="count"/> is less than or equal to zero.</param>
            /// <param name="count">The number of elements in this list. If less than or equal to 0, then this is an empty list.</param>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
            public GenerateList(Func<int, T> generator, int count)
            {
                this.count = Math.Max(count, 0);
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

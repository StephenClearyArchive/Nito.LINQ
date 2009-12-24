// <copyright file="ListSource.ValueList.cs" company="Nito Programs">
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
        /// A read-only list of identical values.
        /// </summary>
        /// <typeparam name="T">The type of object contained in the list.</typeparam>
        private sealed class ValueList<T> : Implementation.ReadOnlyListBase<T>
        {
            /// <summary>
            /// The value of every item in this list.
            /// </summary>
            private readonly T value;

            /// <summary>
            /// The number of items in this list.
            /// </summary>
            private readonly int count;

            /// <summary>
            /// Initializes a new instance of the <see cref="ValueList&lt;T&gt;"/> class with the specified value and number of values.
            /// </summary>
            /// <param name="value">The value of every item in this list.</param>
            /// <param name="count">The number of items in this list.</param>
            public ValueList(T value, int count)
            {
                this.value = value;
                this.count = Math.Max(0, count);
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
            /// Gets an element at the specified index. Always returns the same value.
            /// </summary>
            /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
            /// <returns>The element at the specified index.</returns>
            protected override T DoGetItem(int index)
            {
                return this.value;
            }
        }
    }
}

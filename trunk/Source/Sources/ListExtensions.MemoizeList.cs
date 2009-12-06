// <copyright file="ListExtensions.MemoizeList.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides useful extension methods for the <see cref="List{T}"/> class.
    /// </summary>
    public static partial class ListExtensions
    {
        /// <summary>
        /// A caching wrapper around a source list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        private sealed class MemoizeList<T> : Implementation.ReadOnlyListBase<T>
        {
            /// <summary>
            /// The source list.
            /// </summary>
            private readonly IList<T> source;

            /// <summary>
            /// Whether each entry in <see cref="values"/> is valid or not. An entry is valid iff it has already been read. This <see cref="BitArray"/> has the same length as <see cref="source"/> and <see cref="values"/>.
            /// </summary>
            private readonly BitArray valid;

            /// <summary>
            /// The cache of values read from <see cref="source"/>. Each entry in this array may be valid or invalid, as determined by <see cref="valid"/>. This array has the same length as <see cref="valid"/> and <see cref="source"/>.
            /// </summary>
            private readonly T[] values;

            /// <summary>
            /// Initializes a new instance of the <see cref="MemoizeList&lt;T&gt;"/> class.
            /// </summary>
            /// <param name="source">The source list.</param>
            public MemoizeList(IList<T> source)
            {
                this.source = source;
                int count = source.Count;
                this.valid = new BitArray(count);
                this.values = new T[count];
            }

            /// <summary>
            /// Gets the number of elements contained in this list.
            /// </summary>
            /// <value></value>
            /// <returns>The number of elements contained in this list.</returns>
            public override int Count
            {
                get { return this.valid.Count; }
            }

            /// <summary>
            /// Gets an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
            /// <returns>The element at the specified index.</returns>
            protected override T DoGetItem(int index)
            {
                if (this.valid[index])
                {
                    return this.values[index];
                }
                else
                {
                    T ret = this.source[index];
                    this.values[index] = ret;
                    this.valid[index] = true;
                    return ret;
                }
            }
        }
    }
}

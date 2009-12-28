// <copyright file="ListExtensions.GenericList.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito.Linq
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Provides useful extension methods for the <see cref="List{T}"/> class.
    /// </summary>
    public static partial class ListExtensions
    {
        /// <summary>
        /// Wraps a non-generic source list, treating it as a generic list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        private sealed class GenericList<T> : Implementation.ListBase<T>
        {
            /// <summary>
            /// The wrapped non-generic list.
            /// </summary>
            private IList source;

            /// <summary>
            /// Backing field for <see cref="IsReadOnly"/>.
            /// </summary>
            private bool isReadOnly;

            /// <summary>
            /// Initializes a new instance of the <see cref="GenericList&lt;T&gt;"/> class with the specified source and read-only setting.
            /// </summary>
            /// <param name="source">The non-generic source list to wrap.</param>
            /// <param name="isReadOnly">The read-only setting for this list.</param>
            public GenericList(IList source, bool isReadOnly)
            {
                this.source = source;
                this.isReadOnly = isReadOnly;
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
            /// Gets a value indicating whether this list is read-only.
            /// </summary>
            /// <returns>true if this list is read-only; otherwise, false.</returns>
            public override bool IsReadOnly
            {
                get
                {
                    return this.isReadOnly;
                }
            }

            /// <summary>
            /// Removes all items from this list.
            /// </summary>
            /// <exception cref="T:System.NotSupportedException">
            /// This list is read-only.
            /// </exception>
            public override void Clear()
            {
                this.source.Clear();
            }

            /// <summary>
            /// Gets an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
            /// <returns>The element at the specified index.</returns>
            protected override T DoGetItem(int index)
            {
                return (T)this.source[index];
            }

            /// <summary>
            /// Sets an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
            /// <param name="item">The element to store in the list.</param>
            protected override void DoSetItem(int index, T item)
            {
                this.source[index] = item;
            }

            /// <summary>
            /// Inserts an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index at which the element should be inserted. This index is guaranteed to be valid.</param>
            /// <param name="item">The element to store in the list.</param>
            protected override void DoInsert(int index, T item)
            {
                this.source.Insert(index, item);
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

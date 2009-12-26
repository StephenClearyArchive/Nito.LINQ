// <copyright file="Implementation.AnonymousReadOnlyList.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito.Implementation
{
    using System;

    /// <summary>
    /// Provides a delegate-based implementation of a read-only list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    internal sealed class AnonymousReadOnlyList<T> : ReadOnlyListBase<T>
    {
        /// <summary>
        /// The delegate used to get the number of items in the list.
        /// </summary>
        private Func<int> count;

        /// <summary>
        /// The delegate used to get items in the list.
        /// </summary>
        private Func<int, T> getItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymousReadOnlyList&lt;T&gt;"/> class with the specified implementation.
        /// </summary>
        /// <param name="getItem">The delegate used to get items in the list.</param>
        /// <param name="count">The delegate used to get the number of items in the list.</param>
        public AnonymousReadOnlyList(Func<int, T> getItem, Func<int> count)
        {
            this.count = count;
            this.getItem = getItem;
        }

        /// <summary>
        /// Gets the number of elements contained in this list.
        /// </summary>
        /// <returns>The number of elements contained in this list.</returns>
        public override int Count
        {
            get { return this.count(); }
        }

        /// <summary>
        /// Gets an element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
        /// <returns>The element at the specified index.</returns>
        protected override T DoGetItem(int index)
        {
            return this.getItem(index);
        }
    }
}

// <copyright file="Implementation.ReadOnlyListBase.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito.Implementation
{
    using System;

    /// <summary>
    /// Provides implementations of most list methods, for any list types that either do not allow item modification OR do not allow list modification. The default implementations of the methods in this class do not allow either.
    /// </summary>
    /// <typeparam name="T">The type of item contained in the list.</typeparam>
    public abstract class ReadOnlyListBase<T> : ListBase<T>
    {
        /// <summary>
        /// Gets a value indicating whether this list is read-only. This implementation always returns <c>true</c>.
        /// </summary>
        /// <value></value>
        /// <returns>true if this list is read-only; otherwise, false.</returns>
        public override bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Removes all elements from the list. This implementation always throws <see cref="NotSupportedException"/>.
        /// </summary>
        public override void Clear()
        {
            throw this.ReadOnlyException();
        }

        /// <summary>
        /// Returns an exception stating that this list is read-only.
        /// </summary>
        /// <returns>A <see cref="NotSupportedException"/> indicating that the list is read-only.</returns>
        protected virtual NotSupportedException ReadOnlyException()
        {
            return new NotSupportedException("This operation is not supported on a read-only list.");
        }

        /// <summary>
        /// Sets an element at the specified index. This implementation always throws <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
        /// <param name="item">The element to store in the list.</param>
        protected override void DoSetItem(int index, T item)
        {
            throw this.ReadOnlyException();
        }

        /// <summary>
        /// Inserts an element at the specified index. This implementation always throws <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="index">The zero-based index at which the element should be inserted. This index is guaranteed to be valid.</param>
        /// <param name="item">The element to store in the list.</param>
        protected override void DoInsert(int index, T item)
        {
            throw this.ReadOnlyException();
        }

        /// <summary>
        /// Removes an element at the specified index. This implementation always throws <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove. This index is guaranteed to be valid.</param>
        protected override void DoRemoveAt(int index)
        {
            throw this.ReadOnlyException();
        }
    }
}

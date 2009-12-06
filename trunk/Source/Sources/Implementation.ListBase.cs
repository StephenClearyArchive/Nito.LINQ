// <copyright file="Implementation.ListBase.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides common implementations of some list methods.
    /// </summary>
    /// <typeparam name="T">The type of element contained in the list.</typeparam>
    public abstract class ListBase<T> : IList<T>
    {
        /// <summary>
        /// Gets the number of elements contained in this list.
        /// </summary>
        /// <returns>The number of elements contained in this list.</returns>
        public abstract int Count { get; }

        /// <summary>
        /// Gets a value indicating whether this list is read-only. This implementation always returns <c>false</c>.
        /// </summary>
        /// <returns>true if this list is read-only; otherwise, false.</returns>
        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item to get or set.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is not a valid index in this list.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// This property is set and the list is read-only.
        /// </exception>
        public virtual T this[int index]
        {
            get
            {
                ListHelper.CheckExistingIndexArgument(this.Count, index);
                return this.DoGetItem(index);
            }

            set
            {
                ListHelper.CheckExistingIndexArgument(this.Count, index);
                this.DoSetItem(index, value);
            }
        }

        /// <summary>
        /// Inserts an item to this list at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into this list.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is not a valid index in this list.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// This list is read-only.
        /// </exception>
        public virtual void Insert(int index, T item)
        {
            ListHelper.CheckNewIndexArgument(this.Count, index);
            this.DoInsert(index, item);
        }

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is not a valid index in this list.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// This list is read-only.
        /// </exception>
        public virtual void RemoveAt(int index)
        {
            ListHelper.CheckExistingIndexArgument(this.Count, index);
            this.DoRemoveAt(index);
        }

        /// <summary>
        /// Removes all items from this list.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        /// This list is read-only.
        /// </exception>
        public abstract void Clear();

        /// <summary>
        /// Determines the index of a specific item in this list.
        /// </summary>
        /// <param name="item">The object to locate in this list.</param>
        /// <returns>The index of <paramref name="item"/> if found in this list; otherwise, -1.</returns>
        public virtual int IndexOf(T item)
        {
            return ((IEnumerable<T>)this).IndexOf(item);
        }

        /// <summary>
        /// Adds an item to the end of this list.
        /// </summary>
        /// <param name="item">The object to add to this list.</param>
        /// <exception cref="T:System.NotSupportedException">
        /// This list is read-only.
        /// </exception>
        public virtual void Add(T item)
        {
            this.DoInsert(this.Count, item);
        }

        /// <summary>
        /// Determines whether this list contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in this list.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in this list; otherwise, false.
        /// </returns>
        public virtual bool Contains(T item)
        {
            return this.Contains(item, null);
        }

        /// <summary>
        /// Copies the elements of this list to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from this slice. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="array"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="arrayIndex"/> is less than 0.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
        /// -or-
        /// The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
        /// </exception>
        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array", "Array is null");
            }

            int count = this.Count;
            ListHelper.CheckRangeArguments(array.Length, arrayIndex, count);
            for (int i = 0; i != count; ++i)
            {
                array[arrayIndex + i] = this[i];
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from this list.
        /// </summary>
        /// <param name="item">The object to remove from this list.</param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from this list; otherwise, false. This method also returns false if <paramref name="item"/> is not found in this list.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// This list is read-only.
        /// </exception>
        public virtual bool Remove(T item)
        {
            int index = this.IndexOf(item);
            if (index == -1)
            {
                return false;
            }

            this.DoRemoveAt(index);
            return true;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            int count = this.Count;
            for (int i = 0; i != count; ++i)
            {
                yield return this.DoGetItem(i);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Gets an element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
        /// <returns>The element at the specified index.</returns>
        protected abstract T DoGetItem(int index);

        /// <summary>
        /// Sets an element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
        /// <param name="item">The element to store in the list.</param>
        protected abstract void DoSetItem(int index, T item);

        /// <summary>
        /// Inserts an element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the element should be inserted. This index is guaranteed to be valid.</param>
        /// <param name="item">The element to store in the list.</param>
        protected abstract void DoInsert(int index, T item);

        /// <summary>
        /// Removes an element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove. This index is guaranteed to be valid.</param>
        protected abstract void DoRemoveAt(int index);
    }
}

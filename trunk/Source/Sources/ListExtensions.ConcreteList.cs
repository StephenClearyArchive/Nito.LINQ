// <copyright file="ListExtensions.ConcreteList.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito.Linq
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides useful extension methods for the <see cref="List{T}"/> class.
    /// </summary>
    public static partial class ListExtensions
    {
        /// <summary>
        /// Wraps a generic source list, treating it as a non-generic list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        private sealed class ConcreteList<T> : Implementation.ListBase<T>, IList
        {
            /// <summary>
            /// The wrapped generic list.
            /// </summary>
            private IList<T> source;

            /// <summary>
            /// Backing field for <see cref="IList.IsFixedSize"/>.
            /// </summary>
            private bool isFixedSize;

            /// <summary>
            /// Backing field for <see cref="IList.IsReadOnly"/>.
            /// </summary>
            private bool isReadOnly;

            /// <summary>
            /// Backing field for <see cref="ICollection.SyncRoot"/>.
            /// </summary>
            private object syncRoot;

            /// <summary>
            /// Initializes a new instance of the <see cref="ConcreteList&lt;T&gt;"/> class.
            /// </summary>
            /// <param name="source">The generic source list to wrap.</param>
            /// <param name="isFixedSize">The fixed-size setting for this list.</param>
            /// <param name="isReadOnly">The read-only setting for this list.</param>
            public ConcreteList(IList<T> source, bool isFixedSize, bool isReadOnly)
            {
                this.source = source;
                this.isFixedSize = isFixedSize;
                this.isReadOnly = isReadOnly;
                this.syncRoot = new object();
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
                get { return this.source.IsReadOnly; }
            }

            /// <summary>
            /// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> has a fixed size.
            /// </summary>
            /// <returns>true if the <see cref="T:System.Collections.IList"/> has a fixed size; otherwise, false.
            /// </returns>
            bool IList.IsFixedSize
            {
                get { return this.isFixedSize; }
            }

            /// <summary>
            /// Gets a value indicating whether this list is read-only.
            /// </summary>
            /// <returns>true if this list is read-only; otherwise, false.</returns>
            bool IList.IsReadOnly
            {
                get { return this.isReadOnly; }
            }

            /// <summary>
            /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
            /// </summary>
            /// <returns>true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.
            /// </returns>
            bool ICollection.IsSynchronized
            {
                get { return false; }
            }

            /// <summary>
            /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
            /// </summary>
            /// <returns>
            /// An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
            /// </returns>
            object ICollection.SyncRoot
            {
                get { return this.syncRoot; }
            }

            /// <summary>
            /// Gets or sets the <see cref="System.Object"/> at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index at which to get or set the item.</param>
            /// <exception cref="T:System.ArgumentOutOfRangeException">
            /// <paramref name="index"/> is not a valid index in this list.
            /// </exception>
            /// <exception cref="T:System.NotSupportedException">
            /// This property is set and the list is read-only.
            /// </exception>
            /// <exception cref="ArgumentException">This property is set and the type of the item indicates it is not appropriate for storing in this list.</exception>
            object IList.this[int index]
            {
                get
                {
                    return this[index];
                }

                set
                {
                    if (!this.ObjectIsT(value))
                    {
                        throw this.WrongObjectType();
                    }

                    this[index] = (T)value;
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
            /// Adds an item to the <see cref="T:System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The <see cref="T:System.Object"/> to add to the <see cref="T:System.Collections.IList"/>.</param>
            /// <returns>
            /// The position into which the new element was inserted.
            /// </returns>
            /// <exception cref="T:System.NotSupportedException">
            /// The <see cref="T:System.Collections.IList"/> is read-only.
            /// -or-
            /// The <see cref="T:System.Collections.IList"/> has a fixed size.
            /// </exception>
            /// <exception cref="ArgumentException">The type of the item indicates it is not appropriate for storing in this list.</exception>
            int IList.Add(object value)
            {
                if (!this.ObjectIsT(value))
                {
                    throw this.WrongObjectType();
                }

                this.Add((T)value);
                return this.Count - 1;
            }

            /// <summary>
            /// Determines whether the <see cref="T:System.Collections.IList"/> contains a specific value.
            /// </summary>
            /// <param name="value">The <see cref="T:System.Object"/> to locate in the <see cref="T:System.Collections.IList"/>.</param>
            /// <returns>
            /// true if the <see cref="T:System.Object"/> is found in the <see cref="T:System.Collections.IList"/>; otherwise, false.
            /// </returns>
            bool IList.Contains(object value)
            {
                if (!this.ObjectIsT(value))
                {
                    return false;
                }

                return this.Contains((T)value);
            }

            /// <summary>
            /// Determines the index of a specific item in the <see cref="T:System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The <see cref="T:System.Object"/> to locate in the <see cref="T:System.Collections.IList"/>.</param>
            /// <returns>
            /// The index of <paramref name="value"/> if found in the list; otherwise, -1.
            /// </returns>
            int IList.IndexOf(object value)
            {
                if (!this.ObjectIsT(value))
                {
                    return -1;
                }

                return this.IndexOf((T)value);
            }

            /// <summary>
            /// Inserts an item to the <see cref="T:System.Collections.IList"/> at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index at which <paramref name="value"/> should be inserted.</param>
            /// <param name="value">The <see cref="T:System.Object"/> to insert into the <see cref="T:System.Collections.IList"/>.</param>
            /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>.</exception>
            /// <exception cref="T:System.NotSupportedException">
            /// The <see cref="T:System.Collections.IList"/> is read-only.
            /// -or-
            /// The <see cref="T:System.Collections.IList"/> has a fixed size.
            /// </exception>
            /// <exception cref="ArgumentException">The type of the item indicates it is not appropriate for storing in this list.</exception>
            void IList.Insert(int index, object value)
            {
                if (!this.ObjectIsT(value))
                {
                    throw this.WrongObjectType();
                }

                this.Insert(index, (T)value);
            }

            /// <summary>
            /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The <see cref="T:System.Object"/> to remove from the <see cref="T:System.Collections.IList"/>.</param>
            /// <exception cref="T:System.NotSupportedException">
            /// The <see cref="T:System.Collections.IList"/> is read-only.
            /// -or-
            /// The <see cref="T:System.Collections.IList"/> has a fixed size.
            /// </exception>
            void IList.Remove(object value)
            {
                if (!this.ObjectIsT(value))
                {
                    return;
                }

                this.Remove((T)value);
            }

            /// <summary>
            /// Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
            /// </summary>
            /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
            /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
            /// <exception cref="T:System.ArgumentNullException">
            /// <paramref name="array"/> is null.
            /// </exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException">
            /// <paramref name="index"/> is less than zero.
            /// </exception>
            /// <exception cref="T:System.ArgumentException">
            /// <paramref name="array"/> is multidimensional.
            /// -or-
            /// <paramref name="index"/> is equal to or greater than the length of <paramref name="array"/>.
            /// -or-
            /// The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>.
            /// </exception>
            /// <exception cref="T:System.ArgumentException">
            /// The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
            /// </exception>
            void ICollection.CopyTo(Array array, int index)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array", "Array is null.");
                }

                if (array.Rank != 1)
                {
                    throw new ArgumentException("Multidimensional arrays are not supported.");
                }

                try
                {
                    Array.Copy(this.ToArray(), 0, array, 0, this.Count);
                }
                catch (ArrayTypeMismatchException ex)
                {
                    throw new ArgumentException("Invalid array argument; see inner exception for details.", ex);
                }
            }

            /// <summary>
            /// Gets an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
            /// <returns>The element at the specified index.</returns>
            protected override T DoGetItem(int index)
            {
                return this.source[index];
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

            /// <summary>
            /// Returns whether or not the type of a given item indicates it is appropriate for storing in this list.
            /// </summary>
            /// <param name="item">The item to test.</param>
            /// <returns><c>true</c> if the item is appropriate to store in this list; otherwise, <c>false</c>.</returns>
            private bool ObjectIsT(object item)
            {
                if (item is T)
                {
                    return true;
                }

                if (item == null && typeof(T).IsValueType)
                {
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Returns an exception indicating that the type of an item indicates it is not appropriate for storing in this list.
            /// </summary>
            /// <returns>An exception indicating that the type of an item indicates it is not appropriate for storing in this list.</returns>
            private Exception WrongObjectType()
            {
                return new ArgumentException("Object is not compatible with the type of elements contained in this list.");
            }
        }
    }
}

// <copyright file="AnonymousEnumerable.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// An object that implements <see cref="IEnumerable{T}"/> using a delegate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    public sealed class AnonymousEnumerable<T> : IEnumerable<T>
    {
        /// <summary>
        /// Gets or sets the get-enumerator delegate, which retrieves an enumerator that iterates through the collection.
        /// </summary>
        public Func<IEnumerator<T>> GetEnumerator { get; set; }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.GetEnumerator();
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
    }
}

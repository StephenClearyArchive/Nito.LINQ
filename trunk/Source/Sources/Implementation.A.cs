// <copyright file="Implementation.A.cs" company="Nito Programs">
//     Copyright (c) 2010 Nito Programs.
// </copyright>

namespace Nito.Linq.Implementation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A type that constructs anonymous types from delegates.
    /// </summary>
    public static class A
    {
        /// <summary>
        /// Constructs an object that implements <see cref="IComparer{T}"/> using a delegate.
        /// </summary>
        /// <typeparam name="T">The type of items to compare.</typeparam>
        /// <param name="compare">The Compare delegate, which compares two objects and returns a value less than 0 if its first argument is less than its second argument, 0 if its two arguments are equal, or greater than 0 if its first argument is greater than its second argument.</param>
        /// <returns>An object that implements <see cref="IComparer{T}"/> using a delegate.</returns>
        public static AnonymousComparer<T> Comparer<T>(Func<T, T, int> compare)
        {
#if WITHRX
            return new AnonymousComparer<T>(compare);
#else
            return new AnonymousComparer<T> { Compare = compare };
#endif
        }
    }
}

// <copyright file="SortedListSource.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito.Linq
{
    using System.Collections.Generic;
    using System;
    using Nito.Linq.Implementation;

    /// <summary>
    /// Sorted list sources.
    /// </summary>
    public static class SortedListSource
    {
        /// <summary>
        /// Creates an empty sorted list. The list is sorted by the specified comparison object.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="comparer">The comparison object.</param>
        /// <returns>An empty sorted list.</returns>
        public static ISortedList<T> Empty<T>(IComparer<T> comparer)
        {
            return new SortedListWrapper<T>(ListSource.Empty<T>(), comparer);
        }

        /// <summary>
        /// Creates an empty sorted list. The list is sorted by the default comparison object.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <returns>An empty sorted list.</returns>
        public static ISortedList<T> Empty<T>()
        {
            return new SortedListWrapper<T>(ListSource.Empty<T>(), Comparer<T>.Default);
        }

        /// <summary>
        /// Creates an empty sorted list. The list is sorted by the specified comparison delegate.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="comparer">The comparison delegate.</param>
        /// <returns>An empty sorted list.</returns>
        public static ISortedList<T> Empty<T>(Func<T, T, int> comparer)
        {
            return new SortedListWrapper<T>(ListSource.Empty<T>(), A.Comparer(comparer));
        }

        /// <summary>
        /// Converts a single value into a sorted list containing a single value. The list is treated as though it were sorted by the specified comparison object.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <param name="comparer">The comparison object that defines how the list is sorted.</param>
        /// <returns>A sorted list containing a single element, <paramref name="source"/>.</returns>
        public static ISortedList<T> Return<T>(T source, IComparer<T> comparer)
        {
            return new SortedListWrapper<T>(ListSource.Return(source), comparer);
        }

        /// <summary>
        /// Converts a single value into a sorted list containing a single value. The list is treated as though it were sorted by the default comparison object.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <returns>A sorted list containing a single element, <paramref name="source"/>.</returns>
        public static ISortedList<T> Return<T>(T source)
        {
            return new SortedListWrapper<T>(ListSource.Return(source), Comparer<T>.Default);
        }

        /// <summary>
        /// Converts a single value into a sorted list containing a single value. The list is treated as though it were sorted by the specified comparison delegate.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <param name="comparer">The comparison delegate that defines how the list is sorted.</param>
        /// <returns>A sorted list containing a single element, <paramref name="source"/>.</returns>
        public static ISortedList<T> Return<T>(T source, Func<T, T, int> comparer)
        {
            return new SortedListWrapper<T>(ListSource.Return(source), A.Comparer(comparer));
        }

        /// <summary>
        /// Converts a single value into a sorted list containing that value the specified number of times. The list is treated as though it were sorted by the specified comparison object.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <param name="comparer">The comparison object that defines how the list is sorted.</param>
        /// <param name="count">The number of times <paramref name="source"/> is repeated. If <paramref name="count"/> is less than or equal to 0, an empty list is returned.</param>
        /// <returns>A sorted list containing <paramref name="count"/> elements, all equal to <paramref name="source"/>.</returns>
        public static ISortedList<T> Repeat<T>(T source, IComparer<T> comparer, int count)
        {
            return new SortedListWrapper<T>(ListSource.Repeat(source, count), comparer);
        }

        /// <summary>
        /// Converts a single value into a sorted list containing that value the specified number of times. The list is treated as though it were sorted by the default comparison object.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <param name="count">The number of times <paramref name="source"/> is repeated. If <paramref name="count"/> is less than or equal to 0, an empty list is returned.</param>
        /// <returns>A sorted list containing <paramref name="count"/> elements, all equal to <paramref name="source"/>.</returns>
        public static ISortedList<T> Repeat<T>(T source, int count)
        {
            return new SortedListWrapper<T>(ListSource.Repeat(source, count), Comparer<T>.Default);
        }

        /// <summary>
        /// Converts a single value into a sorted list containing that value the specified number of times. The list is treated as though it were sorted by the specified comparison delegate.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <param name="comparer">The comparison delegate that defines how the list is sorted.</param>
        /// <param name="count">The number of times <paramref name="source"/> is repeated. If <paramref name="count"/> is less than or equal to 0, an empty list is returned.</param>
        /// <returns>A sorted list containing <paramref name="count"/> elements, all equal to <paramref name="source"/>.</returns>
        public static ISortedList<T> Repeat<T>(T source, Func<T, T, int> comparer, int count)
        {
            return new SortedListWrapper<T>(ListSource.Repeat(source, count), A.Comparer(comparer));
        }
    }
}

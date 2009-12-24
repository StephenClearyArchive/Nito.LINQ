﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nito
{
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
            return new SortedListExtensions.AnonymousSortedList<T>(ListSource.Empty<T>(), comparer);
        }

        /// <summary>
        /// Creates an empty sorted list. The list is sorted by the default comparison object.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <returns>An empty sorted list.</returns>
        public static ISortedList<T> Empty<T>()
        {
            return Empty(Comparer<T>.Default);
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
            return new SortedListExtensions.AnonymousSortedList<T>(ListSource.Return(source), comparer);
        }

        /// <summary>
        /// Converts a single value into a sorted list containing a single value. The list is treated as though it were sorted by the default comparison object.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <returns>A sorted list containing a single element, <paramref name="source"/>.</returns>
        public static ISortedList<T> Return<T>(T source)
        {
            return Return(source, Comparer<T>.Default);
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
            return new SortedListExtensions.AnonymousSortedList<T>(ListSource.Repeat(source, count), comparer);
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
            return Repeat(source, Comparer<T>.Default, count);
        }
    }
}

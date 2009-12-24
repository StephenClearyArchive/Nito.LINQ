using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nito
{
    /// <summary>
    /// Sorted sequence sources.
    /// </summary>
    public static class SortedEnumerableSource
    {
        /// <summary>
        /// Creates a sorted, empty sequence. The sequence is sorted by the specified comparison object.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="comparer">The comparison object.</param>
        /// <returns>The sorted, empty sequence.</returns>
        public static ISortedEnumerable<T> Empty<T>(IComparer<T> comparer)
        {
            return new SortedEnumerableExtensions.AnonymousSortedEnumerable<T>(Enumerable.Empty<T>(), comparer);
        }

        /// <summary>
        /// Creates a sorted, empty sequence. The sequence is sorted by the default comparison object.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <returns>The sorted, empty sequence.</returns>
        public static ISortedEnumerable<T> Empty<T>()
        {
            return Empty(Comparer<T>.Default);
        }

        /// <summary>
        /// Converts a single value into a sorted sequence containing a single value. The sequence is treated as though it were sorted by the specified comparison object.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <param name="comparer">The comparison object that defines how the sequence is sorted.</param>
        /// <returns>A sorted sequence containing a single element, <paramref name="source"/>.</returns>
        public static ISortedEnumerable<T> Return<T>(T source, IComparer<T> comparer)
        {
            return new SortedEnumerableExtensions.AnonymousSortedEnumerable<T>(EnumerableSource.Return(source), comparer);
        }

        /// <summary>
        /// Converts a single value into a sorted sequence containing a single value. The sequence is treated as though it were sorted by the default comparison object.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <returns>A sorted sequence containing a single element, <paramref name="source"/>.</returns>
        public static ISortedEnumerable<T> Return<T>(T source)
        {
            return Return(source, Comparer<T>.Default);
        }

        /// <summary>
        /// Converts a single value into a sorted sequence containing that value the specified number of times. The sequence is treated as though it were sorted by the specified comparison object.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <param name="comparer">The comparison object that defines how the sequence is sorted.</param>
        /// <param name="count">The number of times <paramref name="source"/> is repeated. If <paramref name="count"/> is less than or equal to 0, an empty sequence is returned.</param>
        /// <returns>A sorted sequence containing <paramref name="count"/> elements, all equal to <paramref name="source"/>.</returns>
        public static ISortedEnumerable<T> Repeat<T>(T source, IComparer<T> comparer, int count)
        {
            return new SortedEnumerableExtensions.AnonymousSortedEnumerable<T>(EnumerableSource.Repeat(source, count), comparer);
        }

        /// <summary>
        /// Converts a single value into a sorted sequence containing that value the specified number of times. The sequence is treated as though it were sorted by the default comparison object.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <param name="count">The number of times <paramref name="source"/> is repeated. If <paramref name="count"/> is less than or equal to 0, an empty sequence is returned.</param>
        /// <returns>A sorted sequence containing <paramref name="count"/> elements, all equal to <paramref name="source"/>.</returns>
        public static ISortedEnumerable<T> Repeat<T>(T source, int count)
        {
            return Repeat(source, Comparer<T>.Default, count);
        }

        /// <summary>
        /// Converts a single value into a sorted sequence containing that value an infinite number of times. The sequence is treated as though it were sorted by the specified comparison object.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <param name="comparer">The comparison object that defines how the sequence is sorted.</param>
        /// <returns>A sorted sequence containing an infinite number of elements, all equal to <paramref name="source"/>.</returns>
        public static ISortedEnumerable<T> Repeat<T>(T source, IComparer<T> comparer)
        {
            return new SortedEnumerableExtensions.AnonymousSortedEnumerable<T>(EnumerableSource.Repeat(source), comparer);
        }

        /// <summary>
        /// Converts a single value into a sorted sequence containing that value an infinite number of times. The sequence is treated as though it were sorted by the default comparison object.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <returns>A sorted sequence containing an infinite number of elements, all equal to <paramref name="source"/>.</returns>
        public static ISortedEnumerable<T> Repeat<T>(T source)
        {
            return Repeat(source, Comparer<T>.Default);
        }
    }
}

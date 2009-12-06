// <copyright file="EnumerableEx.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito
{
    using System.Collections.Generic;

    /// <summary>
    /// Duplicates Rx functionality for sequences.
    /// </summary>
    internal static class EnumerableEx
    {
        /// <summary>
        /// Converts a single value into a sequence containing a single value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <returns>A sequence containing a single element, <paramref name="source"/>.</returns>
        public static IEnumerable<T> Return<T>(T source)
        {
            yield return source;
        }

        /// <summary>
        /// Converts a single value into a sequence containing that value the specified number of times.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <param name="count">The number of times <paramref name="source"/> is repeated. If <paramref name="count"/> is less than or equal to 0, an empty sequence is returned.</param>
        /// <returns>A sequence containing <paramref name="count"/> elements, all equal to <paramref name="source"/>.</returns>
        public static IEnumerable<T> Repeat<T>(T source, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                yield return source;
            }
        }

        /// <summary>
        /// Converts a single value into a sequence containing that value an infinite number of times.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <returns>A sequence containing an infinite number of elements, all equal to <paramref name="source"/>.</returns>
        public static IEnumerable<T> Repeat<T>(T source)
        {
            while (true)
            {
                yield return source;
            }
        }

        /// <summary>
        /// Prepends a value to a source sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="value">The value to prepend.</param>
        /// <returns>A sequence containing <paramref name="value"/> followed by <paramref name="source"/>.</returns>
        public static IEnumerable<T> StartWith<T>(this IEnumerable<T> source, T value)
        {
            yield return value;
            foreach (T item in source)
            {
                yield return item;
            }
        }
    }
}

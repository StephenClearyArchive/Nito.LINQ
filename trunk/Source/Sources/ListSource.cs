// <copyright file="ListSource.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// List sources.
    /// </summary>
    public static partial class ListSource
    {
        /// <summary>
        /// Returns an empty list.
        /// </summary>
        /// <typeparam name="T">The type of elements contained in the list.</typeparam>
        /// <returns>An empty list.</returns>
        public static IList<T> Empty<T>()
        {
            return Repeat(default(T), 0);
        }

        /// <summary>
        /// Converts a single value into a list containing a single value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <returns>A list containing a single element, <paramref name="source"/>.</returns>
        public static IList<T> Return<T>(T source)
        {
            return new ValueList<T>(source, 1);
        }

        /// <summary>
        /// Converts a single value into a list containing that value the specified number of times.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <param name="count">The number of times <paramref name="source"/> is repeated. If <paramref name="count"/> is less than or equal to 0, an empty list is returned.</param>
        /// <returns>A list containing <paramref name="count"/> elements, all equal to <paramref name="source"/>.</returns>
        public static IList<T> Repeat<T>(T source, int count)
        {
            return new ValueList<T>(source, count);
        }

        /// <summary>
        /// Returns a read-only list that generates its elements when they are read.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="count">The number of elements in the list. Must be greater than or equal to 0.</param>
        /// <param name="generator">The delegate that is used to generate the elements.</param>
        /// <returns>A read-only list that generates its elements on demand.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
        public static IList<T> Generate<T>(int count, Func<T> generator)
        {
            return new GenerateList<T>(count, _ => generator());
        }

        /// <summary>
        /// Returns a read-only list that generates its elements when they are read, passing the element's index to the generator delegate.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="count">The number of elements in the list. Must be greater than or equal to 0.</param>
        /// <param name="generator">The delegate that is used to generate the elements.</param>
        /// <returns>A read-only list that generates its elements on demand.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
        public static IList<T> Generate<T>(int count, Func<int, T> generator)
        {
            return new GenerateList<T>(count, generator);
        }
    }
}

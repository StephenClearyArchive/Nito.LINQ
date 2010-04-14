// <copyright file="ListSource.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito.Linq
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
        /// <returns>An empty read-only list.</returns>
        public static IList<T> Empty<T>()
        {
            return new Implementation.AnonymousReadOnlyList<T>(null, () => 0);
        }

        /// <summary>
        /// Converts a single value into a list containing a single value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <returns>A read-only list containing a single element, <paramref name="source"/>.</returns>
        public static IList<T> Return<T>(T source)
        {
            return new Implementation.AnonymousReadOnlyList<T>(_ => source, () => 1);
        }

        /// <summary>
        /// Converts a single value into a list containing that value the specified number of times.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <param name="count">The number of times <paramref name="source"/> is repeated. If <paramref name="count"/> is less than or equal to 0, an empty list is returned.</param>
        /// <returns>A read-only list containing <paramref name="count"/> elements, all equal to <paramref name="source"/>.</returns>
        public static IList<T> Repeat<T>(T source, int count)
        {
            if (count <= 0)
            {
                return ListSource.Empty<T>();
            }

            return new Implementation.AnonymousReadOnlyList<T>(_ => source, () => count);
        }

        /// <summary>
        /// Returns a read-only list that generates its elements when they are read.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="generator">The delegate that is used to generate the elements. This may be <c>null</c> if <paramref name="count"/> is less than or equal to 0.</param>
        /// <param name="count">The number of elements in the list. If <paramref name="count"/> is less than or equal to 0, an empty list is returned.</param>
        /// <returns>A read-only list that generates its elements on demand.</returns>
        public static IList<T> Generate<T>(Func<T> generator, int count)
        {
            if (count <= 0)
            {
                return ListSource.Empty<T>();
            }

            return new Implementation.AnonymousReadOnlyList<T>(_ => generator(), () => count);
        }

        /// <summary>
        /// Returns a read-only list that generates its elements when they are read, passing the element's index to the generator delegate.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="generator">The delegate that is used to generate the elements. This may be <c>null</c> if <paramref name="count"/> is less than or equal to 0.</param>
        /// <param name="count">The number of elements in the list. If <paramref name="count"/> is less than or equal to 0, an empty list is returned.</param>
        /// <returns>A read-only list that generates its elements on demand.</returns>
        public static IList<T> Generate<T>(Func<int, T> generator, int count)
        {
            if (count <= 0)
            {
                return ListSource.Empty<T>();
            }

            return new Implementation.AnonymousReadOnlyList<T>(generator, () => count);
        }

        /// <summary>
        /// Returns a read-only list of dynamic size that generates its elements when they are read.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="generator">The delegate that is used to generate the elements. This may be <c>null</c> if <paramref name="counter"/> returns a number less than or equal to 0.</param>
        /// <param name="counter">The delegate that is used to count the number of elements in the list.</param>
        /// <returns>A read-only list that generates its elements on demand.</returns>
        public static IList<T> Generate<T>(Func<T> generator, Func<int> counter)
        {
            return new Implementation.AnonymousReadOnlyList<T>(_ => generator(), counter);
        }

        /// <summary>
        /// Returns a read-only list of dynamic size that generates its elements when they are read, passing the element's index to the generator delegate.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="generator">The delegate that is used to generate the elements. This may be <c>null</c> if <paramref name="counter"/> returns a number less than or equal to 0.</param>
        /// <param name="counter">The delegate that is used to count the number of elements in the list.</param>
        /// <returns>A read-only list that generates its elements on demand.</returns>
        public static IList<T> Generate<T>(Func<int, T> generator, Func<int> counter)
        {
            return new Implementation.AnonymousReadOnlyList<T>(generator, counter);
        }
    }
}

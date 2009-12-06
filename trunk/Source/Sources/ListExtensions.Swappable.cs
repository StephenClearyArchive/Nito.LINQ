// <copyright file="ListExtensions.Swappable.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito
{
    using System.Collections.Generic;

    /// <summary>
    /// Extension methods for <see cref="ISortedList{T}"/>.
    /// </summary>
    public static partial class ListExtensions
    {
        /// <summary>
        /// Non-generic interface that allows swapping list elements.
        /// </summary>
        public interface ISwappable
        {
            /// <summary>
            /// Gets the number of elements in this list.
            /// </summary>
            int Count { get; }

            /// <summary>
            /// Swaps two elements in the list.
            /// </summary>
            /// <param name="indexA">The index of the first element to swap.</param>
            /// <param name="indexB">The index of the second element to swap.</param>
            void Swap(int indexA, int indexB);
        }

        /// <summary>
        /// Provides a non-generic <see cref="ISwappable"/> interface for the source list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source list.</typeparam>
        /// <param name="source">The source list.</param>
        /// <returns>A non-generic <see cref="ISwappable"/> interface for the source list.</returns>
        public static ISwappable AsSwappable<T>(this IList<T> source)
        {
            return new SwappableList<T>(source);
        }

        /// <summary>
        /// Swaps two elements in-place in the source list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source list.</typeparam>
        /// <param name="source">The source list.</param>
        /// <param name="indexA">The index of the first element to swap.</param>
        /// <param name="indexB">The index of the second element to swap.</param>
        public static void Swap<T>(this IList<T> source, int indexA, int indexB)
        {
            T tmp = source[indexA];
            source[indexA] = source[indexB];
            source[indexB] = tmp;
        }

        /// <summary>
        /// Swaps two element positions in-place in the source list and in a sequence of swappable lists.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source list.</typeparam>
        /// <param name="source">The source list.</param>
        /// <param name="others">Other lists to keep in sync with the source list.</param>
        /// <param name="indexA">The index of the first element position to swap.</param>
        /// <param name="indexB">The index of the second element position to swap.</param>
        internal static void SwapAll<T>(this IList<T> source, IEnumerable<ISwappable> others, int indexA, int indexB)
        {
            source.Swap(indexA, indexB);
            foreach (ISwappable other in others)
            {
                other.Swap(indexA, indexB);
            }
        }

        /// <summary>
        /// A wrapper around a generic list that allows swapping elements.
        /// </summary>
        /// <typeparam name="T">The type of items contained in the list.</typeparam>
        private sealed class SwappableList<T> : ISwappable
        {
            /// <summary>
            /// The source list.
            /// </summary>
            private IList<T> source;

            /// <summary>
            /// Initializes a new instance of the <see cref="SwappableList&lt;T&gt;"/> class with a specified source list.
            /// </summary>
            /// <param name="source">The source list.</param>
            public SwappableList(IList<T> source)
            {
                this.source = source;
            }

            /// <summary>
            /// Gets the number of elements in this list.
            /// </summary>
            public int Count
            {
                get { return this.source.Count; }
            }

            /// <summary>
            /// Swaps two elements in the list.
            /// </summary>
            /// <param name="indexA">The index of the first element to swap.</param>
            /// <param name="indexB">The index of the second element to swap.</param>
            public void Swap(int indexA, int indexB)
            {
                this.source.Swap(indexA, indexB);
            }
        }
    }
}

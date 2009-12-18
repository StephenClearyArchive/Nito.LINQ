// <copyright file="ListExtensions.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Provides useful extension methods for lists.
    /// </summary>
    public static partial class ListExtensions
    {
        #region Converters

        /// <summary>
        /// Returns the source typed as <see cref="IList{T}"/>. This method has no effect other than to restrict the compile-time type of an object implementing <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="list"/>.</typeparam>
        /// <param name="list">The source list.</param>
        /// <returns>The source list, typed as <see cref="IList{T}"/>.</returns>
        public static IList<T> AsList<T>(this IList<T> list)
        {
            return list;
        }

        #endregion

        #region Sources

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

        #endregion

        #region Operators

        /// <summary>
        /// Returns a read-only list wrapper for a given list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source list.</typeparam>
        /// <param name="list">The source list.</param>
        /// <returns>A read-only wrapper for the source list.</returns>
        public static IList<T> AsReadOnly<T>(this IList<T> list)
        {
            return new System.Collections.ObjectModel.ReadOnlyCollection<T>(list);
        }

        /// <summary>
        /// Projects the elements of a specified list into a result list.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source list.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting list.</typeparam>
        /// <param name="list">The source list.</param>
        /// <param name="selector">The projection delegate.</param>
        /// <returns>The projected list.</returns>
        public static IList<TResult> Select<TSource, TResult>(this IList<TSource> list, Func<TSource, TResult> selector)
        {
            return new ProjectList<TSource, TResult>(list, (item, index) => selector(item));
        }

        /// <summary>
        /// Projects the elements of a specified list into a result list.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source list.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting list.</typeparam>
        /// <param name="list">The source list.</param>
        /// <param name="selector">The projection delegate, which takes the index of the item as its second parameter.</param>
        /// <returns>The projected list.</returns>
        public static IList<TResult> Select<TSource, TResult>(this IList<TSource> list, Func<TSource, int, TResult> selector)
        {
            return new ProjectList<TSource, TResult>(list, selector);
        }

        /// <summary>
        /// Projects the elements of a specified list into a result list that supports updating.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source list.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting list.</typeparam>
        /// <param name="list">The source list.</param>
        /// <param name="selector">The projection delegate from source elements to result elements.</param>
        /// <param name="reverseSelector">The projection delegate from result elements to source elements.</param>
        /// <returns>The projected list.</returns>
        public static IList<TResult> Select<TSource, TResult>(this IList<TSource> list, Func<TSource, TResult> selector, Func<TResult, TSource> reverseSelector)
        {
            return new ReadWriteProjectList<TSource, TResult>(list, selector, reverseSelector);
        }

        /// <summary>
        /// Returns a sliced list, which acts as a window into a subset of the original list.
        /// </summary>
        /// <typeparam name="T">The type of object contained in the list.</typeparam>
        /// <param name="list">The list to slice.</param>
        /// <param name="offset">The offset into the list at which the slice begins. If <paramref name="offset"/> is less than or equal to zero, then a list equivalent to the source list is returned; if <paramref name="offset"/> is greater than or equal to the count of the source list, then an empty list is returned.</param>
        /// <returns>A list that is a slice of the source list.</returns>
        public static IList<T> Skip<T>(this IList<T> list, int offset)
        {
            if (offset <= 0)
            {
                return list;
            }

            if (offset >= list.Count)
            {
                return Empty<T>();
            }

            return new SliceList<T>(list, offset, list.Count - offset);
        }

        /// <summary>
        /// Returns a sliced list, which acts as a window into a subset of the original list.
        /// </summary>
        /// <typeparam name="T">The type of object contained in the list.</typeparam>
        /// <param name="list">The list to slice.</param>
        /// <param name="count">The number of elements in the slice. If <paramref name="count"/> is less than or equal to zero, then an empty list is returned; if <paramref name="count"/> is greater than or equal to the count of the source list, then a list equivalent to the source list is returned.</param>
        /// <returns>A list that is a slice of the source list.</returns>
        public static IList<T> Take<T>(this IList<T> list, int count)
        {
            if (count <= 0)
            {
                return Empty<T>();
            }

            if (count >= list.Count)
            {
                return list;
            }

            return new SliceList<T>(list, 0, count);
        }

        /// <summary>
        /// Returns a sliced list, which acts as a window into a subset of the original list. Similar to Skip followed by Take, only this has tighter restrictions on the <paramref name="offset"/> and <paramref name="count"/> parameters.
        /// </summary>
        /// <typeparam name="T">The type of object contained in the list.</typeparam>
        /// <param name="list">The list to slice.</param>
        /// <param name="offset">The offset into the list at which the slice begins. Must be a valid index into the source list, or equal to the count of the source list.</param>
        /// <param name="count">The number of elements in the slice. May not be less than zero. If count is greater than 0, then every value in the range [offset, offset + count) must be valid indexes into the source list.</param>
        /// <returns>A list that is a slice of the source list.</returns>
        public static IList<T> Slice<T>(this IList<T> list, int offset, int count)
        {
            return new SliceList<T>(list, offset, count);
        }

        /// <summary>
        /// Steps through a list using a specified step size. The first element of the resulting list is the first element of the source list.
        /// </summary>
        /// <typeparam name="T">The type of object contained in the list.</typeparam>
        /// <param name="list">The list to step through.</param>
        /// <param name="step">The step size. Must be greater than 0.</param>
        /// <returns>The stepped list.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="step"/> is less than or equal to 0.</exception>
        public static IList<T> Step<T>(this IList<T> list, int step)
        {
            return new StepList<T>(list, step);
        }

        /// <summary>
        /// Returns a list that acts as though the list has been reversed.
        /// </summary>
        /// <typeparam name="T">The type of elements contained in the list.</typeparam>
        /// <param name="list">The source list.</param>
        /// <returns>The reverse list.</returns>
        public static IList<T> Reverse<T>(this IList<T> list)
        {
            return new ReverseList<T>(list);
        }

        /// <summary>
        /// Repeats a list of values a specified number of times.
        /// </summary>
        /// <typeparam name="T">The type of object contained in the list.</typeparam>
        /// <param name="list">The list of values to repeat.</param>
        /// <param name="count">The number of times to repeat all elements in the source list. If <paramref name="count"/> is less than or equal to 0, an empty list is returned.</param>
        /// <returns>A list containing the source list repeated the specified number of times.</returns>
        public static IList<T> Repeat<T>(this IList<T> list, int count)
        {
            return new RepeatList<T>(list, count);
        }

        /// <summary>
        /// Concatenates the specified lists.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source lists.</typeparam>
        /// <param name="list">The first source list to concatenate.</param>
        /// <param name="others">The other source lists to concatenate.</param>
        /// <returns>The concatenated list.</returns>
        public static IList<T> Concat<T>(this IList<T> list, params IList<T>[] others)
        {
            IList<T>[] lists = new IList<T>[others.Length + 1];
            lists[0] = list;
            others.CopyTo(lists, 1);
            return new ConcatList<T>(lists);
        }

        /// <summary>
        /// Concatenates the specified lists.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source lists.</typeparam>
        /// <param name="lists">The source lists to concatenate.</param>
        /// <returns>The concatenated list.</returns>
        public static IList<T> Concat<T>(this IList<IList<T>> lists)
        {
            return new ConcatList<T>(lists);
        }

        /// <summary>
        /// Flattens a list of lists into one long list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the resulting list.</typeparam>
        /// <param name="list">The list of lists.</param>
        /// <returns>The flattened list.</returns>
        public static IList<T> FlattenList<T>(this IList<IList<T>> list)
        {
            return new ConcatList<T>(list);
        }

        /// <summary>
        /// Combines two source lists into a result list. If the source lists are of different lengths, the resulting list has a length equal to the smaller of the two.
        /// </summary>
        /// <typeparam name="TFirst">The type of elements in the first source list.</typeparam>
        /// <typeparam name="TSecond">The type of elements in the second source list.</typeparam>
        /// <typeparam name="TResult">The type of elements in the resulting list.</typeparam>
        /// <param name="first">The first source list.</param>
        /// <param name="second">The second source list.</param>
        /// <param name="zipper">The delegate that creates a new element from corresponding elements of the two source lists.</param>
        /// <returns>The combined list.</returns>
        public static IList<TResult> Zip<TFirst, TSecond, TResult>(this IList<TFirst> first, IList<TSecond> second, Func<TFirst, TSecond, TResult> zipper)
        {
            int listSize = Math.Min(first.Count, second.Count);
            return new ProjectList<TFirst, TResult>(first.Take(listSize), (a, index) => zipper(a, second[index]));
        }

        /// <summary>
        /// Combines three source lists into a result list. If the source lists are of different lengths, the resulting list has a length equal to the smallest of the three.
        /// </summary>
        /// <typeparam name="TFirst">The type of elements in the first source list.</typeparam>
        /// <typeparam name="TSecond">The type of elements in the second source list.</typeparam>
        /// <typeparam name="TThird">The type of elements in the third source list.</typeparam>
        /// <typeparam name="TResult">The type of elements in the resulting list.</typeparam>
        /// <param name="first">The first source list.</param>
        /// <param name="second">The second source list.</param>
        /// <param name="third">The third source list.</param>
        /// <param name="zipper">The delegate that creates a new element from corresponding elements of the two source lists.</param>
        /// <returns>The combined list.</returns>
        public static IList<TResult> Zip<TFirst, TSecond, TThird, TResult>(this IList<TFirst> first, IList<TSecond> second, IList<TThird> third, Func<TFirst, TSecond, TThird, TResult> zipper)
        {
            int listSize = Math.Min(Math.Min(first.Count, second.Count), third.Count);
            return new ProjectList<TFirst, TResult>(first.Take(listSize), (a, index) => zipper(a, second[index], third[index]));
        }

        /// <summary>
        /// Returns a read-only list wrapper that remembers the values of its source list. Acts as a cache for the source list elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source list.</typeparam>
        /// <param name="list">The source list.</param>
        /// <returns>A read-only cache wrapper for the source list.</returns>
        public static IList<T> Memoize<T>(this IList<T> list)
        {
            return new MemoizeList<T>(list);
        }

        /// <summary>
        /// Returns a read-only list wrapper that evaluates all members of its source list. Acts as a cache for the entire source list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source list.</typeparam>
        /// <param name="list">The source list.</param>
        /// <returns>A read-only cache of the source list.</returns>
        public static IList<T> MemoizeAll<T>(this IList<T> list)
        {
            return list.ToList().AsReadOnly();
        }

        /// <summary>
        /// Rotates a list by a given offset.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source list.</typeparam>
        /// <param name="list">The source list.</param>
        /// <param name="offset">The number of elements to rotate. If <paramref name="offset"/> is less than or equal to 0, or if <paramref name="offset"/> is greater than or equal to the number of elements in the list, then the result of this method is identical to the source list.</param>
        /// <returns>The rotated list.</returns>
        public static IList<T> Rotate<T>(this IList<T> list, int offset)
        {
            return list.Skip(offset).Concat(list.Take(offset));
        }

        /// <summary>
        /// Randomly reorders the elements of a list in-place using the specified random number generator delegate.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source list.</typeparam>
        /// <param name="list">The source list.</param>
        /// <param name="randomNumberGenerator">The random number generator delegate; when invoked with a value <c>n</c>, this delegate must return a random number in the range [0, n).</param>
        /// <param name="others">Other lists to be kept in sync with the randomized list. As <paramref name="list"/> is randomized, the same relative elements are rearranged in these lists.</param>
        public static void RandomShuffle<T>(this IList<T> list, Func<int, int> randomNumberGenerator, params ISwappable[] others)
        {
            int n = list.Count;
            while (n > 1)
            {
                int k = randomNumberGenerator(n);
                --n;
#if DEBUG
                Debug.Assert(0 <= k && k <= n, "RNG must return value within the range [0, maxValue)");
#endif
                list.SwapAll(others, k, n);
            }
        }

        /// <summary>
        /// Randomly reorders the elements of a list in-place using the specified random number generator.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source list.</typeparam>
        /// <param name="list">The source list.</param>
        /// <param name="randomNumberGenerator">The random number generator.</param>
        /// <param name="others">Other lists to be kept in sync with the randomized list. As <paramref name="list"/> is randomized, the same relative elements are rearranged in these lists.</param>
        public static void RandomShuffle<T>(this IList<T> list, Random randomNumberGenerator, params ISwappable[] others)
        {
            RandomShuffle(list, x => randomNumberGenerator.Next(x), others);
        }

        /// <summary>
        /// Randomly reorders the elements of a list in-place using the default pseudo-random number generator.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source list.</typeparam>
        /// <param name="list">The source list.</param>
        /// <param name="others">Other lists to be kept in sync with the randomized list. As <paramref name="list"/> is randomized, the same relative elements are rearranged in these lists.</param>
        public static void RandomShuffle<T>(this IList<T> list, params ISwappable[] others)
        {
            RandomShuffle(list, new Random(), others);
        }

        /// <summary>
        /// Creates and returns a view of the source list in which the elements are in a random order, using the specified random number generator delegate.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source list.</typeparam>
        /// <param name="list">The source list. As long as the returned view is referenced, the size of the source list should not change.</param>
        /// <param name="randomNumberGenerator">The random number generator delegate; when invoked with a value <c>n</c>, this delegate must return a random number in the range [0, n).</param>
        /// <param name="others">Other lists to be kept in sync with the randomized view. As the view is randomized, the same relative elements are rearranged in these lists.</param>
        /// <returns>The randomized view of the source list.</returns>
        public static IList<T> RandomShuffleIndirect<T>(this IList<T> list, Func<int, int> randomNumberGenerator, params ISwappable[] others)
        {
            var indirect = new IndirectList<T>(list);
            indirect.Indices.RandomShuffle(randomNumberGenerator, others);
            return indirect;
        }

        /// <summary>
        /// Creates and returns a view of the source list in which the elements are in a random order, using the specified random number generator.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source list.</typeparam>
        /// <param name="list">The source list. As long as the returned view is referenced, the size of the source list should not change.</param>
        /// <param name="randomNumberGenerator">The random number generator.</param>
        /// <param name="others">Other lists to be kept in sync with the randomized view. As the view is randomized, the same relative elements are rearranged in these lists.</param>
        /// <returns>The randomized view of the source list.</returns>
        public static IList<T> RandomShuffleIndirect<T>(this IList<T> list, Random randomNumberGenerator, params ISwappable[] others)
        {
            return RandomShuffleIndirect(list, x => randomNumberGenerator.Next(x), others);
        }

        /// <summary>
        /// Creates and returns a view of the source list in which the elements are in a random order, using the default pseudo-random number generator.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source list.</typeparam>
        /// <param name="list">The source list. As long as the returned view is referenced, the size of the source list should not change.</param>
        /// <param name="others">Other lists to be kept in sync with the randomized view. As the view is randomized, the same relative elements are rearranged in these lists.</param>
        /// <returns>The randomized view of the source list.</returns>
        public static IList<T> RandomShuffleIndirect<T>(this IList<T> list, params ISwappable[] others)
        {
            return RandomShuffleIndirect(list, new Random(), others);
        }

        #endregion

        #region Consumers

        /// <summary>
        /// Determines the index of the last matching element in a list.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="list"/>.</typeparam>
        /// <param name="list">The list in which to locate the value.</param>
        /// <param name="match">The delegate that defines the conditions of the element to search for.</param>
        /// <returns>The index of the last element that returned true from <paramref name="match"/> if found in the list; otherwise, -1.</returns>
        public static int LastIndexOf<T>(this IList<T> list, Func<T, bool> match)
        {
            for (int i = list.Count - 1; i > -1; --i)
            {
                if (match(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Determines the index of the last matching element in a list.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="list"/>.</typeparam>
        /// <param name="list">The list in which to locate the value.</param>
        /// <param name="value">The value to locate in the list.</param>
        /// <returns>The index of the last instance of <paramref name="value"/> if found in the list; otherwise, -1.</returns>
        public static int LastIndexOf<T>(this IList<T> list, T value)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            return list.LastIndexOf((item) => comparer.Equals(value, item));
        }

        /// <summary>
        /// Returns the last element of a list that satisfies a specified condition.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="list"/>.</typeparam>
        /// <param name="list">The list in which to locate the value.</param>
        /// <param name="match">The delegate that defines the conditions of the element to search for.</param>
        /// <returns>The last element of the list that returned true from <paramref name="match"/>.</returns>
        /// <exception cref="InvalidOperationException">No element satisfies the condition.</exception>
        public static T Last<T>(this IList<T> list, Func<T, bool> match)
        {
            return list.Reverse().First(match);
        }

        /// <summary>
        /// Returns the last element of a list that satisfies a specified condition, or a default value if no element is found.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="list"/>.</typeparam>
        /// <param name="list">The list in which to locate the value.</param>
        /// <param name="match">The delegate that defines the conditions of the element to search for.</param>
        /// <returns>The last element of the list that returned true from <paramref name="match"/>, or <c>default(T)</c> if no element is found.</returns>
        public static T LastOrDefault<T>(this IList<T> list, Func<T, bool> match)
        {
            return list.Reverse().FirstOrDefault(match);
        }

        /// <summary>
        /// Copies a range of the list to a range in a destination list. The elements are copied in index order.
        /// </summary>
        /// <typeparam name="T">The type of elements contained in the lists.</typeparam>
        /// <param name="list">The source list.</param>
        /// <param name="index">The zero-based index into the source list at which copying begins.</param>
        /// <param name="destination">The destination list.</param>
        /// <param name="destinationIndex">The zero-based index into the destination list at which copying begins.</param>
        /// <param name="count">The number of elements to copy.</param>
        public static void CopyTo<T>(this IList<T> list, int index, IList<T> destination, int destinationIndex, int count)
        {
            Implementation.ListHelper.CheckRangeArguments(list.Count, index, count);
            Implementation.ListHelper.CheckRangeArguments(destination.Count, destinationIndex, count);

            for (int i = 0; i != count; ++i)
            {
                destination[destinationIndex + i] = list[index + i];
            }
        }

        /// <summary>
        /// Copies the entire list to a destination list, starting at the specified index of the destination list. The elements are copied in index order.
        /// </summary>
        /// <typeparam name="T">The type of elements contained in the lists.</typeparam>
        /// <param name="list">The source list.</param>
        /// <param name="destination">The destination list.</param>
        /// <param name="destinationIndex">The zero-based index into the destination list at which copying begins.</param>
        public static void CopyTo<T>(this IList<T> list, IList<T> destination, int destinationIndex)
        {
            list.CopyTo(0, destination, destinationIndex, list.Count);
        }

        /// <summary>
        /// Copies the entire list to a destination list, starting at the beginning of the destination list. The elements are copied in index order.
        /// </summary>
        /// <typeparam name="T">The type of elements contained in the lists.</typeparam>
        /// <param name="list">The source list.</param>
        /// <param name="destination">The destination list.</param>
        public static void CopyTo<T>(this IList<T> list, IList<T> destination)
        {
            list.CopyTo(0, destination, 0, list.Count);
        }

        /// <summary>
        /// Copies a range of the list to a range in a destination list. The elements are copied in reverse index order.
        /// </summary>
        /// <typeparam name="T">The type of elements contained in the lists.</typeparam>
        /// <param name="list">The source list.</param>
        /// <param name="index">The zero-based index into the source list at which copying begins.</param>
        /// <param name="destination">The destination list.</param>
        /// <param name="destinationIndex">The zero-based index into the destination list at which copying begins.</param>
        /// <param name="count">The number of elements to copy.</param>
        public static void CopyBackward<T>(this IList<T> list, int index, IList<T> destination, int destinationIndex, int count)
        {
            Implementation.ListHelper.CheckRangeArguments(list.Count, index, count);
            Implementation.ListHelper.CheckRangeArguments(destination.Count, destinationIndex, count);

            for (int i = count - 1; i != -1; --i)
            {
                destination[destinationIndex + i] = list[index + i];
            }
        }

        /// <summary>
        /// Compares two sequences and determines if they are equal, using the specified element equality comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements contained in the lists.</typeparam>
        /// <param name="list">The first source list.</param>
        /// <param name="other">The second source list.</param>
        /// <param name="comparer">The comparison object used to compare elements for equality.</param>
        /// <returns><c>true</c> if every element in both lists are equal; otherwise, <c>false</c>.</returns>
        public static bool SequenceEqual<T>(this IList<T> list, IList<T> other, IEqualityComparer<T> comparer)
        {
            if (list.Count != other.Count)
            {
                return false;
            }

            return list.AsEnumerable().SequenceEqual(other, comparer);
        }

        /// <summary>
        /// Compares two sequences and determines if they are equal, using the default element equality comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements contained in the lists.</typeparam>
        /// <param name="list">The first source list.</param>
        /// <param name="other">The second source list.</param>
        /// <returns><c>true</c> if every element in both lists are equal; otherwise, <c>false</c>.</returns>
        public static bool SequenceEqual<T>(this IList<T> list, IList<T> other)
        {
            return list.SequenceEqual(other, EqualityComparer<T>.Default);
        }

        #endregion
    }
}

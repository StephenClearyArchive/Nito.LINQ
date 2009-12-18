// <copyright file="SortedEnumerableExtensions.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a sequence that is sorted by a single comparison.
    /// </summary>
    /// <typeparam name="T">The type of items in the sequence.</typeparam>
    public interface ISortedEnumerable<T> : IEnumerable<T>
    {
        /// <summary>
        /// Gets a comparison object that defines how this sequence is sorted.
        /// </summary>
        IComparer<T> Comparer { get; }
    }

    /// <summary>
    /// Extension methods for <see cref="ISortedEnumerable{T}"/>.
    /// </summary>
    public static class SortedEnumerableExtensions
    {
        /// <summary>
        /// Returns a value indicating whether this sequence is sorted according to the default comparison object.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>Whether this sequence is sorted according to the default comparison object.</returns>
        public static bool IsSorted<T>(this IEnumerable<T> source)
        {
            return IsSorted(source, Comparer<T>.Default);
        }

        /// <summary>
        /// Returns a value indicating whether this sequence is sorted according to the given comparison object.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="comparer">The comparison object.</param>
        /// <returns>Whether this sequence is sorted according to the given comparison object.</returns>
        public static bool IsSorted<T>(this IEnumerable<T> source, IComparer<T> comparer)
        {
            using (IEnumerator<T> iterator = source.GetEnumerator())
            {
                if (!iterator.MoveNext())
                {
                    // Empty sequences are always sorted
                    return true;
                }

                T last = iterator.Current;
                while (iterator.MoveNext())
                {
                    if (comparer.Compare(last, iterator.Current) > 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Returns the source typed as <see cref="ISortedEnumerable{T}"/>. This method has no effect other than to restrict the compile-time type of an object implementing <see cref="ISortedEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="list"/>.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>The source sequence, typed as <see cref="ISortedEnumerable{T}"/>.</returns>
        public static ISortedEnumerable<T> AsSortedEnumerable<T>(this ISortedEnumerable<T> source)
        {
            return source;
        }

        /// <summary>
        /// Treats a sequence as though it were already sorted.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="source">The sequence, which is already sorted.</param>
        /// <param name="comparer">The comparison object that defines how the sequence is sorted.</param>
        /// <returns>The sorted sequence.</returns>
        public static ISortedEnumerable<T> AsSorted<T>(this IEnumerable<T> source, IComparer<T> comparer)
        {
            return new AnonymousSortedEnumerable<T>(source, comparer);
        }

        /// <summary>
        /// Treats a sequence as though it were already sorted by the item type's default comparison.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="source">The sequence, which is already sorted.</param>
        /// <returns>The sorted sequence.</returns>
        public static ISortedEnumerable<T> AsSorted<T>(this IEnumerable<T> source)
        {
            return source.AsSorted(Comparer<T>.Default);
        }

#if !SILVERLIGHT3 // SL3 does not have SortedList<TKey, TValue>
        /// <summary>
        /// Treats a <see cref="SortedList{TKey,TValue}"/> as a sorted sequence, sorted by the comparison function of the <see cref="SortedList{TKey,TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the <see cref="SortedList{TKey,TValue}"/>.</typeparam>
        /// <typeparam name="TValue">The type of values in the <see cref="SortedList{TKey,TValue}"/>.</typeparam>
        /// <param name="source">The <see cref="SortedList{TKey,TValue}"/>.</param>
        /// <returns>The <see cref="SortedList{TKey,TValue}"/> as a sorted sequence.</returns>
        public static ISortedEnumerable<KeyValuePair<TKey, TValue>> AsSorted<TKey, TValue>(this SortedList<TKey, TValue> source)
        {
            return source.AsSorted(new AnonymousComparer<KeyValuePair<TKey, TValue>> { Compare = (x, y) => source.Comparer.Compare(x.Key, y.Key) });
        }
#endif

#if !SILVERLIGHT3 // SL3 does not have SortedDictionary<TKey, TValue>
        /// <summary>
        /// Treats a <see cref="SortedDictionary{TKey,TValue}"/> as a sorted sequence, sorted by the comparison function of the <see cref="SortedDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the <see cref="SortedDictionary{TKey,TValue}"/>.</typeparam>
        /// <typeparam name="TValue">The type of values in the <see cref="SortedDictionary{TKey,TValue}"/>.</typeparam>
        /// <param name="source">The <see cref="SortedDictionary{TKey,TValue}"/>.</param>
        /// <returns>The <see cref="SortedDictionary{TKey,TValue}"/> as a sorted sequence.</returns>
        public static ISortedEnumerable<KeyValuePair<TKey, TValue>> AsSorted<TKey, TValue>(this SortedDictionary<TKey, TValue> source)
        {
            return source.AsSorted(new AnonymousComparer<KeyValuePair<TKey, TValue>> { Compare = (x, y) => source.Comparer.Compare(x.Key, y.Key) });
        }
#endif

        /// <summary>
        /// Creates a sorted, empty sequence. The sequence is sorted by the specified comparison object.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="comparer">The comparison object.</param>
        /// <returns>The sorted, empty sequence.</returns>
        public static ISortedEnumerable<T> Empty<T>(IComparer<T> comparer)
        {
            return new AnonymousSortedEnumerable<T>(Enumerable.Empty<T>(), comparer);
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
            return new AnonymousSortedEnumerable<T>(EnumerableExtensions.Return(source), comparer);
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
            return new AnonymousSortedEnumerable<T>(EnumerableExtensions.Repeat(source, count), comparer);
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
            return new AnonymousSortedEnumerable<T>(EnumerableExtensions.Repeat(source), comparer);
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

        /// <summary>
        /// Searches a sorted sequence for a given value, returning the index of the first matching item if found. If not found, the return value is the bitwise complement of the next element larger than the value.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="source">The sorted sequence.</param>
        /// <param name="item">The item to search for in the sequence.</param>
        /// <returns>The index of the first occurence of <paramref name="item"/> if it was in the list; otherwise, the bitwise complement of the next larger element in the sequence.</returns>
        public static int LowerBound<T>(this ISortedEnumerable<T> source, T item)
        {
            IComparer<T> comparer = source.Comparer;
            return source.LowerBound(x => comparer.Compare(item, x));
        }

        /// <summary>
        /// Searches a sorted sequence for a given value, returning the index of the first matching item if found. If not found, the return value is the bitwise complement of the next element larger than the value. The finder function must be compatible with the comparer used to sort the sequence.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="source">The sorted sequence.</param>
        /// <param name="finder">The finder function to use to find the item. This function should return 0 for a match, a negative value (meaning "search lower") if its parameter is too large, or a positive value (meaning "search higher") if its parameter is too small.</param>
        /// <returns>The index of the first item that causes <paramref name="finder"/> to return 0, if any; otherwise, the bitwise complement of the next larger element in the list.</returns>
        public static int LowerBound<T>(this ISortedEnumerable<T> source, Func<T, int> finder)
        {
            int index = 0;
            foreach (T item in source)
            {
                int test = finder(item);
                if (test == 0)
                {
                    return index;
                }
                else if (test < 0)
                {
                    return ~index;
                }

                ++index;
            }

            return ~index;
        }

        /// <summary>
        /// Searches a sorted sequence for a given value, returning the index one past the last matching item if found. If not found, the return value is the bitwise complement of the next element larger than the value.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="source">The sorted sequence.</param>
        /// <param name="item">The item to search for in the sequence.</param>
        /// <returns>The index one past the last occurence of <paramref name="item"/> if it was in the sequence; otherwise, the bitwise complement of the next larger element in the sequence.</returns>
        public static int UpperBound<T>(this ISortedEnumerable<T> source, T item)
        {
            IComparer<T> comparer = source.Comparer;
            return source.UpperBound(x => comparer.Compare(item, x));
        }

        /// <summary>
        /// Searches a sorted sequence using a given finder function, returning the index one past the last matching item if found. If not found, the return value is the bitwise complement of the next element larger than the value. The finder function must be compatible with the comparer used to sort the sequence.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="source">The sorted sequence.</param>
        /// <param name="finder">The finder function to use to find the item. This function should return 0 for a match, a negative value (meaning "search lower") if its parameter is too large, or a positive value (meaning "search higher") if its parameter is too small.</param>
        /// <returns>The index one past the last item that causes <paramref name="finder"/> to return 0, if any; otherwise, the bitwise complement of the index one past the last item that causes <paramref name="finder"/> to return a positive result.</returns>
        public static int UpperBound<T>(this ISortedEnumerable<T> source, Func<T, int> finder)
        {
            int index = 0;
            bool found = false;
            foreach (T item in source)
            {
                int test = finder(item);
                if (test == 0)
                {
                    found = true;
                }
                else if (test < 0)
                {
                    if (found)
                    {
                        return index;
                    }
                    else
                    {
                        return ~index;
                    }
                }

                ++index;
            }

            return ~index;
        }

        /// <summary>
        /// Searches a sorted sequence for all instances of a given value.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="source">The sorted sequence.</param>
        /// <param name="item">The item to search for in the sequence.</param>
        /// <param name="begin">The lower bound of the range of matching values. [begin, end) may be an empty range.</param>
        /// <param name="end">The upper bound of the range of matching values. [begin, end) may be an empty range.</param>
        public static void EqualRange<T>(this ISortedEnumerable<T> source, T item, out int begin, out int end)
        {
            IComparer<T> comparer = source.Comparer;
            source.EqualRange(x => comparer.Compare(item, x), out begin, out end);
        }

        /// <summary>
        /// Searches a sorted sequence using a given finder function. The finder function must be compatible with the comparer used to sort the sequence.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="source">The sorted sequence.</param>
        /// <param name="finder">The finder function to use to find the item. This function should return 0 for a match, a negative value (meaning "search lower") if its parameter is too large, or a positive value (meaning "search higher") if its parameter is too small.</param>
        /// <param name="begin">The lower bound of the range of values causing <paramref name="finder"/> to return 0. [begin, end) may be an empty range.</param>
        /// <param name="end">The upper bound of the range of values causing <paramref name="finder"/> to return 0. [begin, end) may be an empty range.</param>
        public static void EqualRange<T>(this ISortedEnumerable<T> source, Func<T, int> finder, out int begin, out int end)
        {
            begin = -1;

            int index = 0;
            foreach (T item in source)
            {
                int test = finder(item);
                if (test == 0)
                {
                    if (begin == -1)
                    {
                        begin = index;
                    }
                }
                else if (test < 0)
                {
                    if (begin == -1)
                    {
                        begin = index;
                    }

                    end = index;
                    return;
                }

                ++index;
            }

            if (begin == -1)
            {
                begin = index;
            }

            end = index;
        }

        /// <summary>
        /// Searches a sorted sequence using a given finder function. The finder function must be compatible with the comparer used to sort the sequence.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="source">The sorted sequence.</param>
        /// <param name="finder">The finder function to use to find the item. This function should return 0 for a match, a negative value (meaning "search lower") if its parameter is too large, or a positive value (meaning "search higher") if its parameter is too small.</param>
        /// <returns><c>true</c> if there is at least one item that causes <paramref name="finder"/> to return 0, if any; otherwise, <c>false</c>.</returns>
        public static bool Contains<T>(this ISortedEnumerable<T> source, Func<T, int> finder)
        {
            return LowerBound<T>(source, finder) >= 0;
        }

        /// <summary>
        /// Searches a sorted sequence for a given value.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="source">The sorted sequence.</param>
        /// <param name="item">The item to search for in the sequence.</param>
        /// <returns><c>true</c> if there is at least one item that matches <paramref name="item"/>, if any; otherwise, <c>false</c>.</returns>
        public static bool Contains<T>(this ISortedEnumerable<T> source, T item)
        {
            return LowerBound<T>(source, item) >= 0;
        }

        /// <summary>
        /// Searches a sorted sequence using a given finder function, returning the index of the first matching item if found. If not found, the return value is -1. The finder function must be compatible with the comparer used to sort the sequence.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="source">The sorted sequence.</param>
        /// <param name="finder">The finder function to use to find the item. This function should return 0 for a match, a negative value (meaning "search lower") if its parameter is too large, or a positive value (meaning "search higher") if its parameter is too small.</param>
        /// <returns>The index of the first item that causes <paramref name="finder"/> to return 0, if any; otherwise, -1.</returns>
        public static int IndexOf<T>(this ISortedEnumerable<T> source, Func<T, int> finder)
        {
            int ret = LowerBound<T>(source, finder);
            if (ret >= 0)
            {
                return ret;
            }

            return -1;
        }

        /// <summary>
        /// Searches a sorted sequence for a given value, returning the index of the first matching item if found. If not found, the return value is -1.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="source">The sorted sequence.</param>
        /// <param name="item">The item to search for in the sequence.</param>
        /// <returns>The index of the first item that matches <paramref name="item"/>, if any; otherwise, -1.</returns>
        public static int IndexOf<T>(this ISortedEnumerable<T> source, T item)
        {
            int ret = LowerBound(source, item);
            if (ret >= 0)
            {
                return ret;
            }

            return -1;
        }

        /// <summary>
        /// Searches a sorted sequence using a given finder function, returning the index of the last matching item if found. If not found, the return value is -1. The finder function must be compatible with the comparer used to sort the sequence.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="source">The sorted sequence.</param>
        /// <param name="finder">The finder function to use to find the item. This function should return 0 for a match, a negative value (meaning "search lower") if its parameter is too large, or a positive value (meaning "search higher") if its parameter is too small.</param>
        /// <returns>The index of the last item that causes <paramref name="finder"/> to return 0, if any; otherwise, -1.</returns>
        public static int LastIndexOf<T>(this ISortedEnumerable<T> source, Func<T, int> finder)
        {
            int ret = UpperBound(source, finder);
            if (ret >= 0)
            {
                return ret - 1;
            }

            return -1;
        }

        /// <summary>
        /// Searches a sorted sequence for a given value, returning the index of the last matching item if found. If not found, the return value is -1.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="source">The sorted sequence.</param>
        /// <param name="item">The item to search for in the sequence.</param>
        /// <returns>The index of the last occurence of <paramref name="item"/>, if any; otherwise, -1.</returns>
        public static int LastIndexOf<T>(this ISortedEnumerable<T> source, T item)
        {
            IComparer<T> comparer = source.Comparer;
            return LastIndexOf<T>(source, x => comparer.Compare(item, x));
        }

        /// <summary>
        /// Skips the first few elements of a source sequence.
        /// </summary>
        /// <typeparam name="T">The type of object contained in the sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="offset">The offset into the sequence at which the returned sequence begins. Must be a valid index into the source sequence, or equal to the count of the source sequence.</param>
        /// <returns>A sequence that skips the first <paramref name="offset"/> elements of the source sequence.</returns>
        public static ISortedEnumerable<T> Skip<T>(this ISortedEnumerable<T> source, int offset)
        {
            return new AnonymousSortedEnumerable<T>(Enumerable.Skip(source, offset), source.Comparer);
        }

        /// <summary>
        /// Steps through a sequence using a specified step size.
        /// </summary>
        /// <typeparam name="T">The type of object contained in the sequence.</typeparam>
        /// <param name="source">The sequence to step through.</param>
        /// <param name="step">The step size. Must be greater than 0.</param>
        /// <returns>The stepped sequence.</returns>
        public static ISortedEnumerable<T> Step<T>(this ISortedEnumerable<T> source, int step)
        {
            return new AnonymousSortedEnumerable<T>(EnumerableExtensions.Step(source, step), source.Comparer);
        }

        /// <summary>
        /// Returns the first few elements of a source sequence.
        /// </summary>
        /// <typeparam name="T">The type of object contained in the sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="count">The number of elements to return. May not be less than zero. If count is greater than 0, then every value in the range [0, count) must be valid indexes into the source sequence.</param>
        /// <returns>A sequence that includes the first <paramref name="count"/> elements of the source sequence.</returns>
        public static ISortedEnumerable<T> Take<T>(this ISortedEnumerable<T> source, int count)
        {
            return new AnonymousSortedEnumerable<T>(Enumerable.Take(source, count), source.Comparer);
        }

        /// <summary>
        /// Produces a sorted sequence that is the merging of this sequence with all sequence arguments. Each argument must be a sequence that is sorted with a comparison object equivalent to this sequence's comparison object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="source">The first source sequence.</param>
        /// <param name="others">The other source sequences.</param>
        /// <returns>The merging of the first source sequence with all the other source sequences, as a sorted sequence.</returns>
        public static ISortedEnumerable<T> MergeSorted<T>(this ISortedEnumerable<T> source, params ISortedEnumerable<T>[] others)
        {
            return MergeCore(others.StartWith(source), source.Comparer);
        }

        /// <summary>
        /// Given a non-empty sequence of sorted sequences, produces a sorted sequence that is the merging of those sequences. All sorted sequences in <paramref name="sources"/> must be sorted using equivalent comparison objects.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="sources">The sequence of sorted sequences.</param>
        /// <returns>The merging of the source sequences, as a sorted sequence.</returns>
        public static ISortedEnumerable<T> MergeSorted<T>(this IEnumerable<ISortedEnumerable<T>> sources)
        {
            return MergeCore(sources, sources.First().Comparer);
        }

        /// <summary>
        /// Produces a sorted sequence that is the merging of this sequence with all value arguments. The arguments need not be in a sorted order.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="values">The values to merge into the source source.</param>
        /// <returns>The merging of the source sequence with all values.</returns>
        public static ISortedEnumerable<T> MergeSorted<T>(this ISortedEnumerable<T> source, params T[] values)
        {
            IComparer<T> comparer = source.Comparer;
            var sources = values.Select(value => Return(value, comparer));
            return MergeCore(sources.StartWith(source), comparer);
        }

        /// <summary>
        /// Produces a sorted sequence that is the union of this sequence with all sequence arguments. Each argument must be a sequence that is sorted with a comparison object equivalent to this sequence's comparison object. Items that are duplicated in at least one of the input(s) are duplicated in the output.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="source">The first source sequence.</param>
        /// <param name="others">The other source sequences.</param>
        /// <returns>The union of the first source sequence with all the other source sequences, as a sorted sequence.</returns>
        public static ISortedEnumerable<T> UnionWithDuplicates<T>(this ISortedEnumerable<T> source, params ISortedEnumerable<T>[] others)
        {
            return UnionCore(others.StartWith(source), source.Comparer);
        }

        /// <summary>
        /// Produces a sorted sequence that is the union of this sequence with all sequence arguments. Each argument must be a sequence that is sorted with a comparison object equivalent to this sequence's comparison object. The resulting sequence is distinct (no duplicate items).
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="source">The first source sequence.</param>
        /// <param name="others">The other source sequences.</param>
        /// <returns>The distinct union of the first source sequence with all the other source sequences, as a sorted sequence.</returns>
        public static ISortedEnumerable<T> Union<T>(this ISortedEnumerable<T> source, params ISortedEnumerable<T>[] others)
        {
            return source.UnionWithDuplicates(others).Distinct();
        }

        /// <summary>
        /// Given a non-empty sequence of sorted sequences, produces a sorted sequence that is the union of those sequences. All sorted sequences in <paramref name="sources"/> must be sorted using equivalent comparison objects. Items that are duplicated in at least one of the input(s) are duplicated in the output.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="sources">The sequence of sorted sequences.</param>
        /// <returns>The union of the source sequences, as a sorted sequence.</returns>
        public static ISortedEnumerable<T> UnionWithDuplicates<T>(this IEnumerable<ISortedEnumerable<T>> sources)
        {
            return UnionCore(sources, sources.First().Comparer);
        }

        /// <summary>
        /// Given a non-empty sequence of sorted sequences, produces a sorted sequence that is the union of those sequences. All sorted sequences in <paramref name="sources"/> must be sorted using equivalent comparison objects. The resulting sequence is distinct (no duplicate items).
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="sources">The sequence of sorted sequences.</param>
        /// <returns>The distinct union of the source sequences, as a sorted sequence.</returns>
        public static ISortedEnumerable<T> Union<T>(this IEnumerable<ISortedEnumerable<T>> sources)
        {
            return sources.UnionWithDuplicates().Distinct();
        }

        /// <summary>
        /// Produces a sorted sequence that is the union of this sequence with all value arguments. The arguments need not be in a sorted order. Items that are duplicated in at least one of the input(s) are duplicated in the output.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="values">The values to merge into the source source.</param>
        /// <returns>The union of the source sequence with all values.</returns>
        public static ISortedEnumerable<T> UnionWithDuplicates<T>(this ISortedEnumerable<T> source, params T[] values)
        {
            IComparer<T> comparer = source.Comparer;
            var sources = values.Select(value => Return(value, comparer));
            return UnionCore(sources.StartWith(source), comparer);
        }

        /// <summary>
        /// Produces a sorted sequence that is the union of this sequence with all value arguments. The arguments need not be in a sorted order. The resulting sequence is distinct (no duplicate items).
        /// </summary>
        /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="values">The values to merge into the source source.</param>
        /// <returns>The distinct union of the source sequence with all values.</returns>
        public static ISortedEnumerable<T> Union<T>(this ISortedEnumerable<T> source, params T[] values)
        {
            return source.UnionWithDuplicates(values).Distinct();
        }

        /// <summary>
        /// Produces a sorted sequence that is the intersection of this sequence with all sequence arguments. Each argument must be a sequence that is sorted with a comparison object equivalent to this sequence's comparison object. Items that are duplicated in all input sequences are duplicated in the output.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="source">The first source sequence.</param>
        /// <param name="others">The other source sequences.</param>
        /// <returns>The intersection of the first source sequence with all the other source sequences, as a sorted sequence.</returns>
        public static ISortedEnumerable<T> IntersectWithDuplicates<T>(this ISortedEnumerable<T> source, params ISortedEnumerable<T>[] others)
        {
            return IntersectCore(others.StartWith(source), source.Comparer);
        }

        /// <summary>
        /// Produces a sorted sequence that is the intersection of this sequence with all sequence arguments. Each argument must be a sequence that is sorted with a comparison object equivalent to this sequence's comparison object. The resulting sequence is distinct (no duplicate items).
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="source">The first source sequence.</param>
        /// <param name="others">The other source sequences.</param>
        /// <returns>The intersection of the first source sequence with all the other source sequences, as a distinct, sorted sequence.</returns>
        public static ISortedEnumerable<T> Intersect<T>(this ISortedEnumerable<T> source, params ISortedEnumerable<T>[] others)
        {
            return source.IntersectWithDuplicates(others).Distinct();
        }

        /// <summary>
        /// Given a non-empty sequence of sorted sequences, produces a sorted sequence that is the intersection of those sequences. All sorted sequences in <paramref name="sources"/> must be sorted using equivalent comparison objects. Items that are duplicated in all input sequences are duplicated in the output.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="sources">The sequence of sorted sequences.</param>
        /// <returns>The intersection of the source sequences, as a sorted sequence.</returns>
        public static ISortedEnumerable<T> IntersectWithDuplicates<T>(this IEnumerable<ISortedEnumerable<T>> sources)
        {
            return IntersectCore(sources, sources.First().Comparer);
        }

        /// <summary>
        /// Given a non-empty sequence of sorted sequences, produces a sorted sequence that is the intersection of those sequences. All sorted sequences in <paramref name="sources"/> must be sorted using equivalent comparison objects. The resulting sequence is distinct (no duplicate items).
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="sources">The sequence of sorted sequences.</param>
        /// <returns>The intersection of the source sequences, as a distinct, sorted sequence.</returns>
        public static ISortedEnumerable<T> Intersect<T>(this IEnumerable<ISortedEnumerable<T>> sources)
        {
            return sources.IntersectWithDuplicates().Distinct();
        }

        /// <summary>
        /// Produces a sorted sequence that is the difference of this sequence with another sequence. The other sequence must be a sequence that is sorted with a comparison object equivalent to this sequence's comparison object. Items that are duplicated in the source sequences must be duplicated in the other sequence to prevent them from being included in the resulting sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="source">The first source sequence.</param>
        /// <param name="other">The other source sequence.</param>
        /// <returns>A sorted sequence containing all elements in the first source sequence except for elements in the other source sequence.</returns>
        public static ISortedEnumerable<T> ExceptWithDuplicates<T>(this ISortedEnumerable<T> source, ISortedEnumerable<T> other)
        {
            IComparer<T> comparer = source.Comparer;
            return new AnonymousSortedEnumerable<T>(ExceptCore(source, other, comparer), comparer);
        }

        /// <summary>
        /// Produces a sorted sequence that is the difference of this sequence with another sequence. The other sequence must be a sequence that is sorted with a comparison object equivalent to this sequence's comparison object. The resulting sequence is distinct (no duplicate items).
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="source">The first source sequence.</param>
        /// <param name="other">The other source sequence.</param>
        /// <returns>A sorted sequence containing all distinct elements in the first source sequence except for elements in the other source sequence.</returns>
        public static ISortedEnumerable<T> Except<T>(this ISortedEnumerable<T> source, ISortedEnumerable<T> other)
        {
            return source.Distinct().ExceptWithDuplicates(other);
        }

        /// <summary>
        /// Returns distinct elements from a sorted sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>Each distinct element in <paramref name="source"/>.</returns>
        public static ISortedEnumerable<T> Distinct<T>(this ISortedEnumerable<T> source)
        {
            IComparer<T> comparer = source.Comparer;
            return new AnonymousSortedEnumerable<T>(DistinctCore(source, comparer), comparer);
        }

        /// <summary>
        /// Returns distinct elements from an implicitly sorted sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The implicitly sorted source sequence.</param>
        /// <param name="comparer">The comparison object. The implicitly sorted sequence <paramref name="source"/> must be sorted using a comparison object equivalent to <paramref name="comparer"/>.</param>
        /// <returns>Each distinct element in <paramref name="source"/>, as an implicitly sorted sequence.</returns>
        private static IEnumerable<T> DistinctCore<T>(IEnumerable<T> source, IComparer<T> comparer)
        {
            using (IEnumerator<T> iterator = source.GetEnumerator())
            {
                if (!iterator.MoveNext())
                {
                    yield break;
                }

                T lastValue = iterator.Current;
                yield return lastValue;
                while (iterator.MoveNext())
                {
                    if (comparer.Compare(lastValue, iterator.Current) != 0)
                    {
                        lastValue = iterator.Current;
                        yield return lastValue;
                    }
                }
            }
        }

        /// <summary>
        /// Given a sequence of sorted sequences, produces a sorted sequence that is the union of those sequences according to a specified comparison object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="sources">The sequence of sorted sequences.</param>
        /// <param name="comparer">The comparison object. All sorted sequences in <paramref name="sources"/> must be sorted using a comparison object equivalent to <paramref name="comparer"/>.</param>
        /// <returns>The union of the source sequences, as a sorted sequence.</returns>
        private static ISortedEnumerable<T> UnionCore<T>(IEnumerable<ISortedEnumerable<T>> sources, IComparer<T> comparer)
        {
            return new AnonymousSortedEnumerable<T>(sources.Cast<IEnumerable<T>>().Aggregate((x, y) => UnionCore(x, y, comparer)), comparer);
        }

        /// <summary>
        /// Given two implicitly sorted sequences, produces an implicitly sorted sequence that is the union of those sequences according to a specified comparison object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="first">The first implicitly sorted sequence.</param>
        /// <param name="second">The second implicitly sorted sequence.</param>
        /// <param name="comparer">The comparison object. Both <paramref name="first"/> and <paramref name="second"/> must be sorted using a comparison object equivalent to <paramref name="comparer"/>.</param>
        /// <returns>The union of <paramref name="first"/> and <paramref name="second"/>, as an implicitly sorted sequence.</returns>
        private static IEnumerable<T> UnionCore<T>(IEnumerable<T> first, IEnumerable<T> second, IComparer<T> comparer)
        {
            using (ForwardIterator<T> firstIterator = new ForwardIterator<T>(first))
            using (ForwardIterator<T> secondIterator = new ForwardIterator<T>(second))
            {
                while (!firstIterator.Done && !secondIterator.Done)
                {
                    int test = comparer.Compare(firstIterator.Current, secondIterator.Current);
                    if (test < 0)
                    {
                        yield return firstIterator.Current;
                        firstIterator.MoveNext();
                    }
                    else if (test > 0)
                    {
                        yield return secondIterator.Current;
                        secondIterator.MoveNext();
                    }
                    else
                    {
                        yield return firstIterator.Current;
                        firstIterator.MoveNext();
                        secondIterator.MoveNext();
                    }
                }

                if (!firstIterator.Done)
                {
                    foreach (T value in firstIterator.RemainingValues)
                    {
                        yield return value;
                    }
                }

                if (!secondIterator.Done)
                {
                    foreach (T value in secondIterator.RemainingValues)
                    {
                        yield return value;
                    }
                }
            }
        }

        /// <summary>
        /// Given a sequence of sorted sequences, produces a sorted sequence that is the merging of those sequences according to a specified comparison object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="sources">The sequence of sorted sequences.</param>
        /// <param name="comparer">The comparison object. All sorted sequences in <paramref name="sources"/> must be sorted using a comparison object equivalent to <paramref name="comparer"/>.</param>
        /// <returns>The merging of the source sequences, as a sorted sequence.</returns>
        private static ISortedEnumerable<T> MergeCore<T>(IEnumerable<ISortedEnumerable<T>> sources, IComparer<T> comparer)
        {
            return new AnonymousSortedEnumerable<T>(sources.Cast<IEnumerable<T>>().Aggregate((x, y) => MergeCore(x, y, comparer)), comparer);
        }

        /// <summary>
        /// Given two implicitly sorted sequences, produces an implicitly sorted sequence that is the merging of those sequences according to a specified comparison object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="first">The first implicitly sorted sequence.</param>
        /// <param name="second">The second implicitly sorted sequence.</param>
        /// <param name="comparer">The comparison object. Both <paramref name="first"/> and <paramref name="second"/> must be sorted using a comparison object equivalent to <paramref name="comparer"/>.</param>
        /// <returns>The merging of <paramref name="first"/> and <paramref name="second"/>, as an implicitly sorted sequence.</returns>
        private static IEnumerable<T> MergeCore<T>(IEnumerable<T> first, IEnumerable<T> second, IComparer<T> comparer)
        {
            using (ForwardIterator<T> firstIterator = new ForwardIterator<T>(first))
            using (ForwardIterator<T> secondIterator = new ForwardIterator<T>(second))
            {
                while (!firstIterator.Done && !secondIterator.Done)
                {
                    int test = comparer.Compare(firstIterator.Current, secondIterator.Current);
                    if (test <= 0)
                    {
                        yield return firstIterator.Current;
                        firstIterator.MoveNext();
                    }
                    else if (test > 0)
                    {
                        yield return secondIterator.Current;
                        secondIterator.MoveNext();
                    }
                }

                if (!firstIterator.Done)
                {
                    foreach (T value in firstIterator.RemainingValues)
                    {
                        yield return value;
                    }
                }

                if (!secondIterator.Done)
                {
                    foreach (T value in secondIterator.RemainingValues)
                    {
                        yield return value;
                    }
                }
            }
        }

        /// <summary>
        /// Given a sequence of sorted sequences, produces a sorted sequence that is the intersection of those sequences according to a specified comparison object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="sources">The sequence of sorted sequences.</param>
        /// <param name="comparer">The comparison object. All sorted sequences in <paramref name="sources"/> must be sorted using a comparison object equivalent to <paramref name="comparer"/>.</param>
        /// <returns>The intersection of the source sequences, as a sorted sequence.</returns>
        private static ISortedEnumerable<T> IntersectCore<T>(IEnumerable<ISortedEnumerable<T>> sources, IComparer<T> comparer)
        {
            return new AnonymousSortedEnumerable<T>(sources.Cast<IEnumerable<T>>().Aggregate((x, y) => IntersectCore(x, y, comparer)), comparer);
        }

        /// <summary>
        /// Given two implicitly sorted sequences, produces an implicitly sorted sequence that is the intersection of those sequences according to a specified comparison object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="first">The first implicitly sorted sequence.</param>
        /// <param name="second">The second implicitly sorted sequence.</param>
        /// <param name="comparer">The comparison object. Both <paramref name="first"/> and <paramref name="second"/> must be sorted using a comparison object equivalent to <paramref name="comparer"/>.</param>
        /// <returns>The intersection of <paramref name="first"/> and <paramref name="second"/>, as an implicitly sorted sequence.</returns>
        private static IEnumerable<T> IntersectCore<T>(IEnumerable<T> first, IEnumerable<T> second, IComparer<T> comparer)
        {
            using (ForwardIterator<T> firstIterator = new ForwardIterator<T>(first))
            using (ForwardIterator<T> secondIterator = new ForwardIterator<T>(second))
            {
                while (!firstIterator.Done && !secondIterator.Done)
                {
                    int test = comparer.Compare(firstIterator.Current, secondIterator.Current);
                    if (test < 0)
                    {
                        firstIterator.MoveNext();
                    }
                    else if (test > 0)
                    {
                        secondIterator.MoveNext();
                    }
                    else
                    {
                        yield return firstIterator.Current;
                        firstIterator.MoveNext();
                        secondIterator.MoveNext();
                    }
                }
            }
        }

        /// <summary>
        /// Given two implicitly sorted sequences, produces an implicitly sorted sequence that is the difference of those sequences according to a specified comparison object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="first">The first implicitly sorted sequence.</param>
        /// <param name="second">The second implicitly sorted sequence.</param>
        /// <param name="comparer">The comparison object. Both <paramref name="first"/> and <paramref name="second"/> must be sorted using a comparison object equivalent to <paramref name="comparer"/>.</param>
        /// <returns>The difference of <paramref name="first"/> and <paramref name="second"/>, as an implicitly sorted sequence.</returns>
        private static IEnumerable<T> ExceptCore<T>(IEnumerable<T> first, IEnumerable<T> second, IComparer<T> comparer)
        {
            using (ForwardIterator<T> firstIterator = new ForwardIterator<T>(first))
            using (ForwardIterator<T> secondIterator = new ForwardIterator<T>(second))
            {
                while (!firstIterator.Done && !secondIterator.Done)
                {
                    int test = comparer.Compare(firstIterator.Current, secondIterator.Current);
                    if (test < 0)
                    {
                        yield return firstIterator.Current;
                        firstIterator.MoveNext();
                    }
                    else if (test > 0)
                    {
                        secondIterator.MoveNext();
                    }
                    else
                    {
                        firstIterator.MoveNext();
                        secondIterator.MoveNext();
                    }
                }

                if (!firstIterator.Done)
                {
                    foreach (T value in firstIterator.RemainingValues)
                    {
                        yield return value;
                    }
                }
            }
        }

        /// <summary>
        /// An enumerator that caches the return value from <see cref="MoveNext"/> and starts on the first value.
        /// </summary>
        /// <typeparam name="T">The type of element contained in the enumerator.</typeparam>
        private sealed class ForwardIterator<T> : IDisposable
        {
            /// <summary>
            /// The source enumerator.
            /// </summary>
            private IEnumerator<T> source;

            /// <summary>
            /// Initializes a new instance of the <see cref="ForwardIterator&lt;T&gt;"/> class with the given source enumerable.
            /// </summary>
            /// <param name="source">The source enumerable.</param>
            public ForwardIterator(IEnumerable<T> source)
            {
                this.source = source.GetEnumerator();
                this.Done = !this.source.MoveNext();
            }

            /// <summary>
            /// Gets a value indicating whether this iterator is done.
            /// </summary>
            public bool Done { get; private set; }

            /// <summary>
            /// Gets the current value of the iterator. Only valid if <see cref="Done"/> is false.
            /// </summary>
            public T Current
            {
                get { return this.source.Current; }
            }

            /// <summary>
            /// Gets an enumeration that, starting with <see cref="Current"/>, yields all remaining values in the source enumerator.
            /// </summary>
            public IEnumerable<T> RemainingValues
            {
                get
                {
                    yield return this.source.Current;
                    while (this.source.MoveNext())
                    {
                        yield return this.source.Current;
                    }

                    this.Done = true;
                }
            }

            /// <summary>
            /// Moves the iterator forward one value. Returns false if <see cref="Done"/> is now true.
            /// </summary>
            /// <returns>Whether the sequence is still valid.</returns>
            public bool MoveNext()
            {
                this.Done = !this.source.MoveNext();
                return !this.Done;
            }

            /// <summary>
            /// Disposes the iterator.
            /// </summary>
            public void Dispose()
            {
                this.source.Dispose();
            }
        }

        /// <summary>
        /// Wraps a source sequence and comparison object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        private sealed class AnonymousSortedEnumerable<T> : ISortedEnumerable<T>
        {
            /// <summary>
            /// The source sequence.
            /// </summary>
            private IEnumerable<T> source;

            /// <summary>
            /// Initializes a new instance of the <see cref="AnonymousSortedEnumerable&lt;T&gt;"/> class with the specified source sequence and comparison object.
            /// </summary>
            /// <param name="source">The source sequence.</param>
            /// <param name="comparer">The comparison object.</param>
            public AnonymousSortedEnumerable(IEnumerable<T> source, IComparer<T> comparer)
            {
                this.source = source;
                this.Comparer = comparer;
            }

            /// <summary>
            /// Gets a comparison object that defines how this sequence is sorted.
            /// </summary>
            public IComparer<T> Comparer { get; private set; }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
            /// </returns>
            public IEnumerator<T> GetEnumerator()
            {
                return this.source.GetEnumerator();
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
}

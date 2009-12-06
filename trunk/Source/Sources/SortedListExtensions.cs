﻿// <copyright file="SortedListExtensions.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Represents a list that is sorted by a single comparison.
    /// </summary>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    public interface ISortedList<T> : IList<T>, ISortedEnumerable<T>
    {
    }

    /// <summary>
    /// Extension methods for <see cref="ISortedList{T}"/>.
    /// </summary>
    public static partial class SortedListExtensions
    {
        #region Converters

        /// <summary>
        /// Returns the source typed as <see cref="ISortedList{T}"/>. This method has no effect other than to restrict the compile-time type of an object implementing <see cref="ISortedList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="list"/>.</typeparam>
        /// <param name="list">The source list.</param>
        /// <returns>The source list, typed as <see cref="ISortedList{T}"/>.</returns>
        public static ISortedList<T> AsSortedList<T>(this ISortedList<T> list)
        {
            return list;
        }

        /// <summary>
        /// Treats a list as though it were already sorted.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The list, which is already sorted.</param>
        /// <param name="comparer">The comparison object that defines how the list is sorted.</param>
        /// <returns>The sorted list.</returns>
        public static ISortedList<T> AsSorted<T>(this IList<T> list, IComparer<T> comparer)
        {
            return new AnonymousSortedList<T>(list, comparer);
        }

        /// <summary>
        /// Treats a list as though it were already sorted by the item type's default comparison.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The list, which is already sorted.</param>
        /// <returns>The sorted list.</returns>
        public static ISortedList<T> AsSorted<T>(this IList<T> list)
        {
            return list.AsSorted(Comparer<T>.Default);
        }

        /// <summary>
        /// Creates a sorted list from a sorted source sequence.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>The sorted list.</returns>
        public static ISortedList<T> ToSortedList<T>(this ISortedEnumerable<T> source)
        {
            return new AnonymousSortedList<T>(source.ToList(), source.Comparer);
        }

        #endregion

        #region Sources

        /// <summary>
        /// Creates an empty sorted list. The list is sorted by the specified comparison object.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="comparer">The comparison object.</param>
        /// <returns>An empty sorted list.</returns>
        public static ISortedList<T> Empty<T>(IComparer<T> comparer)
        {
            return new AnonymousSortedList<T>(ListExtensions.Empty<T>(), comparer);
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
            return new AnonymousSortedList<T>(ListExtensions.Return(source), comparer);
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
            return new AnonymousSortedList<T>(ListExtensions.Repeat(source, count), comparer);
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

        #endregion

        #region Operators

        /// <summary>
        /// Returns a list that acts as though it has been reversed, with a reversed comparison object.
        /// </summary>
        /// <typeparam name="T">The type of elements contained in the list.</typeparam>
        /// <param name="list">The source list.</param>
        /// <returns>A list sorted in reverse order of the original list.</returns>
        public static ISortedList<T> Reverse<T>(this ISortedList<T> list)
        {
            // Reverse the list and its comparison object
            return new AnonymousSortedList<T>(ListExtensions.Reverse(list), new AnonymousComparer<T> { Compare = (x, y) => list.Comparer.Compare(y, x) });
        }

        /// <summary>
        /// Returns a read-only list wrapper that remembers the values of its source list. Acts as a cache for the source list elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source list.</typeparam>
        /// <param name="list">The source list.</param>
        /// <returns>A read-only cache wrapper for the source list.</returns>
        public static ISortedList<T> Memoize<T>(this ISortedList<T> list)
        {
            return new AnonymousSortedList<T>(ListExtensions.Memoize(list), list.Comparer);
        }

        /// <summary>
        /// Returns a read-only list wrapper that evaluates all members of its source list. Acts as a cache for the entire source list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source list.</typeparam>
        /// <param name="list">The source list.</param>
        /// <returns>A read-only cache wrapper for the source list.</returns>
        public static ISortedList<T> MemoizeAll<T>(this ISortedList<T> list)
        {
            return new AnonymousSortedList<T>(ListExtensions.MemoizeAll(list), list.Comparer);
        }

        /// <summary>
        /// Returns a sliced list, which acts as a window into a subset of the original list.
        /// </summary>
        /// <typeparam name="T">The type of object contained in the list.</typeparam>
        /// <param name="list">The list to slice.</param>
        /// <param name="offset">The offset into the list at which the slice begins. Must be a valid index into the source list, or equal to the count of the source list.</param>
        /// <returns>A list that is a slice of the source list.</returns>
        public static ISortedList<T> Skip<T>(this ISortedList<T> list, int offset)
        {
            return new AnonymousSortedList<T>(ListExtensions.Skip(list, offset), list.Comparer);
        }

        /// <summary>
        /// Returns a sliced list, which acts as a window into a subset of the original list. Similar to Skip followed by Take, only this is more efficient and preserves IList semantics.
        /// </summary>
        /// <typeparam name="T">The type of object contained in the list.</typeparam>
        /// <param name="list">The list to slice.</param>
        /// <param name="offset">The offset into the list at which the slice begins. Must be a valid index into the source list, or equal to the count of the source list.</param>
        /// <param name="count">The number of elements in the slice. May not be less than zero. If count is greater than 0, then every value in the range [offset, offset + count) must be valid indexes into the source list.</param>
        /// <returns>A list that is a slice of the source list.</returns>
        public static ISortedList<T> Slice<T>(this ISortedList<T> list, int offset, int count)
        {
            return new AnonymousSortedList<T>(ListExtensions.Slice(list, offset, count), list.Comparer);
        }

        /// <summary>
        /// Steps through a list using a specified step size.
        /// </summary>
        /// <typeparam name="T">The type of object contained in the list.</typeparam>
        /// <param name="list">The list to step through.</param>
        /// <param name="step">The step size. Must be greater than 0.</param>
        /// <returns>The stepped list.</returns>
        public static ISortedList<T> Step<T>(this ISortedList<T> list, int step)
        {
            return new AnonymousSortedList<T>(ListExtensions.Step(list, step), list.Comparer);
        }

        /// <summary>
        /// Returns a sliced list, which acts as a window into a subset of the original list.
        /// </summary>
        /// <typeparam name="T">The type of object contained in the list.</typeparam>
        /// <param name="list">The list to slice.</param>
        /// <param name="count">The number of elements in the slice. May not be less than zero. If count is greater than 0, then every value in the range [0, count) must be valid indexes into the source list.</param>
        /// <returns>A list that is a slice of the source list.</returns>
        public static ISortedList<T> Take<T>(this ISortedList<T> list, int count)
        {
            return new AnonymousSortedList<T>(ListExtensions.Take(list, count), list.Comparer);
        }

        /// <summary>
        /// Sorts a list in-place using the default comparison object.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The list to be sorted.</param>
        /// <param name="others">Other lists to be kept in sync with the sorted list. As <paramref name="list"/> is sorted, the same relative elements are rearranged in these lists.</param>
        /// <returns>The sorted list.</returns>
        public static ISortedList<T> Sort<T>(this IList<T> list, params ListExtensions.ISwappable[] others)
        {
            return Sort(list, Comparer<T>.Default, others);
        }

        /// <summary>
        /// Sorts a list in-place using the specified comparison object.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The list to be sorted.</param>
        /// <param name="comparer">The comparison object.</param>
        /// <param name="others">Other lists to be kept in sync with the sorted list. As <paramref name="list"/> is sorted, the same relative elements are rearranged in these lists.</param>
        /// <returns>The sorted list.</returns>
        public static ISortedList<T> Sort<T>(this IList<T> list, IComparer<T> comparer, params ListExtensions.ISwappable[] others)
        {
            int count = list.Count;
            foreach (ListExtensions.ISwappable other in others)
            {
                if (other.Count < count)
                {
                    throw new ArgumentException("Lists kept in sync while sorting must be at least as long as the list to sort.");
                }
            }

            QuickSortCore(new SortParameters<T> { List = list, Comparer = comparer, Others = others }, 0, count);
            return list.AsSorted(comparer);
        }

        /// <summary>
        /// Creates and returns a sorted view of the source list, using the specified comparison object.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The list to be sorted. As long as the returned view is referenced, the source list should not be modified in any way.</param>
        /// <param name="comparer">The comparison object.</param>
        /// <param name="others">Other lists to be kept in sync with the sorted view. As the view is sorted, the same relative elements are rearranged in these lists.</param>
        /// <returns>The sorted view of the source list.</returns>
        public static ISortedList<T> SortIndirect<T>(this IList<T> list, IComparer<T> comparer, params ListExtensions.ISwappable[] others)
        {
            var indirect = new IndirectList<T>(list);
            indirect.Indices.Sort(indirect.GetComparer(comparer), others);
            return indirect.AsSorted(comparer);
        }

        /// <summary>
        /// Creates and returns a sorted view of the source list, using the default comparison object.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The list to be sorted. As long as the returned view is referenced, the source list should not be modified in any way.</param>
        /// <param name="others">Other lists to be kept in sync with the sorted view. As the view is sorted, the same relative elements are rearranged in these lists.</param>
        /// <returns>The sorted view of the source list.</returns>
        public static ISortedList<T> SortIndirect<T>(this IList<T> list, params ListExtensions.ISwappable[] others)
        {
            return SortIndirect(list, Comparer<T>.Default, others);
        }

        #endregion

        #region Consumers

        /// <summary>
        /// Searches a sorted list using a given finder function. The finder function must be compatible with the comparer used to sort the list.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The sorted list.</param>
        /// <param name="finder">The finder function to use to find the item. This function should return 0 for a match, a negative value (meaning "search lower") if its parameter is too large, or a positive value (meaning "search higher") if its parameter is too small.</param>
        /// <returns><c>true</c> if there is at least one item that causes <paramref name="finder"/> to return 0, if any; otherwise, <c>false</c>.</returns>
        public static bool Contains<T>(this ISortedList<T> list, Func<T, int> finder)
        {
            return BinarySearch(list, finder) >= 0;
        }

        /// <summary>
        /// Searches a sorted list using a given finder function, returning the index of the first matching item if found. If not found, the return value is -1. The finder function must be compatible with the comparer used to sort the list.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The sorted list.</param>
        /// <param name="finder">The finder function to use to find the item. This function should return 0 for a match, a negative value (meaning "search lower") if its parameter is too large, or a positive value (meaning "search higher") if its parameter is too small.</param>
        /// <returns>The index of the first item that causes <paramref name="finder"/> to return 0, if any; otherwise, -1.</returns>
        public static int IndexOf<T>(this ISortedList<T> list, Func<T, int> finder)
        {
            int ret = LowerBound(list, finder);
            if (ret >= 0)
            {
                return ret;
            }

            return -1;
        }

        /// <summary>
        /// Searches a sorted list using a given finder function, returning the index of the last matching item if found. If not found, the return value is -1. The finder function must be compatible with the comparer used to sort the list.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The sorted list.</param>
        /// <param name="finder">The finder function to use to find the item. This function should return 0 for a match, a negative value (meaning "search lower") if its parameter is too large, or a positive value (meaning "search higher") if its parameter is too small.</param>
        /// <returns>The index of the last item that causes <paramref name="finder"/> to return 0, if any; otherwise, -1.</returns>
        public static int LastIndexOf<T>(this ISortedList<T> list, Func<T, int> finder)
        {
            int ret = UpperBound(list, finder);
            if (ret >= 0)
            {
                return ret - 1;
            }

            return -1;
        }

        /// <summary>
        /// Searches a sorted list for a given value, returning the index of the last matching item if found. If not found, the return value is -1.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The sorted list.</param>
        /// <param name="item">The item to search for in the list.</param>
        /// <returns>The index of the last occurence of <paramref name="item"/>, if any; otherwise, -1.</returns>
        public static int LastIndexOf<T>(this ISortedList<T> list, T item)
        {
            IComparer<T> comparer = list.Comparer;
            return LastIndexOf<T>(list, x => comparer.Compare(item, x));
        }

        /// <summary>
        /// Searches a sorted list for a given value, returning its index if found. If not found, the return value is the bitwise complement of the next element larger than the value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The sorted list.</param>
        /// <param name="item">The item to search for in the list.</param>
        /// <returns>The index of <paramref name="item"/> if it was in the list; otherwise, the bitwise complement of the next larger element in the list.</returns>
        public static int BinarySearch<T>(this ISortedList<T> list, T item)
        {
            IComparer<T> comparer = list.Comparer;
            return list.BinarySearch(x => comparer.Compare(item, x));
        }

        /// <summary>
        /// Searches a sorted list using a given finder function. If not found, the return value is the bitwise complement of the next element larger than the value. The finder function must be compatible with the comparer used to sort the list.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The sorted list.</param>
        /// <param name="finder">The finder function to use to find the item. This function should return 0 for a match, a negative value (meaning "search lower") if its parameter is too large, or a positive value (meaning "search higher") if its parameter is too small.</param>
        /// <returns>The index of an item that causes <paramref name="finder"/> to return 0, if any; otherwise, the bitwise complement of the next larger element in the list.</returns>
        public static int BinarySearch<T>(this ISortedList<T> list, Func<T, int> finder)
        {
            int begin = 0, end = list.Count;

            return BinarySearchCore(list, finder, ref begin, ref end);
        }

        /// <summary>
        /// Searches a sorted list for a given value, returning the index of the first matching item if found. If not found, the return value is the bitwise complement of the next element larger than the value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The sorted list.</param>
        /// <param name="item">The item to search for in the list.</param>
        /// <returns>The index of the first occurence of <paramref name="item"/> if it was in the list; otherwise, the bitwise complement of the next larger element in the list.</returns>
        public static int LowerBound<T>(this ISortedList<T> list, T item)
        {
            IComparer<T> comparer = list.Comparer;
            return list.LowerBound(x => comparer.Compare(item, x));
        }

        /// <summary>
        /// Searches a sorted list using a given finder function, returning the index of the first matching item if found. If not found, the return value is the bitwise complement of the next element larger than the value. The finder function must be compatible with the comparer used to sort the list.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The sorted list.</param>
        /// <param name="finder">The finder function to use to find the item. This function should return 0 for a match, a negative value (meaning "search lower") if its parameter is too large, or a positive value (meaning "search higher") if its parameter is too small.</param>
        /// <returns>The index of the first item that causes <paramref name="finder"/> to return 0, if any; otherwise, the bitwise complement of the next larger element in the list.</returns>
        public static int LowerBound<T>(this ISortedList<T> list, Func<T, int> finder)
        {
            int begin = 0, end = list.Count;

            int mid = BinarySearchCore(list, finder, ref begin, ref end);
            if (mid < 0)
            {
                return mid;
            }

            LowerBoundCore(list, finder, ref begin, mid);

            return begin;
        }

        /// <summary>
        /// Searches a sorted list for a given value, returning the index one past the last matching item if found. If not found, the return value is the bitwise complement of the next element larger than the value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The sorted list.</param>
        /// <param name="item">The item to search for in the list.</param>
        /// <returns>The index one past the last occurence of <paramref name="item"/> if it was in the list; otherwise, the bitwise complement of the next larger element in the list.</returns>
        public static int UpperBound<T>(this ISortedList<T> list, T item)
        {
            IComparer<T> comparer = list.Comparer;
            return list.UpperBound(x => comparer.Compare(item, x));
        }

        /// <summary>
        /// Searches a sorted list using a given finder function, returning the index one past the last matching item if found. If not found, the return value is the bitwise complement of the next element larger than the value. The finder function must be compatible with the comparer used to sort the list.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The sorted list.</param>
        /// <param name="finder">The finder function to use to find the item. This function should return 0 for a match, a negative value (meaning "search lower") if its parameter is too large, or a positive value (meaning "search higher") if its parameter is too small.</param>
        /// <returns>The index one past the last item that causes <paramref name="finder"/> to return 0, if any; otherwise, the bitwise complement of the index one past the last item that causes <paramref name="finder"/> to return a positive result.</returns>
        public static int UpperBound<T>(this ISortedList<T> list, Func<T, int> finder)
        {
            int begin = 0, end = list.Count;

            int mid = BinarySearchCore(list, finder, ref begin, ref end);
            if (mid < 0)
            {
                return mid;
            }

            UpperBoundCore(list, finder, mid, ref end);

            return end;
        }

        /// <summary>
        /// Searches a sorted list for all instances of a given value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The sorted list.</param>
        /// <param name="item">The item to search for in the list.</param>
        /// <param name="begin">The lower bound of the range of matching values. [begin, end) may be an empty range.</param>
        /// <param name="end">The upper bound of the range of matching values. [begin, end) may be an empty range.</param>
        public static void EqualRange<T>(this ISortedList<T> list, T item, out int begin, out int end)
        {
            IComparer<T> comparer = list.Comparer;
            list.EqualRange(x => comparer.Compare(item, x), out begin, out end);
        }

        /// <summary>
        /// Searches a sorted list using a given finder function. The finder function must be compatible with the comparer used to sort the list.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The sorted list.</param>
        /// <param name="finder">The finder function to use to find the item. This function should return 0 for a match, a negative value (meaning "search lower") if its parameter is too large, or a positive value (meaning "search higher") if its parameter is too small.</param>
        /// <param name="begin">The lower bound of the range of values causing <paramref name="finder"/> to return 0. [begin, end) may be an empty range.</param>
        /// <param name="end">The upper bound of the range of values causing <paramref name="finder"/> to return 0. [begin, end) may be an empty range.</param>
        public static void EqualRange<T>(this ISortedList<T> list, Func<T, int> finder, out int begin, out int end)
        {
            begin = 0;
            end = list.Count;

            int mid = BinarySearchCore(list, finder, ref begin, ref end);
            if (mid < 0)
            {
                begin = ~mid;
                end = ~mid;
                return;
            }

            LowerBoundCore(list, finder, ref begin, mid);
            UpperBoundCore(list, finder, mid, ref end);
        }

        /// <summary>
        /// Inserts a value into a sorted list in-place, maintaining the sort order. Returns the index of the inserted item. To "insert" an item without modifying the source list, call <see cref="O:SortedEnumerableExtensions.MergeSorted"/>.
        /// </summary>
        /// <typeparam name="T">The type of object contained in the list.</typeparam>
        /// <param name="list">The sorted list into which to insert.</param>
        /// <param name="item">The item to insert into the list.</param>
        /// <returns>The index at which the new item was inserted.</returns>
        public static int Insert<T>(this ISortedList<T> list, T item)
        {
            int index = list.BinarySearch(item);
            if (index < 0)
            {
                index = ~index;
            }

            list.Insert(index, item);
            return index;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Performs a binary search over a sorted list, returning both a match and the narrowed range.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The sorted list.</param>
        /// <param name="finder">The finder function to use to find the item. This function should return 0 for a match, a negative value (meaning "search lower") if its parameter is too large, or a positive value (meaning "search higher") if its parameter is too small.</param>
        /// <param name="begin">On input, contains the beginning index at which to search. On output, contains the index of an item less than the found item, or the first item equal to the found item.</param>
        /// <param name="end">On input, contains the ending index at which to search. On output, contains the index one past an item greater than the found item, or the index one past the last item equal to the found item.</param>
        /// <returns>The index of an item that causes <paramref name="finder"/> to return 0, if any; otherwise, the bitwise complement of the next larger element in the list.</returns>
        private static int BinarySearchCore<T>(IList<T> list, Func<T, int> finder, ref int begin, ref int end)
        {
            while (begin != end)
            {
                int mid = begin + ((end - begin) / 2);
#if DEBUG
                Debug.Assert(mid >= begin, "mid must be in the range [begin, end)");
                Debug.Assert(mid < end, "mid must be in the range [begin, end)");
#endif

                int test = finder(list[mid]);
                if (test == 0)
                {
                    return mid;
                }
                else if (test < 0)
                {
                    end = mid;
                }
                else if (test > 0)
                {
                    begin = mid + 1;
                }
            }

            return ~end;
        }

        /// <summary>
        /// Modifies <paramref name="begin"/> so that it refers to the first matching item.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The sorted list.</param>
        /// <param name="finder">The finder function to use to find the item. This function should return 0 for a match, a negative value (meaning "search lower") if its parameter is too large, or a positive value (meaning "search higher") if its parameter is too small.</param>
        /// <param name="begin">On input, contains the beginning index at which to search. On output, contains the index of the first matching item.</param>
        /// <param name="end">The ending index at which to search. The item at this index must match.</param>
        private static void LowerBoundCore<T>(IList<T> list, Func<T, int> finder, ref int begin, int end)
        {
            while (begin != end)
            {
                int mid = begin + ((end - begin) / 2);
#if DEBUG
                Debug.Assert(mid >= begin, "mid must be in the range [begin, end)");
                Debug.Assert(mid < end, "mid must be in the range [begin, end)");
#endif

                int test = finder(list[mid]);
                if (test == 0)
                {
                    end = mid;
                }
                else
                {
#if DEBUG
                    Debug.Assert(test > 0, "No elements in the range [begin, end) may be larger than the match");
#endif

                    begin = mid + 1;
                }
            }
        }

        /// <summary>
        /// Modifies <paramref name="end"/> so that it refers to one past the last matching item.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The sorted list.</param>
        /// <param name="finder">The finder function to use to find the item. This function should return 0 for a match, a negative value (meaning "search lower") if its parameter is too large, or a positive value (meaning "search higher") if its parameter is too small.</param>
        /// <param name="begin">The beginning index at which to search. The item at this index must match.</param>
        /// <param name="end">On input, contains the ending index at which to search. On output, contains the index one past the last matching item.</param>
        private static void UpperBoundCore<T>(IList<T> list, Func<T, int> finder, int begin, ref int end)
        {
            while (begin != end)
            {
                int mid = begin + ((end - begin) / 2);
#if DEBUG
                Debug.Assert(mid >= begin, "mid must be in the range [begin, end)");
                Debug.Assert(mid < end, "mid must be in the range [begin, end)");
#endif

                int test = finder(list[mid]);
                if (test == 0)
                {
                    begin = mid + 1;
                }
                else
                {
#if DEBUG
                    Debug.Assert(test < 0, "No elements in the range [begin, end) may be smaller than the match");
#endif

                    end = mid;
                }
            }
        }

        /// <summary>
        /// Sorts a portion of a list. The range to sort is [<paramref name="left"/>, <paramref name="right"/>).
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="parameters">The source list, comparison object, and other lists that must be kept in sync.</param>
        /// <param name="begin">The beginning of the range to sort.</param>
        /// <param name="end">The ending of the range to sort.</param>
        private static void QuickSortCore<T>(SortParameters<T> parameters, int begin, int end)
        {
            // Empty lists and single-element lists are sorted by definition
            if (begin >= end - 1)
            {
                return;
            }

            // Choose a pivot element (we just choose the first one), and partition around it
            int newPivotIndex = QuickSortPartition(parameters, begin, end, begin);

            // Sort the left side (not including the pivot element)
            QuickSortCore(parameters, begin, newPivotIndex);

            // Sort the right side (not including the pivot element)
            QuickSortCore(parameters, newPivotIndex + 1, end);
        }

        /// <summary>
        /// Partitions the elements of a list around a given pivot element. The range to partition is [<paramref name="begin"/>, <paramref name="end"/>), and this range must be at least 2 elements long.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="parameters">The source list, comparison object, and other lists that must be kept in sync.</param>
        /// <param name="begin">The beginning of the range to partition.</param>
        /// <param name="end">The ending of the range to partition.</param>
        /// <param name="pivotIndex">The index of the element chosen as the pivot.</param>
        /// <returns>The new index of the pivot element.</returns>
        private static int QuickSortPartition<T>(SortParameters<T> parameters, int begin, int end, int pivotIndex)
        {
            T pivotValue = parameters.List[pivotIndex];

            // Move the pivot value to the end of the list temporarily
            parameters.Swap(pivotIndex, end - 1);

            // Make our way through the list (except the pivot value at the end), moving each element less than the pivot
            int storeIndex = begin;
            for (int i = begin; i != end - 1; ++i)
            {
                if (parameters.Comparer.Compare(parameters.List[i], pivotValue) < 0)
                {
                    parameters.Swap(i, storeIndex);
                    ++storeIndex;
                }
            }

            // Move pivot back to its proper place
            parameters.Swap(storeIndex, end - 1);

            // Return the new index of the pivot element
            return storeIndex;
        }

        /// <summary>
        /// Wraps a source list and comparison object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        private sealed class AnonymousSortedList<T> : Implementation.ListBase<T>, ISortedList<T>
        {
            /// <summary>
            /// The source list.
            /// </summary>
            private IList<T> source;

            /// <summary>
            /// Initializes a new instance of the <see cref="AnonymousSortedList&lt;T&gt;"/> class with the specified source list and comparison object.
            /// </summary>
            /// <param name="source">The source list.</param>
            /// <param name="comparer">The comparison object.</param>
            public AnonymousSortedList(IList<T> source, IComparer<T> comparer)
            {
                this.source = source;
                this.Comparer = comparer;
            }

            /// <summary>
            /// Gets a comparison object that defines how this list is sorted.
            /// </summary>
            public IComparer<T> Comparer { get; private set; }

            /// <summary>
            /// Gets a value indicating whether this list is read-only. This list is read-only iff its source list is read-only.
            /// </summary>
            /// <returns>true if this list is read-only; otherwise, false.</returns>
            public override bool IsReadOnly
            {
                get { return this.source.IsReadOnly; }
            }

            /// <summary>
            /// Gets the number of elements contained in this list.
            /// </summary>
            /// <returns>The number of elements contained in this list.</returns>
            public override int Count
            {
                get { return this.source.Count; }
            }

            /// <summary>
            /// Removes all items from this list.
            /// </summary>
            /// <exception cref="T:System.NotSupportedException">
            /// This list is read-only.
            /// </exception>
            public override void Clear()
            {
                this.source.Clear();
            }

            /// <summary>
            /// Determines the index of a specific item in this list.
            /// </summary>
            /// <param name="item">The object to locate in this list.</param>
            /// <returns>
            /// The index of <paramref name="item"/> if found in this list; otherwise, -1.
            /// </returns>
            public override int IndexOf(T item)
            {
                int ret = this.LowerBound(item);
                if (ret >= 0)
                {
                    return ret;
                }

                return -1;
            }

            /// <summary>
            /// Determines whether this list contains a specific value.
            /// </summary>
            /// <param name="item">The object to locate in this list.</param>
            /// <returns>
            /// true if <paramref name="item"/> is found in this list; otherwise, false.
            /// </returns>
            public override bool Contains(T item)
            {
                return this.BinarySearch(item) >= 0;
            }

            /// <summary>
            /// Gets an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
            /// <returns>The element at the specified index.</returns>
            protected override T DoGetItem(int index)
            {
                return this.source[index];
            }

            /// <summary>
            /// Sets an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
            /// <param name="item">The element to store in the list.</param>
            protected override void DoSetItem(int index, T item)
            {
                this.source[index] = item;
            }

            /// <summary>
            /// Inserts an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index at which the element should be inserted. This index is guaranteed to be valid.</param>
            /// <param name="item">The element to store in the list.</param>
            protected override void DoInsert(int index, T item)
            {
                this.source.Insert(index, item);
            }

            /// <summary>
            /// Removes an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the element to remove. This index is guaranteed to be valid.</param>
            protected override void DoRemoveAt(int index)
            {
                this.source.RemoveAt(index);
            }
        }

        /// <summary>
        /// A parameter holder class that exists only to reduce stack usage in the recursive QuickSort algorithm.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source list.</typeparam>
        private sealed class SortParameters<T>
        {
            /// <summary>
            /// Gets or sets the source list.
            /// </summary>
            public IList<T> List { get; set; }

            /// <summary>
            /// Gets or sets the comparison object.
            /// </summary>
            public IComparer<T> Comparer { get; set; }

            /// <summary>
            /// Gets or sets any other lists that need to be kept in sync with the source list.
            /// </summary>
            public IEnumerable<ListExtensions.ISwappable> Others { get; set; }

            /// <summary>
            /// Swaps two elements in the source list and all other lists.
            /// </summary>
            /// <param name="indexA">The index of the first element to swap.</param>
            /// <param name="indexB">The index of the second element to swap.</param>
            public void Swap(int indexA, int indexB)
            {
                this.List.SwapAll(this.Others, indexA, indexB);
            }
        }

        #endregion
    }
}

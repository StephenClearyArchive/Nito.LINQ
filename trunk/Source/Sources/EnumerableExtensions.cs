// <copyright file="EnumerableExtensions.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extension methods for sequences.
    /// </summary>
    public static class EnumerableExtensions
    {
#if !WITHRX
        /// <summary>
        /// Repeats a sequence of values a specified number of times. Identical to Rx's <c>EnumerableEx.Repeat</c>.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The source sequence containing the values to repeat.</param>
        /// <param name="count">The number of times all the values in <paramref name="source"/> are repeated. If <paramref name="count"/> is less than or equal to 0, an empty sequence is returned.</param>
        /// <returns>The resulting repeated sequence.</returns>
        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> source, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                foreach (T item in source)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Repeats a sequence of values an infinite number of times. Identical to Rx's <c>EnumerableEx.Repeat</c>.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The source sequence containing the values to repeat.</param>
        /// <returns>The resulting repeated sequence.</returns>
        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> source)
        {
            while (true)
            {
                foreach (T item in source)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Concatenates the specified sequences. Identical to Rx's <c>EnumerableEx.Concat</c>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source sequences.</typeparam>
        /// <param name="sources">The sequence of source sequences.</param>
        /// <returns>The concatenated sequence.</returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<IEnumerable<T>> sources)
        {
            return sources.Flatten();
        }

        /// <summary>
        /// Attaches a delegate to each element of the sequence, so that the delegate is invoked immediately before each element of the sequence is yielded.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="action">The delegate to invoke as each element of the sequence is yielded.</param>
        /// <returns>A sequence equivalent to the source sequence, but with a delegate attached to each element.</returns>
        public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            return source.Select(x => { action(x); return x; });
        }

        /// <summary>
        /// Immediately executes a delegate for each element of the sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="action">The delegate to invoke for each element of the sequence.</param>
        public static void Run<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Evaluates a sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        public static void Run<T>(this IEnumerable<T> source)
        {
            source.Run((_) => { });
        }

        /// <summary>
        /// Prepends a value to a source sequence. Identical to Rx's <c>EnumerableEx.StartWith</c>.
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

        /// <summary>
        /// Combines two source sequences into a result sequence. If the source sequences are of different lengths, the resulting sequence has a length equal to the smaller of the two. Identical to Rx's <c>EnumerableEx.Zip</c>.
        /// </summary>
        /// <typeparam name="TFirst">The type of elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of elements in the second source sequence.</typeparam>
        /// <typeparam name="TResult">The type of elements in the resulting sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="zipper">The delegate that creates a new element from corresponding elements of the two source sequences.</param>
        /// <returns>The resulting combined sequence.</returns>
        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> zipper)
        {
            var firstEnumerator = first.GetEnumerator();
            var secondEnumerator = second.GetEnumerator();

            while (firstEnumerator.MoveNext() && secondEnumerator.MoveNext())
            {
                yield return zipper(firstEnumerator.Current, secondEnumerator.Current);
            }
        }

        /// <summary>
        /// Repeats another sequence once for each element of this sequence, and flattens the results.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of elements in the resulting sequence.</typeparam>
        /// <param name="source">The source sequence; the <paramref name="other"/> sequence is repeated once for each element in this sequence.</param>
        /// <param name="other">The other sequence.</param>
        /// <returns>The resulting combined sequence.</returns>
        public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, IEnumerable<TResult> other)
        {
            return source.SelectMany(_ => other);
        }

        /// <summary>
        /// Binds a sequence to a parameter variable, allowing it to be used multiple times. Does not prevent side effects.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of elements in the resulting sequence.</typeparam>
        /// <param name="source">The source sequence, which is passed to the <paramref name="binder"/> delegate.</param>
        /// <param name="binder">The delegate which acts as a binding; the source sequence is passed to this delegate as a parameter.</param>
        /// <returns>The result of the <paramref name="binder"/> delegate when applied to the <paramref name="source"/> sequence.</returns>
        public static IEnumerable<TResult> Let<TSource, TResult>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> binder)
        {
            return binder(source);
        }

        /// <summary>
        /// Applies an accumulator delegate over a sequence, yielding each intermediate result. The first element of the returned sequence is the first element of the source sequence. Each element of the source sequence is only evaluated once.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="accumulator">The accumulator delegate. The first parameter passed to the accumulator delegate is the previous intermediate result; the second parameter is the current element of the source sequence.</param>
        /// <returns>The results of the accumulator delegate.</returns>
        public static IEnumerable<T> Scan<T>(this IEnumerable<T> source, Func<T, T, T> accumulator)
        {
            return EnumerableSource.Defer(() =>
                {
                    T current = default(T);
                    bool currentValid = false;

                    return source.Select(x =>
                        {
                            if (currentValid)
                            {
                                current = accumulator(current, x);
                            }
                            else
                            {
                                currentValid = true;
                                current = x;
                            }

                            return current;
                        });
                });
        }

        /// <summary>
        /// Applies an accumulator delegate over a sequence starting with a given seed value, yielding each intermediate result. The <paramref name="seed"/> value is the first element of the source sequence. Each element of the source sequence is only evaluated once.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of elements in the resulting sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="seed">The initial seed value.</param>
        /// <param name="accumulator">The accumulator delegate. The first parameter passed to the accumulator delegate is the previous intermediate result; the second parameter is the current element of the source sequence.</param>
        /// <returns>The results of the accumulator delegate.</returns>
        public static IEnumerable<TResult> Scan<TSource, TResult>(this IEnumerable<TSource> source, TResult seed, Func<TResult, TSource, TResult> accumulator)
        {
            IEnumerable<TResult> scan = EnumerableSource.Defer(() =>
                {
                    TResult current = seed;
                    return source.Select(x => current = accumulator(seed, x));
                });

            return EnumerableSource.Return(seed).Concat(scan);
        }
#endif

        /// <summary>
        /// Concatenates the specified sequences. Similar to <see cref="Enumerable.Concat"/>, except this method allows any number of sequences to be concatenated. Similar to Rx's <c>EnumerableEx.Concat</c>, except this method is an extension method.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source sequences.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="others">The other source sequences.</param>
        /// <returns>The concatenated sequence.</returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> first, IEnumerable<T> second, params IEnumerable<T>[] others)
        {
            return others.StartWith(second).StartWith(first).Concat();
        }

        /// <summary>
        /// Combines three source sequences into a result sequence. If the source sequences are of different lengths, the resulting sequence has a length equal to the smallest of the three.
        /// </summary>
        /// <typeparam name="TFirst">The type of elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of elements in the second source sequence.</typeparam>
        /// <typeparam name="TThird">The type of elements in the third source sequence.</typeparam>
        /// <typeparam name="TResult">The type of elements in the resulting sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="third">The third source sequence.</param>
        /// <param name="zipper">The delegate that creates a new element from corresponding elements of the three source sequences.</param>
        /// <returns>The resulting combined sequence.</returns>
        public static IEnumerable<TResult> Zip<TFirst, TSecond, TThird, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, Func<TFirst, TSecond, TThird, TResult> zipper)
        {
            var firstEnumerator = first.GetEnumerator();
            var secondEnumerator = second.GetEnumerator();
            var thirdEnumerator = third.GetEnumerator();

            while (firstEnumerator.MoveNext() && secondEnumerator.MoveNext() && thirdEnumerator.MoveNext())
            {
                yield return zipper(firstEnumerator.Current, secondEnumerator.Current, thirdEnumerator.Current);
            }
        }

        /// <summary>
        /// Flattens a sequence of sequences into one long sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the resulting sequence.</typeparam>
        /// <param name="list">The sequence of sequences.</param>
        /// <returns>The flattened sequence.</returns>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> list)
        {
            return list.SelectMany(x => x);
        }

        /// <summary>
        /// Steps through a sequence using a specified step size. The first element of the resulting sequence is the first element of the source sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The sequence to step through.</param>
        /// <param name="step">The step size. Must be greater than 0.</param>
        /// <returns>The stepped sequence.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="step"/> is less than or equal to 0.</exception>
        public static IEnumerable<T> Step<T>(this IEnumerable<T> source, int step)
        {
            if (step <= 0)
            {
                throw new ArgumentOutOfRangeException("step", "The step parameter must be greater than 0");
            }

            if (step == 1)
            {
                return source;
            }

            return StepCore<T>(source, step);
        }

        /// <summary>
        /// Rotates a sequence by a given offset. This method may enumerate the sequence more than once.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The source sequence to rotate.</param>
        /// <param name="offset">The number of elements to rotate. If <paramref name="offset"/> is less than or equal to 0, or if <paramref name="offset"/> is greater than or equal to the number of elements in the sequence, then the result of this method is identical to the source sequence.</param>
        /// <returns>The rotated sequence.</returns>
        public static IEnumerable<T> Rotate<T>(this IEnumerable<T> source, int offset)
        {
            return source.Skip(offset).Concat(source.Take(offset));
        }

        /// <summary>
        /// Determines the index of an element in a sequence using the default equality comparer.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence in which to locate the value.</param>
        /// <param name="value">The value to locate in the sequence.</param>
        /// <returns>The index of <paramref name="value"/> if found in the sequence; otherwise, -1.</returns>
        public static int IndexOf<T>(this IEnumerable<T> source, T value)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            return source.IndexOf((item) => comparer.Equals(value, item));
        }

        /// <summary>
        /// Determines the index of the first matching element in a sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence in which to locate the value.</param>
        /// <param name="match">The delegate that defines the conditions of the element to search for.</param>
        /// <returns>The index of the first element that returned true from <paramref name="match"/> if found in the sequence; otherwise, -1.</returns>
        public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> match)
        {
            int ret = 0;
            foreach (T sourceItem in source)
            {
                if (match(sourceItem))
                {
                    return ret;
                }

                ++ret;
            }

            return -1;
        }

        /// <summary>
        /// Determines the index of the last matching element in a sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence in which to locate the value.</param>
        /// <param name="match">The delegate that defines the conditions of the element to search for.</param>
        /// <returns>The index of the last element that returned true from <paramref name="match"/> if found in the sequence; otherwise, -1.</returns>
        public static int LastIndexOf<T>(this IEnumerable<T> source, Func<T, bool> match)
        {
            int ret = -1;
            int i = 0;
            foreach (T sourceItem in source)
            {
                if (match(sourceItem))
                {
                    ret = i;
                }

                ++i;
            }

            return ret;
        }

        /// <summary>
        /// Determines the index of the last matching element in a sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence in which to locate the value.</param>
        /// <param name="value">The value to locate in the sequence.</param>
        /// <returns>The index of the last instance of <paramref name="value"/> if found in the sequence; otherwise, -1.</returns>
        public static int LastIndexOf<T>(this IEnumerable<T> source, T value)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            return source.LastIndexOf((item) => comparer.Equals(value, item));
        }

        /// <summary>
        /// Returns the index of the smallest item in a sequence, according to the provided comparison object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The source sequence to search.</param>
        /// <param name="comparer">The comparison object.</param>
        /// <returns>The index of the smallest item in the sequence, or -1 if the sequence is empty.</returns>
        public static int IndexOfMin<T>(this IEnumerable<T> source, IComparer<T> comparer)
        {
            T min = default(T);
            int ret = -1;
            int index = 0;
            foreach (T item in source)
            {
                if (ret == -1)
                {
                    min = item;
                    ret = index;
                }
                else
                {
                    if (comparer.Compare(item, min) < 0)
                    {
                        min = item;
                        ret = index;
                    }
                }

                ++index;
            }

            return ret;
        }

        /// <summary>
        /// Returns the index of the smallest item in a sequence, according to the default comparison object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The source sequence to search.</param>
        /// <returns>The index of the smallest item in the sequence, or -1 if the sequence is empty.</returns>
        public static int IndexOfMin<T>(this IEnumerable<T> source)
        {
            return IndexOfMin(source, Comparer<T>.Default);
        }

        /// <summary>
        /// Returns the smallest item in a sequence, according to the provided comparison object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The source sequence to search.</param>
        /// <param name="comparer">The comparison object.</param>
        /// <returns>The smallest item in the sequence, or the default value of <typeparamref name="T"/> if the sequence is empty.</returns>
        public static T Min<T>(this IEnumerable<T> source, IComparer<T> comparer)
        {
            T min = default(T);
            bool minHasValue = false;
            foreach (T item in source)
            {
                if (!minHasValue)
                {
                    min = item;
                    minHasValue = true;
                }
                else
                {
                    if (comparer.Compare(item, min) < 0)
                    {
                        min = item;
                    }
                }
            }

            return min;
        }

        /// <summary>
        /// Returns the index of the largest item in a sequence, according to the provided comparison object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The source sequence to search.</param>
        /// <param name="comparer">The comparison object.</param>
        /// <returns>The index of the largest item in the sequence, or -1 if the sequence is empty.</returns>
        public static int IndexOfMax<T>(this IEnumerable<T> source, IComparer<T> comparer)
        {
            return IndexOfMin(source, new AnonymousComparer<T> { Compare = (x, y) => comparer.Compare(y, x) });
        }

        /// <summary>
        /// Returns the index of the largest item in a sequence, according to the default comparison object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The source sequence to search.</param>
        /// <returns>The index of the largest item in the sequence, or -1 if the sequence is empty.</returns>
        public static int IndexOfMax<T>(this IEnumerable<T> source)
        {
            IComparer<T> comparer = Comparer<T>.Default;
            return IndexOfMin(source, new AnonymousComparer<T> { Compare = (x, y) => comparer.Compare(y, x) });
        }

        /// <summary>
        /// Returns the largest item in a sequence, according to the provided comparison object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The source sequence to search.</param>
        /// <param name="comparer">The comparison object.</param>
        /// <returns>The largest item in the sequence, or the default value of <typeparamref name="T"/> if the sequence is empty.</returns>
        public static T Max<T>(this IEnumerable<T> source, IComparer<T> comparer)
        {
            return Min(source, new AnonymousComparer<T> { Compare = (x, y) => comparer.Compare(y, x) });
        }

        /// <summary>
        /// Steps through a sequence using a specified step size. The first element of the resulting sequence is the first element of the source sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The sequence to step through.</param>
        /// <param name="step">The step size. Must be greater than 0.</param>
        /// <returns>The stepped sequence.</returns>
        private static IEnumerable<T> StepCore<T>(IEnumerable<T> source, int step)
        {
            int count = 0;
            foreach (T item in source)
            {
                if (count == 0)
                {
                    yield return item;
                }

                count = (count + 1) % step;
            }
        }
    }
}

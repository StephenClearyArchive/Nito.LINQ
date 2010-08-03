// <copyright file="EnumerableSource.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Sequence sources.
    /// </summary>
    public static class EnumerableSource
    {
        /// <summary>
        /// Creates an empty sequence. Identical to <see cref="Enumerable.Empty{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements (not) in the sequence.</typeparam>
        /// <returns>An empty sequence.</returns>
        public static IEnumerable<T> Empty<T>()
        {
            return Enumerable.Empty<T>();
        }

        /// <summary>
        /// Converts a single value into a sequence containing a single value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <returns>A sequence containing a single element, <paramref name="source"/>.</returns>
        public static IEnumerable<T> Return<T>(T source)
        {
#if WITHRX
            return EnumerableEx.Return(source);
#else
            yield return source;
#endif
        }

        /// <summary>
        /// Converts a single value into a sequence containing that value the specified number of times. Identical to Rx's <c>EnumerableEx.Repeat</c>. Identical to <see cref="Enumerable.Repeat"/>, except that if <paramref name="count"/> is less than 0, this method returns an empty sequence instead of raising an exception.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <param name="count">The number of times <paramref name="source"/> is repeated. If <paramref name="count"/> is less than or equal to 0, an empty sequence is returned.</param>
        /// <returns>A sequence containing <paramref name="count"/> elements, all equal to <paramref name="source"/>.</returns>
        public static IEnumerable<T> Repeat<T>(T source, int count)
        {
#if WITHRX
            return EnumerableEx.Repeat(source, count);
#else
            for (int i = 0; i < count; ++i)
            {
                yield return source;
            }
#endif
        }

        /// <summary>
        /// Converts a single value into a sequence containing that value an infinite number of times.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The value.</param>
        /// <returns>A sequence containing an infinite number of elements, all equal to <paramref name="source"/>.</returns>
        public static IEnumerable<T> Repeat<T>(T source)
        {
#if WITHRX
            return EnumerableEx.Repeat(source);
#else
            while (true)
            {
                yield return source;
            }
#endif
        }

        /// <summary>
        /// Generates an infinite sequence by iterating from an initial state; for each state, a sequence is generated.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of elements in the sequence.</typeparam>
        /// <param name="initialState">The starting value of the state.</param>
        /// <param name="resultSelector">The generator delegate that generates a sequence from a state value.</param>
        /// <param name="iterate">The iterator delegate that moves the state from one value to the next.</param>
        /// <returns>An infinite sequence.</returns>
        public static IEnumerable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, TState> iterate, Func<TState, IEnumerable<TResult>> resultSelector)
        {
#if WITHRX
            return EnumerableEx.Generate(initialState, resultSelector, iterate);
#else
            return Generate<TState, IEnumerable<TResult>>(initialState, x => true, iterate, resultSelector).Flatten();
#endif
        }

        /// <summary>
        /// Generates a sequence by iterating from an initial state until the condition delegate returns <c>false</c>; for each state, a sequence is generated.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of elements in the sequence.</typeparam>
        /// <param name="initialState">The starting value of the state.</param>
        /// <param name="condition">The condition delegate that determines if a state value constitutes the end of the sequence.</param>
        /// <param name="resultSelector">The generator delegate that generates a sequence from a state value.</param>
        /// <param name="iterate">The iterator delegate that moves the state from one value to the next.</param>
        /// <returns>A generated sequence.</returns>
        public static IEnumerable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, IEnumerable<TResult>> resultSelector)
        {
#if WITHRX
            return EnumerableEx.Generate(initialState, condition, resultSelector, iterate);
#else
            return Generate<TState, IEnumerable<TResult>>(initialState, condition, iterate, resultSelector).Flatten();
#endif
        }

        /// <summary>
        /// Generates an infinite sequence by iterating from an initial state; for each state, a single value is generated.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of elements in the sequence.</typeparam>
        /// <param name="initialState">The starting value of the state.</param>
        /// <param name="resultSelector">The generator delegate that generates a sequence value from a state value.</param>
        /// <param name="iterate">The iterator delegate that moves the state from one value to the next.</param>
        /// <returns>A generated sequence.</returns>
        public static IEnumerable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, TState> iterate, Func<TState, TResult> resultSelector)
        {
            return Generate<TState, TResult>(initialState, x => true, iterate, resultSelector);
        }

        /// <summary>
        /// Generates a sequence by iterating from an initial state until the condition delegate returns <c>false</c>; for each state, a single value is generated.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of elements in the sequence.</typeparam>
        /// <param name="initialState">The starting value of the state.</param>
        /// <param name="condition">The condition delegate that determines if a state value constitutes the end of the sequence.</param>
        /// <param name="resultSelector">The generator delegate that generates a sequence value from a state value.</param>
        /// <param name="iterate">The iterator delegate that moves the state from one value to the next.</param>
        /// <returns>A generated sequence.</returns>
        public static IEnumerable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector)
        {
#if WITHRX
            return EnumerableEx.Generate(initialState, condition, resultSelector, iterate);
#else
            TState state = initialState;
            while (condition(state))
            {
                yield return resultSelector(state);
                state = iterate(state);
            }
#endif
        }

        /// <summary>
        /// Creates a sequence that invokes the factory delegate whenever the sequence is enumerated.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="factory">The factory delegate that is invoked whenever the returned sequence is enumerated.</param>
        /// <returns>A sequence that invokes the factory delegate whenever it is enumerated.</returns>
        public static IEnumerable<T> Defer<T>(Func<IEnumerable<T>> factory)
        {
#if WITHRX
            return EnumerableEx.Defer(factory);
#else
            return new AnonymousEnumerable<T> { GetEnumerator = () => factory().GetEnumerator() };
#endif
        }

        /// <summary>
        /// Generates a sequence of integers. Similar to <see cref="Enumerable.Range"/>, except that this method will return an empty sequence if <paramref name="count"/> is less than 0.
        /// </summary>
        /// <param name="start">The first integer in the returned sequence.</param>
        /// <param name="count">The number of integers in the returned sequence. If <paramref name="count"/> is less than or equal to 0, an empty sequence is returned.</param>
        /// <returns>A sequence of integers.</returns>
        public static IEnumerable<int> Range(int start, int count)
        {
            return Enumerable.Range(start, Math.Max(count, 0));
        }

        /// <summary>
        /// Generates an infinite sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="generator">The generator delegate that generates each value in the sequence.</param>
        /// <returns>An infinite sequence.</returns>
        public static IEnumerable<T> Generate<T>(Func<T> generator)
        {
            while (true)
            {
                yield return generator();
            }
        }

        /// <summary>
        /// Generates an infinite sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="generator">The generator delegate that generates each value in the sequence.</param>
        /// <returns>A generated sequence.</returns>
        public static IEnumerable<T> Generate<T>(Func<int, T> generator)
        {
            for (int i = 0; true; i = unchecked(i + 1))
            {
                yield return generator(i);
            }
        }

        /// <summary>
        /// Generates a sequence of a specified length.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="generator">The generator delegate that generates each value in the sequence. This may be <c>null</c> if <paramref name="count"/> is less than or equal to 0.</param>
        /// <param name="count">The number of elements in the resulting sequence. If <paramref name="count"/> is less than or equal to 0, an empty sequence is returned.</param>
        /// <returns>A generated sequence.</returns>
        public static IEnumerable<T> Generate<T>(Func<int, T> generator, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                yield return generator(i);
            }
        }

        /// <summary>
        /// Generates a sequence of a specified length.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="generator">The generator delegate that generates each value in the sequence. This may be <c>null</c> if <paramref name="count"/> is less than or equal to 0.</param>
        /// <param name="count">The number of elements in the resulting sequence. If <paramref name="count"/> is less than or equal to 0, an empty sequence is returned.</param>
        /// <returns>A generated sequence.</returns>
        public static IEnumerable<T> Generate<T>(Func<T> generator, int count)
        {
            return Generate(_ => generator(), count);
        }
    }
}

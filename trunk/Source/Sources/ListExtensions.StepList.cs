// <copyright file="ListExtensions.StepList.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito.Linq
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides useful extension methods for the <see cref="List{T}"/> class.
    /// </summary>
    public static partial class ListExtensions
    {
        /// <summary>
        /// Indexes into a source list using a step size.
        /// </summary>
        /// <typeparam name="T">The type of object contained in the list.</typeparam>
        private sealed class StepList<T> : Implementation.ReadOnlyListBase<T>
        {
            /// <summary>
            /// The source list.
            /// </summary>
            private readonly IList<T> source;

            /// <summary>
            /// The step size to use when traversing the source list.
            /// </summary>
            private readonly int step;

            /// <summary>
            /// Initializes a new instance of the <see cref="StepList&lt;T&gt;"/> class.
            /// </summary>
            /// <param name="source">The source list.</param>
            /// <param name="step">The step size to use when traversing the source list.</param>
            public StepList(IList<T> source, int step)
            {
                if (step <= 0)
                {
                    throw new ArgumentOutOfRangeException("step", "The step parameter must be greater than 0");
                }

                this.source = source;
                this.step = step;
            }

            /// <summary>
            /// Gets the number of elements contained in this list.
            /// </summary>
            /// <value></value>
            /// <returns>The number of elements contained in this list.</returns>
            public override int Count
            {
                get
                {
                    if (this.source.Count == 0)
                    {
                        return 0;
                    }

                    return ((this.source.Count - 1) / this.step) + 1;
                }
            }

            /// <summary>
            /// Gets an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
            /// <returns>The element at the specified index.</returns>
            protected override T DoGetItem(int index)
            {
                return this.source[index * this.step];
            }

            /// <summary>
            /// Sets an element at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the element to get. This index is guaranteed to be valid.</param>
            /// <param name="item">The element to store in the list.</param>
            protected override void DoSetItem(int index, T item)
            {
                this.source[index * this.step] = item;
            }
        }
    }
}

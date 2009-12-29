// <copyright file="SequenceComparer.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito.Linq
{
    using System.Collections.Generic;

    /// <summary>
    /// A comparison object that performs a lexicographical comparison of sequences.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    public sealed class SequenceComparer<T> : IComparer<IEnumerable<T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceComparer&lt;T&gt;"/> class with the specified item comparison object.
        /// </summary>
        /// <param name="comparer">The item comparison object, used to compare elements of the sequences.</param>
        public SequenceComparer(IComparer<T> comparer)
        {
            this.Comparer = comparer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceComparer&lt;T&gt;"/> class with the default item comparison object.
        /// </summary>
        public SequenceComparer()
            : this(Comparer<T>.Default)
        {
        }

        /// <summary>
        /// Gets the item comparison object.
        /// </summary>
        public IComparer<T> Comparer { get; private set; }

        /// <summary>
        /// Compares two sequences and returns a value less than 0 if <paramref name="x"/> is less than <paramref name="y"/>, 0 if <paramref name="x"/> is equal to <paramref name="y"/>, or greater than 0 if <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first sequence to compare.</param>
        /// <param name="y">The second sequence to compare.</param>
        /// <returns>A value less than 0 if <paramref name="x"/> is less than <paramref name="y"/>, 0 if <paramref name="x"/> is equal to <paramref name="y"/>, or greater than 0 if <paramref name="x"/> is greater than <paramref name="y"/>.</returns>
        public int Compare(IEnumerable<T> x, IEnumerable<T> y)
        {
            using (IEnumerator<T> leftIterator = x.GetEnumerator())
            using (IEnumerator<T> rightIterator = y.GetEnumerator())
            {
                while (true)
                {
                    bool leftDone = !leftIterator.MoveNext();
                    bool rightDone = !rightIterator.MoveNext();

                    if (leftDone)
                    {
                        if (rightDone)
                        {
                            // Both sequences completed at the same time
                            return 0;
                        }
                        else
                        {
                            // The left sequence completed before the right sequence
                            return -1;
                        }
                    }
                    else if (rightDone)
                    {
                        // The right sequence completed before the left sequence
                        return 1;
                    }

                    // Compare the current elements of both sequences
                    int test = this.Comparer.Compare(leftIterator.Current, rightIterator.Current);
                    if (test != 0)
                    {
                        // If the elements are not equal, then they determine the order of the sequences
                        return test;
                    }
                }
            }
        }
    }
}

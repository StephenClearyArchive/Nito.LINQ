// <copyright file="Implementation.ListHelper.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito.Implementation
{
    using System;

    /// <summary>
    /// Helper classes and methods for implementing lists.
    /// </summary>
    public static class ListHelper
    {
        /// <summary>
        /// Checks the <paramref name="offset"/> and <paramref name="count"/> arguments for validity when applied to a source of a given length. Allows 0-element ranges, including a 0-element range at the end of the source.
        /// </summary>
        /// <param name="sourceLength">The length of the source. This parameter is not checked for validity.</param>
        /// <param name="offset">The index into source at which the range begins.</param>
        /// <param name="count">The number of elements in the range.</param>
        /// <exception cref="ArgumentOutOfRangeException">Either <paramref name="offset"/> or <paramref name="count"/> is less than 0.</exception>
        /// <exception cref="ArgumentException">The range [offset, offset + count) is not within the range [0, sourceLength).</exception>
        public static void CheckRangeArguments(int sourceLength, int offset, int count)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", "Invalid offset " + offset);
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", "Invalid count " + count);
            }

            if (sourceLength - offset < count)
            {
                throw new ArgumentException("Invalid offset (" + offset + ") or count + (" + count + ") for source length " + sourceLength);
            }
        }

        /// <summary>
        /// Checks the <paramref name="index"/> argument to see if it refers to an existing element in a source of a given length.
        /// </summary>
        /// <param name="sourceLength">The length of the source. This parameter is not checked for validity.</param>
        /// <param name="index">The index into the source.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index to an existing element for the source.</exception>
        public static void CheckExistingIndexArgument(int sourceLength, int index)
        {
            if (index < 0 || index >= sourceLength)
            {
                throw new ArgumentOutOfRangeException("index", "Invalid existing index " + index + " for source length " + sourceLength);
            }
        }

        /// <summary>
        /// Checks the <paramref name="index"/> argument to see if it refers to a valid insertion point in a source of a given length.
        /// </summary>
        /// <param name="sourceLength">The length of the source. This parameter is not checked for validity.</param>
        /// <param name="index">The index into the source.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index to an insertion point for the source.</exception>
        public static void CheckNewIndexArgument(int sourceLength, int index)
        {
            if (index < 0 || index > sourceLength)
            {
                throw new ArgumentOutOfRangeException("index", "Invalid new index " + index + " for source length " + sourceLength);
            }
        }
    }
}

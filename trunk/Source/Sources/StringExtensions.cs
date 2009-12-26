﻿// <copyright file="StringExtensions.cs" company="Nito Programs">
//     Copyright (c) 2009 Nito Programs.
// </copyright>

namespace Nito.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Provides useful extension methods for string operations.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Gets all text elements (Unicode glyphs) for a given string.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <returns>All the text elements in the source string.</returns>
        public static IEnumerable<string> TextElements(this string source)
        {
            var enumerator = StringInfo.GetTextElementEnumerator(source);
            while (enumerator.MoveNext())
            {
                yield return enumerator.GetTextElement();
            }
        }

        /// <summary>
        /// Concatenates a separator between each element of a string enumeration.
        /// </summary>
        /// <param name="source">The string enumeration.</param>
        /// <param name="separator">The separator string. This may not be null.</param>
        /// <returns>The concatenated string.</returns>
        public static string Join(this IEnumerable<string> source, string separator)
        {
            StringBuilder ret = new StringBuilder();
            bool first = true;
            foreach (string str in source)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    ret.Append(separator);
                }

                ret.Append(str);
            }

            return ret.ToString();
        }

        /// <summary>
        /// Concatenates a sequence of strings.
        /// </summary>
        /// <param name="source">The sequence of strings.</param>
        /// <returns>The concatenated string.</returns>
        public static string Join(this IEnumerable<string> source)
        {
            return source.Join(string.Empty);
        }
    }
}

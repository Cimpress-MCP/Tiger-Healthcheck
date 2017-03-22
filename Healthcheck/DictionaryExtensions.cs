using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Tiger.Healthcheck
{
    /// <summary>Extensions to the functionality of dictionaries.</summary>
    /// <remarks>Dictionaries of all kinds.</remarks>
    static class DictionaryExtensions
    {
        /// <summary>Adds a range of values to a provided dictionary.</summary>
        /// <param name="value">The dictionary to which to add a range of values.</param>
        /// <param name="range">The range of values to add to <paramref name="value"/>.</param>
        /// <typeparam name="TKey">The type of the keys of <paramref name="value"/>.</typeparam>
        /// <typeparam name="TValue">The type of the values of <paramref name="value"/>.</typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="range"/> is <see langword="null"/>.</exception>
        public static void Add<TKey, TValue>(
            [NotNull] this IDictionary<TKey, TValue> value,
            [NotNull] IEnumerable<KeyValuePair<TKey, TValue>> range)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (range == null) { throw new ArgumentNullException(nameof(range)); }

            foreach (var kvp in range)
            {
                value.Add(kvp.Key, kvp.Value);
            }
        }
    }
}

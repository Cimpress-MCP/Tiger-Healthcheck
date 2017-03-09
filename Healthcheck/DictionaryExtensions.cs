using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Tiger.Healthcheck
{
    /// <summary>Extensions to the functionality of dictionaries.</summary>
    /// <remarks>Dictionaries of all kinds.</remarks>
    static class DictionaryExtensions
    {
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

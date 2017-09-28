// <copyright file="DictionaryExtensions.cs" company="Cimpress, Inc.">
//   Copyright 2017 Cimpress, Inc.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>

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

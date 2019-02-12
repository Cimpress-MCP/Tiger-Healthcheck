// <copyright file="KeyValuePairExtensions.cs" company="Cimpress, Inc.">
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

using System.Collections.Generic;
using System.ComponentModel;
using static System.ComponentModel.EditorBrowsableState;

namespace Tiger.Healthcheck
{
    /// <summary>Extensions to the functionality of <see cref="KeyValuePair{TKey, TValue}"/>.</summary>
    static class KeyValuePairExtensions
    {
        /// <summary>Deconstructs the provided value into its component values.</summary>
        /// <typeparam name="TKey">The type of <see cref="KeyValuePair{TKey, TValue}.Key"/>.</typeparam>
        /// <typeparam name="TValue">The type of <see cref="KeyValuePair{TKey, TValue}.Value"/>.</typeparam>
        /// <param name="pair">The value to deconstruct.</param>
        /// <param name="key">
        /// When this method returns, the <see cref="KeyValuePair{TKey, TValue}.Key"/> value of <paramref name="pair"/>.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <param name="value">
        /// When this method returns, the <see cref="KeyValuePair{TKey, TValue}.Value"/> value of <paramref name="pair"/>.
        /// This parameter is passed uninitialized.
        /// </param>
        [EditorBrowsable(Never)]
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key, out TValue value)
        {
            key = pair.Key;
            value = pair.Value;
        }
    }
}

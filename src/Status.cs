// <copyright file="Status.cs" company="Cimpress, Inc.">
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
using System.Globalization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;
using static System.StringComparer;

namespace Tiger.Healthcheck
{
    /// <summary>Represents the status of a healthcheck operation.</summary>
    [SwaggerSchemaFilter(typeof(StatusSchemaFilter))]
    [JsonObject(
        NamingStrategyType = typeof(SnakeCaseNamingStrategy),
        NamingStrategyParameters = new object[] { false, true, true })]
    sealed class Status
    {
        /// <summary>Initializes a new instance of the <see cref="Status"/> class.</summary>
        /// <param name="message">A message describing the status.</param>
        /// <param name="generatedAt">The time at which the report is generated.</param>
        /// <param name="duration">The time it took to generate.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is <see langword="null"/>.</exception>
        public Status(
            [NotNull] string message,
            DateTimeOffset generatedAt,
            TimeSpan duration)
        {
            Extensions[nameof(message)] = message ?? throw new ArgumentNullException(nameof(message));
            GeneratedAt = generatedAt;
            Duration = duration.TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>Gets the time at which this report was generated.</summary>
        public DateTimeOffset GeneratedAt { get; }

        /// <summary>Gets the number of milliseconds it took to generate the report.</summary>
        [JsonProperty("DurationMillis")]
        [NotNull]
        public string Duration { get; }

        /// <summary>Gets test results keyed by component.</summary>
        [NotNull]
        public Dictionary<string, Test> Tests { get; } = new Dictionary<string, Test>(Ordinal);

        /// <summary>Gets a mapping of JSON extension data.</summary>
        [JsonExtensionData, NotNull]
        public Dictionary<string, dynamic> Extensions { get; } = new Dictionary<string, dynamic>(Ordinal);

        /// <summary>
        /// Determines whether <see cref="Tests"/> should be included in
        /// the JSON serialization of this instance.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if <see cref="Tests"/> should be included;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool ShouldSerializeTests() => Tests.Count != 0;
    }
}

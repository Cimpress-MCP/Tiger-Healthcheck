// <copyright file="Test.cs" company="Cimpress, Inc.">
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
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;
using static Newtonsoft.Json.DefaultValueHandling;
using static Tiger.Healthcheck.State;

namespace Tiger.Healthcheck
{
    /// <summary>Represents a status test.</summary>
    [SwaggerSchemaFilter(typeof(TestSchemaFilter))]
    [JsonObject(
        NamingStrategyType = typeof(SnakeCaseNamingStrategy),
        NamingStrategyParameters = new object[] { false, true, false })]
    public sealed class Test
    {
        /// <summary>Initializes a new instance of the <see cref="Test"/> class.</summary>
        /// <param name="duration">The time taken to run the test.</param>
        /// <param name="testedAt">The date and time at which this test was executed.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is negative.</exception>
        internal Test(TimeSpan duration, DateTimeOffset testedAt)
        {
            if (duration < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(duration), duration, "Duration is negative.");
            }

            Duration = duration.TotalMilliseconds;
            TestedAt = testedAt;
        }

        /// <summary>Initializes a new instance of the <see cref="Test"/> class.</summary>
        /// <param name="duration">The time taken to run the test.</param>
        /// <param name="testedAt">The date and time at which this test was executed.</param>
        /// <param name="error">A description of the test failure.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is negative.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
        internal Test(TimeSpan duration, DateTimeOffset testedAt, [NotNull] string error)
            : this(duration, testedAt)
        {
            Error = error ?? throw new ArgumentNullException(nameof(error));
        }

        /// <summary>Gets the number of milliseconds taken to run this test.</summary>
        [JsonProperty("DurationMillis")]
        public double Duration { get; }

        /// <summary>Gets the final state of this test.</summary>
        public State Result => Error == null
            ? Passed
            : Failed;

        /// <summary>Gets the time at which this test was executed.</summary>
        public DateTimeOffset TestedAt { get; }

        /// <summary>Gets a description of any error conditions.</summary>
        [JsonProperty(DefaultValueHandling = Ignore)]
        public string Error { get; }

        /// <summary>Creates a passed test.</summary>
        /// <param name="duration">The time taken to run the test.</param>
        /// <param name="testedAt">The date and time at which this test was executed.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is negative.</exception>
        /// <returns>A passed test.</returns>
        [NotNull]
        public static Test Pass(TimeSpan duration, DateTimeOffset testedAt) =>
            new Test(duration, testedAt);

        /// <summary>Creates a failed test.</summary>
        /// <param name="duration">The time taken to run the test.</param>
        /// <param name="testedAt">The date and time at which this test was executed.</param>
        /// <param name="error">A description of the error that caused this test to fail.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is negative.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
        /// <returns>A failed test.</returns>
        [NotNull]
        public static Test Fail(TimeSpan duration, DateTimeOffset testedAt, [NotNull] string error) =>
            new Test(duration, testedAt, error);
    }
}

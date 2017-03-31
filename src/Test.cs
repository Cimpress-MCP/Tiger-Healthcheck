using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using static JetBrains.Annotations.ImplicitUseTargetFlags;
using static Newtonsoft.Json.Required;

namespace Tiger.Healthcheck
{
    /// <summary>Represents a status test.</summary>
    [SwaggerSchemaFilter(typeof(TestSchemaFilter))]
    [UsedImplicitly(Members)]
    public abstract class Test
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

        /// <summary>Gets the number of milliseconds taken to run this test.</summary>
        [JsonProperty("duration_millis", Required = Always)]
        public double Duration { get; }

        /// <summary>Gets the final state of this test.</summary>
        [JsonProperty(Required = Always)]
        public abstract State Result { get; }

        /// <summary>Gets the time at which this test was executed.</summary>
        [JsonProperty("tested_at", Required = Always)]
        public DateTimeOffset TestedAt { get; }

        /// <summary>Creates a passed test.</summary>
        /// <param name="duration">The time taken to run the test.</param>
        /// <param name="testedAt">The date and time at which this test was executed.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is negative.</exception>
        /// <returns>A passed test.</returns>
        [NotNull]
        public static Test Pass(TimeSpan duration, DateTimeOffset testedAt) =>
            new Passed(duration, testedAt);

        /// <summary>Creates a failed test.</summary>
        /// <param name="duration">The time taken to run the test.</param>
        /// <param name="testedAt">The date and time at which this test was executed.</param>
        /// <param name="error">A description of the error that caused this test to fail.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is negative.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
        /// <returns>A failed test.</returns>
        [NotNull]
        public static Test Fail(TimeSpan duration, DateTimeOffset testedAt, [NotNull] string error) =>
            new Failed(duration, testedAt, error);

        /// <summary>Represents a passed status test.</summary>
        sealed class Passed
            : Test
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Passed"/> class.
            /// </summary>
            /// <param name="duration">The time taken to run the test.</param>
            /// <param name="testedAt">The date and time at which this test was executed.</param>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is negative.</exception>
            public Passed(TimeSpan duration, DateTimeOffset testedAt)
                : base(duration, testedAt)
            {
            }

            /// <inheritdoc/>
            public override State Result => State.Passed;
        }

        /// <summary>Represents a failed status test.</summary>
        sealed class Failed
            : Test
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Failed"/> class.
            /// </summary>
            /// <param name="duration">The time taken to run the test.</param>
            /// <param name="testedAt">The date and time at which this test was executed.</param>
            /// <param name="error">A description of the error that caused this test to fail.</param>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is negative.</exception>
            /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
            public Failed(TimeSpan duration, DateTimeOffset testedAt, [NotNull] string error)
                : base(duration, testedAt)
            {
                Error = error ?? throw new ArgumentNullException(nameof(error));
            }

            /// <inheritdoc/>
            public override State Result => State.Failed;

            /// <summary>Gets a description of any error conditions.</summary>
            [JsonProperty(Required = Always)]
            public string Error { get; }
        }
    }
}

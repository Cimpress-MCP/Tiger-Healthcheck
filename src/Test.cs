using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using static JetBrains.Annotations.ImplicitUseTargetFlags;
using static Newtonsoft.Json.Required;

namespace Tiger.Healthcheck
{
    /// <summary>Represents a status test.</summary>
    [UsedImplicitly(Members)]
    public abstract class Test
    {
        /// <summary>Number of milliseconds taken to run this test</summary>
        [JsonProperty("duration_millis", Required = Always)]
        public float Duration { get; }

        /// <summary>The final state of this test</summary>
        [JsonProperty(Required = Always)]
        public abstract State Result { get; }

        /// <summary>The time at which this test was executed</summary>
        [JsonProperty(Required = Always)]
        public DateTimeOffset TestedAt { get; }

        /// <summary>Creates a passed test.</summary>
        /// <param name="duration">The number of milliseconds taken to run the test.</param>
        /// <param name="testedAt">The date and time at which this test was executed.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is negative.</exception>
        /// <returns>A passed test.</returns>
        [NotNull]
        public static Test Pass(float duration, DateTimeOffset testedAt) =>
            new Passed(duration, testedAt);

        /// <summary>Creates a failed test.</summary>
        /// <param name="duration">The number of milliseconds taken to run the test.</param>
        /// <param name="testedAt">The date and time at which this test was executed.</param>
        /// <param name="error">A description of the error that caused this test to fail.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is negative.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
        /// <returns>A failed test.</returns>
        [NotNull]
        public static Test Fail(float duration, DateTimeOffset testedAt, [NotNull] string error) =>
            new Failed(duration, testedAt, error);

        /// <summary>Represents a passed status test.</summary>
        sealed class Passed
            : Test
        {
            /// <inheritdoc/>
            public override State Result => State.Passed;

            /// <summary>
            /// Initializes a new instance of the <see cref="Passed"/> class.
            /// </summary>
            /// <param name="duration">The number of milliseconds taken to run the test.</param>
            /// <param name="testedAt">The date and time at which this test was executed.</param>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is negative.</exception>
            public Passed(float duration, DateTimeOffset testedAt)
                : base(duration, testedAt)
            {
            }
        }

        /// <summary>Represents a failed status test.</summary>
        sealed class Failed
            : Test
        {
            /// <inheritdoc/>
            public override State Result => State.Failed;

            /// <summary>A description of any error conditions</summary>
            [JsonProperty(Required = Always)]
            public string Error { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Failed"/> class.
            /// </summary>
            /// <param name="duration">The number of milliseconds taken to run the test.</param>
            /// <param name="testedAt">The date and time at which this test was executed.</param>
            /// <param name="error">A description of the error that caused this test to fail.</param>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is negative.</exception>
            /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
            public Failed(float duration, DateTimeOffset testedAt, [NotNull] string error)
                : base(duration, testedAt)
            {
                Error = error ?? throw new ArgumentNullException(nameof(error));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Test"/> class.</summary>
        /// <param name="duration">The number of milliseconds taken to run the test.</param>
        /// <param name="testedAt">The date and time at which this test was executed.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is negative.</exception>
        internal Test(float duration, DateTimeOffset testedAt)
        {
            if (duration < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(duration), duration, "Duration is negative.");
            }

            Duration = duration;
            TestedAt = testedAt;
        }
    }
}

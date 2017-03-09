using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using static System.Globalization.CultureInfo;
using static System.StringComparer;
using static JetBrains.Annotations.ImplicitUseKindFlags;
using static JetBrains.Annotations.ImplicitUseTargetFlags;
using static Newtonsoft.Json.Required;

namespace Tiger.Healthcheck
{
    /// <summary>Represents the status of a livecheck operation.</summary>
    [SwaggerSchemaFilter(typeof(StatusSchemaFilter))]
    [UsedImplicitly(Access, Members)]
    sealed class Status
    {
        /// <summary>The time at which this report was generated</summary>
        [JsonProperty("generated_at", Required = Always)]
        public DateTimeOffset GeneratedAt { get; }

        /// <summary>The number of milliseconds it took to generate the report</summary>
        [JsonProperty("duration_millis", Required = Always)]
        public string Duration { get; }

        /// <summary>Test results keyed by component</summary>
        [JsonProperty(Required = Always), NotNull]
        public IDictionary<string, Test> Tests { get; } =
            new Dictionary<string, Test>(Ordinal);

        /// <summary>Gets a mapping of JSON extension data.</summary>
        [JsonExtensionData, NotNull]
        public IDictionary<string, dynamic> Extensions { get; } =
            new Dictionary<string, dynamic>(Ordinal);

        /// <summary>Initializes a new instance of <see cref="Status"/>.</summary>
        /// <param name="message">A message describing the status.</param>
        /// <param name="generatedAt">The time at which the report is generated.</param>
        /// <param name="duration">The number of milliseconds it took to generate.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is <see langword="null"/>.</exception>
        public Status(
            [NotNull] string message,
            DateTimeOffset generatedAt,
            double duration)
        {
            Extensions[nameof(message)] = message ?? throw new ArgumentNullException(nameof(message));
            GeneratedAt = generatedAt;
            Duration = duration.ToString(InvariantCulture);
        }
    }
}

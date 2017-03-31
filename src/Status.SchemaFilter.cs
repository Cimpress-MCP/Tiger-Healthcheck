using System;
using JetBrains.Annotations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using static System.Globalization.CultureInfo;
using static System.TimeSpan;
using static Tiger.Healthcheck.Test;

namespace Tiger.Healthcheck
{
    /// <summary>Describes the schema of <see cref="Status"/>.</summary>
    sealed class StatusSchemaFilter
        : ISchemaFilter
    {
        /// <inheritdoc/>
        void ISchemaFilter.Apply([NotNull] Schema model, [NotNull] SchemaFilterContext context)
        {
            if (model == null) { throw new ArgumentNullException(nameof(model)); }
            if (context == null) { throw new ArgumentNullException(nameof(context)); }

            model.Description = "Represents the status of a healthcheck operation.";

            model.Properties["generated_at"].Description = "The time at which this report was generated.";
            model.Properties["duration_millis"].Description = "The number of milliseconds it took to generate the report.";
            model.Properties["tests"].Description = "Test results keyed by component.";

            // note(cosborn) Recreate the specification's example.
            model.Example = new Status(
                "Welcome bacʞ.",
                DateTimeOffset.Parse("2015-06-25T14:33:33.383Z", InvariantCulture),
                FromMilliseconds(15.8))
            {
                Tests =
                {
                    ["cassandra"] = Pass(FromMilliseconds(5.6), DateTimeOffset.Parse("2015-06-25T14:33:15.229Z", InvariantCulture)),
                    ["redis"] = Fail(
                        FromMilliseconds(15.6),
                        DateTimeOffset.Parse("2015-06-25T14:33:15.286Z", InvariantCulture),
                        "Unable to connect to myredis.mydomain.com:6379")
                }
            };
        }
    }
}

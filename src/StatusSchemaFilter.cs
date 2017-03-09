using System;
using JetBrains.Annotations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
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

            // note(cosborn) Recreate the specification's example.
            model.Example = new Status(
                "Welcome bacʞ.",
                DateTimeOffset.Parse("2015-06-25T14:33:33.383Z"),
                15.8)
            {
                Tests =
                {
                    ["cassandra"] = Pass(5.6f, DateTimeOffset.Parse("2015-06-25T14:33:15.229Z")),
                    ["redis"] = Fail(
                        15.6f,
                        DateTimeOffset.Parse("2015-06-25T14:33:15.286Z"),
                        "Unable to connect to myredis.mydomain.com:6379")
                }
            };
        }
    }
}

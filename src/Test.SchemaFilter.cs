using System;
using JetBrains.Annotations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tiger.Healthcheck
{
    /// <summary>Describes the schema of <see cref="Test"/>.</summary>
    sealed class TestSchemaFilter
        : ISchemaFilter
    {
        /// <inheritdoc/>
        void ISchemaFilter.Apply([NotNull] Schema model, [NotNull] SchemaFilterContext context)
        {
            if (model == null) { throw new ArgumentNullException(nameof(model)); }
            if (context == null) { throw new ArgumentNullException(nameof(context)); }

            model.Description = "Represents a status test.";

            model.Properties["duration_millis"].Description = "The number of milliseconds taken to run this test.";
            model.Properties["result"].Description = "The final state of this test.";
            model.Properties["tested_at"].Description = "The time at which this test was executed.";
            model.Properties["error"].Description = "A description of any error conditions.";
        }
    }
}

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tiger.Healthcheck
{
    /// <summary>Describes the operation of <see cref="HealthcheckController"/>.</summary>
    sealed class HealthcheckGetOperationFilter
        : IOperationFilter
    {
        /// <inheritdoc/>
        void IOperationFilter.Apply([NotNull] Operation operation, [NotNull] OperationFilterContext context)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }
            if (context == null) { throw new ArgumentNullException(nameof(context)); }

            operation.Summary = "Reports system health.";
            operation.Description = "Performs healthcheck of the service, possibly including subsystems.";
            operation.Responses["200"].Description = "The service is healthy.";
            operation.Responses["503"].Description = "The service is unhealthy.";
        }
    }
}

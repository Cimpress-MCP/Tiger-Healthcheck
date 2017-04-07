using System;
using JetBrains.Annotations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tiger.Healthcheck
{
    /// <content>Operation filters for Swagger document generation.</content>
    public sealed partial class HealthcheckController
    {
        sealed class GetOperationFilter
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
}

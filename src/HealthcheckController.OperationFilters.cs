// <copyright file="HealthcheckController.OperationFilters.cs" company="Cimpress, Inc.">
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

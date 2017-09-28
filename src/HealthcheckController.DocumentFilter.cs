// <copyright file="HealthcheckController.DocumentFilter.cs" company="Cimpress, Inc.">
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

using System.Collections.Generic;
using JetBrains.Annotations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using static JetBrains.Annotations.ImplicitUseKindFlags;

namespace Tiger.Healthcheck
{
    /// <content>Document filter for Swagger document generation.</content>
    public sealed partial class HealthcheckController
    {
        /// <summary>Provides a description of the healthcheck controller to Swagger.</summary>
        [UsedImplicitly(InstantiatedNoFixedConstructorSignature)]
        public sealed class DocumentFilter
            : IDocumentFilter
        {
            /// <inheritdoc/>
            void IDocumentFilter.Apply(
                [NotNull] SwaggerDocument swaggerDoc,
                DocumentFilterContext context)
            {
                if (swaggerDoc.Tags == null) { swaggerDoc.Tags = new List<Tag>(); }

                swaggerDoc.Tags.Add(new Tag
                {
                    Name = "Healthcheck",
                    Description = "Manages the reporting of system health."
                });
            }
        }
    }
}

﻿using System.Collections.Generic;
using JetBrains.Annotations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using static JetBrains.Annotations.ImplicitUseKindFlags;

namespace Tiger.Healthcheck
{
    /// <summary>Provides a description of the healthcheck controller to Swagger.</summary>
    [UsedImplicitly(InstantiatedNoFixedConstructorSignature)]
    sealed class HealthcheckDescriptionDocumentFilter
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
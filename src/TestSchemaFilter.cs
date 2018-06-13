// <copyright file="TestSchemaFilter.cs" company="Cimpress, Inc.">
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
    /// <summary>Describes the schema of <see cref="Test"/>.</summary>
    sealed class TestSchemaFilter
        : ISchemaFilter
    {
        /// <inheritdoc/>
        void ISchemaFilter.Apply([NotNull] Schema model, [NotNull] SchemaFilterContext context)
        {
            if (model is null) { throw new ArgumentNullException(nameof(model)); }
            if (context is null) { throw new ArgumentNullException(nameof(context)); }

            model.Description = "Represents a status test.";

            model.Properties["duration_millis"].Description = "The number of milliseconds taken to run this test.";
            model.Properties["result"].Description = "The final state of this test.";
            model.Properties["tested_at"].Description = "The time at which this test was executed.";
            model.Properties["error"].Description = "A description of any error conditions.";
        }
    }
}

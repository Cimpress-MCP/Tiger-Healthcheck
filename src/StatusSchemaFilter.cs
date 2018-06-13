// <copyright file="StatusSchemaFilter.cs" company="Cimpress, Inc.">
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
using static System.Globalization.CultureInfo;
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
            if (model is null) { throw new ArgumentNullException(nameof(model)); }
            if (context is null) { throw new ArgumentNullException(nameof(context)); }

            model.Description = "Represents the status of a healthcheck operation.";

            model.Properties["generated_at"].Description = "The time at which this report was generated.";
            model.Properties["duration_millis"].Description = "The number of milliseconds it took to generate the report.";
            model.Properties["tests"].Description = "Test results keyed by component.";

            // note(cosborn) Recreate the specification's example, mostly.
            model.Example = new Status(
                message: "Welcome bacʞ.",
                generatedAt: DateTimeOffset.Parse("2015-06-25T14:33:33.383Z", InvariantCulture),
                duration: TimeSpan.FromMilliseconds(15.8))
            {
                Tests =
                {
                    ["cassandra"] = Pass(
                        TimeSpan.FromMilliseconds(5.6),
                        DateTimeOffset.Parse("2015-06-25T14:33:15.229Z", InvariantCulture)),
                    ["redis"] = Fail(
                        TimeSpan.FromMilliseconds(15.6),
                        DateTimeOffset.Parse("2015-06-25T14:33:15.286Z", InvariantCulture),
                        "Unable to connect to myredis.mydomain.com:6379")
                }
            };
        }
    }
}

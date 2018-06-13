// <copyright file="MvcCoreBuilderExtensions.cs" company="Cimpress, Inc.">
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
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tiger.Clock;
using Tiger.Healthcheck;
using static System.Net.Http.HttpMethod;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>Extends the functionality of <see cref="IMvcCoreBuilder"/> for healthchecking.</summary>
    [PublicAPI]
    public static class MvcCoreBuilderExtensions
    {
        /// <summary>Adds healthchecking services to the specified <see cref="IMvcCoreBuilder"/>.</summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/> to which to add services.</param>
        /// <returns>The modified application builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/>.</exception>
        [NotNull]
        public static IMvcCoreBuilder AddHealthcheck([NotNull] this IMvcCoreBuilder builder)
        {
            if (builder is null) { throw new ArgumentNullException(nameof(builder)); }

            builder.AddCors(o => o.AddPolicy(
                "Healthcheck",
                b => b.AllowAnyOrigin()
                      .WithMethods(Get.Method)
                      .DisallowCredentials()
                      .SetPreflightMaxAge(TimeSpan.FromDays(1))));
            builder.Services.TryAddScoped<IClock, StandardClock>();
            builder.Services.AddSwaggerGen(o => o.DocumentFilter<HealthcheckController.DocumentFilter>());

            return builder;
        }
    }
}

// <copyright file="MvcBuilderExtensions.cs" company="Cimpress, Inc.">
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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tiger.Clock;

namespace Tiger.Healthcheck
{
    /// <summary>Extends the functionality of <see cref="IMvcBuilder"/> for healthchecking.</summary>
    [PublicAPI]
    public static class MvcBuilderExtensions
    {
        /// <summary>Adds healthchecking services to the specified <see cref="IMvcBuilder"/>.</summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/> to which to add services.</param>
        /// <returns>The modified application builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/>.</exception>
        [NotNull]
        public static IMvcBuilder AddHealthcheck([NotNull] this IMvcBuilder builder)
        {
            if (builder == null) { throw new ArgumentNullException(nameof(builder)); }

            builder.Services.AddCors(o =>
                o.AddPolicy("Healthcheck", b =>
                    b.AllowAnyOrigin()
                     .WithMethods("GET")
                     .DisallowCredentials()
                     .SetPreflightMaxAge(TimeSpan.FromDays(1))));
            builder.Services.TryAddScoped<IClock, StandardClock>();
            builder.Services.AddSwaggerGen(o => o.DocumentFilter<HealthcheckController.DocumentFilter>());

            return builder;
        }

        /// <summary>
        /// Adds healthchecking services to the specified <see cref="IMvcBuilder"/>
        /// and configures subsystem healthcheckers.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/> to which to add services.</param>
        /// <param name="configure">
        /// An <see cref="Action{T}"/> to configure the provided <see cref="HealthcheckBuilder"/>.
        /// </param>
        /// <returns>The modified application builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="configure"/> is <see langword="null"/>.</exception>
        [NotNull]
        public static IMvcBuilder AddHealthcheck(
            [NotNull] this IMvcBuilder builder,
            [NotNull, InstantHandle] Action<HealthcheckBuilder> configure)
        {
            if (builder == null) { throw new ArgumentNullException(nameof(builder)); }
            if (configure == null) { throw new ArgumentNullException(nameof(configure)); }

            return builder
                .AddHealthcheck()
                .ConfigureHealthcheck(configure);
        }

        /// <summary>Configures subsystem healthcheckers.</summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/> on which to configure services.</param>
        /// <param name="configure">
        /// An <see cref="Action{T}"/> to configure the provided <see cref="HealthcheckBuilder"/>.
        /// </param>
        /// <returns>The modified application builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="configure"/> is <see langword="null"/>.</exception>
        [NotNull]
        public static IMvcBuilder ConfigureHealthcheck(
            [NotNull] this IMvcBuilder builder,
            [NotNull, InstantHandle] Action<HealthcheckBuilder> configure)
        {
            if (builder == null) { throw new ArgumentNullException(nameof(builder)); }
            if (configure == null) { throw new ArgumentNullException(nameof(configure)); }

            var healthcheckBuilder = new HealthcheckBuilder();
            configure(healthcheckBuilder);
            builder.Services.TryAddEnumerable(healthcheckBuilder.Build());

            return builder;
        }
    }
}

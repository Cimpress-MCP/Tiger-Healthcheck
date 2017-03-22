using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tiger.Clock;

namespace Tiger.Healthcheck
{
    /// <summary>Extends the functionality of <see cref="IMvcCoreBuilder"/> for healthchecking.</summary>
    [PublicAPI]
    public static class MvcCoreBuilderExtensions
    {
        /// <summary>Adds healthchecking services to the specified <see cref="IMvcCoreBuilder"/>.</summary>
        /// <param name="builder">The <see cref="IMvcCoreBuilder"/> to which to add services.</param>
        /// <returns>The modified application builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/>.</exception>
        [NotNull]
        public static IMvcCoreBuilder AddHealthcheck(
            [NotNull] this IMvcCoreBuilder builder)
        {
            if (builder == null) { throw new ArgumentNullException(nameof(builder)); }

            builder.AddCors(o =>
                o.AddPolicy("Healthcheck", b =>
                    b.AllowAnyOrigin()
                     .WithMethods("GET")
                     .DisallowCredentials()
                     .SetPreflightMaxAge(TimeSpan.FromDays(1))));
            builder.Services.TryAddScoped<IClock, StandardClock>();
            builder.Services.AddSwaggerGen(o => o.DocumentFilter<HealthcheckDescriptionDocumentFilter>());

            return builder;
        }

        /// <summary>
        /// Adds healthchecking services to the specified <see cref="IMvcCoreBuilder"/>
        /// and configures subsystem healthcheckers.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcCoreBuilder"/> to which to add services.</param>
        /// <param name="configure">
        /// An <see cref="Action{T}"/> to configure the provided <see cref="HealthcheckBuilder"/>.
        /// </param>
        /// <returns>The modified application builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="configure"/> is <see langword="null"/>.</exception>
        [NotNull]
        public static IMvcCoreBuilder AddHealthcheck(
            [NotNull] this IMvcCoreBuilder builder,
            [NotNull, InstantHandle] Action<HealthcheckBuilder> configure)
        {
            if (builder == null) { throw new ArgumentNullException(nameof(builder)); }
            if (configure == null) { throw new ArgumentNullException(nameof(configure)); }

            return builder
                .AddHealthcheck()
                .ConfigureHealthcheck(configure);
        }

        /// <summary>Configures subsystem healthcheckers.</summary>
        /// <param name="builder">
        /// The <see cref="IMvcCoreBuilder"/> on which to configure services.
        /// </param>
        /// <param name="configure">
        /// An <see cref="Action{T}"/> to configure the provided <see cref="HealthcheckBuilder"/>.
        /// </param>
        /// <returns>The modified application builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="configure"/> is <see langword="null"/>.</exception>
        [NotNull]
        public static IMvcCoreBuilder ConfigureHealthcheck(
            [NotNull] this IMvcCoreBuilder builder,
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
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using static Microsoft.Extensions.DependencyInjection.ServiceDescriptor;

namespace Tiger.Healthcheck
{
    /// <summary>Exposes methods to build a composite healthchecking test.</summary>
    [PublicAPI]
    public sealed class HealthcheckBuilder
    {
        readonly ServiceCollection _healthCheckers = new ServiceCollection();

        /// <summary>Initializes a new instance of the <see cref="HealthcheckBuilder"/> class.</summary>
        internal HealthcheckBuilder()
        {
        }

        /// <summary>Adds the specified healthchecker to the collection of healthcheckers.</summary>
        /// <typeparam name="T">The type of the healthchecker to add.</typeparam>
        /// <returns>The current healthcheck builder.</returns>
        public void Add<T>()
            where T: class, IHealthchecker =>
            _healthCheckers.Add(Scoped<IHealthchecker, T>());

        /// <summary>Builds a new <see cref="IServiceCollection"/> using the entries added.</summary>
        /// <returns>The constructed <see cref="IServiceCollection"/>.</returns>
        [NotNull, ItemNotNull]
        internal IServiceCollection Build() => _healthCheckers;
    }
}

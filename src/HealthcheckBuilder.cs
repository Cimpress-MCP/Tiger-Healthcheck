// <copyright file="HealthcheckBuilder.cs" company="Cimpress, Inc.">
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
        readonly IServiceCollection _healthCheckers = new ServiceCollection();

        /// <summary>Initializes a new instance of the <see cref="HealthcheckBuilder"/> class.</summary>
        internal HealthcheckBuilder()
        {
        }

        /// <summary>Adds the specified healthchecker to the collection of healthcheckers.</summary>
        /// <typeparam name="T">The type of the healthchecker to add.</typeparam>
        public void Add<T>()
            where T : class, IHealthchecker =>
            _healthCheckers.TryAddEnumerable(Scoped<IHealthchecker, T>());

        /// <summary>Builds a new <see cref="IServiceCollection"/> using the entries added.</summary>
        /// <returns>The constructed <see cref="IServiceCollection"/>.</returns>
        [NotNull, ItemNotNull]
        internal IServiceCollection Build() => _healthCheckers;
    }
}

// <copyright file="IHealthchecker.cs" company="Cimpress, Inc.">
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
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Tiger.Healthcheck
{
    /// <summary>Tests the health of a service subsystem.</summary>
    [PublicAPI]
    public interface IHealthchecker
    {
        /// <summary>Gets the name of the subsystem healthcheck test to be performed.</summary>
        [NotNull]
        string Name { get; }

        /// <summary>Tests a service subsystem for health.</summary>
        /// <param name="generationTime">The time at which the test was generated.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        /// <returns>The result of testing the service dependency, be it success or failure.</returns>
        /// <remarks>
        /// <para>What it means for a service to be "healthy" is entirely left to the discretion
        /// of the implementor. Many service subsystems accessed through client libraries will
        /// have a built-in method such as "Ping", which can be a useful indicator of health.</para>
        /// </remarks>
        [NotNull, ItemNotNull]
        Task<Test> TestHealthAsync(
            DateTimeOffset generationTime,
            CancellationToken cancellationToken = default);
    }
}

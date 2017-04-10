using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Tiger.Clock;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static System.StringComparer;
using static Tiger.Healthcheck.State;

namespace Tiger.Healthcheck
{
    /// <summary>Manages the reporting of system health.</summary>
    [PublicAPI]
    [Route("[controller]"), EnableCors("Healthcheck"), AllowAnonymous]
    public sealed partial class HealthcheckController
        : ControllerBase
    {
        readonly IClock _clock;
        readonly IEnumerable<IHealthchecker> _healthcheckers;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthcheckController"/> class.
        /// </summary>
        /// <param name="clock">A source of system time.</param>
        /// <param name="healthcheckers">A collection of objects capable of determining subsystem health.</param>
        /// <exception cref="ArgumentNullException"><paramref name="clock"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="healthcheckers"/> is <see langword="null"/>.</exception>
        public HealthcheckController(
            [NotNull] IClock clock,
            [NotNull] IEnumerable<IHealthchecker> healthcheckers)
        {
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
            _healthcheckers = healthcheckers ?? throw new ArgumentNullException(nameof(healthcheckers));
        }

        /// <summary>Reports system health.</summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        /// <response code="200">The service is healthy.</response>
        /// <response code="503">The service is unhealthy.</response>
        /// <returns>Response indicating the health of the service.</returns>
        /// <remarks>Performs healthcheck of the service, possibly including subsystems.</remarks>
        [HttpGet]
        [ProducesResponseType(typeof(Status), Status200OK)]
        [ProducesResponseType(typeof(Status), Status503ServiceUnavailable)]
        [SwaggerOperationFilter(typeof(GetOperationFilter))]
        [NotNull, ItemNotNull]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var generationTime = _clock.Now;
            var statusTimer = Stopwatch.StartNew();

            // note(cosborn) Transform into the shape that the healthcheck RFC expects.
            var healthTasks = _healthcheckers.Select(async h => new
            {
                h.Name,
                Test = await h.TestHealthAsync(generationTime, cancellationToken)
            });
            var healths = await Task.WhenAll(healthTasks);

            statusTimer.Stop();

            var status = new Status("Welcome bacʞ.", generationTime, statusTimer.Elapsed)
            {
                Tests = { healths.ToDictionary(h => h.Name, h => h.Test, Ordinal) }
            };

            return status.Tests.Values.Any(t => t.Result == Failed)
                ? StatusCode(Status503ServiceUnavailable, status)
                : Ok(status);
        }
    }
}

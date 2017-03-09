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
using Tiger.Clock;
using Tiger.Types;
using static System.StringComparer;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static Tiger.Healthcheck.State;

namespace Tiger.Healthcheck
{
    /// <summary>Manages the reporting of system health.</summary>
    [PublicAPI]
    [Route("[controller]"), EnableCors("Healthcheck"), AllowAnonymous]
    public class HealthcheckController
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
        /// <response code="200">The service is healthy.</response>
        /// <response code="503">The service is unhealthy.</response>
        /// <returns>Response indicating the health of the service.</returns>
        /// <remarks>Performs healthcheck of the service.</remarks>
        [HttpGet]
        [ProducesResponseType(typeof(Status), Status200OK)]
        [ProducesResponseType(typeof(Status), Status503ServiceUnavailable)]
        [NotNull, ItemNotNull]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var generationTime = _clock.Now;
            var statusTimer = Stopwatch.StartNew();

            // note(cosborn) Transform into the shape that the healthcheck RFC expects.
            var healths = await _healthcheckers.Select(async h => new
            {
                h.Name,
                Test = await h.TestHealthAsync(generationTime, cancellationToken)
            }).Pipe(Task.WhenAll);

            statusTimer.Stop();

            var status = new Status("Welcome bacʞ.", generationTime, statusTimer.ElapsedMilliseconds)
            {
                Tests = { healths.ToDictionary(h => h.Name, h => h.Test, Ordinal) }
            };

            return status.Tests.Values.Any(t => t.Result == Failed)
                ? StatusCode(Status503ServiceUnavailable, status)
                : Ok(status);
        }
    }
}

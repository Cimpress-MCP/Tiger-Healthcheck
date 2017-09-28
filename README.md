# Tiger.Healthcheck

## What It Is

Tiger.Healthcheck is a library for automating the creation of service healthcheck functionality and endpoints.

## Why You Want It

From [RFC-Cimpress-1](https://corewiki.cimpress.net/wiki/RFC-Cimpress-1_-_API_Publication_Requirements):

> ### APIs MUST:
> - expose an instance healthcheck
>
> ### APIs SHOULD:
> - Use the [healthcheck.spec](https://github.com/Cimpress-MCP/healthcheck.spec) specification for healthchecks

This library will bring your ASP.NET Core service into full compliance with the healthcheck requirements of RFC-Cimpress-1 by calling a single extension method.

### Subsystem Health

The healthcheck specification allows for the health of subsystems to be taken into account when determining service health. This can be achieved using this library by implementing the `IHealthchecker` interface. For example, this class will check the health of an Elasticsearch instance:

```csharp
/// <summary>Checks the health of an Elasticsearch service subsystem.</summary>
public sealed class ElasticsearchHealthchecker
    : IHealthchecker
{
    /// <inheritdoc/>
    string IHealthchecker.Name => "elasticsearch";

    readonly IElasticClient _client;

    /// <summary>Initializes a new instance of the <see cref="ElasticsearchHealthchecker"/> class.</summary>
    /// <param name="client">A client for accessing an instance of Elasticsearch.</param>
    /// <exception cref="ArgumentNullException"><paramref name="client"/> is <see langword="null"/>.</exception>
    public ElasticsearchHealthchecker([NotNull] IElasticClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    /// <inheritdoc/>
    async Task<Test> IHealthchecker.TestHealthAsync(
        DateTimeOffset generationTime,
        CancellationToken cancellationToken)
    {
        var timer = Stopwatch.StartNew();
        var pingCall = await _client.PingAsync(cancellationToken: cancellationToken).Map(r => r.ApiCall);
        timer.Stop();

        return pingCall.Success
            ? Pass(timer.Elapsed, generationTime)
            : Fail(timer.Elapsed, generationTime, pingCall.DebugInformation);
    }
}
```

This can be associated with the service healthchecker in the service's `ConfigureServices` method in the `Startup` class:

```csharp
services.AddHealthchecker(b => b.Add<ElasticsearchHealthchecker>());
```

## How You Develop It

This project is using the standard [`dotnet`](https://dot.net) build tool. A brief primer:

- Restore NuGet dependencies: `dotnet restore`
- Build the entire solution: `dotnet build`
- Run all unit tests: `dotnet test`
- Pack for publishing: `dotnet pack -o "$(pwd)/dist/"`

The parameter `--configuration` (shortname `-c`) can be supplied to the `build`, `test`, and `pack` steps with the following meaningful values:

- “Debug” (the default)
- “Release”

This repository is attempting to use the [GitFlow](http://jeffkreeftmeijer.com/2010/why-arent-you-using-git-flow/) branching methodology. Results may be mixed, please be aware.

## Thank You

Seriously, though. Thank you for using this software. The author hopes it performs admirably for you.

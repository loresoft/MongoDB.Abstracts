using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace MongoDB.Abstracts.Tests;

public abstract class TestServiceBase
{
    protected ITestOutputHelper OutputHelper { get; }
    protected IConfiguration Configuration { get; }
    protected IServiceProvider Services { get; }

    protected TestServiceBase(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;

        var builder = new ConfigurationBuilder();
        Configure(builder);

        Configuration = builder.Build();

        var services = new ServiceCollection();
        ConfigureServices(services);

        Services = services.BuildServiceProvider();
    }

    protected virtual void Configure(IConfigurationBuilder configuration)
    {
        var enviromentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Test";

        configuration
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{enviromentName}.json", true)
            .AddEnvironmentVariables();
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        services
            .AddSingleton(Configuration)
            .AddLogging((builder) => builder
                .AddXUnit(OutputHelper)
                .SetMinimumLevel(LogLevel.Debug)
            )
            .AddSingleton(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("MongoUnitTest");

                return MongoFactory.GetDatabaseFromConnectionString(connectionString);
            })
            .AddSingleton(typeof(IMongoEntityQuery<>), typeof(MongoEntityQuery<>))
            .AddSingleton(typeof(IMongoEntityRepository<>), typeof(MongoEntityRepository<>));
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MongoDB.Driver.Core.Configuration;

using XUnit.Hosting;

namespace MongoDB.Abstracts.Tests;

public class DatabaseFixture : TestApplicationFixture
{
    protected override void ConfigureApplication(HostApplicationBuilder builder)
    {
        base.ConfigureApplication(builder);

        var services = builder.Services;

        // using connection string named "MongoUnitTest" from appsettings.json
        // configure logging settings for MongoDB driver

        services.AddMongoRepository(
            nameOrConnectionString: "MongoUnitTest",
            configuration: (provider, setting) =>
            {
                var loggerFactory = provider.GetService<ILoggerFactory>();
                setting.LoggingSettings = new LoggingSettings(loggerFactory);
            }
        );

        services.AddMongoRepository<DiscriminatorConnection>("mongodb://localhost:27017/DiscriminatorUnitTesting");

        services.AddKeyedMongoDatabase(
            nameOrConnectionString: "mongodb://localhost:27017/MongoKeyedDatabase",
            serviceKey: "MongoKeyedDatabase");

        services.AddMongoDBAbstractsTests();
    }
}


public readonly struct DiscriminatorConnection { }

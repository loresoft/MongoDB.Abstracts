using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using XUnit.Hosting;

namespace MongoDB.Abstracts.Tests;

public class DatabaseFixture : TestApplicationFixture
{
    protected override void ConfigureApplication(HostApplicationBuilder builder)
    {
        base.ConfigureApplication(builder);

        var services = builder.Services;

        services.AddMongoRepository("MongoUnitTest");

        services.AddMongoRepository<DiscriminatorConnection>("mongodb://localhost:27017/DiscriminatorUnitTesting");

        services.AddMongoDatabase("mongodb://localhost:27017/MongoKeyedDatabase", "MongoKeyedDatabase");

        services.AddMongoDBAbstractsTests();
    }
}


public readonly struct DiscriminatorConnection { }

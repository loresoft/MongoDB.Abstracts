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

        services.AddMongoDB("MongoUnitTest");
        services.AddMongoDB("mongodb://localhost:27017/UnitTesting", "MongoUnitTest");

        services.AddMongoDBAbstractsTests();
    }
}

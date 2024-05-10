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

        services.AddSingleton(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("MongoUnitTest");
            return MongoFactory.GetDatabaseFromConnectionString(connectionString);
        });

        services.AddSingleton(typeof(IMongoEntityQuery<>), typeof(MongoEntityQuery<>));
        services.AddSingleton(typeof(IMongoEntityRepository<>), typeof(MongoEntityRepository<>));

        services.AddMongoDBAbstractsTests();
    }
}

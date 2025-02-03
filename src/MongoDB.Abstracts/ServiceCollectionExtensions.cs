// Ignore Spelling: Mongo

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MongoDB.Abstracts;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the MongoDB services with the specified connection string.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="nameOrConnectionString">The connection string or the name of connection string located in the application configuration.</param>
    /// <returns>
    /// The same service collection so that multiple calls can be chained.
    /// </returns>
    public static IServiceCollection AddMongoDB(this IServiceCollection services, string nameOrConnectionString)
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        if (nameOrConnectionString is null)
            throw new ArgumentNullException(nameof(nameOrConnectionString));


        services.TryAddSingleton(sp =>
        {
            var connectionString = ResolveConnectionString(sp, nameOrConnectionString);
            return MongoFactory.GetDatabaseFromConnectionString(connectionString);
        });

        services.TryAddSingleton(typeof(IMongoEntityQuery<>), typeof(MongoEntityQuery<>));
        services.TryAddSingleton(typeof(IMongoEntityRepository<>), typeof(MongoEntityRepository<>));

        return services;
    }

    /// <summary>
    /// Adds the MongoDB services with the specified connection string and service key.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="nameOrConnectionString">The connection string or the name of connection string located in the application configuration.</param>
    /// <param name="serviceKey">The service key.</param>
    /// <returns>
    /// The same service collection so that multiple calls can be chained.
    /// </returns>
    public static IServiceCollection AddMongoDB(this IServiceCollection services, string nameOrConnectionString, object? serviceKey)
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        if (nameOrConnectionString is null)
            throw new ArgumentNullException(nameof(nameOrConnectionString));

        services.TryAddKeyedSingleton(
            serviceKey: serviceKey,
            implementationFactory: (sp, key) =>
            {
                var connectionString = ResolveConnectionString(sp, nameOrConnectionString);
                return MongoFactory.GetDatabaseFromConnectionString(connectionString);
            }
        );

        services.TryAddKeyedSingleton(typeof(IMongoEntityQuery<>), serviceKey, typeof(MongoEntityQuery<>));
        services.TryAddKeyedSingleton(typeof(IMongoEntityRepository<>), serviceKey, typeof(MongoEntityRepository<>));

        return services;
    }

    private static string ResolveConnectionString(IServiceProvider serviceProvider, string nameOrConnectionString)
    {
        var isConnectionString = nameOrConnectionString.IndexOfAny([';', '=']) > 0;
        if (isConnectionString)
            return nameOrConnectionString;

        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        // first try connection strings section
        var connectionString = configuration.GetConnectionString(nameOrConnectionString);
        if (!string.IsNullOrEmpty(connectionString))
            return connectionString;

        // next try root collection
        connectionString = configuration[nameOrConnectionString];
        if (!string.IsNullOrEmpty(connectionString))
            return connectionString;

        return nameOrConnectionString;
    }

}

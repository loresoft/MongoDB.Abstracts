// Ignore Spelling: Mongo

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using MongoDB.Driver;

namespace MongoDB.Abstracts;

/// <summary>
/// Provides extension methods for <see cref="IServiceCollection"/> to configure MongoDB repository and database services.
/// </summary>
/// <remarks>
/// <para>
/// This static class provides convenient extension methods for registering MongoDB-related services in dependency
/// injection containers. It supports both simple single-database scenarios and complex multi-database scenarios
/// using discriminator types for type-safe service resolution.
/// </para>
/// <para>
/// The extension methods handle connection string resolution from configuration, database instance creation,
/// and service registration patterns that integrate seamlessly with ASP.NET Core and other .NET applications
/// using Microsoft.Extensions.DependencyInjection.
/// </para>
/// <para>
/// All registration methods use singleton lifetime for database connections and repository services to optimize
/// performance and resource utilization while maintaining thread safety for concurrent operations.
/// </para>
/// </remarks>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers MongoDB repository and query services with the dependency injection container using a single database connection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="nameOrConnectionString">The MongoDB connection string or the name of a connection string located in the application configuration.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="nameOrConnectionString"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This method registers the core MongoDB repository and query services for applications using a single database.
    /// It automatically configures <see cref="IMongoEntityQuery{TEntity}"/> and <see cref="IMongoEntityRepository{TEntity}"/>
    /// services using singleton lifetime for optimal performance.
    /// </para>
    /// <para>
    /// The method delegates database registration to <see cref="AddMongoDatabase(IServiceCollection, string)"/> and then
    /// registers the generic repository and query interfaces with their concrete implementations. This provides a
    /// complete MongoDB data access solution with minimal configuration effort.
    /// </para>
    /// <para>
    /// Connection string resolution supports both direct connection strings and configuration-based lookup,
    /// providing flexibility for different deployment scenarios and configuration management approaches.
    /// </para>
    /// </remarks>
    public static IServiceCollection AddMongoRepository(this IServiceCollection services, string nameOrConnectionString)
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        if (nameOrConnectionString is null)
            throw new ArgumentNullException(nameof(nameOrConnectionString));


        services.AddMongoDatabase(nameOrConnectionString);

        services.TryAddSingleton(typeof(IMongoEntityQuery<>), typeof(MongoEntityQuery<>));
        services.TryAddSingleton(typeof(IMongoEntityRepository<>), typeof(MongoEntityRepository<>));

        return services;
    }

    /// <summary>
    /// Registers MongoDB repository and query services with discriminator support for multi-database scenarios.
    /// </summary>
    /// <typeparam name="TDiscriminator">The type used to discriminate between different MongoDB database connections.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="nameOrConnectionString">The MongoDB connection string or the name of a connection string located in the application configuration.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="nameOrConnectionString"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This method registers MongoDB repository and query services with discriminator support, enabling type-safe
    /// dependency injection for applications requiring multiple database connections. It configures
    /// <see cref="IMongoEntityQuery{TDiscriminator, TEntity}"/> and <see cref="IMongoEntityRepository{TDiscriminator, TEntity}"/>
    /// services with singleton lifetime for optimal performance.
    /// </para>
    /// <para>
    /// The discriminator pattern enables the same entity types to be used across different database contexts
    /// while maintaining clean separation and type safety. This is particularly useful for multi-tenant applications,
    /// microservices architectures, or scenarios requiring read/write replica separation.
    /// </para>
    /// <para>
    /// The method uses <see cref="MongoDiscriminator{TDiscriminator}"/> to wrap the database connection,
    /// providing the foundation for type-safe service resolution in complex application architectures.
    /// </para>
    /// </remarks>
    public static IServiceCollection AddMongoRepository<TDiscriminator>(this IServiceCollection services, string nameOrConnectionString)
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        if (nameOrConnectionString is null)
            throw new ArgumentNullException(nameof(nameOrConnectionString));


        services.AddMongoDatabase<TDiscriminator>(nameOrConnectionString);

        services.TryAddSingleton(typeof(IMongoEntityQuery<,>), typeof(MongoEntityQuery<,>));
        services.TryAddSingleton(typeof(IMongoEntityRepository<,>), typeof(MongoEntityRepository<,>));

        return services;
    }

    /// <summary>
    /// Registers a MongoDB database service with the dependency injection container for single-database scenarios.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="nameOrConnectionString">The MongoDB connection string or the name of a connection string located in the application configuration.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="nameOrConnectionString"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This method registers an <see cref="IMongoDatabase"/> service as a singleton, providing a shared database
    /// connection for all MongoDB operations in the application. The database instance is created using the
    /// <see cref="MongoFactory"/> utility with proper connection string resolution.
    /// </para>
    /// <para>
    /// Connection string resolution is handled automatically, supporting both direct connection strings and
    /// configuration-based lookup through the application's <see cref="IConfiguration"/> service. This provides
    /// flexibility for different deployment scenarios and configuration management approaches.
    /// </para>
    /// <para>
    /// The singleton lifetime ensures optimal resource utilization and performance while maintaining thread safety
    /// for concurrent database operations across the application.
    /// </para>
    /// </remarks>
    public static IServiceCollection AddMongoDatabase(this IServiceCollection services, string nameOrConnectionString)
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

        return services;
    }

    /// <summary>
    /// Registers a MongoDB database service with discriminator support for multi-database scenarios.
    /// </summary>
    /// <typeparam name="TDiscriminator">The type used to discriminate between different MongoDB database connections.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="nameOrConnectionString">The MongoDB connection string or the name of a connection string located in the application configuration.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="nameOrConnectionString"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This method registers a <see cref="MongoDiscriminator{TDiscriminator}"/> service as a singleton, wrapping
    /// the MongoDB database connection with type-safe discriminator support. This enables multiple database
    /// connections to be registered and resolved independently using different discriminator types.
    /// </para>
    /// <para>
    /// The discriminator pattern provides compile-time type safety for dependency injection scenarios where
    /// multiple database connections are required. Each discriminator type represents a distinct database
    /// context, enabling clean separation of multi-tenant data, read/write replicas, or domain-specific databases.
    /// </para>
    /// <para>
    /// Connection string resolution and database creation follow the same patterns as single-database scenarios,
    /// with the additional discriminator wrapper providing the foundation for type-safe multi-connection support.
    /// </para>
    /// </remarks>
    public static IServiceCollection AddMongoDatabase<TDiscriminator>(this IServiceCollection services, string nameOrConnectionString)
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        if (nameOrConnectionString is null)
            throw new ArgumentNullException(nameof(nameOrConnectionString));


        services.TryAddSingleton(sp =>
        {
            var connectionString = ResolveConnectionString(sp, nameOrConnectionString);
            var database = MongoFactory.GetDatabaseFromConnectionString(connectionString);

            return new MongoDiscriminator<TDiscriminator>(database);
        });

        return services;
    }

    /// <summary>
    /// Registers a MongoDB database service with a specific service key for keyed service scenarios.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="nameOrConnectionString">The MongoDB connection string or the name of a connection string located in the application configuration.</param>
    /// <param name="serviceKey">The service key used for keyed service registration and resolution.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="nameOrConnectionString"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This method registers an <see cref="IMongoDatabase"/> service using keyed service registration, enabling
    /// multiple database connections to be registered and resolved using different service keys. This approach
    /// is useful for scenarios where string-based or object-based keys provide more flexibility than type-based discrimination.
    /// </para>
    /// <para>
    /// Keyed services provide an alternative to discriminator types for multi-database scenarios, offering
    /// runtime-based service resolution using keys rather than compile-time type safety. This can be useful
    /// for dynamic configuration scenarios or when database selection depends on runtime conditions.
    /// </para>
    /// <para>
    /// The service key can be any object type, providing flexibility for different keying strategies including
    /// strings, enums, or custom key objects. Connection string resolution follows standard patterns with
    /// automatic configuration lookup support.
    /// </para>
    /// </remarks>
    public static IServiceCollection AddMongoDatabase(this IServiceCollection services, string nameOrConnectionString, object? serviceKey)
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

        return services;
    }

    /// <summary>
    /// Resolves a connection string from either a direct connection string or a configuration key name.
    /// </summary>
    /// <param name="services">The <see cref="IServiceProvider"/> used to access configuration services.</param>
    /// <param name="nameOrConnectionString">The connection string value or the name of a connection string located in the application configuration.</param>
    /// <returns>The resolved MongoDB connection string ready for use with MongoDB driver.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This method provides intelligent connection string resolution by first attempting to detect if the provided
    /// value is a direct connection string (by checking for common connection string characters) or a configuration key name.
    /// If it appears to be a direct connection string, it is returned unchanged.
    /// </para>
    /// <para>
    /// For configuration-based resolution, the method first attempts to resolve the value from the standard
    /// ConnectionStrings configuration section, then falls back to searching the root configuration collection.
    /// This provides flexibility for different configuration organization approaches.
    /// </para>
    /// <para>
    /// If no configuration value is found, the original input is returned as-is, providing a fallback that
    /// supports direct connection string usage even when configuration lookup fails. This ensures robust
    /// behavior across different deployment and configuration scenarios.
    /// </para>
    /// </remarks>
    public static string ResolveConnectionString(this IServiceProvider services, string nameOrConnectionString)
    {
        var isConnectionString = nameOrConnectionString.IndexOfAny([';', '=', ':', '/']) > 0;
        if (isConnectionString)
            return nameOrConnectionString;

        var configuration = services.GetRequiredService<IConfiguration>();

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

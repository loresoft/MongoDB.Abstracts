using MongoDB.Driver;

namespace MongoDB.Abstracts;

/// <summary>
/// Provides a typed wrapper for MongoDB database connections using discriminator types for dependency injection.
/// </summary>
/// <typeparam name="TDiscriminator">The type used to discriminate between different MongoDB database connections.</typeparam>
/// <remarks>
/// <para>
/// This class enables type-safe dependency injection scenarios where multiple MongoDB database connections
/// need to be distinguished and resolved independently. The discriminator type acts as a compile-time
/// identifier that allows dependency injection containers to register and resolve multiple database
/// connections without ambiguity.
/// </para>
/// <para>
/// Common discriminator patterns include enums, marker interfaces, or simple struct types that represent
/// different database contexts such as tenant-specific databases, read/write replicas, or separate
/// application domains. This approach eliminates the need for string-based service keys or complex
/// factory patterns while maintaining type safety.
/// </para>
/// <para>
/// The discriminator pattern is particularly useful in multi-tenant applications, microservices
/// architectures, or scenarios where the same entity types exist across multiple MongoDB databases
/// but require separate connection management.
/// </para>
/// </remarks>
public class MongoDiscriminator<TDiscriminator>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MongoDiscriminator{TDiscriminator}"/> class with the specified MongoDB database.
    /// </summary>
    /// <param name="mongoDatabase">The MongoDB database instance to be associated with this discriminator.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="mongoDatabase"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// The provided MongoDB database instance is typically configured with connection strings, authentication,
    /// and other database-specific settings appropriate for the discriminator type. The database instance
    /// should be properly initialized and ready for operations.
    /// </para>
    /// <para>
    /// This constructor is commonly called by dependency injection containers during service resolution,
    /// where different database instances are associated with their respective discriminator types.
    /// </para>
    /// </remarks>
    public MongoDiscriminator(IMongoDatabase mongoDatabase)
    {
        MongoDatabase = mongoDatabase ?? throw new ArgumentNullException(nameof(mongoDatabase));
    }

    /// <summary>
    /// Gets the MongoDB database instance associated with this discriminator.
    /// </summary>
    /// <value>
    /// The <see cref="IMongoDatabase"/> instance that provides access to MongoDB collections and operations
    /// for the specific database context represented by the discriminator type.
    /// </value>
    /// <remarks>
    /// <para>
    /// This property provides direct access to the underlying MongoDB database for advanced operations
    /// that may not be covered by repository abstractions, such as database-level commands, collection
    /// management, or transaction handling.
    /// </para>
    /// <para>
    /// The database instance maintains its connection settings, authentication context, and other
    /// configuration that was established during initialization, ensuring consistent behavior
    /// across all operations performed through this discriminator.
    /// </para>
    /// </remarks>
    public IMongoDatabase MongoDatabase { get; }
}

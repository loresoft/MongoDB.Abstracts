// Ignore Spelling: Mongo

using System.Linq.Expressions;

using MongoDB.Driver;

namespace MongoDB.Abstracts;

/// <summary>
/// Provides a complete MongoDB repository implementation specifically for entities implementing <see cref="IMongoEntity"/>.
/// </summary>
/// <typeparam name="TEntity">The type of the MongoDB entity that implements <see cref="IMongoEntity"/>.</typeparam>
/// <remarks>
/// <para>
/// This class extends <see cref="MongoRepository{TEntity, TKey}"/> with string-based keys and provides a ready-to-use
/// repository implementation for MongoDB entities that follow the <see cref="IMongoEntity"/> pattern. It automatically
/// handles key extraction and expression generation based on the standardized Id property, eliminating the need
/// for custom implementation in most standard MongoDB entity scenarios.
/// </para>
/// <para>
/// The repository includes all CRUD operations (Create, Read, Update, Delete) with built-in automatic audit timestamp
/// management for entities implementing <see cref="IMongoEntity"/>. Created and Updated timestamps are automatically
/// managed during insert and update operations to maintain accurate audit trails without manual intervention.
/// </para>
/// <para>
/// This implementation is designed to work seamlessly with dependency injection containers and provides a complete
/// data access solution for MongoDB entities without requiring additional implementation effort. It inherits all
/// performance optimizations including lazy collection initialization, proper async/await patterns, and MongoDB
/// driver integration from the base repository class.
/// </para>
/// </remarks>
public class MongoEntityRepository<TEntity> : MongoRepository<TEntity, string>, IMongoEntityRepository<TEntity>
    where TEntity : class, IMongoEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MongoEntityRepository{TEntity}"/> class with the specified MongoDB database.
    /// </summary>
    /// <param name="mongoDatabase">The MongoDB database instance used for repository operations on the entity collection.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="mongoDatabase"/> is <see langword="null" />.</exception>
    /// <remarks>
    /// <para>
    /// The constructor inherits all initialization behavior from the base repository class, including lazy collection
    /// initialization and performance optimizations. The database instance should be properly configured with
    /// connection strings, authentication, write concerns, and other MongoDB-specific settings.
    /// </para>
    /// <para>
    /// This constructor is typically called by dependency injection containers during service resolution,
    /// where the database instance is configured according to application requirements and connection patterns.
    /// </para>
    /// </remarks>
    public MongoEntityRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {

    }

    /// <summary>
    /// Extracts the string-based identifier from the specified MongoDB entity.
    /// </summary>
    /// <param name="entity">The MongoDB entity to extract the identifier from.</param>
    /// <returns>The string identifier from the entity's Id property as defined by <see cref="IMongoEntity"/>.</returns>
    /// <remarks>
    /// <para>
    /// This implementation provides automatic key extraction for entities implementing <see cref="IMongoEntity"/>,
    /// using the standardized Id property as the entity's unique identifier. This eliminates the need for custom
    /// key extraction logic and ensures consistent entity identification across all repository operations.
    /// </para>
    /// <para>
    /// The returned identifier is used internally by repository operations for update, delete, and key-based
    /// query operations, ensuring reliable entity identification and optimal query performance when combined
    /// with appropriate MongoDB indexes on the Id field.
    /// </para>
    /// </remarks>
    protected override string EntityKey(TEntity entity)
    {
        return entity.Id;
    }

    /// <summary>
    /// Creates a filter expression that matches entities with the specified string identifier.
    /// </summary>
    /// <param name="key">The string identifier to create the filter expression for.</param>
    /// <returns>A lambda expression that filters entities by comparing their Id property with the specified key.</returns>
    /// <remarks>
    /// <para>
    /// This implementation provides automatic filter expression generation for string-based entity identifiers,
    /// creating expressions that compare the entity's Id property with the provided key value. The generated
    /// expressions are optimized for MongoDB query translation and leverage any indexes created on the Id field.
    /// </para>
    /// <para>
    /// The filter expressions support efficient key-based queries using MongoDB's native filtering capabilities
    /// and are used throughout the repository for find, update, delete, and other identifier-based operations.
    /// This eliminates the need for custom expression logic in standard MongoDB entity scenarios.
    /// </para>
    /// </remarks>
    protected override Expression<Func<TEntity, bool>> KeyExpression(string key)
    {
        return entity => entity.Id == key;
    }
}

/// <summary>
/// Provides a MongoDB repository implementation with connection discrimination support for multi-database scenarios.
/// </summary>
/// <typeparam name="TDiscriminator">The type used to discriminate between different MongoDB database connections.</typeparam>
/// <typeparam name="TEntity">The type of the MongoDB entity that implements <see cref="IMongoEntity"/>.</typeparam>
/// <remarks>
/// <para>
/// This class extends <see cref="MongoEntityRepository{TEntity}"/> to support scenarios where multiple MongoDB
/// database connections need to be distinguished using discriminator types. It leverages the
/// <see cref="MongoDiscriminator{TDiscriminator}"/> pattern to provide type-safe dependency injection
/// for complex application architectures requiring multiple database contexts.
/// </para>
/// <para>
/// The discriminator-based approach enables the same entity types to be used across different database
/// contexts while maintaining clean separation, type safety, and clear dependency injection patterns.
/// This is particularly valuable in scenarios such as multi-tenant applications with tenant-specific databases,
/// microservices architectures with domain-separated data stores, or applications requiring read/write
/// replica separation for performance optimization.
/// </para>
/// </remarks>
public class MongoEntityRepository<TDiscriminator, TEntity>(MongoDiscriminator<TDiscriminator> mongoDiscriminator)
    : MongoEntityRepository<TEntity>(mongoDiscriminator.MongoDatabase), IMongoEntityRepository<TDiscriminator, TEntity>
    where TEntity : class, IMongoEntity;


// Ignore Spelling: Mongo

using System.Linq.Expressions;

using MongoDB.Driver;

namespace MongoDB.Abstracts;

/// <summary>
/// Provides a concrete MongoDB query implementation specifically for entities implementing <see cref="IMongoEntity"/>.
/// </summary>
/// <typeparam name="TEntity">The type of the MongoDB entity that implements <see cref="IMongoEntity"/>.</typeparam>
/// <remarks>
/// <para>
/// This class extends <see cref="MongoQuery{TEntity, TKey}"/> with string-based keys and provides a ready-to-use
/// implementation for MongoDB entities that follow the <see cref="IMongoEntity"/> pattern. It automatically
/// handles key extraction and expression generation based on the standardized Id property.
/// </para>
/// <para>
/// The implementation eliminates the need for derived classes to provide custom key handling logic,
/// making it ideal for standard MongoDB entities that use string-based ObjectId identifiers.
/// All query operations inherit the lazy collection initialization and performance optimizations
/// from the base class while providing entity-specific key management.
/// </para>
/// <para>
/// This class is designed to work seamlessly with the repository pattern and dependency injection
/// containers, providing a complete query solution for MongoDB entities without requiring
/// additional implementation effort.
/// </para>
/// </remarks>
public class MongoEntityQuery<TEntity> : MongoQuery<TEntity, string>, IMongoEntityQuery<TEntity>
    where TEntity : class, IMongoEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MongoEntityQuery{TEntity}"/> class with the specified MongoDB database.
    /// </summary>
    /// <param name="mongoDatabase">The <see cref="IMongoDatabase"/> instance used for query operations on the entity collection.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="mongoDatabase"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// The constructor inherits the lazy collection initialization behavior from the base class,
    /// ensuring optimal startup performance and flexible configuration patterns. The database
    /// instance should be properly configured with connection settings and authentication.
    /// </para>
    /// <para>
    /// This constructor is typically called by dependency injection containers during service
    /// resolution, where the database instance is configured according to application requirements.
    /// </para>
    /// </remarks>
    public MongoEntityQuery(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }

    /// <summary>
    /// Extracts the string-based identifier from the specified MongoDB entity.
    /// </summary>
    /// <param name="entity">The MongoDB entity to extract the identifier from.</param>
    /// <returns>The string identifier from the entity's Id property.</returns>
    /// <remarks>
    /// <para>
    /// This implementation provides automatic key extraction for entities implementing <see cref="IMongoEntity"/>,
    /// for custom key extraction logic in most standard MongoDB entity scenarios.
    /// </para>
    /// <para>
    /// The returned identifier is used internally by repository operations for update, delete,
    /// and key-based query operations, ensuring consistent entity identification across all operations.
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
    /// creating expressions that compare the entity's Id property with the provided key value. This supports
    /// efficient key-based queries using MongoDB's native filtering capabilities.
    /// </para>
    /// <para>
    /// The generated expression is optimized for MongoDB query translation and leverages any indexes
    /// created on the Id field for optimal query performance. This eliminates the need for custom
    /// expression logic in standard MongoDB entity scenarios.
    /// </para>
    /// </remarks>
    protected override Expression<Func<TEntity, bool>> KeyExpression(string key)
    {
        return entity => entity.Id == key;
    }
}

/// <summary>
/// Provides a MongoDB query implementation with connection discrimination support for multi-database scenarios.
/// </summary>
/// <typeparam name="TDiscriminator">The type used to discriminate between different MongoDB database connections.</typeparam>
/// <typeparam name="TEntity">The type of the MongoDB entity that implements <see cref="IMongoEntity"/>.</typeparam>
/// <remarks>
/// <para>
/// This class extends <see cref="MongoEntityQuery{TEntity}"/> to support scenarios where multiple MongoDB
/// database connections need to be distinguished using discriminator types. It leverages the
/// <see cref="MongoDiscriminator{TDiscriminator}"/> pattern to provide type-safe dependency injection
/// for multi-tenant applications or microservices architectures.
/// </para>
/// <para>
/// The discriminator-based approach enables the same entity types to be used across different database
/// contexts while maintaining clean separation and type safety. This is particularly useful in scenarios
/// such as tenant-specific databases, read/write replica separation, or domain-specific data contexts.
/// </para>
/// </remarks>
public class MongoEntityQuery<TDiscriminator, TEntity>(MongoDiscriminator<TDiscriminator> mongoDiscriminator)
    : MongoEntityQuery<TEntity>(mongoDiscriminator.MongoDatabase), IMongoEntityQuery<TDiscriminator, TEntity>
    where TEntity : class, IMongoEntity;

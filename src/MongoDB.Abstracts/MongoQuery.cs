// Ignore Spelling: Mongo

using System.Diagnostics;
using System.Linq.Expressions;

using MongoDB.Driver;

namespace MongoDB.Abstracts;

/// <summary>
/// Provides a base implementation for MongoDB query operations with lazy collection initialization and extensible design.
/// </summary>
/// <typeparam name="TEntity">The type of the MongoDB entity to query.</typeparam>
/// <typeparam name="TKey">The type of the entity's primary key identifier.</typeparam>
/// <remarks>
/// <para>
/// This abstract class implements the <see cref="IMongoQuery{TEntity, TKey}"/> interface and provides a complete
/// foundation for MongoDB query operations. It includes lazy collection initialization, proper async/await patterns,
/// and extensible hooks for customizing collection creation and index management.
/// </para>
/// <para>
/// The class uses lazy initialization for the MongoDB collection to defer database connection until the first
/// query operation, improving application startup performance and allowing for dynamic configuration scenarios.
/// </para>
/// <para>
/// Derived classes must implement abstract methods for key extraction and expression generation, enabling
/// type-safe query operations while maintaining flexibility for different entity key strategies.
/// </para>
/// </remarks>
public abstract class MongoQuery<TEntity, TKey> : IMongoQuery<TEntity, TKey>
    where TEntity : class
{
    private readonly Lazy<IMongoCollection<TEntity>> _collection;
    private readonly IMongoDatabase _mongoDatabase;

    /// <summary>
    /// Initializes a new instance of the <see cref="MongoQuery{TEntity, TKey}"/> class with the specified MongoDB database.
    /// </summary>
    /// <param name="mongoDatabase">The MongoDB database instance used for collection operations.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="mongoDatabase"/> is <see langword="null" />.</exception>
    /// <remarks>
    /// <para>
    /// The constructor initializes lazy collection creation to defer database operations until the first query.
    /// This approach improves startup performance and allows for more flexible configuration patterns.
    /// </para>
    /// <para>
    /// The provided database instance should be properly configured with connection strings, authentication,
    /// and other settings before being passed to this constructor.
    /// </para>
    /// </remarks>
    protected MongoQuery(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase ?? throw new ArgumentNullException(nameof(mongoDatabase));
        _collection = new Lazy<IMongoCollection<TEntity>>(CreateCollection);
    }

    /// <summary>
    /// Gets the MongoDB collection instance used for executing queries.
    /// </summary>
    /// <value>
    /// The <see cref="IMongoCollection{TEntity}"/> that provides access to MongoDB operations for the entity type.
    /// </value>
    /// <remarks>
    /// <para>
    /// This property uses lazy initialization to create the collection on first access, which includes
    /// collection name resolution, index creation, and any custom configuration applied by derived classes.
    /// </para>
    /// <para>
    /// The collection instance is cached after first access for optimal performance in subsequent operations.
    /// </para>
    /// </remarks>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IMongoCollection<TEntity> Collection => _collection.Value;

    /// <summary>
    /// Finds a single entity by its unique identifier using the derived class key strategy.
    /// </summary>
    /// <param name="key">The unique identifier of the entity to find.</param>
    /// <returns>The entity with the specified identifier if found; otherwise, <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// This implementation uses the abstract <see cref="KeyExpression(TKey)"/> method to generate the appropriate
    /// filter expression for the entity type, ensuring type-safe key-based queries.
    /// </remarks>
    public TEntity? Find(TKey key)
    {
        if (key is null)
            throw new ArgumentNullException(nameof(key));

        var filter = KeyExpression(key);

        return Collection
            .Find(filter)
            .FirstOrDefault();
    }

    /// <summary>
    /// Asynchronously finds a single entity by its unique identifier using the derived class key strategy.
    /// </summary>
    /// <param name="key">The unique identifier of the entity to find.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the entity with the specified
    /// identifier if found; otherwise, <see langword="null"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// This implementation uses proper async/await patterns with ConfigureAwait(false) for optimal performance
    /// in library scenarios and uses the abstract <see cref="KeyExpression(TKey)"/> method for type-safe queries.
    /// </remarks>
    public async Task<TEntity?> FindAsync(TKey key, CancellationToken cancellationToken = default)
    {
        if (key is null)
            throw new ArgumentNullException(nameof(key));

        var filter = KeyExpression(key);

        var result = await Collection
            .Find(filter)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return result;
    }

    /// <summary>
    /// Finds the first entity that matches the specified criteria expression.
    /// </summary>
    /// <param name="criteria">A lambda expression that defines the search criteria.</param>
    /// <returns>The first entity that matches the criteria if found; otherwise, <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="criteria"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// This method provides direct criteria-based querying using MongoDB's native filter expressions,
    /// ensuring optimal query performance and proper translation to MongoDB query syntax.
    /// </remarks>
    public TEntity? FindOne(Expression<Func<TEntity, bool>> criteria)
    {
        if (criteria is null)
            throw new ArgumentNullException(nameof(criteria));

        return Collection
            .Find(criteria)
            .FirstOrDefault();
    }

    /// <summary>
    /// Asynchronously finds the first entity that matches the specified criteria expression.
    /// </summary>
    /// <param name="criteria">A lambda expression that defines the search criteria.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the first entity that matches
    /// the criteria if found; otherwise, <see langword="null"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="criteria"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// This implementation uses proper async/await patterns with ConfigureAwait(false) and provides
    /// criteria-based querying with optimal MongoDB query translation.
    /// </remarks>
    public async Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default)
    {
        if (criteria is null)
            throw new ArgumentNullException(nameof(criteria));

        var result = await Collection
            .Find(criteria)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return result;
    }

    /// <summary>
    /// Finds all entities that match the specified criteria expression as a queryable interface.
    /// </summary>
    /// <param name="criteria">A lambda expression that defines the search criteria.</param>
    /// <returns>An <see cref="IQueryable{TEntity}"/> containing all entities that match the criteria.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="criteria"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// This method returns a queryable that supports further LINQ composition and deferred execution,
    /// allowing for complex query building while maintaining optimal MongoDB query translation.
    /// </remarks>
    public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria)
    {
        if (criteria is null)
            throw new ArgumentNullException(nameof(criteria));

        return Collection
            .AsQueryable()
            .Where(criteria);
    }

    /// <summary>
    /// Asynchronously finds all entities that match the specified criteria expression.
    /// </summary>
    /// <param name="criteria">A lambda expression that defines the search criteria.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a read-only list of all
    /// entities that match the criteria.
    /// </returns>
    /// <remarks>
    /// This method immediately executes the query and materializes the results, providing a concrete
    /// list for scenarios where deferred execution is not desired.
    /// </remarks>
    public async Task<IReadOnlyList<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default)
    {
        var results = await Collection
            .Find(criteria)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return results;
    }

    /// <summary>
    /// Gets all entities in the collection as a queryable interface for LINQ-based operations.
    /// </summary>
    /// <returns>An <see cref="IQueryable{TEntity}"/> that provides LINQ query capabilities over the entire collection.</returns>
    /// <remarks>
    /// This method provides unrestricted access to the collection through LINQ, enabling complex query
    /// composition and deferred execution with proper MongoDB query translation.
    /// </remarks>
    public IQueryable<TEntity> All()
    {
        return Collection.AsQueryable();
    }

    /// <summary>
    /// Gets the total number of documents in the MongoDB collection.
    /// </summary>
    /// <returns>The total count of documents in the collection.</returns>
    /// <remarks>
    /// This method uses MongoDB's efficient CountDocuments operation with an empty filter to get
    /// the total collection size without transferring document data.
    /// </remarks>
    public long Count()
    {
        return Collection.CountDocuments(FilterDefinition<TEntity>.Empty);
    }

    /// <summary>
    /// Asynchronously gets the total number of documents in the MongoDB collection.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation with the total count of documents.</returns>
    /// <remarks>
    /// This method uses MongoDB's efficient CountDocumentsAsync operation for optimal performance
    /// in asynchronous contexts without blocking the calling thread.
    /// </remarks>
    public Task<long> CountAsync(CancellationToken cancellationToken = default)
    {
        return Collection.CountDocumentsAsync(FilterDefinition<TEntity>.Empty, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Gets the number of documents in the collection that match the specified criteria.
    /// </summary>
    /// <param name="criteria">A lambda expression that defines the counting criteria.</param>
    /// <returns>The number of documents that match the specified criteria.</returns>
    /// <remarks>
    /// This method uses MongoDB's efficient CountDocuments operation with the provided filter
    /// to count matching documents without transferring document data.
    /// </remarks>
    public long Count(Expression<Func<TEntity, bool>> criteria)
    {
        return Collection.CountDocuments(criteria);
    }

    /// <summary>
    /// Asynchronously gets the number of documents in the collection that match the specified criteria.
    /// </summary>
    /// <param name="criteria">A lambda expression that defines the counting criteria.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation with the count of matching documents.</returns>
    /// <remarks>
    /// This method uses MongoDB's efficient CountDocumentsAsync operation for optimal performance
    /// in asynchronous contexts with criteria-based filtering.
    /// </remarks>
    public Task<long> CountAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default)
    {
        return Collection.CountDocumentsAsync(criteria, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Extracts the key value from the specified entity instance.
    /// </summary>
    /// <param name="entity">The entity instance to extract the key from.</param>
    /// <returns>The key value for the specified entity.</returns>
    /// <remarks>
    /// <para>
    /// This abstract method must be implemented by derived classes to define how entity keys are extracted.
    /// The implementation should return the appropriate key value based on the entity's identifier property.
    /// </para>
    /// <para>
    /// This method is used internally for operations that need to determine an entity's key value,
    /// such as update and delete operations that work with entity instances.
    /// </para>
    /// </remarks>
    protected abstract TKey EntityKey(TEntity entity);

    /// <summary>
    /// Creates a filter expression that matches entities with the specified key value.
    /// </summary>
    /// <param name="key">The key value to create the filter expression for.</param>
    /// <returns>A lambda expression that filters entities by the specified key value.</returns>
    /// <remarks>
    /// <para>
    /// This abstract method must be implemented by derived classes to define how key-based filtering
    /// is performed. The returned expression should compare the entity's key property with the provided key value.
    /// </para>
    /// <para>
    /// The expression is used by find operations to locate entities by their unique identifiers,
    /// ensuring type-safe and efficient key-based queries.
    /// </para>
    /// </remarks>
    protected abstract Expression<Func<TEntity, bool>> KeyExpression(TKey key);

    /// <summary>
    /// Determines the MongoDB collection name for the entity type.
    /// </summary>
    /// <returns>The name of the MongoDB collection to use for the entity type.</returns>
    /// <remarks>
    /// <para>
    /// The default implementation returns the entity type name as the collection name.
    /// Derived classes can override this method to provide custom collection naming strategies,
    /// such as pluralization, prefixes, or configuration-based naming.
    /// </para>
    /// <para>
    /// This method is called during collection initialization and the result is used throughout
    /// the lifetime of the query instance for all MongoDB operations.
    /// </para>
    /// </remarks>
    protected virtual string CollectionName()
    {
        return typeof(TEntity).Name;
    }

    /// <summary>
    /// Creates and configures the MongoDB collection instance with indexes and custom settings.
    /// </summary>
    /// <returns>A configured <see cref="IMongoCollection{TEntity}"/> ready for query operations.</returns>
    /// <remarks>
    /// <para>
    /// This method orchestrates the complete collection creation process, including name resolution,
    /// collection retrieval from the database, and index creation. It provides the primary extension
    /// point for derived classes to customize collection behavior.
    /// </para>
    /// <para>
    /// The method is called once during lazy initialization and the result is cached for subsequent use.
    /// Override this method to implement custom collection configuration or initialization logic.
    /// </para>
    /// </remarks>
    protected virtual IMongoCollection<TEntity> CreateCollection()
    {
        var database = _mongoDatabase;

        string collectionName = CollectionName();
        var mongoCollection = CreateCollection(database, collectionName);

        EnsureIndexes(mongoCollection);

        return mongoCollection;
    }

    /// <summary>
    /// Creates the MongoDB collection instance from the database using the specified collection name.
    /// </summary>
    /// <param name="database">The MongoDB database instance to get the collection from.</param>
    /// <param name="collectionName">The name of the collection to retrieve.</param>
    /// <returns>The <see cref="IMongoCollection{TEntity}"/> instance for the specified collection name.</returns>
    /// <remarks>
    /// <para>
    /// This method provides the core collection retrieval logic using the MongoDB driver's
    /// GetCollection method. Override this method to apply custom collection settings such as
    /// read/write concerns, serialization options, or collection-specific configurations.
    /// </para>
    /// <para>
    /// The default implementation uses standard collection retrieval without additional settings.
    /// </para>
    /// </remarks>
    protected virtual IMongoCollection<TEntity> CreateCollection(IMongoDatabase database, string collectionName)
    {
        return database.GetCollection<TEntity>(collectionName);
    }

    /// <summary>
    /// Creates or ensures the existence of required indexes on the MongoDB collection.
    /// </summary>
    /// <param name="mongoCollection">The MongoDB collection to create indexes on.</param>
    /// <remarks>
    /// <para>
    /// This virtual method provides an extension point for derived classes to define and create
    /// indexes that optimize query performance for their specific entity types and access patterns.
    /// </para>
    /// <para>
    /// The default implementation does nothing. Override this method to create indexes using
    /// the MongoDB driver's index creation APIs, ensuring optimal query performance for your entities.
    /// </para>
    /// <para>
    /// Index creation is performed during collection initialization and should be idempotent
    /// to handle multiple application startups gracefully.
    /// </para>
    /// </remarks>
    protected virtual void EnsureIndexes(IMongoCollection<TEntity> mongoCollection)
    {

    }
}

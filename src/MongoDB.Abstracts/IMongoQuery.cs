using System.Linq.Expressions;

using MongoDB.Driver;

namespace MongoDB.Abstracts;

/// <summary>
/// Defines a contract for common MongoDB query operations with generic entity and key types.
/// </summary>
/// <typeparam name="TEntity">The type of the MongoDB entity to query.</typeparam>
/// <typeparam name="TKey">The type of the entity's primary key identifier.</typeparam>
/// <remarks>
/// <para>
/// This interface provides a comprehensive set of query operations for MongoDB collections,
/// including synchronous and asynchronous methods for finding, counting, and retrieving entities.
/// It serves as the foundation for more specialized query interfaces.
/// </para>
/// <para>
/// The interface supports both LINQ-style querying through <see cref="IQueryable{T}"/> and
/// expression-based filtering for optimal MongoDB query performance.
/// </para>
/// </remarks>
public interface IMongoQuery<TEntity, in TKey>
    where TEntity : class
{
    /// <summary>
    /// Gets the underlying <see cref="IMongoCollection{TEntity}"/> used for executing queries.
    /// </summary>
    /// <value>
    /// The MongoDB collection instance that provides direct access to MongoDB driver operations.
    /// </value>
    /// <remarks>
    /// This property exposes the native MongoDB collection for advanced operations that may
    /// not be covered by the interface methods, such as aggregation pipelines, bulk operations,
    /// or MongoDB-specific features.
    /// </remarks>
    IMongoCollection<TEntity> Collection { get; }

    /// <summary>
    /// Gets all entities in the collection as an <see cref="IQueryable{TEntity}"/> for LINQ-based queries.
    /// </summary>
    /// <returns>
    /// An <see cref="IQueryable{TEntity}"/> that can be used with LINQ operators to build complex queries.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method returns a queryable interface that allows for LINQ-based query composition.
    /// The queries are translated to MongoDB aggregation pipelines when executed.
    /// </para>
    /// <para>
    /// Use this method when you need to build complex queries using LINQ syntax, but be aware
    /// that not all LINQ operations may be supported by the MongoDB LINQ provider.
    /// </para>
    /// </remarks>
    IQueryable<TEntity> All();

    /// <summary>
    /// Finds a single entity by its unique identifier.
    /// </summary>
    /// <param name="key">The unique identifier of the entity to find.</param>
    /// <returns>
    /// The entity with the specified identifier if found; otherwise, <see langword="null"/>.
    /// </returns>
    /// <remarks>
    /// This is a synchronous operation that blocks the calling thread. Consider using
    /// <see cref="FindAsync(TKey, CancellationToken)"/> for better performance in async contexts.
    /// </remarks>
    TEntity? Find(TKey key);

    /// <summary>
    /// Asynchronously finds a single entity by its unique identifier.
    /// </summary>
    /// <param name="key">The unique identifier of the entity to find.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the entity
    /// with the specified identifier if found; otherwise, <see langword="null"/>.
    /// </returns>
    /// <remarks>
    /// This is the preferred method for finding entities by ID in asynchronous contexts
    /// as it doesn't block the calling thread.
    /// </remarks>
    Task<TEntity?> FindAsync(TKey key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds the first entity that matches the specified criteria expression.
    /// </summary>
    /// <param name="criteria">A lambda expression that defines the search criteria.</param>
    /// <returns>
    /// The first entity that matches the criteria if found; otherwise, <see langword="null"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method returns only the first matching entity. If multiple entities match the criteria,
    /// only the first one encountered is returned. The order depends on the natural order in MongoDB.
    /// </para>
    /// <para>
    /// This is a synchronous operation. Consider using <see cref="FindOneAsync(Expression{Func{TEntity, bool}}, CancellationToken)"/>
    /// for better performance in async contexts.
    /// </para>
    /// </remarks>
    TEntity? FindOne(Expression<Func<TEntity, bool>> criteria);

    /// <summary>
    /// Asynchronously finds the first entity that matches the specified criteria expression.
    /// </summary>
    /// <param name="criteria">A lambda expression that defines the search criteria.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the first entity
    /// that matches the criteria if found; otherwise, <see langword="null"/>.
    /// </returns>
    /// <remarks>
    /// This method returns only the first matching entity. If multiple entities match the criteria,
    /// only the first one encountered is returned. The order depends on the natural order in MongoDB.
    /// </remarks>
    Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds all entities that match the specified criteria expression as an <see cref="IQueryable{TEntity}"/>.
    /// </summary>
    /// <param name="criteria">A lambda expression that defines the search criteria.</param>
    /// <returns>
    /// An <see cref="IQueryable{TEntity}"/> containing all entities that match the criteria.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method returns a queryable that can be further composed with additional LINQ operations
    /// before execution. The query is not executed until enumerated (e.g., ToList(), foreach).
    /// </para>
    /// <para>
    /// For immediate execution with a materialized list, consider using <see cref="FindAllAsync(Expression{Func{TEntity, bool}}, CancellationToken)"/>.
    /// </para>
    /// </remarks>
    IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria);

    /// <summary>
    /// Asynchronously finds all entities that match the specified criteria expression.
    /// </summary>
    /// <param name="criteria">A lambda expression that defines the search criteria.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a read-only list
    /// of all entities that match the criteria.
    /// </returns>
    /// <remarks>
    /// This method immediately executes the query and returns a materialized list of results.
    /// For deferred execution and additional LINQ composition, use <see cref="FindAll(Expression{Func{TEntity, bool}})"/>.
    /// </remarks>
    Task<IReadOnlyList<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total number of entities in the collection.
    /// </summary>
    /// <returns>
    /// The total number of entities in the MongoDB collection.
    /// </returns>
    /// <remarks>
    /// This is a synchronous operation that blocks the calling thread. Consider using
    /// <see cref="CountAsync(CancellationToken)"/> for better performance in async contexts.
    /// </remarks>
    long Count();

    /// <summary>
    /// Asynchronously gets the total number of entities in the collection.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the total
    /// number of entities in the MongoDB collection.
    /// </returns>
    /// <remarks>
    /// This is the preferred method for counting entities in asynchronous contexts
    /// as it doesn't block the calling thread.
    /// </remarks>
    Task<long> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the number of entities in the collection that match the specified criteria.
    /// </summary>
    /// <param name="criteria">A lambda expression that defines the search criteria.</param>
    /// <returns>
    /// The number of entities that match the specified criteria.
    /// </returns>
    /// <remarks>
    /// This is a synchronous operation that blocks the calling thread. Consider using
    /// <see cref="CountAsync(Expression{Func{TEntity, bool}}, CancellationToken)"/> for better performance in async contexts.
    /// </remarks>
    long Count(Expression<Func<TEntity, bool>> criteria);

    /// <summary>
    /// Asynchronously gets the number of entities in the collection that match the specified criteria.
    /// </summary>
    /// <param name="criteria">A lambda expression that defines the search criteria.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the number
    /// of entities that match the specified criteria.
    /// </returns>
    /// <remarks>
    /// This is the preferred method for counting entities with criteria in asynchronous contexts
    /// as it doesn't block the calling thread.
    /// </remarks>
    Task<long> CountAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default);
}

/// <summary>
/// Defines a contract for MongoDB query operations with connection discrimination support.
/// </summary>
/// <typeparam name="TDiscriminator">The type used to discriminate between different MongoDB connections or contexts. This type serves as a marker to distinguish between multiple registrations of the same entity type.</typeparam>
/// <typeparam name="TEntity">The type of the MongoDB entity to query. Must be a reference type.</typeparam>
/// <typeparam name="TKey">The type of the entity's primary key identifier.</typeparam>
/// <remarks>
/// <para>
/// This interface extends <see cref="IMongoQuery{TEntity, TKey}"/> to support scenarios where multiple
/// MongoDB connections, database contexts, or data sources need to be distinguished using a discriminator type.
/// This is particularly useful in multi-tenant applications, multi-database scenarios, or when working with
/// different connection strings for the same entity type.
/// </para>
/// <para>
/// The discriminator type parameter acts as a compile-time marker that allows dependency injection containers
/// to register and resolve multiple instances of query services for the same entity type but different contexts.
/// Common discriminator types include enums, marker classes, or string constants wrapped in types.
/// </para>
/// <para>
/// This interface inherits all query operations from <see cref="IMongoQuery{TEntity, TKey}"/> and does not
/// add additional members. The discriminator only affects service registration and resolution.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Using an enum as discriminator for different database contexts
/// public enum DatabaseContext { Primary, Secondary, Archive }
///
/// // Register different instances for the same entity type
/// services.AddScoped&lt;IMongoQuery&lt;DatabaseContext.Primary, User, string&gt;&gt;, UserQuery&gt;();
/// services.AddScoped&lt;IMongoQuery&lt;DatabaseContext.Secondary, User, string&gt;&gt;, UserQuery&gt;();
///
/// // Resolve specific instance in a controller
/// public class UserController : Controller
/// {
///     private readonly IMongoQuery&lt;DatabaseContext.Primary, User, string&gt; _primaryQuery;
///
///     public UserController(IMongoQuery&lt;DatabaseContext.Primary, User, string&gt; primaryQuery)
///     {
///         _primaryQuery = primaryQuery;
///     }
/// }
/// </code>
/// </example>
public interface IMongoQuery<TDiscriminator, TEntity, TKey> : IMongoQuery<TEntity, TKey>
    where TEntity : class;

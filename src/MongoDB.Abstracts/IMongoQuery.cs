// Ignore Spelling: Mongo

using System.Linq.Expressions;

using MongoDB.Driver;

namespace MongoDB.Abstracts;

/// <summary>
/// An <c>interface</c> for common MongoDB query operations.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TKey">The type of the key.</typeparam>
public interface IMongoQuery<TEntity, in TKey>
    where TEntity : class
{
    /// <summary>
    /// Gets the underling <see cref="IMongoCollection{TEntity}"/> used for queries.
    /// </summary>
    /// <value>
    /// The underling <see cref="IMongoCollection{TEntity}"/>.
    /// </value>
    IMongoCollection<TEntity> Collection { get; }

    /// <summary>
    /// Get all entities as an <see cref="IQueryable{TEntity}"/>.
    /// </summary>
    /// <returns>An <see cref="IQueryable{TEntity}"/> of entities.</returns>
    IQueryable<TEntity> All();


    /// <summary>
    /// Find the entity with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key of the entity to find.</param>
    /// <returns>An instance of TEntity that has the specified identifier if found, otherwise null.</returns>
    TEntity? Find(TKey key);

    /// <summary>
    /// Find the entity with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key of the entity to find.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An instance of TEnity that has the specified identifier if found, otherwise null.</returns>
    Task<TEntity?> FindAsync(TKey key, CancellationToken cancellationToken = default);


    /// <summary>
    /// Find the first entity using the specified <paramref name="criteria"/> expression.
    /// </summary>
    /// <param name="criteria">The criteria expression.</param>
    /// <returns>
    /// An instance of TEntity that matches the criteria if found, otherwise null.
    /// </returns>
    TEntity? FindOne(Expression<Func<TEntity, bool>> criteria);

    /// <summary>
    /// Find the first entity using the specified <paramref name="criteria"/> expression.
    /// </summary>
    /// <param name="criteria">The criteria expression.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// An instance of TEnity that matches the criteria if found, otherwise null.
    /// </returns>
    Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default);


    /// <summary>
    /// Find all entities using the specified <paramref name="criteria"/> expression.
    /// </summary>
    /// <param name="criteria">The criteria expression.</param>
    /// <returns></returns>
    IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria);

    /// <summary>
    /// Find all entities using the specified <paramref name="criteria"/> expression.
    /// </summary>
    /// <param name="criteria">The criteria expression.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<IReadOnlyList<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default);


    /// <summary>
    /// Gets the number of entities in the data store.
    /// </summary>
    /// <returns>The number of entities in the data store.</returns>
    long Count();

    /// <summary>
    /// Gets the number of entities in the collection.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of entities in the data store.</returns>
    Task<long> CountAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// Gets the number of entities in the data store with the specified <paramref name="criteria"/>.
    /// </summary>
    /// <returns>The number of entities in the data store specified criteria.</returns>
    long Count(Expression<Func<TEntity, bool>> criteria);

    /// <summary>
    /// Gets the number of entities in the collection with the specified <paramref name="criteria"/>.
    /// </summary>
    /// <param name="criteria">The criteria expression.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of entities in the data store.</returns>
    Task<long> CountAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default);
}

// Ignore Spelling: Mongo

using System.Linq.Expressions;

namespace MongoDB.Abstracts;

/// <summary>
/// An <c>interface</c> for common MongoDB data operations.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TKey">The type of the key.</typeparam>
public interface IMongoRepository<TEntity, in TKey> : IMongoQuery<TEntity, TKey>
    where TEntity : class
{
    /// <summary>
    /// Inserts the specified <paramref name="entity"/> to the underlying data repository.
    /// </summary>
    /// <param name="entity">The entity to be inserted.</param>
    /// <returns>The entity that was inserted.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
    TEntity Insert(TEntity entity);

    /// <summary>
    /// Inserts the specified <paramref name="entity" /> to the underlying data repository.
    /// </summary>
    /// <param name="entity">The entity to be inserted.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// The entity that was inserted.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Inserts the specified <paramref name="entities"/> in a batch operation to the underlying data repository.
    /// </summary>
    /// <param name="entities">The entities to be inserted.</param>
    /// <exception cref="ArgumentNullException"><paramref name="entities"/> is <see langword="null" />.</exception>
    void InsertBatch(IEnumerable<TEntity> entities);


    /// <summary>
    /// Updates the specified <paramref name="entity"/> in the underlying data repository.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    /// <returns>The entity that was updated.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
    TEntity Update(TEntity entity);

    /// <summary>
    /// Updates the specified <paramref name="entity" /> in the underlying data repository.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// The entity that was updated.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);


    /// <summary>
    /// Saves the specified <paramref name="entity"/> in the underlying data repository by inserting if doesn't exist, or updating if it does.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    /// <returns>The entity that was updated.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
    TEntity Upsert(TEntity entity);

    /// <summary>
    /// Saves the specified <paramref name="entity" /> in the underlying data repository by inserting if doesn't exist, or updating if it does.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// The entity that was updated.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
    Task<TEntity> UpsertAsync(TEntity entity, CancellationToken cancellationToken = default);


    /// <summary>
    /// Deletes an entity with the specified <paramref name="key"/> from the underlying data repository.
    /// </summary>
    /// <param name="key">The key of the entity to delete.</param>
    /// <returns>The number of documents deleted</returns>
    long Delete(TKey key);

    /// <summary>
    /// Deletes an entity with the specified <paramref name="id" /> from the underlying data repository.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of documents deleted</returns>
    /// <exception cref="ArgumentNullException"><paramref name="id"/> is <see langword="null" />.</exception>
    Task<long> DeleteAsync(TKey id, CancellationToken cancellationToken = default);


    /// <summary>
    /// Deletes the specified <paramref name="entity"/> from the underlying data repository.
    /// </summary>
    /// <param name="entity">The entity to be deleted.</param>
    /// <returns>The number of documents deleted</returns>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
    long Delete(TEntity entity);

    /// <summary>
    /// Deletes the specified <paramref name="entity" /> from the underlying data repository.
    /// </summary>
    /// <param name="entity">The entity to be deleted.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of documents deleted</returns>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
    Task<long> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);


    /// <summary>
    /// Deletes all from collection that meet the specified <paramref name="criteria" />.
    /// </summary>
    /// <param name="criteria">The criteria.</param>
    /// <returns></returns>
    /// <returns>The number of documents deleted</returns>
    /// <exception cref="ArgumentNullException"><paramref name="criteria"/> is <see langword="null" />.</exception>
    long DeleteAll(Expression<Func<TEntity, bool>> criteria);

    /// <summary>
    /// Deletes all from collection that meet the specified <paramref name="criteria" />.
    /// </summary>
    /// <param name="criteria">The criteria expression.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of documents deleted</returns>
    /// <exception cref="ArgumentNullException"><paramref name="criteria"/> is <see langword="null" />.</exception>
    Task<long> DeleteAllAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default);
}

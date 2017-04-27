using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDB.Abstracts
{
    /// <summary>
    /// An <c>interface</c> for common MongoDB data operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IMongoRepository<TEntity, TKey> : IEntityRepository<TEntity, TKey>, IMongoQuery<TEntity, TKey>
        where TEntity : class
    {
        /// <summary>
        /// Inserts the specified <paramref name="entity" /> to the underlying data repository.
        /// </summary>
        /// <param name="entity">The entity to be inserted.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The entity that was inserted.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">entity</exception>
        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Updates the specified <paramref name="entity" /> in the underlying data repository.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The entity that was updated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">entity</exception>
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Saves the specified <paramref name="entity" /> in the underlying data repository by inserting if doesn't exist, or updating if it does.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The entity that was updated.
        /// </returns>
        Task<TEntity> SaveAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes an entity with the specified <paramref name="id" /> from the underlying data repository.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of documents deleted</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <see langword="null" />.</exception>
        Task<long> DeleteAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes the specified <paramref name="entity" /> from the underlying data repository.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of documents deleted</returns>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
        Task<long> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes all documents from MongoDB collection.
        /// </summary>
        /// <returns></returns>
        long DeleteAll();

        /// <summary>
        /// Deletes all documents from MongoDB collection.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<long> DeleteAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes all from collection that meet the specified <paramref name="criteria" />.
        /// </summary>
        /// <param name="criteria">The criteria expression.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<long> DeleteAllAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default(CancellationToken));
    }
}
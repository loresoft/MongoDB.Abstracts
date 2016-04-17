using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoDB.Repository
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
        /// <returns>
        /// The entity that was inserted.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">entity</exception>
        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// Updates the specified <paramref name="entity" /> in the underlying data repository.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>
        /// The entity that was updated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">entity</exception>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Saves the specified <paramref name="entity" /> in the underlying data repository by inserting if doesn't exist, or updating if it does.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>
        /// The entity that was updated.
        /// </returns>
        Task<TEntity> SaveAsync(TEntity entity);

        /// <summary>
        /// Deletes an entity with the specified <paramref name="id" /> from the underlying data repository.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The number of documents deleted</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <see langword="null" />.</exception>
        Task<long> DeleteAsync(TKey id);

        /// <summary>
        /// Deletes the specified <paramref name="entity" /> from the underlying data repository.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        /// <returns>The number of documents deleted</returns>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
        Task<long> DeleteAsync(TEntity entity);

        /// <summary>
        /// Deletes all documents from MongoDB collection.
        /// </summary>
        /// <returns></returns>
        long DeleteAll();

        /// <summary>
        /// Deletes all documents from MongoDB collection.
        /// </summary>
        /// <returns></returns>
        Task<long> DeleteAllAsync();

        /// <summary>
        /// Deletes all from collection that meet the specified <paramref name="criteria" />.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        Task<long> DeleteAllAsync(Expression<Func<TEntity, bool>> criteria);
    }
}
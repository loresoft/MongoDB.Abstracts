using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.Abstracts
{
    /// <summary>
    /// An <c>interface</c> for common MongoDB query operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IMongoQuery<TEntity, TKey> : IEntityQuery<TEntity, TKey>
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
        /// Find the entity with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the entity to find.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An instance of TEnity that has the specified identifier if found, otherwise null.</returns>
        Task<TEntity> FindAsync(TKey key, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Find the first entity using the specified <paramref name="criteria"/> expression.
        /// </summary>
        /// <param name="criteria">The criteria expression.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// An instance of TEnity that matches the criteria if found, otherwise null.
        /// </returns>
        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Find all entities using the specified <paramref name="criteria"/> expression.
        /// </summary>
        /// <param name="criteria">The criteria expression.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
        /// Gets the number of entities in the collection.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<long> CountAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the number of entities in the collection with the specified <paramref name="criteria"/>.
        /// </summary>
        /// <param name="criteria">The criteria expression.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<long> CountAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default(CancellationToken));
    }
}
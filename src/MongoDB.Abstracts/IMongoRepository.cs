// Ignore Spelling: Mongo

using System.Linq.Expressions;

namespace MongoDB.Abstracts;

/// <summary>
/// Defines a contract for comprehensive MongoDB data operations with generic entity and key types.
/// </summary>
/// <typeparam name="TEntity">The type of the MongoDB entity to manage.</typeparam>
/// <typeparam name="TKey">The type of the entity's primary key identifier.</typeparam>
/// <remarks>
/// <para>
/// This interface extends <see cref="IMongoQuery{TEntity, TKey}"/> to provide a complete repository pattern
/// implementation for MongoDB operations. It includes all CRUD operations (Create, Read, Update, Delete)
/// along with specialized operations like upsert and batch insert.
/// </para>
/// <para>
/// The repository pattern encapsulates the logic needed to access data sources, centralizing common
/// data access functionality and providing better maintainability while decoupling infrastructure
/// or technology used to access databases from the domain model layer.
/// </para>
/// <para>
/// All operations support both synchronous and asynchronous execution patterns, with async methods
/// being preferred for better application scalability and responsiveness.
/// </para>
/// </remarks>
public interface IMongoRepository<TEntity, in TKey> : IMongoQuery<TEntity, TKey>
    where TEntity : class
{
    /// <summary>
    /// Inserts a new entity into the MongoDB collection.
    /// </summary>
    /// <param name="entity">The entity to be inserted into the collection.</param>
    /// <returns>The entity that was inserted, potentially with updated properties such as generated identifiers.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
    /// <remarks>
    /// <para>
    /// This is a synchronous operation that blocks the calling thread. The entity may be modified
    /// during insertion (e.g., auto-generated IDs, timestamps). Consider using <see cref="InsertAsync(TEntity, CancellationToken)"/>
    /// for better performance in async contexts.
    /// </para>
    /// <para>
    /// If the entity has an existing identifier that conflicts with a document in the collection,
    /// a duplicate key exception will be thrown.
    /// </para>
    /// </remarks>
    TEntity Insert(TEntity entity);

    /// <summary>
    /// Asynchronously inserts a new entity into the MongoDB collection.
    /// </summary>
    /// <param name="entity">The entity to be inserted into the collection.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the entity that was inserted,
    /// potentially with updated properties such as generated identifiers.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
    /// <remarks>
    /// <para>
    /// This is the preferred method for inserting entities in asynchronous contexts as it doesn't block
    /// the calling thread. The entity may be modified during insertion (e.g., auto-generated IDs, timestamps).
    /// </para>
    /// <para>
    /// If the entity has an existing identifier that conflicts with a document in the collection,
    /// a duplicate key exception will be thrown.
    /// </para>
    /// </remarks>
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Inserts multiple entities into the MongoDB collection in a single batch operation.
    /// </summary>
    /// <param name="entities">The collection of entities to be inserted.</param>
    /// <exception cref="ArgumentNullException"><paramref name="entities"/> is <see langword="null" />.</exception>
    /// <remarks>
    /// <para>
    /// Batch operations are more efficient than individual inserts when working with multiple entities.
    /// This method performs all insertions in a single round-trip to the database, improving performance
    /// and ensuring better consistency.
    /// </para>
    /// <para>
    /// If any entity in the batch has a duplicate key, the entire operation may fail depending on
    /// MongoDB configuration and write concern settings.
    /// </para>
    /// <para>
    /// This is a synchronous operation. Consider implementing an async batch insert for better scalability.
    /// </para>
    /// </remarks>
    void InsertBatch(IEnumerable<TEntity> entities);

    /// <summary>
    /// Updates an existing entity in the MongoDB collection.
    /// </summary>
    /// <param name="entity">The entity with updated values to be saved to the collection.</param>
    /// <returns>The entity that was updated, reflecting any changes made during the update process.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
    /// <remarks>
    /// <para>
    /// This operation replaces the entire document in MongoDB with the provided entity.
    /// The entity must have a valid identifier that exists in the collection.
    /// </para>
    /// <para>
    /// This is a synchronous operation that blocks the calling thread. Consider using
    /// <see cref="UpdateAsync(TEntity, CancellationToken)"/> for better performance in async contexts.
    /// </para>
    /// <para>
    /// If the entity identifier doesn't exist in the collection, the update operation will not affect any documents.
    /// </para>
    /// </remarks>
    TEntity Update(TEntity entity);

    /// <summary>
    /// Asynchronously updates an existing entity in the MongoDB collection.
    /// </summary>
    /// <param name="entity">The entity with updated values to be saved to the collection.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the entity that was updated,
    /// reflecting any changes made during the update process.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
    /// <remarks>
    /// <para>
    /// This is the preferred method for updating entities in asynchronous contexts as it doesn't block
    /// the calling thread. This operation replaces the entire document in MongoDB with the provided entity.
    /// </para>
    /// <para>
    /// The entity must have a valid identifier that exists in the collection. If the entity identifier
    /// doesn't exist in the collection, the update operation will not affect any documents.
    /// </para>
    /// </remarks>
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs an upsert operation - inserts the entity if it doesn't exist, or updates it if it does.
    /// </summary>
    /// <param name="entity">The entity to be inserted or updated in the collection.</param>
    /// <returns>The entity that was inserted or updated, reflecting any changes made during the operation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
    /// <remarks>
    /// <para>
    /// Upsert operations provide atomic insert-or-update functionality. This is particularly useful
    /// when you're not certain whether an entity already exists in the collection.
    /// </para>
    /// <para>
    /// This is a synchronous operation that blocks the calling thread. Consider using
    /// <see cref="UpsertAsync(TEntity, CancellationToken)"/> for better performance in async contexts.
    /// </para>
    /// <para>
    /// The operation uses the entity's identifier to determine whether to insert or update.
    /// </para>
    /// </remarks>
    TEntity Upsert(TEntity entity);

    /// <summary>
    /// Asynchronously performs an upsert operation - inserts the entity if it doesn't exist, or updates it if it does.
    /// </summary>
    /// <param name="entity">The entity to be inserted or updated in the collection.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the entity that was inserted or updated,
    /// reflecting any changes made during the operation.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
    /// <remarks>
    /// <para>
    /// This is the preferred method for upsert operations in asynchronous contexts as it doesn't block
    /// the calling thread. Upsert operations provide atomic insert-or-update functionality.
    /// </para>
    /// <para>
    /// This operation is particularly useful when you're not certain whether an entity already exists
    /// in the collection. The operation uses the entity's identifier to determine whether to insert or update.
    /// </para>
    /// </remarks>
    Task<TEntity> UpsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an entity from the MongoDB collection using its unique identifier.
    /// </summary>
    /// <param name="key">The unique identifier of the entity to delete.</param>
    /// <returns>The number of documents that were deleted from the collection.</returns>
    /// <remarks>
    /// <para>
    /// This is a synchronous operation that blocks the calling thread. Consider using
    /// <see cref="DeleteAsync(TKey, CancellationToken)"/> for better performance in async contexts.
    /// </para>
    /// <para>
    /// Returns 1 if the entity was found and deleted, 0 if no entity with the specified key was found.
    /// </para>
    /// </remarks>
    long Delete(TKey key);

    /// <summary>
    /// Asynchronously deletes an entity from the MongoDB collection using its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the number of documents that were deleted.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="id"/> is <see langword="null" />.</exception>
    /// <remarks>
    /// <para>
    /// This is the preferred method for deleting entities by identifier in asynchronous contexts
    /// as it doesn't block the calling thread.
    /// </para>
    /// <para>
    /// Returns 1 if the entity was found and deleted, 0 if no entity with the specified identifier was found.
    /// </para>
    /// </remarks>
    Task<long> DeleteAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the specified entity from the MongoDB collection.
    /// </summary>
    /// <param name="entity">The entity instance to be deleted from the collection.</param>
    /// <returns>The number of documents that were deleted from the collection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
    /// <remarks>
    /// <para>
    /// This method uses the entity's identifier to locate and delete the corresponding document.
    /// This is a synchronous operation that blocks the calling thread.
    /// </para>
    /// <para>
    /// Consider using <see cref="DeleteAsync(TEntity, CancellationToken)"/> for better performance in async contexts.
    /// Returns 1 if the entity was found and deleted, 0 if no matching entity was found.
    /// </para>
    /// </remarks>
    long Delete(TEntity entity);

    /// <summary>
    /// Asynchronously deletes the specified entity from the MongoDB collection.
    /// </summary>
    /// <param name="entity">The entity instance to be deleted from the collection.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the number of documents that were deleted.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
    /// <remarks>
    /// <para>
    /// This is the preferred method for deleting entities in asynchronous contexts as it doesn't block
    /// the calling thread. This method uses the entity's identifier to locate and delete the corresponding document.
    /// </para>
    /// <para>
    /// Returns 1 if the entity was found and deleted, 0 if no matching entity was found.
    /// </para>
    /// </remarks>
    Task<long> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes all entities from the collection that match the specified criteria.
    /// </summary>
    /// <param name="criteria">A lambda expression that defines the deletion criteria.</param>
    /// <returns>The number of documents that were deleted from the collection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="criteria"/> is <see langword="null" />.</exception>
    /// <remarks>
    /// <para>
    /// This operation can delete multiple documents in a single database operation, making it efficient
    /// for bulk deletions. Use with caution as it can affect many documents at once.
    /// </para>
    /// <para>
    /// This is a synchronous operation that blocks the calling thread. Consider using
    /// <see cref="DeleteAllAsync(Expression{Func{TEntity, bool}}, CancellationToken)"/> for better performance in async contexts.
    /// </para>
    /// <para>
    /// Returns the actual number of documents that matched the criteria and were deleted.
    /// </para>
    /// </remarks>
    long DeleteAll(Expression<Func<TEntity, bool>> criteria);

    /// <summary>
    /// Asynchronously deletes all entities from the collection that match the specified criteria.
    /// </summary>
    /// <param name="criteria">A lambda expression that defines the deletion criteria.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the number of documents that were deleted.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="criteria"/> is <see langword="null" />.</exception>
    /// <remarks>
    /// <para>
    /// This is the preferred method for bulk deletion operations in asynchronous contexts as it doesn't block
    /// the calling thread. This operation can delete multiple documents in a single database operation.
    /// </para>
    /// <para>
    /// Use with caution as it can affect many documents at once. Returns the actual number of documents
    /// that matched the criteria and were deleted.
    /// </para>
    /// </remarks>
    Task<long> DeleteAllAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default);
}

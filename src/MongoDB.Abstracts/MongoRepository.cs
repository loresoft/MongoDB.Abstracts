// Ignore Spelling: Mongo

using System.Linq.Expressions;

using MongoDB.Driver;

namespace MongoDB.Abstracts;

/// <summary>
/// Provides a complete MongoDB repository implementation with CRUD operations and automatic audit timestamp management.
/// </summary>
/// <typeparam name="TEntity">The type of the MongoDB entity to manage.</typeparam>
/// <typeparam name="TKey">The type of the entity's primary key identifier.</typeparam>
/// <remarks>
/// <para>
/// This abstract class extends <see cref="MongoQuery{TEntity, TKey}"/> to provide a full repository pattern
/// implementation for MongoDB operations. It includes all CRUD operations (Create, Read, Update, Delete)
/// with built-in support for automatic audit timestamp management and extensible hooks for custom behavior.
/// </para>
/// <para>
/// The repository automatically manages audit timestamps for entities implementing <see cref="IMongoEntity"/>,
/// setting Created and Updated timestamps during insert and update operations to maintain accurate audit trails.
/// </para>
/// <para>
/// All operations use MongoDB's native driver methods with proper async/await patterns and ConfigureAwait(false)
/// for optimal performance in library scenarios. The implementation provides both synchronous and asynchronous
/// variants of all operations, with async methods being preferred for better scalability.
/// </para>
/// </remarks>
public abstract class MongoRepository<TEntity, TKey> : MongoQuery<TEntity, TKey>, IMongoRepository<TEntity, TKey>
    where TEntity : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MongoRepository{TEntity, TKey}"/> class with the specified MongoDB database.
    /// </summary>
    /// <param name="mongoDatabase">The MongoDB database instance used for repository operations.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="mongoDatabase"/> is <see langword="null" />.</exception>
    /// <remarks>
    /// <para>
    /// The constructor inherits the lazy collection initialization behavior from the base class,
    /// ensuring optimal startup performance and flexible configuration patterns.
    /// </para>
    /// <para>
    /// The provided database instance should be properly configured with connection strings, authentication,
    /// write concerns, and other MongoDB-specific settings before being passed to this constructor.
    /// </para>
    /// </remarks>
    protected MongoRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {

    }

    /// <summary>
    /// Inserts a new entity into the MongoDB collection with automatic audit timestamp management.
    /// </summary>
    /// <param name="entity">The entity to be inserted into the collection.</param>
    /// <returns>The entity that was inserted, with updated audit timestamps and potentially generated identifiers.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This method automatically calls <see cref="BeforeInsert(TEntity)"/> to apply audit timestamps
    /// and any custom pre-insert logic before performing the database operation.
    /// </para>
    /// <para>
    /// For entities implementing <see cref="IMongoEntity"/>, both Created and Updated timestamps
    /// are set to the current UTC time during the insert operation.
    /// </para>
    /// </remarks>
    public TEntity Insert(TEntity entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        BeforeInsert(entity);
        Collection.InsertOne(entity);

        return entity;
    }

    /// <summary>
    /// Asynchronously inserts a new entity into the MongoDB collection with automatic audit timestamp management.
    /// </summary>
    /// <param name="entity">The entity to be inserted into the collection.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the entity that was inserted,
    /// with updated audit timestamps and potentially generated identifiers.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This is the preferred method for inserting entities in asynchronous contexts. The method uses
    /// proper async/await patterns with ConfigureAwait(false) for optimal library performance.
    /// </para>
    /// <para>
    /// Automatic audit timestamp management is applied through <see cref="BeforeInsert(TEntity)"/>
    /// before the database operation is performed.
    /// </para>
    /// </remarks>
    public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        BeforeInsert(entity);

        await Collection
            .InsertOneAsync(entity, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return entity;
    }

    /// <summary>
    /// Inserts multiple entities into the MongoDB collection in a single batch operation with audit timestamp management.
    /// </summary>
    /// <param name="entities">The collection of entities to be inserted.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entities"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This method provides efficient batch insertion by performing all insertions in a single round-trip
    /// to the database. Each entity in the batch receives proper audit timestamp management through
    /// <see cref="BeforeInsert(TEntity)"/> before the batch operation is executed.
    /// </para>
    /// <para>
    /// Batch operations improve performance significantly when inserting multiple entities compared
    /// to individual insert operations, while maintaining the same audit trail functionality.
    /// </para>
    /// </remarks>
    public void InsertBatch(IEnumerable<TEntity> entities)
    {
        if (entities is null)
            throw new ArgumentNullException(nameof(entities));

        var list = entities.ToList();
        list.ForEach(BeforeInsert);

        Collection.InsertMany(list);
    }

    /// <summary>
    /// Updates an existing entity in the MongoDB collection using document replacement with audit timestamp management.
    /// </summary>
    /// <param name="entity">The entity with updated values to be saved to the collection.</param>
    /// <returns>The entity that was updated, with refreshed audit timestamps.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This method uses MongoDB's ReplaceOne operation to replace the entire document with the provided entity.
    /// The <see cref="BeforeUpdate(TEntity)"/> method is called to update audit timestamps and apply custom logic.
    /// </para>
    /// <para>
    /// The operation uses the entity's key (extracted via <see cref="MongoQuery{TEntity, TKey}.EntityKey(TEntity)"/>)
    /// and key expression (via <see cref="MongoQuery{TEntity, TKey}.KeyExpression(TKey)"/>) to locate the document for replacement.
    /// </para>
    /// </remarks>
    public TEntity Update(TEntity entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        BeforeUpdate(entity);

        var options = new ReplaceOptions();
        var key = EntityKey(entity);
        var filter = KeyExpression(key);

        Collection.ReplaceOne(filter, entity, options);

        return entity;
    }

    /// <summary>
    /// Asynchronously updates an existing entity in the MongoDB collection using document replacement with audit timestamp management.
    /// </summary>
    /// <param name="entity">The entity with updated values to be saved to the collection.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the entity that was updated,
    /// with refreshed audit timestamps.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This is the preferred method for updating entities in asynchronous contexts. The method uses
    /// MongoDB's ReplaceOneAsync operation with proper async/await patterns and ConfigureAwait(false).
    /// </para>
    /// <para>
    /// Automatic audit timestamp management is applied through <see cref="BeforeUpdate(TEntity)"/>
    /// before the document replacement operation is performed.
    /// </para>
    /// </remarks>
    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        BeforeUpdate(entity);

        var options = new ReplaceOptions();
        var key = EntityKey(entity);
        var filter = KeyExpression(key);

        await Collection
            .ReplaceOneAsync(filter, entity, options, cancellationToken)
            .ConfigureAwait(false);

        return entity;
    }

    /// <summary>
    /// Performs an upsert operation using document replacement with audit timestamp management.
    /// </summary>
    /// <param name="entity">The entity to be inserted or updated in the collection.</param>
    /// <returns>The entity that was inserted or updated, with appropriate audit timestamps.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This method uses MongoDB's ReplaceOne operation with the IsUpsert option enabled, providing
    /// atomic insert-or-update functionality. The <see cref="BeforeUpdate(TEntity)"/> method is called
    /// to manage audit timestamps appropriately for both insert and update scenarios.
    /// </para>
    /// <para>
    /// The operation uses the entity's key to determine whether the document exists, inserting
    /// a new document if no match is found or replacing the existing document if found.
    /// </para>
    /// </remarks>
    public TEntity Upsert(TEntity entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        BeforeUpdate(entity);

        var options = new ReplaceOptions { IsUpsert = true };
        var key = EntityKey(entity);
        var filter = KeyExpression(key);

        Collection.ReplaceOne(filter, entity, options);

        return entity;
    }

    /// <summary>
    /// Asynchronously performs an upsert operation using document replacement with audit timestamp management.
    /// </summary>
    /// <param name="entity">The entity to be inserted or updated in the collection.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the entity that was
    /// inserted or updated, with appropriate audit timestamps.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This is the preferred method for upsert operations in asynchronous contexts. The method uses
    /// MongoDB's ReplaceOneAsync operation with IsUpsert enabled and proper async/await patterns.
    /// </para>
    /// <para>
    /// The atomic insert-or-update behavior is particularly useful when entity existence is uncertain,
    /// with automatic audit timestamp management handled through <see cref="BeforeUpdate(TEntity)"/>.
    /// </para>
    /// </remarks>
    public async Task<TEntity> UpsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        BeforeUpdate(entity);

        var options = new ReplaceOptions { IsUpsert = true };
        var key = EntityKey(entity);
        var filter = KeyExpression(key);

        await Collection
            .ReplaceOneAsync(filter, entity, options, cancellationToken)
            .ConfigureAwait(false);

        return entity;
    }

    /// <summary>
    /// Deletes an entity from the MongoDB collection using its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <returns>The number of documents that were deleted from the collection (0 or 1).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This method uses MongoDB's DeleteOne operation with a filter generated from the provided identifier.
    /// The filter is created using the abstract <see cref="MongoQuery{TEntity, TKey}.KeyExpression(TKey)"/> method.
    /// </para>
    /// <para>
    /// Returns 1 if a document was found and deleted, or 0 if no document with the specified identifier exists.
    /// </para>
    /// </remarks>
    public long Delete(TKey id)
    {
        if (id is null)
            throw new ArgumentNullException(nameof(id));

        var filter = KeyExpression(id);

        var result = Collection.DeleteOne(filter);

        return result.DeletedCount;
    }

    /// <summary>
    /// Asynchronously deletes an entity from the MongoDB collection using its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the number of documents
    /// that were deleted from the collection (0 or 1).
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This is the preferred method for deleting entities by identifier in asynchronous contexts.
    /// The method uses MongoDB's DeleteOneAsync operation with proper async/await patterns.
    /// </para>
    /// <para>
    /// Returns 1 if a document was found and deleted, or 0 if no document with the specified identifier exists.
    /// </para>
    /// </remarks>
    public async Task<long> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        if (id is null)
            throw new ArgumentNullException(nameof(id));

        var filter = KeyExpression(id);

        var result = await Collection
            .DeleteOneAsync(filter, cancellationToken)
            .ConfigureAwait(false);

        return result.DeletedCount;
    }

    /// <summary>
    /// Deletes the specified entity from the MongoDB collection using its extracted key.
    /// </summary>
    /// <param name="entity">The entity instance to be deleted from the collection.</param>
    /// <returns>The number of documents that were deleted from the collection (0 or 1).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This method extracts the entity's key using <see cref="MongoQuery{TEntity, TKey}.EntityKey(TEntity)"/>
    /// and delegates to the identifier-based <see cref="Delete(TKey)"/> method for the actual deletion operation.
    /// </para>
    /// <para>
    /// This approach provides a convenient entity-based deletion interface while maintaining
    /// consistent deletion logic and performance characteristics.
    /// </para>
    /// </remarks>
    public long Delete(TEntity entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        var key = EntityKey(entity);

        return Delete(key);
    }

    /// <summary>
    /// Asynchronously deletes the specified entity from the MongoDB collection using its extracted key.
    /// </summary>
    /// <param name="entity">The entity instance to be deleted from the collection.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the number of documents
    /// that were deleted from the collection (0 or 1).
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This is the preferred method for entity-based deletion in asynchronous contexts.
    /// The method extracts the entity's key and delegates to the async identifier-based deletion method.
    /// </para>
    /// <para>
    /// This approach maintains consistency with the async patterns while providing convenient
    /// entity-based deletion capabilities.
    /// </para>
    /// </remarks>
    public async Task<long> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        var key = EntityKey(entity);

        return await DeleteAsync(key, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes all entities from the collection that match the specified criteria in a single bulk operation.
    /// </summary>
    /// <param name="criteria">A lambda expression that defines the deletion criteria.</param>
    /// <returns>The number of documents that were deleted from the collection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="criteria"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This method uses MongoDB's DeleteMany operation to efficiently remove multiple documents
    /// in a single database round-trip. The operation can affect a large number of documents,
    /// so use with appropriate caution in production environments.
    /// </para>
    /// <para>
    /// The return value indicates the actual number of documents that matched the criteria
    /// and were successfully deleted from the collection.
    /// </para>
    /// </remarks>
    public long DeleteAll(Expression<Func<TEntity, bool>> criteria)
    {
        if (criteria is null)
            throw new ArgumentNullException(nameof(criteria));

        var result = Collection.DeleteMany(criteria);

        return result.DeletedCount;
    }

    /// <summary>
    /// Asynchronously deletes all entities from the collection that match the specified criteria in a single bulk operation.
    /// </summary>
    /// <param name="criteria">A lambda expression that defines the deletion criteria.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the number of documents
    /// that were deleted from the collection.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="criteria"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This is the preferred method for bulk deletion operations in asynchronous contexts.
    /// The method uses MongoDB's DeleteManyAsync operation with proper async/await patterns.
    /// </para>
    /// <para>
    /// Bulk deletion operations are efficient but can affect many documents. Use appropriate
    /// caution and consider the impact on database performance and transaction log size.
    /// </para>
    /// </remarks>
    public async Task<long> DeleteAllAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default)
    {
        if (criteria is null)
            throw new ArgumentNullException(nameof(criteria));

        var result = await Collection
            .DeleteManyAsync(criteria, cancellationToken)
            .ConfigureAwait(false);

        return result.DeletedCount;
    }

    /// <summary>
    /// Performs pre-insert processing including automatic audit timestamp management for MongoDB entities.
    /// </summary>
    /// <param name="entity">The entity that is being prepared for insertion.</param>
    /// <remarks>
    /// <para>
    /// This virtual method provides an extensibility point for derived classes to implement custom
    /// pre-insert logic such as validation, additional field population, or business rule enforcement.
    /// </para>
    /// <para>
    /// The default implementation automatically sets both Created and Updated timestamps to the current
    /// UTC time for entities implementing <see cref="IMongoEntity"/>, ensuring consistent audit trail management.
    /// </para>
    /// <para>
    /// Override this method in derived classes to add custom insert preparation logic while
    /// calling the base implementation to maintain automatic timestamp management.
    /// </para>
    /// </remarks>
    protected virtual void BeforeInsert(TEntity entity)
    {
        var mongoEntity = entity as IMongoEntity;
        if (mongoEntity is null)
            return;

        mongoEntity.Created = DateTimeOffset.UtcNow;
        mongoEntity.Updated = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Performs pre-update processing including automatic audit timestamp management for MongoDB entities.
    /// </summary>
    /// <param name="entity">The entity that is being prepared for update or upsert operation.</param>
    /// <remarks>
    /// <para>
    /// This virtual method provides an extensibility point for derived classes to implement custom
    /// pre-update logic such as validation, field transformations, or business rule enforcement.
    /// </para>
    /// <para>
    /// The default implementation manages audit timestamps for entities implementing <see cref="IMongoEntity"/>:
    /// it preserves the original Created timestamp (setting it to current time only if unset) and
    /// always updates the Updated timestamp to the current UTC time.
    /// </para>
    /// <para>
    /// This method is used for both update and upsert operations, ensuring consistent audit
    /// timestamp behavior regardless of whether the entity already exists in the collection.
    /// </para>
    /// </remarks>
    protected virtual void BeforeUpdate(TEntity entity)
    {
        var mongoEntity = entity as IMongoEntity;
        if (mongoEntity is null)
            return;

        if (mongoEntity.Created == default)
            mongoEntity.Created = DateTimeOffset.UtcNow;

        mongoEntity.Updated = DateTimeOffset.UtcNow;
    }
}

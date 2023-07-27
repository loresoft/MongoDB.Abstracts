using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using MongoDB.Driver;

namespace MongoDB.Abstracts;

/// <summary>
/// A MongoDB data repository base class.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TKey">The type of the key.</typeparam>
public abstract class MongoRepository<TEntity, TKey> : MongoQuery<TEntity, TKey>, IMongoRepository<TEntity, TKey>
    where TEntity : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MongoRepository{TEntity, TKey}"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="mongoDatabase"/> is <see langword="null" />.</exception>
    protected MongoRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {

    }

    /// <inheritdoc/>
    public TEntity Insert(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        BeforeInsert(entity);
        Collection.InsertOne(entity);

        return entity;
    }

    /// <inheritdoc/>
    public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        BeforeInsert(entity);

        await Collection
            .InsertOneAsync(entity, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return entity;
    }


    /// <inheritdoc/>
    public void InsertBatch(IEnumerable<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        var list = entities.ToList();
        list.ForEach(BeforeInsert);

        Collection.InsertMany(list);
    }


    /// <inheritdoc/>
    public TEntity Update(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        BeforeUpdate(entity);

        var options = new ReplaceOptions();
        var key = EntityKey(entity);
        var filter = KeyExpression(key);

        Collection.ReplaceOne(filter, entity, options);

        return entity;
    }

    /// <inheritdoc/>
    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
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



    /// <inheritdoc/>
    public TEntity Upsert(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        BeforeUpdate(entity);

        var options = new ReplaceOptions { IsUpsert = true };
        var key = EntityKey(entity);
        var filter = KeyExpression(key);

        Collection.ReplaceOne(filter, entity, options);

        return entity;
    }

    /// <inheritdoc/>
    public async Task<TEntity> UpsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
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


    /// <inheritdoc/>
    public long Delete(TKey id)
    {
        if (id == null)
            throw new ArgumentNullException(nameof(id));

        var filter = KeyExpression(id);

        var result = Collection.DeleteOne(filter);

        return result.DeletedCount;
    }

    /// <inheritdoc/>
    public async Task<long> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        if (id == null)
            throw new ArgumentNullException(nameof(id));

        var filter = KeyExpression(id);

        var result = await Collection
            .DeleteOneAsync(filter, cancellationToken)
            .ConfigureAwait(false);

        return result.DeletedCount;
    }


    /// <inheritdoc/>
    public long Delete(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var key = EntityKey(entity);

        return Delete(key);
    }

    /// <inheritdoc/>
    public async Task<long> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var key = EntityKey(entity);

        return await DeleteAsync(key, cancellationToken).ConfigureAwait(false);
    }


    /// <inheritdoc/>
    public long DeleteAll(Expression<Func<TEntity, bool>> criteria)
    {
        if (criteria == null)
            throw new ArgumentNullException(nameof(criteria));

        var result = Collection.DeleteMany(criteria);

        return result.DeletedCount;
    }

    /// <inheritdoc/>
    public async Task<long> DeleteAllAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default)
    {
        if (criteria == null)
            throw new ArgumentNullException(nameof(criteria));

        var result = await Collection
            .DeleteManyAsync(criteria, cancellationToken)
            .ConfigureAwait(false);

        return result.DeletedCount;
    }


    /// <summary>
    /// Called before an insert.
    /// </summary>
    /// <param name="entity">The entity that is being inserted.</param>
    protected virtual void BeforeInsert(TEntity entity)
    {
        var mongoEntity = entity as IMongoEntity;
        if (mongoEntity == null)
            return;

        mongoEntity.Created = DateTimeOffset.UtcNow;
        mongoEntity.Updated = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Called before an update.
    /// </summary>
    /// <param name="entity">The entity that is being updated.</param>
    protected virtual void BeforeUpdate(TEntity entity)
    {
        var mongoEntity = entity as IMongoEntity;
        if (mongoEntity == null)
            return;

        if (mongoEntity.Created == default)
            mongoEntity.Created = DateTimeOffset.UtcNow;

        mongoEntity.Updated = DateTimeOffset.UtcNow;
    }
}

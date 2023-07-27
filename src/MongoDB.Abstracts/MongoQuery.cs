using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using MongoDB.Driver;

namespace MongoDB.Abstracts;

/// <summary>
/// A MongoDB data query base class.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TKey">The type of the key.</typeparam>
public abstract class MongoQuery<TEntity, TKey> : IMongoQuery<TEntity, TKey>
    where TEntity : class
{
    private readonly Lazy<IMongoCollection<TEntity>> _collection;
    private readonly IMongoDatabase _mongoDatabase;

    /// <summary>
    /// Initializes a new instance of the <see cref="MongoQuery{TEntity, TKey}"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="mongoDatabase"/> is <see langword="null" />.</exception>
    protected MongoQuery(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase ?? throw new ArgumentNullException(nameof(mongoDatabase));
        _collection = new Lazy<IMongoCollection<TEntity>>(CreateCollection);
    }


    /// <inheritdoc/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IMongoCollection<TEntity> Collection => _collection.Value;


    /// <inheritdoc/>
    public TEntity Find(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        var filter = KeyExpression(key);

        return Collection
            .Find(filter)
            .FirstOrDefault();
    }

    /// <inheritdoc/>
    public Task<TEntity> FindAsync(TKey key, CancellationToken cancellationToken = default)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        var filter = KeyExpression(key);

        return Collection
            .Find(filter)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public TEntity FindOne(Expression<Func<TEntity, bool>> criteria)
    {
        if (criteria == null)
            throw new ArgumentNullException(nameof(criteria));

        return Collection
            .Find(criteria)
            .FirstOrDefault();
    }

    /// <inheritdoc/>
    public Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default)
    {
        if (criteria == null)
            throw new ArgumentNullException(nameof(criteria));

        return Collection
            .Find(criteria)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria)
    {
        if (criteria == null)
            throw new ArgumentNullException(nameof(criteria));

        return Collection
            .AsQueryable()
            .Where(criteria);
    }

    /// <inheritdoc/>
    public Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default)
    {
        return Collection
            .Find(criteria)
            .ToListAsync(cancellationToken);
    }


    /// <inheritdoc/>
    public IQueryable<TEntity> All()
    {
        return Collection.AsQueryable();
    }


    /// <inheritdoc/>
    public long Count()
    {
        return Collection.CountDocuments(FilterDefinition<TEntity>.Empty);
    }

    /// <inheritdoc/>
    public Task<long> CountAsync(CancellationToken cancellationToken = default)
    {
        return Collection.CountDocumentsAsync(FilterDefinition<TEntity>.Empty, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public long Count(Expression<Func<TEntity, bool>> criteria)
    {
        return Collection.CountDocuments(criteria);
    }

    /// <inheritdoc/>
    public Task<long> CountAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default)
    {
        return Collection.CountDocumentsAsync(criteria, cancellationToken: cancellationToken);
    }


    /// <summary>
    /// Gets the key for the specified <paramref name="entity" />.
    /// </summary>
    /// <param name="entity">The entity to get the key from.</param>
    /// <returns>
    /// The key for the specified entity.
    /// </returns>
    protected abstract TKey EntityKey(TEntity entity);

    /// <summary>
    /// Gets the key expression with the specified <paramref name="key" />.
    /// </summary>
    /// <param name="key">The key to get expression with.</param>
    /// <returns>
    /// The key expression for the specified key.
    /// </returns>
    /// <example>
    /// <code>
    /// Example expression for an entity key.
    /// <![CDATA[entity => entity.Id == key]]>
    /// </code>
    /// </example>
    protected abstract Expression<Func<TEntity, bool>> KeyExpression(TKey key);


    /// <summary>
    /// Gets the name of the collection.
    /// </summary>
    /// <returns></returns>
    protected virtual string CollectionName()
    {
        return typeof(TEntity).Name;
    }

    /// <summary>
    /// Creates the collection.
    /// </summary>
    /// <returns></returns>
    protected virtual IMongoCollection<TEntity> CreateCollection()
    {
        var database = _mongoDatabase;

        string collectionName = CollectionName();
        var mongoCollection = CreateCollection(database, collectionName);

        EnsureIndexes(mongoCollection);

        return mongoCollection;
    }

    /// <summary>
    /// Creates the collection.
    /// </summary>
    /// <param name="database">The database.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <returns></returns>
    protected virtual IMongoCollection<TEntity> CreateCollection(IMongoDatabase database, string collectionName)
    {
        var mongoCollection = database.GetCollection<TEntity>(collectionName);
        return mongoCollection;
    }

    /// <summary>
    /// Create indexes on the collection.
    /// </summary>
    /// <param name="mongoCollection">The mongo collection.</param>
    protected virtual void EnsureIndexes(IMongoCollection<TEntity> mongoCollection)
    {

    }
}

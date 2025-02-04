// Ignore Spelling: Mongo

using System.Linq.Expressions;

using MongoDB.Driver;

namespace MongoDB.Abstracts;

/// <summary>
/// A MongoDB data repository base <see langword="class"/>.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public class MongoEntityRepository<TEntity> : MongoRepository<TEntity, string>, IMongoEntityRepository<TEntity>
    where TEntity : class, IMongoEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MongoEntityRepository{TEntity}"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="mongoDatabase"/> is <see langword="null" />.</exception>
    public MongoEntityRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {

    }

    /// <inheritdoc/>
    protected override string EntityKey(TEntity entity)
    {
        return entity.Id;
    }

    /// <inheritdoc/>
    protected override Expression<Func<TEntity, bool>> KeyExpression(string key)
    {
        return entity => entity.Id == key;
    }
}

/// <summary>
/// A MongoDB data repository base <see langword="class"/> by connection discriminator.  Discriminator used to register multiple instances.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TDiscriminator">The type of the connection discriminator.</typeparam>
public class MongoEntityRepository<TDiscriminator, TEntity>(MongoDiscriminator<TDiscriminator> mongoDiscriminator)
    : MongoEntityRepository<TEntity>(mongoDiscriminator.MongoDatabase), IMongoEntityRepository<TDiscriminator, TEntity>
    where TEntity : class, IMongoEntity;


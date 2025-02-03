// Ignore Spelling: Mongo

using System.Linq.Expressions;

using MongoDB.Driver;

namespace MongoDB.Abstracts;

/// <summary>
/// A MongoDB data query base class.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public class MongoEntityQuery<TEntity> : MongoQuery<TEntity, string>, IMongoEntityQuery<TEntity>
    where TEntity : class, IMongoEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MongoEntityQuery{TEntity}" /> class.
    /// </summary>
    /// <param name="mongoDatabase">The <see cref="IMongoDatabase"/> to use for this instance</param>
    public MongoEntityQuery(IMongoDatabase mongoDatabase) : base(mongoDatabase)
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
/// A MongoDB data query base class by connection discriminator. Discriminator used to register multiple instances.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TDiscriminator">The type of the connection discriminator.</typeparam>
public class MongoEntityQuery<TDiscriminator, TEntity> : MongoEntityQuery<TEntity>, IMongoEntityQuery<TDiscriminator, TEntity>
    where TEntity : class, IMongoEntity
{
    public MongoEntityQuery(MongoDiscriminator<TDiscriminator> mongoDiscriminator) : base(mongoDiscriminator.MongoDatabase)
    {
    }
}

// Ignore Spelling: Mongo

namespace MongoDB.Abstracts;

/// <summary>
/// An <c>interface</c> for common MongoDB data operations.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IMongoEntityRepository<TEntity> : IMongoRepository<TEntity, string>
    where TEntity : class, IMongoEntity
{

}

/// <summary>
/// An <c>interface</c> for common MongoDB data operations by connection discriminator.  Discriminator used to register multiple instances.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TDiscriminator">The type of the connection discriminator.</typeparam>
public interface IMongoEntityRepository<TDiscriminator, TEntity> : IMongoEntityRepository<TEntity>
    where TEntity : class, IMongoEntity
{

}

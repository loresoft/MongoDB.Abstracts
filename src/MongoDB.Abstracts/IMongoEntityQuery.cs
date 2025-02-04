// Ignore Spelling: Mongo

namespace MongoDB.Abstracts;

/// <summary>
/// An <see langword="interface"/> for common MongoDB query operations.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IMongoEntityQuery<TEntity> : IMongoQuery<TEntity, string>
    where TEntity : class, IMongoEntity;

/// <summary>
/// An <see langword="interface"/> for common MongoDB data operations by connection discriminator.  Discriminator used to register multiple instances.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TDiscriminator">The type of the connection discriminator.</typeparam>
public interface IMongoEntityQuery<TDiscriminator, TEntity> : IMongoEntityQuery<TEntity>
    where TEntity : class, IMongoEntity;

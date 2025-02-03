// Ignore Spelling: Mongo

namespace MongoDB.Abstracts;

/// <summary>
/// An <c>interface</c> for common MongoDB query operations.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IMongoEntityQuery<TEntity> : IMongoQuery<TEntity, string>
    where TEntity : class, IMongoEntity
{

}

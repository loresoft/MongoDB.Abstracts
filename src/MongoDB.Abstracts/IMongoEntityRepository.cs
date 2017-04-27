using System;

namespace MongoDB.Abstracts
{
    /// <summary>
    /// An <c>interface</c> for common MongoDB data operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IMongoEntityRepository<TEntity> : IMongoRepository<TEntity, string>
        where TEntity : class, IMongoEntity
    {

    }
}
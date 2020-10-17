using System;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace MongoDB.Abstracts
{
    /// <summary>
    /// A MongoDB data repository base class.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class MongoEntityRepository<TEntity> : MongoRepository<TEntity, string>, IMongoEntityRepository<TEntity>
        where TEntity : class, IMongoEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoEntityRepository{TEntity}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="mongoDatabase"/> is <see langword="null" />.</exception>
        protected MongoEntityRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {

        }

        /// <summary>
        /// Called before an insert.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected override void BeforeInsert(TEntity entity)
        {
            entity.Created = DateTimeOffset.UtcNow;
            entity.Updated = DateTimeOffset.UtcNow;

            base.BeforeInsert(entity);
        }

        /// <summary>
        /// Called before an update.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected override void BeforeUpdate(TEntity entity)
        {
            if (entity.Created == DateTimeOffset.MinValue)
                entity.Created = DateTimeOffset.UtcNow;

            entity.Updated = DateTimeOffset.UtcNow;

            base.BeforeUpdate(entity);
        }

        /// <summary>
        /// Gets the key for the specified <paramref name="entity" />.
        /// </summary>
        /// <param name="entity">The entity to get the key from.</param>
        /// <returns>
        /// The key for the specified entity.
        /// </returns>
        public override string EntityKey(TEntity entity)
        {
            return entity.Id;
        }

        /// <summary>
        /// Gets the key expression with the specified <paramref name="key" />.
        /// </summary>
        /// <param name="key">The key to get expression with.</param>
        /// <returns>
        /// The key expression for the specified key.
        /// </returns>
        /// <example>
        ///   <code>
        /// Example expression for an entity key.
        /// <![CDATA[entity => entity.Id == key]]></code>
        /// </example>
        protected override Expression<Func<TEntity, bool>> KeyExpression(string key)
        {
            return entity => entity.Id == key;
        }
    }
}
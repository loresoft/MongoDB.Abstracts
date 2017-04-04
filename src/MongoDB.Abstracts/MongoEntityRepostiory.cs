using System;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace MongoDB.Abstracts
{
    /// <summary>
    /// A MongoDB data repository base class.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class MongoEntityRepostiory<TEntity> : MongoRepository<TEntity, string>, IMongoEntityRepository<TEntity>
        where TEntity : class, IMongoEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoEntityRepostiory{TEntity}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="mongoDatabase"/> is <see langword="null" />.</exception>
        protected MongoEntityRepostiory(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {

        }

        /// <summary>
        /// Called before an insert.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected override void BeforeInsert(TEntity entity)
        {
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;

            base.BeforeInsert(entity);
        }

        /// <summary>
        /// Called before an update.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected override void BeforeUpdate(TEntity entity)
        {
            if (entity.Created == DateTime.MinValue)
                entity.Created = DateTime.Now;

            entity.Updated = DateTime.Now;

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
        /// Example xpression for an entity key.
        /// <![CDATA[entity => entity.Id == key]]></code>
        /// </example>
        protected override Expression<Func<TEntity, bool>> KeyExpression(string key)
        {
            return entity => entity.Id == key;
        }
    }
}
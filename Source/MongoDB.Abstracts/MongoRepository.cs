using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.Abstracts
{
    /// <summary>
    /// A MongoDB data repository base class.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public abstract class MongoRepository<TEntity, TKey> : MongoQueury<TEntity, TKey>, IMongoRepository<TEntity, TKey>
        where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoRepository{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionName"/> is <see langword="null" />.</exception>
        protected MongoRepository(string connectionName)
            : base(connectionName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoRepository{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="mongoUrl">The mongo URL.</param>
        /// <exception cref="ArgumentNullException"><paramref name="mongoUrl"/> is <see langword="null" />.</exception>
        protected MongoRepository(MongoUrl mongoUrl)
            : base(mongoUrl)
        {
        }


        /// <summary>
        /// Inserts the specified <paramref name="entity" /> to the underlying data repository.
        /// </summary>
        /// <param name="entity">The entity to be inserted.</param>
        /// <returns>
        /// The entity that was inserted.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">entity</exception>
        public TEntity Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            BeforeInsert(entity);
            Collection.InsertOne(entity);

            return entity;
        }
        
        /// <summary>
        /// Inserts the specified <paramref name="entity" /> to the underlying data repository.
        /// </summary>
        /// <param name="entity">The entity to be inserted.</param>
        /// <returns>
        /// The entity that was inserted.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">entity</exception>
        public Task<TEntity> InsertAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            BeforeInsert(entity);

            return Collection
                .InsertOneAsync(entity)
                .ContinueWith(t => entity);
        }

        /// <summary>
        /// Inserts the specified <paramref name="entities" /> to the underlying data repository.
        /// </summary>
        /// <param name="entities"></param>
        /// <exception cref="System.ArgumentNullException">entities</exception>
        public void Insert(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
                Insert(entity);
        }

        /// <summary>
        /// Inserts the specified <paramref name="entities" /> in a batch operation to the underlying data repository.
        /// </summary>
        /// <param name="entities">The entities to be inserted.</param>
        /// <exception cref="System.ArgumentNullException">entities</exception>
        public void InsertBatch(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            var list = entities.ToList();
            list.ForEach(BeforeInsert);

            Collection.InsertMany(list);
        }


        /// <summary>
        /// Updates the specified <paramref name="entity" /> in the underlying data repository.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>
        /// The entity that was updated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">entity</exception>
        public TEntity Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            BeforeUpdate(entity);

            var updateOptions = new UpdateOptions { IsUpsert = true };
            var key = EntityKey(entity);

            var result = Collection.ReplaceOne(KeyExpression(key), entity, updateOptions);

            return entity;
        }
        
        /// <summary>
        /// Updates the specified <paramref name="entity" /> in the underlying data repository.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>
        /// The entity that was updated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">entity</exception>
        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            BeforeUpdate(entity);

            var updateOptions = new UpdateOptions { IsUpsert = true };
            var key = EntityKey(entity);

            return Collection
                .ReplaceOneAsync(KeyExpression(key), entity, updateOptions)
                .ContinueWith(t => entity);
        }

        /// <summary>
        /// Updates the specified <paramref name="entities" /> in the underlying data repository.
        /// </summary>
        /// <param name="entities"></param>
        /// <exception cref="System.ArgumentNullException">entities</exception>
        public void Update(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
                Update(entity);
        }


        /// <summary>
        /// Saves the specified <paramref name="entity" /> in the underlying data repository by inserting if doesn't exist, or updating if it does.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>
        /// The entity that was updated.
        /// </returns>
        public TEntity Save(TEntity entity)
        {
            return Update(entity);
        }
        
        /// <summary>
        /// Saves the specified <paramref name="entity" /> in the underlying data repository by inserting if doesn't exist, or updating if it does.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>
        /// The entity that was updated.
        /// </returns>
        public Task<TEntity> SaveAsync(TEntity entity)
        {
            return UpdateAsync(entity);
        }

        /// <summary>
        /// Saves the specified <paramref name="entities" /> in the underlying data repository by inserting if doesn't exist, or updating if it does.
        /// </summary>
        /// <param name="entities"></param>
        public void Save(IEnumerable<TEntity> entities)
        {
            Update(entities);
        }


        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The number of documents deleted</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <see langword="null" />.</exception>
        public long Delete(TKey id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var result = Collection.DeleteOne(KeyExpression(id));
            return result.DeletedCount;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The number of documents deleted</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <see langword="null" />.</exception>
        public Task<long> DeleteAsync(TKey id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return Collection
                .DeleteOneAsync(KeyExpression(id))
                .ContinueWith(t => t.Result.DeletedCount);
        }


        /// <summary>
        /// Deletes the specified <paramref name="entity" /> from the underlying data repository.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        /// <returns>The number of documents deleted</returns>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
        public long Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var key = EntityKey(entity);
            return Delete(key);
        }

        /// <summary>
        /// Deletes the specified <paramref name="entity" /> from the underlying data repository.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        /// <returns>The number of documents deleted</returns>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
        public Task<long> DeleteAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var key = EntityKey(entity);
            return DeleteAsync(key);
        }

        /// <summary>
        /// Deletes the specified <paramref name="entities" /> from the underlying data repository.
        /// </summary>
        /// <param name="entities">The entities to be deleted.</param>
        /// <returns>
        /// The number of documents deleted
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"><paramref name="entities" /> is <see langword="null" />.</exception>
        public long Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            long count = 0;

            foreach (var entity in entities)
            {
                var key = EntityKey(entity);
                count += Delete(key);
            }

            return count;
        }


        /// <summary>
        /// Deletes all documents from MongoDB collection.
        /// </summary>
        /// <returns>The number of documents deleted</returns>
        public long DeleteAll()
        {
            var result = Collection.DeleteMany(FilterDefinition<TEntity>.Empty);
            return result.DeletedCount;
        }

        /// <summary>
        /// Deletes all documents from MongoDB collection.
        /// </summary>
        /// <returns>The number of documents deleted</returns>
        public Task<long> DeleteAllAsync()
        {
            return Collection
                .DeleteManyAsync(FilterDefinition<TEntity>.Empty)
                .ContinueWith(t => t.Result.DeletedCount);
        }

        /// <summary>
        /// Deletes all from collection that meet the specified <paramref name="criteria" />.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The number of documents deleted</returns>
        public long DeleteAll(Expression<Func<TEntity, bool>> criteria)
        {
            var result = Collection.DeleteMany(criteria);
            return result.DeletedCount;
        }

        /// <summary>
        /// Deletes all from collection that meet the specified <paramref name="criteria" />.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The number of documents deleted</returns>
        public Task<long> DeleteAllAsync(Expression<Func<TEntity, bool>> criteria)
        {
            return Collection
                .DeleteManyAsync(criteria)
                .ContinueWith(t => t.Result.DeletedCount);
        }


        /// <summary>
        /// Called before an insert.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected virtual void BeforeInsert(TEntity entity)
        {
            BeforeUpdate(entity);

            var mongoEntity = entity as IMongoEntity;
            if (mongoEntity == null)
                return;

            mongoEntity.Created = DateTime.Now;
        }

        /// <summary>
        /// Called before an update.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected virtual void BeforeUpdate(TEntity entity)
        {
            var mongoEntity = entity as IMongoEntity;
            if (mongoEntity == null)
                return;

            if (mongoEntity.Created == DateTime.MinValue)
                mongoEntity.Created = DateTime.Now;

            mongoEntity.Updated = DateTime.Now;
        }

    }
}
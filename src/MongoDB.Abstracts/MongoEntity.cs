using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace MongoDB.Abstracts
{
    /// <summary>
    /// A base <c>class</c> for a MongoDB Entity
    /// </summary>
    [BsonIgnoreExtraElements(true, Inherited = true)]
    public abstract class MongoEntity : IMongoEntity
    {
        /// <summary>
        /// Gets or sets the identifier for the entity.
        /// </summary>
        /// <value>
        /// The identifier for the entity.
        /// </value>
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the date the entity was created.
        /// </summary>
        /// <value>
        /// The date the entity was created.
        /// </value>
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// Gets or sets the date the entity was updated.
        /// </summary>
        /// <value>
        /// The date the entity was updated.
        /// </value>
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Updated { get; set; }
    }
}
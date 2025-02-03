// Ignore Spelling: Mongo

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace MongoDB.Abstracts;

/// <summary>
/// A base <c>class</c> for a MongoDB Entity
/// </summary>
[BsonIgnoreExtraElements(true, Inherited = true)]
public abstract class MongoEntity : IMongoEntity
{
    /// <inheritdoc/>
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <inheritdoc/>
    [BsonRepresentation(BsonType.String)]
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

    /// <inheritdoc/>
    [BsonRepresentation(BsonType.String)]
    public DateTimeOffset Updated { get; set; } = DateTimeOffset.UtcNow;
}

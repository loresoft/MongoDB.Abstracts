// Ignore Spelling: Mongo

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace MongoDB.Abstracts;

/// <summary>
/// Provides a base implementation for MongoDB entities with automatic identifier generation and audit timestamps.
/// </summary>
/// <remarks>
/// <para>
/// This abstract class implements the <see cref="IMongoEntity"/> interface and provides a concrete foundation
/// for MongoDB document entities. It includes automatic ObjectId generation, BSON serialization attributes,
/// and default timestamp management for creation and modification tracking.
/// </para>
/// <para>
/// The class is configured to ignore extra elements during deserialization, making it resilient to schema
/// evolution and backward compatibility scenarios. This behavior is inherited by all derived classes.
/// </para>
/// <para>
/// All properties are properly configured with BSON attributes to ensure optimal serialization performance
/// and correct data type mapping between .NET types and MongoDB BSON documents.
/// </para>
/// </remarks>
[BsonIgnoreExtraElements(true, Inherited = true)]
public abstract class MongoEntity : IMongoEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the MongoDB document.
    /// </summary>
    /// <value>
    /// A string representation of a MongoDB ObjectId that serves as the primary key for the document.
    /// </value>
    /// <remarks>
    /// <para>
    /// This property is automatically initialized with a new ObjectId when the entity is created.
    /// The <see cref="StringObjectIdGenerator"/> ensures that new identifiers are generated
    /// automatically during insertion if no explicit value is provided.
    /// </para>
    /// <para>
    /// The BSON representation is configured as ObjectId type for optimal storage and indexing
    /// performance in MongoDB, while maintaining string compatibility in .NET code.
    /// </para>
    /// </remarks>
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <summary>
    /// Gets or sets the timestamp when the entity was initially created.
    /// </summary>
    /// <value>
    /// A <see cref="DateTimeOffset"/> representing the creation time in UTC, automatically set to the current time.
    /// </value>
    /// <remarks>
    /// <para>
    /// This property is automatically initialized to the current UTC time when the entity is instantiated.
    /// Repository implementations typically preserve this value during insert operations to maintain
    /// accurate audit trails.
    /// </para>
    /// <para>
    /// The BSON representation is configured as string type to ensure consistent serialization
    /// and cross-platform compatibility, while preserving timezone information.
    /// </para>
    /// </remarks>
    [BsonRepresentation(BsonType.String)]
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Gets or sets the timestamp when the entity was last modified.
    /// </summary>
    /// <value>
    /// A <see cref="DateTimeOffset"/> representing the last modification time in UTC, automatically set to the current time.
    /// </value>
    /// <remarks>
    /// <para>
    /// This property is automatically initialized to the current UTC time when the entity is instantiated.
    /// Repository implementations typically update this value during update operations to track
    /// the most recent modifications.
    /// </para>
    /// <para>
    /// The BSON representation is configured as string type to ensure consistent serialization
    /// and cross-platform compatibility, while preserving timezone information.
    /// </para>
    /// </remarks>
    [BsonRepresentation(BsonType.String)]
    public DateTimeOffset Updated { get; set; } = DateTimeOffset.UtcNow;
}

// Ignore Spelling: Mongo

namespace MongoDB.Abstracts;

/// <summary>
/// Defines the contract for a MongoDB entity with standard auditing properties.
/// </summary>
/// <remarks>
/// This interface provides a common structure for MongoDB documents that require
/// identity and audit trail capabilities. All implementing entities will have
/// an identifier and timestamps for creation and modification tracking.
/// </remarks>
public interface IMongoEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the MongoDB document.
    /// </summary>
    /// <value>
    /// A string representing the unique identifier. In MongoDB, this typically
    /// corresponds to the _id field and should be unique across the collection.
    /// </value>
    /// <remarks>
    /// When using MongoDB.Driver, this property is commonly mapped to MongoDB's
    /// ObjectId or can be a custom string identifier.
    /// </remarks>
    string Id { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the entity was initially created.
    /// </summary>
    /// <value>
    /// A <see cref="DateTimeOffset"/> representing the creation time in UTC.
    /// </value>
    /// <remarks>
    /// This property should be set once when the entity is first persisted to the database
    /// and should remain immutable thereafter for audit trail purposes.
    /// </remarks>
    DateTimeOffset Created { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the entity was last modified.
    /// </summary>
    /// <value>
    /// A <see cref="DateTimeOffset"/> representing the last modification time in UTC.
    /// </value>
    /// <remarks>
    /// This property should be updated each time the entity is modified and persisted
    /// to the database. It provides a reliable way to track the most recent changes.
    /// </remarks>
    DateTimeOffset Updated { get; set; }
}

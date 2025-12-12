namespace MongoDB.Abstracts;

/// <summary>
/// Defines a contract for entities that have a unique identifier for MongoDB documents.
/// </summary>
/// <typeparam name="TKey">
/// The type of the unique identifier. Common types include <see cref="string"/>,
/// <see cref="System.Guid"/>, or <see cref="MongoDB.Bson.ObjectId"/>.
/// </typeparam>
public interface IMongoIdentifier<TKey>
{
    /// <summary>
    /// Gets or sets the unique identifier for the MongoDB document.
    /// </summary>
    /// <value>
    /// The unique identifier of type <typeparamref name="TKey"/>. In MongoDB, this typically
    /// corresponds to the _id field and must be unique across the collection.
    /// </value>
    /// <remarks>
    /// When using MongoDB.Driver, this property is commonly mapped to MongoDB's _id field.
    /// It can be an ObjectId, string, GUID, or any other type that serves as a unique identifier.
    /// </remarks>
    TKey Id { get; set; }
}

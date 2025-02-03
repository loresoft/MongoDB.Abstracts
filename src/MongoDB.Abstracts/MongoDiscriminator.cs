// Ignore Spelling: Mongo

using MongoDB.Driver;

namespace MongoDB.Abstracts;

/// <summary>
/// A MongoDB database connection discriminator. Used to register multiple instances.
/// </summary>
/// <typeparam name="TDiscriminator">The type of the connection discriminator.</typeparam>
public class MongoDiscriminator<TDiscriminator>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MongoDiscriminator{TDiscriminator}"/> class.
    /// </summary>
    /// <param name="mongoDatabase">The mongo database.</param>
    /// <exception cref="System.ArgumentNullException">mongoDatabase</exception>
    public MongoDiscriminator(IMongoDatabase mongoDatabase)
    {
        MongoDatabase = mongoDatabase ?? throw new ArgumentNullException(nameof(mongoDatabase));
    }

    /// <summary>
    /// Gets the mongo database.
    /// </summary>
    /// <value>
    /// The mongo database.
    /// </value>
    public IMongoDatabase MongoDatabase { get; }
}

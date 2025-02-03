// Ignore Spelling: Mongo

using MongoDB.Driver;

namespace MongoDB.Abstracts;

/// <summary>
/// A helper class for getting MongoDB database connection.
/// </summary>
public static class MongoFactory
{
    /// <summary>
    /// Gets the <see cref="IMongoDatabase"/> with the specified connection string.
    /// </summary>
    /// <param name="connectionString">The MongoDB connection string.</param>
    /// <returns>An instance of <see cref="IMongoDatabase"/>.</returns>
    public static IMongoDatabase GetDatabaseFromConnectionString(string connectionString)
    {
        if (connectionString is null)
            throw new ArgumentNullException(nameof(connectionString));

        var mongoUrl = new MongoUrl(connectionString);

        return GetDatabaseFromMongoUrl(mongoUrl);
    }

    /// <summary>
    /// Gets the <see cref="IMongoDatabase" /> with the specified <see cref="MongoUrl" />.
    /// </summary>
    /// <param name="mongoUrl">The mongo URL.</param>
    /// <returns>
    /// An instance of <see cref="IMongoDatabase" />.
    /// </returns>
    public static IMongoDatabase GetDatabaseFromMongoUrl(MongoUrl mongoUrl)
    {
        if (mongoUrl is null)
            throw new ArgumentNullException(nameof(mongoUrl));

        var client = new MongoClient(mongoUrl);
        var mongoDatabase = client.GetDatabase(mongoUrl.DatabaseName);

        return mongoDatabase;
    }
}

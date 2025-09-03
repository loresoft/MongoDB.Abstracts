// Ignore Spelling: Mongo

using MongoDB.Driver;

namespace MongoDB.Abstracts;

/// <summary>
/// Provides utility methods for creating MongoDB database connections from connection strings and URLs.
/// </summary>
/// <remarks>
/// <para>
/// This static helper class simplifies the process of creating <see cref="IMongoDatabase"/> instances
/// from connection strings or <see cref="MongoUrl"/> objects. It encapsulates the common pattern
/// of parsing connection strings, creating MongoDB clients, and retrieving database references.
/// </para>
/// <para>
/// The factory methods handle the standard MongoDB connection workflow including URL parsing,
/// client creation, and database name extraction, providing a convenient abstraction over
/// the MongoDB driver's connection establishment process.
/// </para>
/// <para>
/// All methods perform proper validation and error handling for connection parameters,
/// ensuring that invalid or null inputs result in appropriate exceptions rather than
/// runtime failures during database operations.
/// </para>
/// </remarks>
public static class MongoFactory
{
    /// <summary>
    /// Creates a MongoDB database instance from the specified connection string.
    /// </summary>
    /// <param name="connectionString">The MongoDB connection string containing server, authentication, and database information.</param>
    /// <returns>An <see cref="IMongoDatabase"/> instance configured according to the connection string parameters.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionString"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This method parses the provided connection string to extract MongoDB server information,
    /// authentication credentials, connection options, and the target database name. It creates
    /// a <see cref="MongoUrl"/> object from the connection string and delegates to the URL-based method.
    /// </para>
    /// <para>
    /// The connection string should follow standard MongoDB connection string format, including
    /// protocol scheme, server addresses, authentication parameters, and database name. Invalid
    /// connection strings will result in parsing exceptions from the MongoDB driver.
    /// </para>
    /// <para>
    /// The returned database instance is configured with all connection settings specified in
    /// the connection string, including read/write preferences, timeout values, and SSL settings.
    /// </para>
    /// </remarks>
    public static IMongoDatabase GetDatabaseFromConnectionString(string connectionString)
    {
        if (connectionString is null)
            throw new ArgumentNullException(nameof(connectionString));

        var mongoUrl = new MongoUrl(connectionString);

        return GetDatabaseFromMongoUrl(mongoUrl);
    }

    /// <summary>
    /// Creates a MongoDB database instance from the specified MongoDB URL object.
    /// </summary>
    /// <param name="mongoUrl">The <see cref="MongoUrl"/> object containing parsed connection information.</param>
    /// <returns>An <see cref="IMongoDatabase"/> instance configured according to the URL parameters.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="mongoUrl"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>
    /// This method creates a <see cref="MongoClient"/> instance using the provided <see cref="MongoUrl"/>
    /// and retrieves the database specified in the URL's database name property. The client is configured
    /// with all connection settings contained in the URL object.
    /// </para>
    /// <para>
    /// The <see cref="MongoUrl"/> object should contain a valid database name. If the database name
    /// is null or empty in the URL, the MongoDB driver will throw an appropriate exception during
    /// database retrieval.
    /// </para>
    /// <para>
    /// The returned database instance inherits all connection settings from the URL, including
    /// server addresses, authentication credentials, connection pooling options, and read/write preferences.
    /// The database is ready for immediate use with repository and query operations.
    /// </para>
    /// </remarks>
    public static IMongoDatabase GetDatabaseFromMongoUrl(MongoUrl mongoUrl)
    {
        if (mongoUrl is null)
            throw new ArgumentNullException(nameof(mongoUrl));

        var client = new MongoClient(mongoUrl);
        var mongoDatabase = client.GetDatabase(mongoUrl.DatabaseName);

        return mongoDatabase;
    }
}

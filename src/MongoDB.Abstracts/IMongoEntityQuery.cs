namespace MongoDB.Abstracts;

/// <summary>
/// Defines a contract for MongoDB query operations specifically for entities implementing <see cref="IMongoEntity"/>.
/// </summary>
/// <typeparam name="TEntity">The type of the MongoDB entity that implements <see cref="IMongoEntity"/>.</typeparam>
/// <remarks>
/// This interface extends <see cref="IMongoQuery{TEntity, TKey}"/> with a string key type,
/// providing specialized query operations for MongoDB entities with string-based identifiers.
/// It inherits all query capabilities including Find, FindOne, FindAll, Count operations,
/// and provides access to the underlying MongoDB collection.
/// </remarks>
/// <example>
/// <code>
/// public class UserQuery : IMongoEntityQuery&lt;User&gt;
/// {
///     // Implementation provides query operations for User entities
/// }
/// </code>
/// </example>
public interface IMongoEntityQuery<TEntity> : IMongoQuery<TEntity, string>
    where TEntity : class, IMongoEntity;

/// <summary>
/// Defines a contract for MongoDB query operations with connection discrimination support.
/// </summary>
/// <typeparam name="TDiscriminator">The type used to discriminate between different MongoDB connections or contexts.</typeparam>
/// <typeparam name="TEntity">The type of the MongoDB entity that implements <see cref="IMongoEntity"/>.</typeparam>
/// <remarks>
/// <para>
/// This interface extends <see cref="IMongoEntityQuery{TEntity}"/> to support scenarios where multiple
/// MongoDB connections or database contexts need to be distinguished using a discriminator type.
/// This is particularly useful in multi-tenant applications or when working with multiple databases.
/// </para>
/// <para>
/// The discriminator is typically used by dependency injection containers to register and resolve
/// multiple instances of the same entity query interface for different database contexts.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Using an enum as discriminator for different tenant databases
/// public enum TenantType { Primary, Secondary }
///
/// public class UserQuery : IMongoEntityQuery&lt;TenantType, User&gt;
/// {
///     // Implementation provides tenant-specific query operations
/// }
///
/// // Registration in DI container
/// services.AddScoped&lt;IMongoEntityQuery&lt;TenantType.Primary, User&gt;&gt;, UserQuery&gt;();
/// services.AddScoped&lt;IMongoEntityQuery&lt;TenantType.Secondary, User&gt;&gt;, UserQuery&gt;();
/// </code>
/// </example>
public interface IMongoEntityQuery<TDiscriminator, TEntity> : IMongoEntityQuery<TEntity>
    where TEntity : class, IMongoEntity;

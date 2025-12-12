namespace MongoDB.Abstracts;

/// <summary>
/// Defines a contract for MongoDB data operations specifically for entities implementing <see cref="IMongoEntity"/>.
/// </summary>
/// <typeparam name="TEntity">The type of the MongoDB entity that implements <see cref="IMongoEntity"/>.</typeparam>
/// <remarks>
/// <para>
/// This interface extends <see cref="IMongoRepository{TEntity, TKey}"/> with a string key type,
/// providing specialized repository operations for MongoDB entities with string-based identifiers.
/// It inherits all CRUD operations (Create, Read, Update, Delete) and query capabilities,
/// including Insert, Update, Upsert, Delete operations, and access to the underlying MongoDB collection.
/// </para>
/// <para>
/// Entities implementing <see cref="IMongoEntity"/> automatically include auditing properties
/// such as Id, Created, and Updated timestamps, which are managed by the repository operations.
/// </para>
/// </remarks>
public interface IMongoEntityRepository<TEntity> : IMongoRepository<TEntity, string>
    where TEntity : class, IMongoEntity;

/// <summary>
/// Defines a contract for MongoDB data operations with connection discrimination support.
/// </summary>
/// <typeparam name="TDiscriminator">The type used to discriminate between different MongoDB connections or database contexts.</typeparam>
/// <typeparam name="TEntity">The type of the MongoDB entity that implements <see cref="IMongoEntity"/>.</typeparam>
/// <remarks>
/// <para>
/// This interface extends <see cref="IMongoEntityRepository{TEntity}"/> to support scenarios where multiple
/// MongoDB connections or database contexts need to be distinguished using a discriminator type.
/// This is particularly useful in multi-tenant applications, microservices architectures, or when
/// working with multiple databases that contain the same entity types.
/// </para>
/// <para>
/// The discriminator type is typically used by dependency injection containers to register and resolve
/// multiple instances of the same entity repository interface for different database contexts.
/// Common discriminator patterns include enums, marker interfaces, or simple struct types that
/// represent different connection contexts.
/// </para>
/// <para>
/// This pattern enables type-safe dependency injection while maintaining clean separation between
/// different data contexts without requiring complex factory patterns or service locators.
/// </para>
/// </remarks>
public interface IMongoEntityRepository<TDiscriminator, TEntity> : IMongoEntityRepository<TEntity>
    where TEntity : class, IMongoEntity;

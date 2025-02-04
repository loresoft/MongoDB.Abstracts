using MongoDB.Driver;

namespace MongoDB.Abstracts.Tests.Services;


[RegisterSingleton]
public class UserRepository : MongoEntityRepository<Models.User>, IUserRepository
{
    public UserRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }

    protected override void BeforeInsert(Models.User entity)
    {
        base.BeforeInsert(entity);

        entity.EmailLower = entity.Email?.ToLowerInvariant();
    }

    protected override void BeforeUpdate(Models.User entity)
    {
        base.BeforeUpdate(entity);

        entity.EmailLower = entity.Email?.ToLowerInvariant();
    }

    protected override void EnsureIndexes(IMongoCollection<Models.User> mongoCollection)
    {
        base.EnsureIndexes(mongoCollection);

        mongoCollection.Indexes.CreateOne(
            new CreateIndexModel<Models.User>(
                Builders<Models.User>.IndexKeys.Ascending(s => s.EmailLower),
                new CreateIndexOptions { Unique = true }
            )
        );
    }

}

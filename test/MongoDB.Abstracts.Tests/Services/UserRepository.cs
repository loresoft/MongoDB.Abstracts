using MongoDB.Abstracts.Tests.Models;
using MongoDB.Driver;

namespace MongoDB.Abstracts.Tests.Services;


public class UserRepository : MongoEntityRepository<User>, IUserRepository
{
    public UserRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }

    protected override void BeforeUpdate(User entity)
    {
        if (string.IsNullOrEmpty(entity.Id))
            entity.EmailLower = entity.Email?.ToLowerInvariant();
    }

}

using MongoDB.Abstracts.Tests.Models;
using MongoDB.Driver;

namespace MongoDB.Abstracts.Tests.Services;


[RegisterSingleton]
public class UserRepository : MongoEntityRepository<MongoDB.Abstracts.Tests.Models.User>, IUserRepository
{
    public UserRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }

    protected override void BeforeUpdate(MongoDB.Abstracts.Tests.Models.User entity)
    {
        if (string.IsNullOrEmpty(entity.Id))
            entity.EmailLower = entity.Email?.ToLowerInvariant();
    }

}

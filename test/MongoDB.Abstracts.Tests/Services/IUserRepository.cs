using MongoDB.Abstracts.Tests.Models;

namespace MongoDB.Abstracts.Tests.Services;

public interface IUserRepository : IMongoEntityRepository<MongoDB.Abstracts.Tests.Models.User>
{

}

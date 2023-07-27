using MongoDB.Abstracts.Tests.Models;
using MongoDB.Driver;

namespace MongoDB.Abstracts.Tests.Services;


public class EntryRepository : MongoEntityRepository<Entry>, IEntryRepository
{
    public EntryRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }
}

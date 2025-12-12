using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using MongoDB.Abstracts.Tests.Models;
using MongoDB.Bson;


namespace MongoDB.Abstracts.Tests;

public class ItemRepositoryTest(DatabaseFixture databaseFixture) : DatabaseTestBase(databaseFixture)
{

    [Fact]
    public async Task ItemEntityRepositoryCrudAsyncTest()
    {
        var generator = new Faker<Item>()
            .RuleFor(p => p.Id, _ => ObjectId.GenerateNewId().ToString())
            .RuleFor(p => p.Name, f => f.Name.FullName())
            .RuleFor(p => p.Description, f => f.Lorem.Sentence())
            .RuleFor(p => p.OwnerId, f => f.PickRandom(Constants.Owners));

        var item = generator.Generate();

        var repository = Services.GetRequiredService<IMongoEntityRepository<Item>>();
        repository.Should().NotBeNull();

        // create
        var createResult = await repository.InsertAsync(item);
        createResult.Should().NotBeNull();
        createResult.Id.Should().Be(item.Id);

        // read
        var readResult = await repository.FindAsync(item.Id);
        readResult.Should().NotBeNull();
        readResult.Id.Should().Be(item.Id);
        readResult.OwnerId.Should().Be(item.OwnerId);

        // update
        readResult.Name = "Big " + readResult.Name;

        var updateResult = await repository.UpdateAsync(readResult);
        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(item.Id);
        updateResult.OwnerId.Should().Be(item.OwnerId);

        // query
        var queryResult = await repository.FindOneAsync(r => r.Name.StartsWith("Big"));
        queryResult.Should().NotBeNull();

        var queryResults = await repository.FindAllAsync(r => r.Name.StartsWith("Big"));
        queryResults.Should().NotBeNull();
        queryResults.Count.Should().BeGreaterThan(0);

        // count
        var fullCount = await repository.CountAsync();
        fullCount.Should().BeGreaterThan(0);

        var count = await repository.CountAsync(r => r.Name.StartsWith("Big"));
        count.Should().BeGreaterThan(0);


        // delete
        await repository.DeleteAsync(readResult);

        var deletedResult = await repository.FindAsync(item.Id);
        deletedResult.Should().BeNull();
    }

    [Fact]
    public async Task ItemRepositoryCrudAsyncTest()
    {
        var generator = new Faker<Item>()
            .RuleFor(p => p.Id, _ => ObjectId.GenerateNewId().ToString())
            .RuleFor(p => p.Name, f => f.Name.FullName())
            .RuleFor(p => p.Description, f => f.Lorem.Sentence())
            .RuleFor(p => p.OwnerId, f => f.PickRandom(Constants.Owners));

        var item = generator.Generate();

        var repository = Services.GetRequiredService<IMongoRepository<Item, string>>();
        repository.Should().NotBeNull();

        // create
        var createResult = await repository.InsertAsync(item);
        createResult.Should().NotBeNull();
        createResult.Id.Should().Be(item.Id);

        // read
        var readResult = await repository.FindAsync(item.Id);
        readResult.Should().NotBeNull();
        readResult.Id.Should().Be(item.Id);
        readResult.OwnerId.Should().Be(item.OwnerId);

        // update
        readResult.Name = "Big " + readResult.Name;

        var updateResult = await repository.UpdateAsync(readResult);
        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(item.Id);
        updateResult.OwnerId.Should().Be(item.OwnerId);

        // query
        var queryResult = await repository.FindOneAsync(r => r.Name.StartsWith("Big"));
        queryResult.Should().NotBeNull();

        var queryResults = await repository.FindAllAsync(r => r.Name.StartsWith("Big"));
        queryResults.Should().NotBeNull();
        queryResults.Count.Should().BeGreaterThan(0);

        // count
        var fullCount = await repository.CountAsync();
        fullCount.Should().BeGreaterThan(0);

        var count = await repository.CountAsync(r => r.Name.StartsWith("Big"));
        count.Should().BeGreaterThan(0);


        // delete
        await repository.DeleteAsync(readResult);

        var deletedResult = await repository.FindAsync(item.Id);
        deletedResult.Should().BeNull();
    }

    [Fact]
    public void ItemEntityRepositoryCrudSyncTest()
    {
        var generator = new Faker<Item>()
            .RuleFor(p => p.Id, _ => ObjectId.GenerateNewId().ToString())
            .RuleFor(p => p.Name, f => f.Name.FullName())
            .RuleFor(p => p.Description, f => f.Lorem.Sentence())
            .RuleFor(p => p.OwnerId, f => f.PickRandom(Constants.Owners));

        var item = generator.Generate();

        var repository = Services.GetRequiredService<IMongoEntityRepository<Item>>();
        repository.Should().NotBeNull();

        // create
        var createResult = repository.Insert(item);
        createResult.Should().NotBeNull();
        createResult.Id.Should().Be(item.Id);

        // read
        var readResult = repository.Find(item.Id);
        readResult.Should().NotBeNull();
        readResult.Id.Should().Be(item.Id);
        readResult.OwnerId.Should().Be(item.OwnerId);

        // update
        readResult.Name = "Big " + readResult.Name;

        var updateResult = repository.Update(readResult);
        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(item.Id);
        updateResult.OwnerId.Should().Be(item.OwnerId);

        // query
        var queryResult = repository.FindOne(r => r.Name.StartsWith("Big"));
        queryResult.Should().NotBeNull();

        var queryResults = repository.FindAll(r => r.Name.StartsWith("Big")).ToList();
        queryResults.Should().NotBeNull();
        queryResults.Count.Should().BeGreaterThan(0);

        // count
        var fullCount = repository.Count();
        fullCount.Should().BeGreaterThan(0);

        var count = repository.Count(r => r.Name.StartsWith("Big"));
        count.Should().BeGreaterThan(0);

        // delete
        repository.Delete(readResult);

        var deletedResult = repository.Find(item.Id);
        deletedResult.Should().BeNull();
    }

    [Fact]
    public void ItemRepositoryCrudSyncTest()
    {
        var generator = new Faker<Item>()
            .RuleFor(p => p.Id, _ => ObjectId.GenerateNewId().ToString())
            .RuleFor(p => p.Name, f => f.Name.FullName())
            .RuleFor(p => p.Description, f => f.Lorem.Sentence())
            .RuleFor(p => p.OwnerId, f => f.PickRandom(Constants.Owners));

        var item = generator.Generate();

        var repository = Services.GetRequiredService<IMongoRepository<Item, string>>();
        repository.Should().NotBeNull();

        // create
        var createResult = repository.Insert(item);
        createResult.Should().NotBeNull();
        createResult.Id.Should().Be(item.Id);

        // read
        var readResult = repository.Find(item.Id);
        readResult.Should().NotBeNull();
        readResult.Id.Should().Be(item.Id);
        readResult.OwnerId.Should().Be(item.OwnerId);

        // update
        readResult.Name = "Big " + readResult.Name;

        var updateResult = repository.Update(readResult);
        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(item.Id);
        updateResult.OwnerId.Should().Be(item.OwnerId);

        // query
        var queryResult = repository.FindOne(r => r.Name.StartsWith("Big"));
        queryResult.Should().NotBeNull();

        var queryResults = repository.FindAll(r => r.Name.StartsWith("Big")).ToList();
        queryResults.Should().NotBeNull();
        queryResults.Count.Should().BeGreaterThan(0);

        // count
        var fullCount = repository.Count();
        fullCount.Should().BeGreaterThan(0);

        var count = repository.Count(r => r.Name.StartsWith("Big"));
        count.Should().BeGreaterThan(0);

        // delete
        repository.Delete(readResult);

        var deletedResult = repository.Find(item.Id);
        deletedResult.Should().BeNull();
    }
}

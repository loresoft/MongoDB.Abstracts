using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using MongoDB.Abstracts.Tests.Models;
using MongoDB.Abstracts.Tests.Services;
using MongoDB.Bson;

namespace MongoDB.Abstracts.Tests;

public class UserRepositoryTest : DatabaseTestBase
{
    public UserRepositoryTest(ITestOutputHelper output, DatabaseFixture databaseFixture) : base(output, databaseFixture)
    {
    }

    [Fact]
    public async Task FullTestAsync()
    {
        var generator = new Faker<User>()
            .RuleFor(p => p.Id, _ => ObjectId.GenerateNewId().ToString())
            .RuleFor(p => p.Name, f => f.Name.FullName())
            .RuleFor(p => p.Email, f => f.Internet.Email())
            .RuleFor(p => p.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(p => p.SecurityStamp, f => Guid.NewGuid().ToString("N"));

        var item = generator.Generate();

        var repository = Services.GetRequiredService<IUserRepository>();
        repository.Should().NotBeNull();

        // create
        var createResult = await repository.InsertAsync(item);
        createResult.Should().NotBeNull();
        createResult.Id.Should().Be(item.Id);

        // read
        var readResult = await repository.FindAsync(item.Id);
        readResult.Should().NotBeNull();
        readResult.Id.Should().Be(item.Id);
        readResult.Email.Should().Be(item.Email);

        // update
        readResult.Name = "Big " + readResult.Name;

        var updateResult = await repository.UpdateAsync(readResult);
        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(item.Id);
        updateResult.Email.Should().Be(item.Email);

        // query
        var queryResult = await repository.FindOneAsync(r => r.Name.StartsWith("Big"));
        queryResult.Should().NotBeNull();

        var queryResults = await repository.FindAllAsync(r => r.Name.StartsWith("Big"));
        queryResults.Should().NotBeNull();
        queryResults.Count.Should().BeGreaterThan(0);

        // delete
        await repository.DeleteAsync(readResult);

        var deletedResult = await repository.FindAsync(item.Id);
        deletedResult.Should().BeNull();
    }

    [Fact]
    public void FullTest()
    {
        var generator = new Faker<User>()
            .RuleFor(p => p.Id, _ => ObjectId.GenerateNewId().ToString())
            .RuleFor(p => p.Name, f => f.Name.FullName())
            .RuleFor(p => p.Email, f => f.Internet.Email())
            .RuleFor(p => p.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(p => p.SecurityStamp, f => Guid.NewGuid().ToString("N"));

        var item = generator.Generate();

        var repository = Services.GetRequiredService<IUserRepository>();
        repository.Should().NotBeNull();

        // create
        var createResult = repository.Insert(item);
        createResult.Should().NotBeNull();
        createResult.Id.Should().Be(item.Id);

        // read
        var readResult = repository.Find(item.Id);
        readResult.Should().NotBeNull();
        readResult.Id.Should().Be(item.Id);
        readResult.Email.Should().Be(item.Email);

        // update
        readResult.Name = "Big " + readResult.Name;

        var updateResult = repository.Update(readResult);
        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(item.Id);
        updateResult.Email.Should().Be(item.Email);

        // query
        var queryResult = repository.FindOne(r => r.Name.StartsWith("Big"));
        queryResult.Should().NotBeNull();

        var queryResults = repository.FindAll(r => r.Name.StartsWith("Big")).ToList();
        queryResults.Should().NotBeNull();
        queryResults.Count.Should().BeGreaterThan(0);

        // delete
        repository.Delete(readResult);

        var deletedResult = repository.Find(item.Id);
        deletedResult.Should().BeNull();
    }
}

using Microsoft.Extensions.DependencyInjection;

using MongoDB.Abstracts.Tests.Models;
using MongoDB.Abstracts.Tests.Services;
using MongoDB.Driver;

namespace MongoDB.Abstracts.Tests;


public class UserRepositoryTest : DatabaseTestBase
{
    public UserRepositoryTest(ITestOutputHelper output, DatabaseFixture databaseFixture) : base(output, databaseFixture)
    {
    }

    [Fact]
    public void ResolveMongoDatabase()
    {
        var mongoDatabase = Services.GetRequiredService<IMongoDatabase>();
        mongoDatabase.Should().NotBeNull();
    }

    [Fact]
    public void ResolveMongoEntityRepository()
    {
        var mongoEntityRepo = Services.GetRequiredService<IMongoEntityRepository<User>>();
        mongoEntityRepo.Should().NotBeNull();
        mongoEntityRepo.Should().BeOfType<UserRepository>();
    }

    [Fact]
    public void ResolveMongoEntityQuery()
    {
        var mongoEntityRepo = Services.GetRequiredService<IMongoEntityQuery<User>>();
        mongoEntityRepo.Should().NotBeNull();
    }

    [Fact]
    public void ResolveIUserRepository()
    {
        var userRepo = Services.GetRequiredService<IUserRepository>();
        userRepo.Should().NotBeNull();
        userRepo.Should().BeOfType<UserRepository>();
    }

    [Fact]
    public void ResolveMongoQuery()
    {
        var mongoQueryRepo = Services.GetRequiredService<IMongoQuery<User, string>>();
        mongoQueryRepo.Should().NotBeNull();
        mongoQueryRepo.Should().BeOfType<UserRepository>();
    }

    [Fact]
    public void ResolveMongoRepository()
    {

        var mongoRepo = Services.GetRequiredService<IMongoRepository<User, string>>();
        mongoRepo.Should().NotBeNull();
        mongoRepo.Should().BeOfType<UserRepository>();
    }

    [Fact]
    public void ResolveUserRepository()
    {
        var userClassRepo = Services.GetRequiredService<UserRepository>();
        userClassRepo.Should().NotBeNull();
        userClassRepo.Should().BeOfType<UserRepository>();
    }




    [Fact]
    public void ResolveMongoDatabaseWithServiceKey()
    {
        var mongoDatabase = Services.GetRequiredKeyedService<IMongoDatabase>("MongoUnitTest");
        mongoDatabase.Should().NotBeNull();
    }

    [Fact]
    public void ResolveMongoEntityRepositoryWithServiceKey()
    {
        var mongoEntityRepo = Services.GetRequiredKeyedService<IMongoEntityRepository<User>>("MongoUnitTest");
        mongoEntityRepo.Should().NotBeNull();
    }

    [Fact]
    public void ResolveMongoEntityQueryWithServiceKey()
    {
        var mongoEntityRepo = Services.GetRequiredKeyedService<IMongoEntityQuery<User>>("MongoUnitTest");
        mongoEntityRepo.Should().NotBeNull();
    }


}

using Microsoft.Extensions.DependencyInjection;

using MongoDB.Abstracts.Tests.Models;
using MongoDB.Abstracts.Tests.Services;
using MongoDB.Driver;

namespace MongoDB.Abstracts.Tests;


public class DependencyInjectionTest(DatabaseFixture databaseFixture) : DatabaseTestBase(databaseFixture)
{

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
    public void ResolveMongoDatabaseWithDiscriminator()
    {
        var mongoDatabase = Services.GetRequiredService<MongoDiscriminator<DiscriminatorConnection>>();
        mongoDatabase.Should().NotBeNull();
        mongoDatabase.MongoDatabase.DatabaseNamespace.DatabaseName.Should().Be("DiscriminatorUnitTesting");
    }

    [Fact]
    public void ResolveMongoEntityRepositoryWithDiscriminator()
    {
        var mongoEntityRepo = Services.GetRequiredService<IMongoEntityRepository<DiscriminatorConnection, User>>();
        mongoEntityRepo.Should().NotBeNull();

        var collection = mongoEntityRepo.Collection;
        collection.Should().NotBeNull();
        collection.CollectionNamespace.FullName.Should().Be("DiscriminatorUnitTesting.User");
    }

    [Fact]
    public void ResolveMongoRepositoryWithDiscriminator()
    {
        var mongoRepo = Services.GetRequiredService<IMongoRepository<DiscriminatorConnection, User, string>>();
        mongoRepo.Should().NotBeNull();

        var collection = mongoRepo.Collection;
        collection.Should().NotBeNull();
        collection.CollectionNamespace.FullName.Should().Be("DiscriminatorUnitTesting.User");
    }

    [Fact]
    public void ResolveMongoEntityQueryWithDiscriminator()
    {
        var mongoEntityRepo = Services.GetRequiredService<IMongoEntityQuery<DiscriminatorConnection, User>>();
        mongoEntityRepo.Should().NotBeNull();

        var collection = mongoEntityRepo.Collection;
        collection.Should().NotBeNull();
        collection.CollectionNamespace.FullName.Should().Be("DiscriminatorUnitTesting.User");
    }

    [Fact]
    public void ResolveMongoQueryWithDiscriminator()
    {
        var mongoRepo = Services.GetRequiredService<IMongoQuery<DiscriminatorConnection, User, string>>();
        mongoRepo.Should().NotBeNull();

        var collection = mongoRepo.Collection;
        collection.Should().NotBeNull();
        collection.CollectionNamespace.FullName.Should().Be("DiscriminatorUnitTesting.User");
    }

    [Fact]
    public void ResolveMongoDatabaseWithServiceKey()
    {
        var mongoDatabase = Services.GetRequiredKeyedService<IMongoDatabase>("MongoKeyedDatabase");
        mongoDatabase.Should().NotBeNull();
        mongoDatabase.DatabaseNamespace.DatabaseName.Should().Be("MongoKeyedDatabase");
    }

}

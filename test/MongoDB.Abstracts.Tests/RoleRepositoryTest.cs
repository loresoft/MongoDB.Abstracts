using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using MongoDB.Abstracts.Tests.Models;
using MongoDB.Bson;

namespace MongoDB.Abstracts.Tests;


public class RoleRepositoryTest(DatabaseFixture databaseFixture) : DatabaseTestBase(databaseFixture)
{

    [Fact]
    public async Task CreateRole()
    {
        var role = new Role
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "CreateRole",
            NormalizedName = "createrole"
        };

        var roleRepo = Services.GetRequiredService<IMongoEntityRepository<Role>>();
        roleRepo.Should().NotBeNull();

        var result = await roleRepo.InsertAsync(role);
        result.Should().NotBeNull();
        result.Id.Should().Be(role.Id);
    }

    [Fact]
    public async Task CreateRoleBatch()
    {
        var roles = new[] {
            new Role
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "BatchRole",
                NormalizedName = "batchrole"
            },
            new Role
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "BatchRole",
                NormalizedName = "batchrole"
            }
        };

        var roleRepo = Services.GetRequiredService<IMongoEntityRepository<Role>>();
        roleRepo.Should().NotBeNull();

        roleRepo.InsertBatch(roles);

        var roleQuery = Services.GetRequiredService<IMongoEntityQuery<Role>>();
        var results = await roleQuery.FindAllAsync(r => r.NormalizedName == "batchrole");
        results.Should().NotBeNull();
        results.Count.Should().BeGreaterThan(0);
    }


    [Fact]
    public async Task SaveRoleAsync()
    {
        var role = new Role
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "SaveRole",
            NormalizedName = "saverole"
        };

        var roleRepo = Services.GetRequiredService<IMongoEntityRepository<Role>>();
        roleRepo.Should().NotBeNull();

        var result = await roleRepo.UpsertAsync(role);
        result.Should().NotBeNull();
        result.Id.Should().Be(role.Id);
    }

    [Fact]
    public void SaveRole()
    {
        var role = new Role
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "SaveRole",
            NormalizedName = "saverole"
        };

        var roleRepo = Services.GetRequiredService<IMongoEntityRepository<Role>>();
        roleRepo.Should().NotBeNull();

        var result = roleRepo.Upsert(role);
        result.Should().NotBeNull();
        result.Id.Should().Be(role.Id);
    }

    [Fact]
    public async Task DeleteRoleAsync()
    {
        var role = new Role
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "DeleteRole",
            NormalizedName = "deleterole"
        };

        var roleRepo = Services.GetRequiredService<IMongoEntityRepository<Role>>();
        roleRepo.Should().NotBeNull();

        var result = await roleRepo.UpsertAsync(role);
        result.Should().NotBeNull();
        result.Id.Should().Be(role.Id);

        var deleted = await roleRepo.DeleteAllAsync(r => r.NormalizedName == "deleterole");
        deleted.Should().BeGreaterThan(0);
    }

    [Fact]
    public void DeleteRole()
    {
        var role = new Role
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "DeleteRole",
            NormalizedName = "deleterole"
        };

        var roleRepo = Services.GetRequiredService<IMongoEntityRepository<Role>>();
        roleRepo.Should().NotBeNull();

        var result = roleRepo.Upsert(role);
        result.Should().NotBeNull();
        result.Id.Should().Be(role.Id);

        var deleted = roleRepo.DeleteAll(r => r.NormalizedName == "deleterole");
        deleted.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CreateUpdateRole()
    {
        var role = new Role
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "CreateRole",
            NormalizedName = "createrole"
        };

        var roleRepo = Services.GetRequiredService<IMongoEntityRepository<Role>>();
        roleRepo.Should().NotBeNull();

        var createResult = await roleRepo.InsertAsync(role);
        createResult.Should().NotBeNull();
        createResult.Id.Should().Be(role.Id);

        createResult.Name = "CreateUpdateRole";
        createResult.NormalizedName = "createupdaterole";

        var updateResult = await roleRepo.UpdateAsync(createResult);
        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(role.Id);
    }

    [Fact]
    public async Task CreateReadRole()
    {
        var role = new Role
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "CreateReadRole",
            NormalizedName = "createreadrole"
        };

        var roleRepo = Services.GetRequiredService<IMongoEntityRepository<Role>>();
        roleRepo.Should().NotBeNull();

        var createResult = await roleRepo.InsertAsync(role);
        createResult.Should().NotBeNull();
        createResult.Id.Should().Be(role.Id);

        var readResult = await roleRepo.FindAsync(role.Id);
        readResult.Should().NotBeNull();
        readResult.Id.Should().Be(role.Id);
    }

    [Fact]
    public async Task CreateDeleteRole()
    {
        var role = new Role
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "CreateDeleteRole",
            NormalizedName = "createdeleterole"
        };

        var roleRepo = Services.GetRequiredService<IMongoEntityRepository<Role>>();
        roleRepo.Should().NotBeNull();

        var createResult = await roleRepo.InsertAsync(role);
        createResult.Should().NotBeNull();
        createResult.Id.Should().Be(role.Id);

        await roleRepo.DeleteAsync(role.Id);

        var findResult = await roleRepo.FindAsync(role.Id);
        findResult.Should().BeNull();
    }

    [Fact]
    public async Task FindAllStartsWith()
    {
        var role = new Role
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "CreateRole",
            NormalizedName = "createrole"
        };

        var roleRepo = Services.GetRequiredService<IMongoEntityRepository<Role>>();
        roleRepo.Should().NotBeNull();

        var createResult = await roleRepo.InsertAsync(role);
        createResult.Should().NotBeNull();
        createResult.Id.Should().Be(role.Id);

        var results = await roleRepo.FindAllAsync(r => r.Name.StartsWith("Create"));
        results.Should().NotBeNull();
        results.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task FindOneStartsWith()
    {
        var role = new Role
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "CreateRole",
            NormalizedName = "createrole"
        };

        var roleRepo = Services.GetRequiredService<IMongoEntityRepository<Role>>();
        roleRepo.Should().NotBeNull();

        var createResult = await roleRepo.InsertAsync(role);
        createResult.Should().NotBeNull();
        createResult.Id.Should().Be(role.Id);

        var findResult = await roleRepo.FindOneAsync(r => r.Name.StartsWith("Create"));
        findResult.Should().NotBeNull();
    }


    [Fact]
    public async Task FindAllEmpty()
    {
        var roleRepo = Services.GetRequiredService<IMongoEntityRepository<Role>>();
        roleRepo.Should().NotBeNull();

        var results = await roleRepo.FindAllAsync(r => r.Name == "blah" + DateTime.Now.Ticks);
        results.Should().NotBeNull();
        results.Count.Should().Be(0);
    }

    [Fact]
    public async Task Quearyable()
    {
        var role = new Role
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "CreateRole",
            NormalizedName = "createrole"
        };

        var roleRepo = Services.GetRequiredService<IMongoEntityRepository<Role>>();
        roleRepo.Should().NotBeNull();

        var createResult = await roleRepo.InsertAsync(role);
        createResult.Should().NotBeNull();
        createResult.Id.Should().Be(role.Id);

        var findResult = roleRepo.All()
            .Where(r => r.Name.StartsWith("Create"))
            .ToList();

        findResult.Should().NotBeNull();
    }
}

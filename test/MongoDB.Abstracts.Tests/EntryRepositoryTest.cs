using System;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using MongoDB.Abstracts.Tests.Models;
using MongoDB.Abstracts.Tests.Services;
using MongoDB.Driver;

using Xunit;
using Xunit.Abstractions;

namespace MongoDB.Abstracts.Tests;


public class EntryRepositoryTest : TestServiceBase
{
    public EntryRepositoryTest(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.Scan(scan => scan
            .FromAssemblyOf<UserRepository>()
            .AddClasses(classes => classes.AssignableTo(typeof(IMongoQuery<,>))).AsImplementedInterfaces().AsSelf()
            .WithSingletonLifetime()
        );
    }

    [Fact]
    public void ResolveServices()
    {
        var mongoEntityRepo = Services.GetRequiredService<IMongoEntityRepository<Entry>>();
        mongoEntityRepo.Should().NotBeNull();
        mongoEntityRepo.Should().BeOfType<EntryRepository>();

        var mongoQueryRepo = Services.GetRequiredService<IMongoQuery<Entry, string>>();
        mongoQueryRepo.Should().NotBeNull();
        mongoQueryRepo.Should().BeOfType<EntryRepository>();

        var mongoRepo = Services.GetRequiredService<IMongoRepository<Entry, string>>();
        mongoRepo.Should().NotBeNull();
        mongoRepo.Should().BeOfType<EntryRepository>();

        var entryRepo = Services.GetRequiredService<IEntryRepository>();
        entryRepo.Should().NotBeNull();
        entryRepo.Should().BeOfType<EntryRepository>();

        var entryClassRepo = Services.GetRequiredService<EntryRepository>();
        entryClassRepo.Should().NotBeNull();
        entryClassRepo.Should().BeOfType<EntryRepository>();
    }

    [Fact]
    public async Task FullTestAsync()
    {
        var item = new Entry
        {
            Name = "Testing " + DateTime.Now.Ticks,
            EntryDate = DateTimeOffset.UtcNow
        };

        var repository = Services.GetRequiredService<IMongoEntityRepository<Entry>>();
        repository.Should().NotBeNull();

        // create
        var createResult = await repository.InsertAsync(item);
        createResult.Should().NotBeNull();
        createResult.Id.Should().Be(item.Id);

        // read
        var readResult = await repository.FindAsync(item.Id);
        readResult.Should().NotBeNull();
        readResult.Id.Should().Be(item.Id);

        // update
        readResult.Name = "Big " + readResult.Name;

        var updateResult = await repository.UpdateAsync(readResult);
        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(item.Id);

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
        var item = new Entry
        {
            Name = "Testing " + DateTime.Now.Ticks,
            EntryDate = DateTimeOffset.UtcNow
        };

        var repository = Services.GetRequiredService<IMongoEntityRepository<Entry>>();
        repository.Should().NotBeNull();

        // create
        var createResult = repository.Insert(item);
        createResult.Should().NotBeNull();
        createResult.Id.Should().Be(item.Id);

        // read
        var readResult = repository.Find(item.Id);
        readResult.Should().NotBeNull();
        readResult.Id.Should().Be(item.Id);

        // update
        readResult.Name = "Big " + readResult.Name;

        var updateResult = repository.Update(readResult);
        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(item.Id);

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

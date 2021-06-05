using System.Linq;
using System.Threading.Tasks;
using DataGenerator;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Abstracts.Tests.Models;
using MongoDB.Abstracts.Tests.Profiles;
using Xunit;
using Xunit.Abstractions;

namespace MongoDB.Abstracts.Tests
{

    public class CommentRepositoryTest : TestServiceBase
    {
        public CommentRepositoryTest(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddSingleton(_ => Generator.Create(c => c.Profile<CommentProfile>()));
        }

        [Fact]
        public async Task FullTestAsync()
        {
            var generator = Services.GetRequiredService<Generator>();

            var item = generator.Single<Comment>();

            var repository = Services.GetRequiredService<IMongoEntityRepository<Comment>>();
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

            // delete
            await repository.DeleteAsync(readResult);

            var deletedResult = await repository.FindAsync(item.Id);
            deletedResult.Should().BeNull();
        }

        [Fact]
        public void FullTest()
        {
            var generator = Services.GetRequiredService<Generator>();

            var item = generator.Single<Comment>();

            var repository = Services.GetRequiredService<IMongoEntityRepository<Comment>>();
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

            // delete
            repository.Delete(readResult);

            var deletedResult = repository.Find(item.Id);
            deletedResult.Should().BeNull();
        }
    }

}

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using MongoDB.Abstracts.Tests.Models;
using MongoDB.Abstracts.Tests.Services;
using MongoDB.Driver;

using Xunit;
using Xunit.Abstractions;

namespace MongoDB.Abstracts.Tests
{

    public class UserRepositoryTest : TestServiceBase
    {
        public UserRepositoryTest(ITestOutputHelper outputHelper) : base(outputHelper)
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
        public void ResolveMongoDatabase()
        {
            var cosmosFactory = Services.GetRequiredService<IMongoDatabase>();
            cosmosFactory.Should().NotBeNull();
        }

        [Fact]
        public void ResolveMongoEntityRepository()
        {
            var mongoEntityRepo = Services.GetRequiredService<IMongoEntityRepository<User>>();
            mongoEntityRepo.Should().NotBeNull();
            mongoEntityRepo.Should().BeOfType<UserRepository>();
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
    }
}

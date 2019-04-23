using System;
using DataGenerator;
using FluentAssertions;
using MongoDB.Abstracts.Tests.Data;
using MongoDB.Bson;
using Xunit;

namespace MongoDB.Repository.Tests
{
    public class UserRepositoryTests
    {
        [Fact]
        public void InsertAndFind()
        {
            Generator.Default.Configuration.Assemblies.IncludeAssemblyFor<User>();

            var user = Generator.Default.Single<User>(c => c.Property(p => p.Id).Value(v => ObjectId.GenerateNewId().ToString()));
            var repo = new UserRepository();
            repo.Insert(user);

            user.Id.Should().NotBeNullOrEmpty();

            var u = repo.Find(user.Id);
            u.Should().NotBeNull();
        }
    }
}
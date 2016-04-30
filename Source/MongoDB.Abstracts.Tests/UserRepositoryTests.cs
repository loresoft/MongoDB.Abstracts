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
            var user = Generator.Default.Single<User>(c => c.Property(p => p.Id).Value(() => ObjectId.GenerateNewId().ToString()));
            var repo = new UserRepository();
            repo.Insert(user);

            user.Id.Should().NotBeNullOrEmpty();

            var u = repo.Find(user.Id);
            u.Should().NotBeNull();
        }
    }
}
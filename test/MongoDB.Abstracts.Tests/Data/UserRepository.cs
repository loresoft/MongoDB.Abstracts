using System;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace MongoDB.Abstracts.Tests.Data
{
    public class UserRepository : MongoRepository<User, string>
    {
        public UserRepository() : base(MongoFactory.GetDatabaseFromConnectionString("mongodb://localhost/Testing"))
        {
        }

        public override string EntityKey(User entity)
        {
            return entity.Id;
        }

        protected override Expression<Func<User, bool>> KeyExpression(string key)
        {
            return user => user.Id == key;
        }
    }
}
using DataGenerator;
using DataGenerator.Sources;
using MongoDB.Bson;

namespace MongoDB.Abstracts.Tests.Profiles
{
    public class UserProfile : MappingProfile<Tests.Models.User>
    {
        public override void Configure()
        {
            Property(p => p.Id).Value(_ => ObjectId.GenerateNewId().ToString());
            Property(p => p.Name).DataSource<NameSource>();
            Property(p => p.Email).DataSource<EmailSource>();
        }
    }
}

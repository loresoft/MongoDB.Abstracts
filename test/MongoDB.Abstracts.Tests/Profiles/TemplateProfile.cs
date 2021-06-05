using System;
using DataGenerator;
using DataGenerator.Sources;
using MongoDB.Abstracts.Tests.Models;
using MongoDB.Bson;

namespace MongoDB.Abstracts.Tests.Profiles
{
    public class TemplateProfile : MappingProfile<Template>
    {
        public override void Configure()
        {
            Property(p => p.Id).Value(_ => ObjectId.GenerateNewId().ToString());
            Property(p => p.Name).DataSource<NameSource>();
            Property(p => p.Description).DataSource<LoremIpsumSource>();
            Property(p => p.OwnerId).DataSource(Constants.Owners);
        }
    }
}

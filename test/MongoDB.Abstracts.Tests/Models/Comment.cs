using System;

namespace MongoDB.Abstracts.Tests.Models
{
    public class Comment : IMongoEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string OwnerId { get; set; }

        public DateTimeOffset Created { get; set; }
        
        public string CreatedBy { get; set; }
        
        public DateTimeOffset Updated { get; set; }
        
        public string UpdatedBy { get; set; }
    }
}

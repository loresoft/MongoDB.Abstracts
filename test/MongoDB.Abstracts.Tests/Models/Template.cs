namespace MongoDB.Abstracts.Tests.Models
{
    public class Template : MongoEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string OwnerId { get; set; }
    }
}

using System;

namespace MongoDB.Abstracts.Tests.Models
{
    public class Entry : MongoEntity
    {
        public string Name { get; set; }

        public DateTimeOffset EntryDate { get; set; }
    }
}
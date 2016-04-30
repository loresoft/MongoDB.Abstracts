using System;
using MongoDB.Repository.Tests;

namespace MongoDB.Abstracts.Tests.Data
{
    public class User : MongoEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public string Note { get; set; }
        public decimal Budget { get; set; }

        public string Password { get; set; }

        public Status Status { get; set; }

        public bool IsActive { get; set; }
    }
}

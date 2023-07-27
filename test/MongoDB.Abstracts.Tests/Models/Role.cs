using System.Collections.Generic;

namespace MongoDB.Abstracts.Tests.Models;


public class Role : MongoEntity
{
    public Role()
    {
        Claims = new List<Claim>();
    }


    public string Name { get; set; }

    public string NormalizedName { get; set; }


    public ICollection<Claim> Claims { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MongoDB.Abstracts.Tests.Models;

public class User : MongoEntity
{
    public User()
    {
        Claims = new List<Claim>();
        Logins = new List<Login>();
        Tokens = new List<Token>();
        Roles = new HashSet<string>();
        Organizations = new HashSet<string>();
    }

    public string Name { get; set; }

    public string Email { get; set; }

    public string EmailLower { get; set; }


    [DefaultValue(false)]
    public bool EmailConfirmed { get; set; }


    public string PasswordHash { get; set; }

    public string SecurityStamp { get; set; }


    public string PhoneNumber { get; set; }

    [DefaultValue(false)]
    public bool PhoneNumberConfirmed { get; set; }


    [DefaultValue(false)]
    public bool TwoFactorEnabled { get; set; }


    public DateTimeOffset? LockoutEndDateUtc { get; set; }

    [DefaultValue(false)]
    public bool LockoutEnabled { get; set; }

    [DefaultValue(0)]
    public int AccessFailedCount { get; set; }


    public HashSet<string> Roles { get; set; }

    public ICollection<Claim> Claims { get; set; }

    public ICollection<Login> Logins { get; set; }

    public ICollection<Token> Tokens { get; set; }

    public HashSet<string> Organizations { get; set; }
}

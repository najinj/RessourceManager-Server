using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_mongo_auth.Models
{
    [CollectionName("Users")]
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public bool RememberMe { get; set; }
        public bool Activated { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
    }

    public class ApplicationRole : MongoIdentityRole<Guid>
    {
        public ApplicationRole() : base()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}

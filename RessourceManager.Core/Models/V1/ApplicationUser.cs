using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System;


namespace RessourceManager.Core.Models.V1
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
}

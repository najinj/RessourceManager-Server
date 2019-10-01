using AspNetCore.Identity.MongoDbCore.Models;
using System;




namespace RessourceManager.Core.Models.V1
{
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

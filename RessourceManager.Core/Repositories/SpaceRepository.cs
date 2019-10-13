using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories.Interfaces;
using RessourceManager.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace RessourceManager.Core.Repositories
{
    public class SpaceRepository : Repository<Space> , ISpaceRepository
    {
        public SpaceRepository(IMongoContext context) : base(context)
        {

        }
    }
}

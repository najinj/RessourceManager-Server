using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories.Interfaces;
using RessourceManager.Infrastructure.Context;
using RessourceManager.Infrastructure.Repositories;


namespace RessourceManager.Infrastructure.Repositories
{
    public class SpaceRepository : Repository<Space> , ISpaceRepository
    {
        public SpaceRepository(IMongoContext context) : base(context)
        {

        }
    }
}

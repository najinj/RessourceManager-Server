using MongoDB.Driver;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RessourceManager.Core.Repositories
{
    public class RessourceTypeRepository : Repository<RessourceType> , IRessourceTypeRepository
    {
        public RessourceTypeRepository(IMongoContext context) : base(context)
        {
            
        }
        public async Task<IEnumerable<RessourceType>> GetByType(int type)
        {
            var ressourceTypes = await DbSet.FindAsync(ressourceType => ressourceType.Type == (RType)type);
            return ressourceTypes.ToList();
        }
    }
}

using MongoDB.Driver;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories.Interfaces;
using RessourceManager.Infrastructure.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RessourceManager.Infrastructure.Repositories
{
    public class AssetRepository : Repository<Asset> , IAssetRepository
    {
        public AssetRepository(IMongoContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Asset>> Get(List<string> ids)
        {
            var assets = await DbSet.FindAsync(asset => ids.Contains(asset.Id));
            return assets.ToList();
        }
    }
}

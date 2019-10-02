using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories.Interfaces;

namespace RessourceManager.Core.Repositories
{
    public class AssetRepository : Repository<Asset> , IAssetRepository
    {
        public AssetRepository(IMongoContext context) : base(context)
        {

        }
    }
}

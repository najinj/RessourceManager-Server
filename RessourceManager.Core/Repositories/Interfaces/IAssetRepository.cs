using RessourceManager.Core.Models.V1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RessourceManager.Core.Repositories.Interfaces
{
    public interface IAssetRepository : IRepository<Asset>
    {
        Task<IEnumerable<Asset>> Get(List<string> ids);
    }
}

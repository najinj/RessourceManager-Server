

using RessourceManager.Core.Models.V1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RessourceManager.Core.Services.Interfaces
{
    public interface IAssetService
    {
        Task<List<Asset>> Get();
        Task<Asset> Get(string id);
        Task<Asset> Create(Asset assetIn);
        Task Update(Asset assetIn);
        void Remove(string id);
    }
}

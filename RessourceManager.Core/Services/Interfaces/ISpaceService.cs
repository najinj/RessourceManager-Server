using RessourceManager.Core.Models.V1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RessourceManager.Core.Services.Interfaces
{
    public interface ISpaceService
    {
        Task<List<Space>> Get();
        Task<Space> Get(string id);
        Task<Space> Create(Space spaceIn);
        void Update(Space spaceIn);
        void Remove(string id);
    }
}

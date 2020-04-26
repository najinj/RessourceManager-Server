using RessourceManager.Core.Models.V1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RessourceManager.Core.Services.Interfaces
{
    public interface IRessourceTypeService
    {
        Task<List<RessourceType>> Get();
        Task<RessourceType> Get(string id);
        Task<List<RessourceType>> GetByType(int type);
        Task<RessourceType> Create(RessourceType ressourceTypeIn);
        Task<RessourceType> Update(RessourceType ressourceTypeIn);
        Task Remove(string id);

    }
}

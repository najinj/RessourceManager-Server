using RessourceManager.Core.Models.V1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RessourceManager.Core.Repositories.Interfaces
{
    public interface IRessourceTypeRepository : IRepository<RessourceType>
    {
        Task<IEnumerable<RessourceType>> GetByType(int type);
    }
}

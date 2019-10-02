using MongoDB.Driver;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories;
using RessourceManager.Core.Repositories.Interfaces;
using RessourceManager.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RessourceManager.Core.Services
{
    public class RessourceTypeService : IRessourceTypeService
    {
        private readonly IRessourceTypeRepository _ressourceTypeRepository;


        public RessourceTypeService(IRessourceTypeRepository ressourceTypeRepository)
        {
            _ressourceTypeRepository = ressourceTypeRepository;
        }

        public async Task<List<RessourceType>> Get()
        {
            var ressourceTypes = await _ressourceTypeRepository.GetAll();
            return ressourceTypes.ToList();
        }

        public async Task<RessourceType> Get(string id)
        {
            var ressourceType = await _ressourceTypeRepository.GetById(Guid.Parse(id));
            return ressourceType;
        }


        public async Task<List<RessourceType>> GetByType(int type)
        {
            var ressourceTypes = await _ressourceTypeRepository.GetByType(type);
            return ressourceTypes.ToList();
        }
        public RessourceType Create(RessourceType ressourceTypeIn)
        {
            try
            {
                ressourceTypeIn.Count = 0; // Make sure the count is 0 at creation
                _ressourceTypeRepository.Add(ressourceTypeIn);
                
            }
            catch (Exception ex)
            {
                return null; // TODO
            }
            return ressourceTypeIn;
        }

        public void Update(RessourceType ressourceTypeIn) => _ressourceTypeRepository.Update(ressourceTypeIn);

        
        public void Remove(string id) =>
            _ressourceTypeRepository.Remove(Guid.Parse(id));
    }
}

using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RessourceManager.Core.Services.Interfaces
{
    public class SpaceService : ISpaceService
    {
        private readonly ISpaceRepository _spaceRepository;
        private readonly IRessourceTypeRepository _ressourceTypeRepository;

        public SpaceService(ISpaceRepository spaceRepository, 
                            IRessourceTypeRepository ressourceTypeRepository)
        {
            _spaceRepository = spaceRepository;
            _ressourceTypeRepository = ressourceTypeRepository;
        }

        public async Task<List<Space>> Get()
        {
            var spaces = await _spaceRepository.GetAll();
            return spaces.ToList();
        }

        public async Task<Space> Get(string id)
        {
            var space = await _spaceRepository.GetById(Guid.Parse(id));
            return space;
        }

        public async Task<Space> Create(Space spaceIn)
        {
            var ressourceTypeIn = await _ressourceTypeRepository.GetById(Guid.Parse(spaceIn.SpaceTypeId));
            if (ressourceTypeIn == null)
                // throw new RessourceTypeNotFoundException("Can't find Ressource Type"); TODO
                return null;
            try
            {
                ressourceTypeIn.Count++; // Increamenting count when adding an asset
                _ressourceTypeRepository.Update(ressourceTypeIn);
                _spaceRepository.Add(spaceIn);

            }
            catch (Exception ex)
            {
                return null;
                //if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                //    throw new SpaceDuplicateKeyException(ex.Message);  TODO
            }
            return spaceIn;
        }

        public void Update(Space spaceIn) =>
            _spaceRepository.Update(spaceIn);

        public async void Remove(Space spaceIn)
        {
            var ressourceTypeIn = await _ressourceTypeRepository.GetById(Guid.Parse(spaceIn.SpaceTypeId));
            if (ressourceTypeIn == null)
                // throw new RessourceTypeNotFoundException("Can't find Ressource Type"); TODO
                return;

            ressourceTypeIn.Count--; // Decreassing count when removing an asset
            _ressourceTypeRepository.Update(ressourceTypeIn);
            _spaceRepository.Remove(Guid.Parse(spaceIn.Id));
        }


        public async void Remove(string id)
        {
            var spaceIn = await _spaceRepository.GetById(Guid.Parse(id));
            if (spaceIn != null)
                Remove(spaceIn);
        }
    }
}

using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories.Interfaces;
using RessourceManager.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RessourceManager.Core.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IRessourceTypeRepository _ressourceTypeRepository;
        private readonly ISpaceRepository _spaceRepository;

        public AssetService(IAssetRepository assetRepository , IRessourceTypeRepository ressourceTypeRepository, ISpaceRepository spaceRepository)
        {
            _assetRepository = assetRepository;
            _ressourceTypeRepository = ressourceTypeRepository;
            _spaceRepository = spaceRepository;
        }

        public async Task<List<Asset>> Get()
        {
            var assets = await _assetRepository.GetAll();
            return assets.ToList();
        }

        public async Task<Asset> Get(string id)
        {
            var asset = await _assetRepository.GetById(Guid.Parse(id));
            return asset;
        }


        public async Task<Asset> Create(Asset assetIn)
        {
            var ressourceTypeIn = await _ressourceTypeRepository.GetById(Guid.Parse(assetIn.AssetTypeId));
            if (ressourceTypeIn == null)
                // throw new RessourceTypeNotFoundException("Can't find Ressource Type"); TODO
                return null;
            try
            {
                if (assetIn.SpaceId != null)
                {
                    var spaceIn = await _spaceRepository.GetById(Guid.Parse(assetIn.SpaceId));
                    if (spaceIn != null)
                        assetIn.Status = Status.Chained;
                    else
                        assetIn.Status = Status.Unchained;
                }
                ressourceTypeIn.Count++; // Increamenting count when adding an asset
                _ressourceTypeRepository.Update(ressourceTypeIn);
                _assetRepository.Add(assetIn);
            }
            catch (Exception ex)
            {
                return null;
                //if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                //    throw new AssetDuplicateKeyException(ex.Message);
            }
            
            return assetIn;
        }

        public async void Update(Asset assetIn)
        {
            var ressourceTypeIn = await _ressourceTypeRepository.GetById(Guid.Parse(assetIn.AssetTypeId));
            if (ressourceTypeIn == null)
                // throw new RessourceTypeNotFoundException("Can't find Ressource Type"); TODO
                return;
            if (assetIn.SpaceId != null)
            {
                var spaceIn = await _spaceRepository.GetById(Guid.Parse(assetIn.SpaceId));
                if (spaceIn != null)
                    assetIn.Status = Status.Chained;
                else
                    assetIn.Status = Status.Unchained;
            }
            _assetRepository.Update(assetIn);

        }


        public async void Remove(Asset assetIn)
        {
            var ressourceTypeIn = await _ressourceTypeRepository.GetById(Guid.Parse(assetIn.AssetTypeId));
            if (ressourceTypeIn == null)
                // throw new RessourceTypeNotFoundException("Can't find Ressource Type"); TODO
                return;

            ressourceTypeIn.Count--; // Decreassing count when removing an asset
            _ressourceTypeRepository.Update(ressourceTypeIn);
            _assetRepository.Remove(Guid.Parse(assetIn.Id));

        }
            

        public async void Remove(string id)
        {
            var assetIn = await _assetRepository.GetById(Guid.Parse(id));
            if (assetIn != null)
                Remove(assetIn);
        }

    }
}

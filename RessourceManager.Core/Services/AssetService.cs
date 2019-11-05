using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RessourceManager.Core.Exceptions.Asset;
using RessourceManager.Core.Exceptions.RessourceType;
using RessourceManager.Core.Exceptions.Space;
using RessourceManager.Core.Helpers;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories.Interfaces;
using RessourceManager.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RessourceManager.Core.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IRessourceTypeRepository _ressourceTypeRepository;
        private readonly ISpaceRepository _spaceRepository;
        private readonly IErrorHandler _errorHandler;

        public AssetService(IAssetRepository assetRepository , 
                            IRessourceTypeRepository ressourceTypeRepository,
                            IErrorHandler errorHandler,
                            ISpaceRepository spaceRepository)
        {
            _assetRepository = assetRepository;
            _ressourceTypeRepository = ressourceTypeRepository;
            _spaceRepository = spaceRepository;
            _errorHandler = errorHandler;
        }

        public async Task<List<Asset>> Get()
        {
            var assets = await _assetRepository.GetAll();
            return assets.ToList();
        }

        public async Task<Asset> Get(string id)
        {
            var asset = await _assetRepository.GetById(id);
            return asset;
        }


        public async Task<Asset> Create(Asset assetIn)
        {
            var ressourceTypeIn = await _ressourceTypeRepository.GetById(assetIn.AssetTypeId);
            if (ressourceTypeIn == null)
                throw new RessourceTypeRepositoryException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.NotFound),
                          nameof(RessourceType), assetIn.AssetTypeId), nameof(assetIn.AssetTypeId));
            try
            {
                if (assetIn.SpaceId != null)
                {
                    var spaceIn = await _spaceRepository.GetById(assetIn.SpaceId);
                    if (spaceIn != null)
                        assetIn.Status = Status.Chained;
                    else
                        throw new SpaceRepositoryException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.NotFound),
                             nameof(Space), assetIn.SpaceId), nameof(assetIn.SpaceId));
                }
                else 
                    assetIn.Status = Status.Unchained;

                ressourceTypeIn.Count++; // Increamenting count when adding an asset
                _ressourceTypeRepository.Update(ressourceTypeIn);
                await _assetRepository.Add(assetIn);
            }
            catch (MongoWriteException mwx)
            {
                if (mwx.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    var pattern = @"\{(?:[^{*}])*\}";
                    Match result = Regex.Match(mwx.Message, pattern);  // get the dublicated feild from the string error msg 
                    JObject duplicatedField = JsonConvert.DeserializeObject<JObject>(result.Value); // parse it  to get the field 
                    throw new AssetRepositoryException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.DuplicateKey),
                       nameof(Asset), duplicatedField.First.Path), duplicatedField.First.Path);
                }
            }
            
            return assetIn;
        }

        public async Task Update(Asset assetIn)
        {
            var ressourceTypeIn = await _ressourceTypeRepository.GetById(assetIn.AssetTypeId);
            if (ressourceTypeIn == null)
                throw new RessourceTypeRepositoryException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.NotFound),
                          nameof(RessourceType), assetIn.AssetTypeId), nameof(assetIn.AssetTypeId));
            if (assetIn.SpaceId != null)
            {
                var spaceIn = await _spaceRepository.GetById(assetIn.SpaceId);
                if (spaceIn != null)
                    assetIn.Status = Status.Chained;
                else
                    assetIn.Status = Status.Unchained;
            }
            try
            {
                await _assetRepository.Update(assetIn);
            }
            catch (MongoWriteException mwx)
            {
                if (mwx.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    var pattern = @"\{(?:[^{*}])*\}";
                    Match result = Regex.Match(mwx.Message, pattern);  // get the dublicated feild from the string error msg 
                    JObject duplicatedField = JsonConvert.DeserializeObject<JObject>(result.Value); // parse it  to get the field 
                    throw new AssetRepositoryException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.DuplicateKey),
                       nameof(Asset), duplicatedField.First.Path), duplicatedField.First.Path);
                }
            }


        }


        public async void Remove(Asset assetIn)
        {
            var ressourceTypeIn = await _ressourceTypeRepository.GetById(assetIn.AssetTypeId);
            if (ressourceTypeIn == null)
                throw new RessourceTypeRepositoryException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.NotFound),
                         nameof(RessourceType), assetIn.AssetTypeId), nameof(assetIn.AssetTypeId));

            ressourceTypeIn.Count--; // Decreassing count when removing an asset
            try
            {
                _ressourceTypeRepository.Update(ressourceTypeIn);
                _assetRepository.Remove(assetIn.Id);
            }
            catch (MongoWriteException mwx)
            {
                if (mwx.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    var pattern = @"\{(?:[^{*}])*\}";
                    Match result = Regex.Match(mwx.Message, pattern);  // get the dublicated feild from the string error msg 
                    JObject duplicatedField = JsonConvert.DeserializeObject<JObject>(result.Value); // parse it  to get the field 
                    throw new AssetRepositoryException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.DuplicateKey),
                       nameof(Asset), duplicatedField.First.Path), duplicatedField.First.Path);
                }
            }

        }
            

        public async void Remove(string id)
        {
            var assetIn = await _assetRepository.GetById(id);
            if (assetIn != null)
                Remove(assetIn);
        }

        public async Task<List<Asset>> Get(List<string> ids)
        {
            var assets = await _assetRepository.Get(ids);
            return assets.ToList();
        }

    }
}

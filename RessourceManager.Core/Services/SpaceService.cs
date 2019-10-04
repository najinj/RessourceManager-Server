using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RessourceManager.Core.Exceptions.Asset;
using RessourceManager.Core.Exceptions.RessourceType;
using RessourceManager.Core.Exceptions.Space;
using RessourceManager.Core.Helpers;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RessourceManager.Core.Services.Interfaces
{
    public class SpaceService : ISpaceService
    {
        private readonly ISpaceRepository _spaceRepository;
        private readonly IRessourceTypeRepository _ressourceTypeRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IErrorHandler _errorHandler;

        public SpaceService(ISpaceRepository spaceRepository, 
                            IRessourceTypeRepository ressourceTypeRepository,
                            IErrorHandler errorHandler,
                            IAssetRepository assetRepository)
        {
            _spaceRepository = spaceRepository;
            _ressourceTypeRepository = ressourceTypeRepository;
            _assetRepository = assetRepository;
            _errorHandler = errorHandler;
        }

        public async Task<List<Space>> Get()
        {
            var spaces = await _spaceRepository.GetAll();
            return spaces.ToList();
        }

        public async Task<Space> Get(string id)
        {
            var space = await _spaceRepository.GetById(id);
            return space;
        }

        public async Task<Space> Create(Space spaceIn)
        {
            var ressourceTypeIn = await _ressourceTypeRepository.GetById(spaceIn.SpaceTypeId);
            if (ressourceTypeIn == null)
                throw new RessourceTypeRepositoryException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.NotFound),
                        nameof(RessourceType), spaceIn.SpaceTypeId), nameof(spaceIn.SpaceTypeId));
            try
            {
                ressourceTypeIn.Count++; // Increamenting count when adding an asset
                _ressourceTypeRepository.Update(ressourceTypeIn);
                await _spaceRepository.Add(spaceIn);

            }
            catch (MongoWriteException mwx)
            {
                if (mwx.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    var pattern = @"\{(?:[^{*}])*\}";
                    Match result = Regex.Match(mwx.Message, pattern);  // get the dublicated feild from the string error msg 
                    JObject duplicatedField = JsonConvert.DeserializeObject<JObject>(result.Value); // parse it  to get the field 
                    throw new SpaceRepositoryException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.DuplicateKey),
                       nameof(Space), duplicatedField.First.Path), duplicatedField.First.Path);
                }
            }
            return spaceIn;
        }

        public void Update(Space spaceIn) {
            try
            {
                _spaceRepository.Update(spaceIn);
            }
            catch (MongoWriteException mwx)
            {
                if (mwx.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    var pattern = @"\{(?:[^{*}])*\}";
                    Match result = Regex.Match(mwx.Message, pattern);  // get the dublicated feild from the string error msg 
                    JObject duplicatedField = JsonConvert.DeserializeObject<JObject>(result.Value); // parse it  to get the field 
                    throw new SpaceRepositoryException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.DuplicateKey),
                       nameof(Space), duplicatedField.First.Path), duplicatedField.First.Path);
                }
            }

        }
        public async void Remove(Space spaceIn)
        {
            var ressourceTypeIn = await _ressourceTypeRepository.GetById(spaceIn.SpaceTypeId);
            if (ressourceTypeIn == null)
                throw new RessourceTypeRepositoryException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.NotFound),
                         nameof(RessourceType), spaceIn.SpaceTypeId), nameof(spaceIn.SpaceTypeId));

            ressourceTypeIn.Count--; // Decreassing count when removing an asset
            _ressourceTypeRepository.Update(ressourceTypeIn);
            _spaceRepository.Remove(spaceIn.Id);
        }

        public async Task<Space> RemoveAssetFromSpace(string assetId, string spaceId) {

            var spaceIn = await _spaceRepository.GetById(spaceId);
            var assetIn = await _assetRepository.GetById(assetId);
            if (spaceIn == null)
                throw new SpaceRepositoryException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.NotFound),
                       nameof(Space)), spaceId);
            if (assetIn == null)
                throw new AssetRepositoryException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.NotFound),
                       nameof(Asset)), assetId);
            var assetToRemove = spaceIn.assests.Where(asset => asset.Id == assetId).FirstOrDefault();
            if (assetToRemove == null)
                throw new AssetRepositoryException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.NotFound),
                       nameof(Asset)), assetId);

            spaceIn.assests.Remove(assetToRemove);
            try
            {
                // if (assetIn.Status == Status.Chained) 
                assetIn.Status = Status.Unchained;  // if we remove a chained asset from space it becomes unchained and would be possible to reserve it           
                _spaceRepository.Update(spaceIn);
                _assetRepository.Update(assetIn); // updating the status of the asset 
            }
            catch (MongoWriteException ex)
            {
                return null;
            }
            return spaceIn;
        }
        public async Task<Space> AddAssetToSpace(string assetId, string spaceId)
        {
            var spaceIn = await _spaceRepository.GetById(spaceId);
            var assetIn = await _assetRepository.GetById(assetId);
            if (spaceIn == null)
                throw new SpaceRepositoryException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.NotFound),
                       nameof(Space)), spaceId);
            if (assetIn == null)
                throw new AssetRepositoryException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.NotFound),
                       nameof(Asset)), assetId);
            try
            {
                assetIn.Status = Status.Chained;
                _spaceRepository.Update(spaceIn);
                _assetRepository.Update(assetIn); // updating the status of the asset 
            }
            catch (MongoWriteException ex)
            {
                return null;
            }
            return spaceIn;
        }


        public async void Remove(string id)
        {
            var spaceIn = await _spaceRepository.GetById(id);
            if (spaceIn != null)
                Remove(spaceIn);
        }
    }
}

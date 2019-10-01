using MongoDB.Driver;
using RessourceManager.Core.Models.V1;
using RessourceManagerApi.Exceptions.Asset;
using RessourceManagerApi.Exceptions.RessourceType;
using System;
using System.Collections.Generic;
using System.Linq;
using test_mongo_auth.Models;


namespace test_mongo_auth.Services
{
    public class AssetService
    {
        private readonly IMongoCollection<Space> _spaces;
        private readonly IMongoCollection<Asset> _assets;
        private readonly IMongoCollection<RessourceType> _ressourceTypes;
        public AssetService(IRessourceDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _assets = database.GetCollection<Asset>(settings.AssetsCollectionName);
            _ressourceTypes = database.GetCollection<RessourceType>(settings.RessourceTypesCollectionName);
            _spaces = database.GetCollection<Space>(settings.SpacesCollectionName);
        }

        public List<Asset> Get() =>
            _assets.Find(asset => true).ToList();

        public Asset Get(string id) =>
            _assets.Find<Asset>(asset => asset.Id == id).FirstOrDefault();

        public Asset Create(Asset assetIn)
        {
            var ressourceTypeIn = _ressourceTypes.Find(resourceType => resourceType.Id == assetIn.AssetTypeId).FirstOrDefault();
            if (ressourceTypeIn == null)
                throw new RessourceTypeNotFoundException("Can't find Ressource Type");
            try
            {
                if (assetIn.SpaceId != null)
                {
                    var spaceIn = _spaces.Find(space => space.Id == assetIn.SpaceId).FirstOrDefault();
                    if (spaceIn != null)
                        assetIn.Status = Status.Chained;
                    else
                        assetIn.Status = Status.Unchained;
                }
                ressourceTypeIn.Count++; // Increamenting count when adding an asset
                _ressourceTypes.ReplaceOne(ressourceType => ressourceType.Id == ressourceTypeIn.Id, ressourceTypeIn);
                _assets.InsertOne(assetIn);

            }
            catch (MongoWriteException ex)
            {
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                    throw new AssetDuplicateKeyException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return assetIn;
        }

        public void Update(string id, Asset assetIn) {
            var ressourceTypeIn = _ressourceTypes.Find(resourceType => resourceType.Id == assetIn.AssetTypeId).FirstOrDefault();
            if (ressourceTypeIn == null)
                throw new RessourceTypeNotFoundException("Can't find Ressource Type");
            if (assetIn.SpaceId != null)
            {
                var spaceIn = _spaces.Find(space => space.Id == assetIn.SpaceId).FirstOrDefault();
                if (spaceIn != null)
                    assetIn.Status = Status.Chained;
                else
                    assetIn.Status = Status.Unchained;
            }
            ressourceTypeIn.Count++; // Decreassing count when removing an asset
            _ressourceTypes.ReplaceOne(ressourceType=> ressourceType.Id == ressourceTypeIn.Id, ressourceTypeIn); 
            _assets.ReplaceOne(asset => asset.Id == id, assetIn);
        
        }
            

        public void Remove(Asset assetIn) =>
            _assets.DeleteOne(asset => asset.Id == assetIn.Id);

        public void Remove(string id) =>
            _assets.DeleteOne(asset => asset.Id == id);
    }
}

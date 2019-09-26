using MongoDB.Driver;
using RessourceManagerApi.Exceptions.Asset;
using RessourceManagerApi.Exceptions.RessourceType;
using System;
using System.Collections.Generic;
using System.Linq;
using test_mongo_auth.Models;
using test_mongo_auth.Models.Ressource;
using test_mongo_auth.Models.RessourceTypes;

namespace test_mongo_auth.Services
{
    public class AssetService
    {
        private readonly IMongoCollection<Asset> _assets;
        private readonly IMongoCollection<RessourceType> _ressourceTypes;
        public AssetService(IRessourceDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _assets = database.GetCollection<Asset>(settings.AssetsCollectionName);
            _ressourceTypes = database.GetCollection<RessourceType>(settings.RessourceTypesCollectionName);
        }

        public List<Asset> Get() =>
            _assets.Find(asset => true).ToList();

        public Asset Get(string id) =>
            _assets.Find<Asset>(asset => asset.Id == id).FirstOrDefault();

        public Asset Create(Asset asset)
        {
            var ressourceTypeIn = _ressourceTypes.Find(resourceType => resourceType.Id == asset.AssetTypeId).FirstOrDefault();
            if (ressourceTypeIn == null)
                throw new RessourceTypeNotFoundException("Can't find Ressource Type");
            try
            {
                ressourceTypeIn.Count++; // Increamenting count when adding an asset
                _ressourceTypes.ReplaceOne(ressourceType => ressourceType.Id == ressourceTypeIn.Id, ressourceTypeIn);
                _assets.InsertOne(asset);

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
            return asset;
        }

        public void Update(string id, Asset assetIn) {
            var ressourceTypeIn = _ressourceTypes.Find(resourceType => resourceType.Id == assetIn.AssetTypeId).FirstOrDefault();
            if (ressourceTypeIn == null)
                throw new RessourceTypeNotFoundException("Can't find Ressource Type");
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

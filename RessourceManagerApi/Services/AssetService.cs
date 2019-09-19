using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_mongo_auth.Models;
using test_mongo_auth.Models.Ressource;

namespace test_mongo_auth.Services
{
    public class AssetService
    {
        private readonly IMongoCollection<Asset> _assets;

        public AssetService(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _assets = database.GetCollection<Asset>(settings.AssetsCollectionName);
        }

        public List<Asset> Get() =>
            _assets.Find(asset => true).ToList();

        public Asset Get(string id) =>
            _assets.Find<Asset>(asset => asset.Id == id).FirstOrDefault();

        public Asset Create(Asset asset)
        {
            _assets.InsertOne(asset);
            return asset;
        }

        public void Update(string id, Asset assetIn) =>
            _assets.ReplaceOne(asset => asset.Id == id, assetIn);

        public void Remove(Asset assetIn) =>
            _assets.DeleteOne(asset => asset.Id == assetIn.Id);

        public void Remove(string id) =>
            _assets.DeleteOne(asset => asset.Id == id);
    }
}

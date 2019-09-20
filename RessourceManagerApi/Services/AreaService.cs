using MongoDB.Driver;
using RessourceManagerApi.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_mongo_auth.Models;
using test_mongo_auth.Models.Ressource;
using test_mongo_auth.Models.RessourceTypes;

namespace test_mongo_auth.Services
{
    public class AreaService
    {
        private readonly IMongoCollection<Area> _areas;
        private readonly IMongoCollection<Asset> _assets;
        private readonly IMongoCollection<RessourceType> _ressourceTypes;
        public AreaService(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _areas = database.GetCollection<Area>(settings.AreasCollectionName);
            _assets = database.GetCollection<Asset>(settings.AssetsCollectionName);
            _ressourceTypes = database.GetCollection<RessourceType>(settings.RessourceTypesCollectionName);
        }

        public List<Area> Get() =>
            _areas.Find(area => true).ToList();

        public Area Get(string id) =>
            _areas.Find(area => area.Id == id).FirstOrDefault();

        public Area Create(Area area)
        {
            var ressourceType = _ressourceTypes.Find(resourceType => resourceType.Id == area.AreaTypeId).FirstOrDefault();
            if (ressourceType == null)
                throw new RessourceTypeNotFoundException("Can't find Ressource Type");
            _areas.InsertOne(area);
            return area;
        }

        public void Update(string id, Area areaIn) =>
            _areas.ReplaceOne(area => area.Id == id, areaIn);

        public void Remove(Area areaIn) =>
            _areas.DeleteOne(area => area.Id == areaIn.Id);

        public void Remove(string id) =>
            _areas.DeleteOne(area => area.Id == id);
    }
}

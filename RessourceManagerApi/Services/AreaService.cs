using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_mongo_auth.Models;
using test_mongo_auth.Models.Ressource;

namespace test_mongo_auth.Services
{
    public class AreaService
    {
        private readonly IMongoCollection<Area> _areas;

        public AreaService(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _areas = database.GetCollection<Area>(settings.AreasCollectionName);
        }

        public List<Area> Get() =>
            _areas.Find(area => true).ToList();

        public Area Get(string id) =>
            _areas.Find<Area>(area => area.Id == id).FirstOrDefault();

        public Area Create(Area area)
        {
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

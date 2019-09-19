using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_mongo_auth.Models;
using test_mongo_auth.Models.RessourceTypes;

namespace test_mongo_auth.Services
{
    public class RessourceTypeService
    {
        private readonly IMongoCollection<RessourceType> _ressourceTypes;

        public RessourceTypeService(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _ressourceTypes = database.GetCollection<RessourceType>(settings.RessourceTypesCollectionName);
        }

        public List<RessourceType> Get() =>
            _ressourceTypes.Find(ressourceType => true).ToList();

        public RessourceType Get(string id) =>
            _ressourceTypes.Find<RessourceType>(ressourceType => ressourceType.Id == id).FirstOrDefault();

        public RessourceType Create(RessourceType ressourceType)
        {
            _ressourceTypes.InsertOne(ressourceType);
            return ressourceType;
        }

        public void Update(string id, RessourceType ressourceTypeIn) =>
            _ressourceTypes.ReplaceOne(ressourceType => ressourceType.Id == id, ressourceTypeIn);

        public void Remove(RessourceType ressourceTypeIn) =>
            _ressourceTypes.DeleteOne(ressourceType => ressourceType.Id == ressourceTypeIn.Id);

        public void Remove(string id) =>
            _ressourceTypes.DeleteOne(ressourceType => ressourceType.Id == id);
    }
}

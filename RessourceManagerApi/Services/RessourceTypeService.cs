using MongoDB.Driver;
using RessourceManagerApi.Exceptions;
using RessourceManagerApi.Exceptions.RessourceType;
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

        public RessourceTypeService(IRessourceDatabaseSettings settings)
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
            try
            {
                ressourceType.Count = 0; // Make sure the count is 0 at creation
                _ressourceTypes.InsertOne(ressourceType);
            }
            catch (MongoWriteException ex)
            {
                if(ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                    throw new RessourceTypeDuplicateKeyException(ex.Message);
            }
            return ressourceType;
        }

        public void Update(string id, RessourceType ressourceTypeIn) =>
            _ressourceTypes.ReplaceOne<RessourceType>(ressourceType => ressourceType.Id == id, ressourceTypeIn);

        public void Remove(RessourceType ressourceTypeIn) =>
            _ressourceTypes.DeleteOne(ressourceType => ressourceType.Id == ressourceTypeIn.Id);

        public void Remove(string id) =>
            _ressourceTypes.DeleteOne(ressourceType => ressourceType.Id == id);
    }
}

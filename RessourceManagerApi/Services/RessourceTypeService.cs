﻿using MongoDB.Driver;
using RessourceManager.Core.Models.V1;
using RessourceManagerApi.Exceptions.RessourceType;
using System.Collections.Generic;
using System.Linq;
using test_mongo_auth.Models;

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
            _ressourceTypes.Find(ressourceType => ressourceType.Id == id).FirstOrDefault();

        public List<RessourceType> Get(int id) =>
            _ressourceTypes.Find(ressourceType => ressourceType.Type == (RType)id).ToList();

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
            _ressourceTypes.ReplaceOne(ressourceType => ressourceType.Id == id, ressourceTypeIn);

        public void Remove(RessourceType ressourceTypeIn) =>
            _ressourceTypes.DeleteOne(ressourceType => ressourceType.Id == ressourceTypeIn.Id);

        public void Remove(string id) =>
            _ressourceTypes.DeleteOne(ressourceType => ressourceType.Id == id);
    }
}

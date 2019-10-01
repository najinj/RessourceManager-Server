using MongoDB.Driver;
using RessourceManager.Core.Models.V1;
using RessourceManagerApi.Exceptions.RessourceType;
using RessourceManagerApi.Exceptions.Space;
using System.Collections.Generic;
using System.Linq;
using test_mongo_auth.Models;

namespace test_mongo_auth.Services
{
    public class SpaceService
    {
        private readonly IMongoCollection<Space> _spaces;
        private readonly IMongoCollection<Asset> _assets;
        private readonly IMongoCollection<RessourceType> _ressourceTypes;
        public SpaceService(IRessourceDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _spaces = database.GetCollection<Space>(settings.SpacesCollectionName);
            _assets = database.GetCollection<Asset>(settings.AssetsCollectionName);
            _ressourceTypes = database.GetCollection<RessourceType>(settings.RessourceTypesCollectionName);
        }

        public List<Space> Get() =>
            _spaces.Find(space => true).ToList();

        public Space Get(string id) =>
            _spaces.Find(space => space.Id == id).FirstOrDefault();

        public Space Create(Space spaceIn)
        {
            var ressourceTypeIn = _ressourceTypes.Find(resourceType => resourceType.Id == spaceIn.SpaceTypeId).FirstOrDefault();
            if (ressourceTypeIn == null)
                throw new RessourceTypeNotFoundException("Can't find Ressource Type");
            try
            {
                ressourceTypeIn.Count++; // Increamenting count when adding an asset
                _ressourceTypes.ReplaceOne(ressourceType => ressourceType.Id == ressourceTypeIn.Id, ressourceTypeIn);
                _spaces.InsertOne(spaceIn);

            }
            catch (MongoWriteException ex)
            {
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                    throw new SpaceDuplicateKeyException(ex.Message);
            }
            return spaceIn;
        }

        public void Update(string id, Space spaceIn) =>
            _spaces.ReplaceOne(space => space.Id == id, spaceIn);

        public void Remove(Space spaceIn)
        {
            var ressourceTypeIn = _ressourceTypes.Find(resourceType => resourceType.Id == spaceIn.SpaceTypeId).FirstOrDefault();
            if (ressourceTypeIn == null)
                throw new RessourceTypeNotFoundException("Can't find Ressource Type");
            ressourceTypeIn.Count++; // Decreassing count when removing an asset
            _ressourceTypes.ReplaceOne(ressourceType => ressourceType.Id == ressourceTypeIn.Id, ressourceTypeIn);
            _spaces.DeleteOne(space => space.Id == spaceIn.Id);
        }
            

        public void Remove(string id) =>
            _spaces.DeleteOne(space => space.Id == id);
    }
}

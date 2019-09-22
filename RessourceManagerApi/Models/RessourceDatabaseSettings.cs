using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_mongo_auth.Models
{
    public class RessourceDatabaseSettings : IRessourceDatabaseSettings
    {
        public string AssetsCollectionName { get; set; }
        public string SpacesCollectionName { get; set; }
        public string RessourceTypesCollectionName { get; set; }
        public string BooksCollectionName { get; set; }
        public string PostsCollectionName { get; set; }
        public string UsersCollectionName { get; set; }
        public string ReservationsCollectionName { get; set; }
        public string EmailSettings { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IRessourceDatabaseSettings
    {
        string AssetsCollectionName { get; set; }
        string SpacesCollectionName { get; set; }
        string RessourceTypesCollectionName { get; set; }
        string BooksCollectionName { get; set; }
        string PostsCollectionName { get; set; }
        string UsersCollectionName { get; set; }
        string ReservationsCollectionName { get; set; }
        string EmailSettings { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}

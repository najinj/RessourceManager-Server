




namespace RessourceManager.Infrastructure.DatabaseSettings
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
}

﻿
namespace RessourceManager.Infrastructure.DatabaseSettings
{
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
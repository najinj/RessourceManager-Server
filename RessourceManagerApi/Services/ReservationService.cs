using MongoDB.Driver;
using RessourceManagerApi.Exceptions.Reservation;
using RessourceManagerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_mongo_auth.Models;
using test_mongo_auth.Models.Ressource;

namespace RessourceManagerApi.Services
{
    public class ReservationService
    {
        private readonly IMongoCollection<Reservation> _reservations;
        private readonly IMongoCollection<Asset> _assets;
        private readonly IMongoCollection<Space> _spaces;

        public ReservationService(IRessourceDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _reservations = database.GetCollection<Reservation>(settings.ReservationsCollectionName);
            _assets = database.GetCollection<Asset>(settings.AssetsCollectionName);
            _spaces = database.GetCollection<Space>(settings.SpacesCollectionName);
        }

        public List<Reservation> Get() => _reservations.Find(reservation => true).ToList();

        public List<Reservation> Get(DateTime startDate) => 
            _reservations.Find(reservation => 
            reservation.Start >= startDate || 
            reservation.StartRecur >= startDate ||
            reservation.End >= startDate ||
            reservation.EndRecur >= startDate
            ).ToList().OrderBy(reservation => reservation.Start).ToList();

        public List<Reservation> GetSpaceReservations(string spaceId) => _reservations.Find(reservation => reservation.SpaceId == spaceId).ToList();


        public Reservation Get(string id) => _reservations.Find(reservation => reservation.Id == id).FirstOrDefault();

        public Reservation Create(Reservation reservation)
        {
            var assetOrSpace = reservation.AssetId != null ? 
                _assets.CountDocuments(asset => asset.Id == reservation.AssetId) : 
                _spaces.CountDocuments(space => space.Id == reservation.SpaceId);
            if(assetOrSpace == 0)
            {
                throw new Exception();
            }
            try
            {
                _reservations.InsertOne(reservation);

            }
            catch (MongoWriteException ex)
            {
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                    throw new ReservationDuplicateKeyException(ex.Message);
            }
            return reservation;
        }

        public void Update(string id, Reservation reservationIn) =>
            _reservations.ReplaceOne(reservation => reservation.Id == id, reservationIn);

        public void Remove(Reservation reservationIn) =>
            _reservations.DeleteOne(reservation => reservation.Id == reservationIn.Id);

        public void Remove(string id) =>
            _reservations.DeleteOne(reservation => reservation.Id == id);
    }
}

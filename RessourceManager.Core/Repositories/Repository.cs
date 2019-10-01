using MongoDB.Driver;
using RessourceManager.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace RessourceManager.Core.Repositories
{
    /*
    public class Repository<TDocument> : IRepository<TDocument> where TDocument : class
    {
        protected readonly IMongoCollection<TDocument> _entities;

        public Repository(IRessourceDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _entities = database.GetCollection<TDocument>(settings.SpacesCollectionName);
        }

        public virtual void Add(TDocument entity)
        {
            _entities.InsertOne(entity);

        }

        public virtual void AddRange(IEnumerable<TDocument> entities)
        {
            _entities.InsertMany(entities);
        }
        public virtual void Update(Expression<Func<TDocument, bool>> predicate,TDocument entity)
        {
             _entities.ReplaceOne(predicate, entity);
        }

        public virtual void UpdateRange(Expression<Func<TDocument, bool>> predicate, IEnumerable<TDocument> entities)
        {
            _entities.UpdateMany(predicate,entities);
        }


        public virtual void Remove(Expression<Func<TDocument, bool>> predicate,TDocument entity)
        {
            _entities.FindOneAndDelete(predicate,entity);
        }

        public virtual void RemoveRange(IEnumerable<TDocument> entities)
        {
            _entities.RemoveRange(entities);
            _context.SaveChanges();
        }


        public virtual IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> predicate)
        {
            return _entities.Find(predicate).ToEnumerable();
        }

        public virtual TDocument GetSingleOrDefault(Expression<Func<TDocument, bool>> predicate)
        {
            return _entities.Find(predicate).FirstOrDefault();
        }

        public virtual IEnumerable<TDocument> GetAll()
        {
            return _entities.ToList();
        }

        public IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public TDocument GetSingleOrDefault(Expression<Func<TDocument, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }

    */
}

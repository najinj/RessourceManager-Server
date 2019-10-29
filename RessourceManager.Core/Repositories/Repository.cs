using MongoDB.Driver;
using RessourceManager.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceStack;
using MongoDB.Bson;
using RessourceManager.Infrastructure.Context;

namespace RessourceManager.Core.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoContext _context;
        protected readonly IMongoCollection<TEntity> DbSet;

        protected Repository(IMongoContext context)
        {
            _context = context;
            DbSet = _context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async virtual Task Add(TEntity obj)
        {                  
            _context.AddCommand(() => DbSet.InsertOneAsync(obj));
            var result = await _context.SaveChanges();                               
        }

        public virtual async Task<TEntity> GetById(string id)
        {
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(id)));
            return data.FirstOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var all = await DbSet.FindAsync(entity => true);
            return all.ToList();
        }

        public virtual async Task Update(TEntity obj)
        {
            _context.AddCommand(() => DbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(obj.GetId().ToString())), obj));
            var result = await _context.SaveChanges();
        }

        public virtual void Remove(string id) => _context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(id))));

        public void Dispose()
        {
            _context?.Dispose();
        }
    }


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

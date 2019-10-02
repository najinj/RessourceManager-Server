﻿using MongoDB.Driver;
using RessourceManager.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceStack;

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

        public virtual void Add(TEntity obj)
        {
            _context.AddCommand(() => DbSet.InsertOneAsync(obj));
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
            return data.SingleOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var all = await DbSet.FindAsync(entity => true);
            return all.ToList();
        }

        public virtual void Update(TEntity obj)
        {
            _context.AddCommand(() => DbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.GetId()), obj));
        }

        public virtual void Remove(Guid id) => _context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id)));

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

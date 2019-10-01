using System;
using System.Collections.Generic;
using System.Linq.Expressions;



namespace RessourceManager.Core.Repositories.Interfaces
{
    public interface IRepository<TDocument> where TDocument : class
    {
        void Add(TDocument entity);
        void AddRange(IEnumerable<TDocument> entities);
        void Update(TDocument entity);
        void UpdateRange(IEnumerable<TDocument> entities);       
        void Remove(TDocument entity);
        void RemoveRange(IEnumerable<TDocument> entities);
        IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> predicate);
        TDocument GetSingleOrDefault(Expression<Func<TDocument, bool>> predicate);
        IEnumerable<TDocument> GetAll();
    }
}

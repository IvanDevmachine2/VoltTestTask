using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VoltTry2.Contracts.Entities;

namespace VoltTry2.Contracts.Repositories
{
    public interface IRepository<TEntity, TKey> where TEntity : IEntity<TKey>
    {
        TEntity GetById(TKey id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        int Count();
        bool Exists(TKey id);
    }

    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : IEntity
    {
    }
}
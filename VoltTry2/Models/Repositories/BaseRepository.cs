using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using VoltTry2.Contracts.Entities;
using VoltTry2.Contracts.Repositories;
using VoltTry2.Models.Entities;

namespace VoltTry2.Models.Repositories
{
    public abstract class BaseRepository<TEntity, TInterface, TContext> : IRepository<TInterface>
        where TEntity : BaseEntity, TInterface
        where TInterface : IEntity
        where TContext : DbContext
    {
        protected readonly TContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected BaseRepository(TContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual TInterface GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual IEnumerable<TInterface> GetAll()
        {
            return _dbSet.ToList().Cast<TInterface>();
        }

        public virtual IEnumerable<TInterface> Find(Expression<Func<TInterface, bool>> predicate)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var body = Expression.Invoke(predicate, parameter);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter);

            return _dbSet.Where(lambda).ToList().Cast<TInterface>();
        }

        public virtual void Add(TInterface entity)
        {
            var concreteEntity = entity as TEntity;
            if (concreteEntity != null)
            {
                concreteEntity.UpdateTimestamps();
                _dbSet.Add(concreteEntity);
                _context.SaveChanges();
            }
        }

        public virtual void AddRange(IEnumerable<TInterface> entities)
        {
            var concreteEntities = entities.OfType<TEntity>().ToList();
            foreach (var entity in concreteEntities)
            {
                entity.UpdateTimestamps();
            }
            _dbSet.AddRange(concreteEntities);
            _context.SaveChanges();
        }

        public virtual void Update(TInterface entity)
        {
            var concreteEntity = entity as TEntity;
            if (concreteEntity != null)
            {
                concreteEntity.UpdateTimestamps();

                // Поиск сущности в контексте (если найдена - меняем то, что в контексте, а не пытаемся добавить сущность повторно)
                var existingEntity = _dbSet.Local.FirstOrDefault(e => e.Id == concreteEntity.Id);
                if (existingEntity != null)
                {
                    _context.Entry(existingEntity).CurrentValues.SetValues(concreteEntity);
                }
                else
                {
                    // Если в контексте нет изменяемой сущности - указываем переданную как новую к изменению
                    _context.Entry(concreteEntity).State = EntityState.Modified;
                }

                _context.SaveChanges();
            }
        }

        public virtual void Remove(TInterface entity)
        {
            var concreteEntity = entity as TEntity;
            if (concreteEntity != null)
            {
                _dbSet.Remove(concreteEntity);
                _context.SaveChanges();
            }
        }

        public virtual void RemoveRange(IEnumerable<TInterface> entities)
        {
            var concreteEntities = entities.OfType<TEntity>().ToList();
            _dbSet.RemoveRange(concreteEntities);
            _context.SaveChanges();
        }

        public virtual int Count()
        {
            return _dbSet.Count();
        }

        public virtual bool Exists(int id)
        {
            return _dbSet.Any(e => e.Id == id);
        }
    }
}
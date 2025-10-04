using Company.DAL.Data.Contexts;
using Company.DAL.Models.Shared;
using Company.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity>(AppDbContext _context) : IGenericRepository<TEntity> where TEntity: BaseEntity
    {
        public IEnumerable<TEntity> GetAll(bool isTracked = false)
        {
            if (isTracked)
                return _context.Set<TEntity>().Where(e => e.IsDeleted == false).ToList();
            else
                return _context.Set<TEntity>().Where(e => e.IsDeleted == false).AsNoTracking().ToList();
        }
        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<TEntity,TResult>> selector)
        {
                return _context.Set<TEntity>().Where(e => e.IsDeleted == false).Select(selector).ToList();
        }
        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity,bool>> predicate)
        {
                return _context.Set<TEntity>()
                               .Where(e => e.IsDeleted == false)
                               .Where(predicate)
                               .ToList();
        }

        public TEntity? GetById(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public void Add(TEntity entity)
        {
            if (entity is null) return;
            _context.Add(entity);
        }
        public void Update(TEntity entity)
        {
            if (entity is null) return;
            _context.Update(entity);
        }
        public void Remove(TEntity entity)
        {
            if (entity is null) return;
            _context.Remove(entity);
        }
    }
}

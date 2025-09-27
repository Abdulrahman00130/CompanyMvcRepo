using Company.DAL.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        int Add(TEntity entity);
        IEnumerable<TEntity> GetAll(bool isTracked = false);
        TEntity? GetById(int id);
        int Remove(TEntity entity);
        int Update(TEntity entity);
    }
}

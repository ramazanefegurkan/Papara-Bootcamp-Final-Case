using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Data.Repositories.GenericRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task Save();
        Task<TEntity?> GetById(long Id);
        Task Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task Delete(long Id);
        Task SoftDelete(long Id);
        Task<List<TEntity>> GetAll(
            Expression<Func<TEntity, bool>>? where = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params string[] includeProperties);
        Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>>? where,
            params string[] includeProperties);
    }
}

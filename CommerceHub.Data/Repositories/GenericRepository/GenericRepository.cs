using CommerceHub.Base.Entity;
using CommerceHub.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Data.Repositories.GenericRepository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly CommerceHubDbContext dbContext;

        public GenericRepository(CommerceHubDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Save()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task<TEntity?> GetById(long Id)
        {
            return await dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == Id && x.IsActive);
        }

        public async Task Insert(TEntity entity)
        {
            await dbContext.Set<TEntity>().AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            dbContext.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            dbContext.Set<TEntity>().Remove(entity);
        }

        public async Task Delete(long Id)
        {
            var entity = await dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == Id);
            if (entity != null)
                dbContext.Set<TEntity>().Remove(entity);
        }

        public async Task<List<TEntity>> GetAll(
            Expression<Func<TEntity, bool>>? where = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params string[] includeProperties)
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            query = query.Where(x => x.IsActive);

            if (where != null)
            {
                query = query.Where(where);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>>? where, params string[] includeProperties)
        {
            var query = dbContext.Set<TEntity>().AsQueryable();

            query = query.Where(x => x.IsActive);

            if (where != null)
            {
                query = query.Where(where);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task SoftDelete(long Id)
        {
            var entity = await dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == Id);
            if (entity != null)
            {
                entity.IsActive = false;
                dbContext.Set<TEntity>().Update(entity);
            }
        }
    }
}

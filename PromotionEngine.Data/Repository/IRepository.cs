using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PromotionEngine.Data.Repository
{
    public interface IRepository
    {
        Task AddAsync<TEntity>(TEntity entity) where TEntity : class;

        Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class;

        Task UpdateListAsync<TEntity>(List<TEntity> entities) where TEntity : class;

        Task<TEntity> GetFirstOrDefaultAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null)
            where TEntity : class;

        Task<int> GetCountAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class;

        Task<IList<TEntity>> GetAllAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class;
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Practices.TransientFaultHandling;
using PromotionEngine.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PromotionEngine.Data.Repository
{
    public class Repository : IRepository
    {
        private readonly ILogger<Repository> _logger;
        private readonly RetryPolicy _retryPolicy;
        private readonly ekartContext _ekartDbContext;

        public Repository(ILogger<Repository> logger, ekartContext ekartDbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ekartDbContext = ekartDbContext;
            _retryPolicy = new RetryPolicy<CustomTransientErrorDetectionStrategy>
                (5, minBackoff: TimeSpan.FromSeconds(0.1), maxBackoff: TimeSpan.FromSeconds(30)
                , deltaBackoff: TimeSpan.FromSeconds(1));
            _retryPolicy.Retrying += RetryPolicyRetrying;
        }

        private void RetryPolicyRetrying(object sender, RetryingEventArgs e)
        {
            _logger.LogInformation(string.Format("Config Service retry count: {0}", e.CurrentRetryCount));

            _logger.LogError(e.LastException.Message);
        }

        public async Task<IList<TEntity>> GetAllAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class
        {
            List<TEntity> result = null;

            try
            {
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    result = await GetQueryable<TEntity>(_ekartDbContext, filter, orderBy, includeProperties, skip, take).ToListAsync().ConfigureAwait(false);
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Repository)}: Exception occurred {ex.Message}");
                throw;
            }

            return result;
        }

        public async Task<TEntity> GetFirstOrDefaultAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null)
            where TEntity : class
        {
            TEntity entity = null;

            try
            {
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    entity = await GetQueryable<TEntity>(_ekartDbContext, filter, orderBy, includeProperties).FirstOrDefaultAsync().ConfigureAwait(false);
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Repository)}: Exception occurred {ex.Message}");
                throw;
            }

            return entity;
        }

        public async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
        {

            try
            {
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    await _ekartDbContext.Set<TEntity>().AddAsync(entity).ConfigureAwait(false);
                    await _ekartDbContext.SaveChangesAsync().ConfigureAwait(false);
                }).ConfigureAwait(false);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError($"{nameof(Repository)}: Exception occurred {dbEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Repository)}: Exception occurred {ex.Message}");
                throw;
            }

        }

        public async Task UpdateAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            try
            {
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    _ekartDbContext.Set<TEntity>().Update(entity);
                    await _ekartDbContext.SaveChangesAsync().ConfigureAwait(false);
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Repository)}: Exception occurred {ex.Message}");
                throw;
            }
        }

        public async Task UpdateListAsync<TEntity>(List<TEntity> entities)
          where TEntity : class
        {
            try
            {
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    _ekartDbContext.Set<TEntity>().UpdateRange(entities);
                    await _ekartDbContext.SaveChangesAsync().ConfigureAwait(false);
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Repository)}: Exception occurred {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetCountAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null)
        where TEntity : class
        {
            int count = 0;

            try
            {
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    count = await GetQueryable<TEntity>(_ekartDbContext, filter).CountAsync().ConfigureAwait(false);
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Repository)}: Exception occurred {ex.Message}");
                throw;
            }

            return count;
        }

        private static IQueryable<TEntity> GetQueryable<TEntity>(ekartContext _ekartDbContext,
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = null,
           int? skip = null,
           int? take = null)
           where TEntity : class
        {
            includeProperties = includeProperties ?? string.Empty;
            IQueryable<TEntity> query = _ekartDbContext.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }
    }
}

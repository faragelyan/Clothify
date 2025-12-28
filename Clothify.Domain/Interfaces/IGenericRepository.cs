using Clothify.Domain.Interfaces.Pagination;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Clothify.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<bool> AddAsync(TEntity entity);
        Task<bool> AddRangeAsync(IEnumerable<TEntity> entityList);
        bool Delete(TEntity entity);
        Task<bool> DeleteManyById<TypeId>(IEnumerable<TypeId> ids);
        Task<bool> DeleteOneById<TypeId>(TypeId id);
        bool DeleteRange(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> GetAllEntitiesAsync(Expression<Func<TEntity, bool>>? filter = null, List<Expression<Func<TEntity, bool>>>? filterList = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? includes = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, bool disableTracking = false, bool ignoreQueryFilter = false, bool splitQuery = false);
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter = null, List<Expression<Func<TEntity, bool>>>? filterList = null, bool disableTracking = false, bool ignoreQueryFilter = false);
        Task<PaginationResponse<TEntity>> GetPaginatedEntitiesAsync(Expression<Func<TEntity, bool>>? filter = null, List<Expression<Func<TEntity, bool>>>? filterList = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? includes = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int pageNumber = 1, int pageSize = 10, bool disableTracking = false, bool ignoreQueryFilter = false);
        Task<IEnumerable<TResult>> GetSelectedEntitiesAsync<TResult>(Expression<Func<TEntity, TResult>> selectExpression, Expression<Func<TEntity, bool>>? filter = null, List<Expression<Func<TEntity, bool>>>? filterList = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? includes = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, bool distinct = false, bool disableTracking = false, bool ignoreQueryFilter = false);
        Task<TEntity?> GetSingleEntityAsync(Expression<Func<TEntity, bool>>? filter = null, List<Expression<Func<TEntity, bool>>>? filterList = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? includes = null, bool disableTracking = false, bool ignoreQueryFilter = false, bool splitQuery = false);
        bool Update(TEntity entity);
        bool UpdateRange(IEnumerable<TEntity> entities);
        Task<TEntity?> FindAsync<TypeId>(TypeId id);

        Task<Dictionary<TKey, TResult>> GetAllEntitiesDictionaryAsync<TKey, TResult>(
                Func<TEntity, TKey> keySelector,
                Func<TEntity, TResult> resultSelector,
                Expression<Func<TEntity, bool>> expression = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                  bool disableTracking = false,
                    bool ignoreQueryFilter = false);
    }
}

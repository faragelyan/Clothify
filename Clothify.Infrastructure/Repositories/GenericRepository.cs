using Clothify.Domain.Interfaces;
using Clothify.Domain.Interfaces.Pagination;
using Clothify.Infrastructure.Peresistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq;
using System.Linq.Expressions;

namespace Clothify.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _context;
        private DbSet<TEntity> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddRangeAsync(IEnumerable<TEntity> entityList)
        {
            try
            {
                await _dbSet.AddRangeAsync(entityList);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<TEntity?> FindAsync<TypeId>(TypeId id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<bool> DeleteOneById<TypeId>(TypeId id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is null)
                return false;

            return Delete(entity);
        }
        public async Task<bool> DeleteManyById<TypeId>(IEnumerable<TypeId> ids)
        {
            List<TEntity> entities = new();

            foreach (var id in ids)
            {
                var entity = await _dbSet.FindAsync(id);

                if (entity is null) return false;
                entities.Add(entity);

            }

            return DeleteRange(entities);
        }

        public bool Delete(TEntity entity)
        {
            try
            {
                _dbSet.Remove(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteRange(IEnumerable<TEntity> entities)
        {
            try
            {
                _dbSet.RemoveRange(entities);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool Update(TEntity entity)
        {
            try
            {
                _context.Update(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateRange(IEnumerable<TEntity> entities)
        {
            try
            {
                _context.UpdateRange(entities);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        // Single Record - no selection 

        public async Task<TEntity?> GetSingleEntityAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        List<Expression<Func<TEntity, bool>>>? filterList = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? includes = null,
        bool disableTracking = false,
        bool ignoreQueryFilter = false,
        bool splitQuery = false
               )
        {
            IQueryable<TEntity> query = _dbSet;

            if (ignoreQueryFilter)
            {
                query = query.IgnoreQueryFilters();
            }

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            if (splitQuery)
            {
                query = query.AsSplitQuery();
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (filter != null)
                query = query.Where(filter);

            if (filterList != null && filterList.Count > 0)
                foreach (var item in filterList)
                    query = query.Where(item);

            var result = await query.FirstOrDefaultAsync();

            return result;
        }




        // Many Records - Pagination
        public async Task<PaginationResponse<TEntity>> GetPaginatedEntitiesAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            List<Expression<Func<TEntity, bool>>>? filterList = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            int pageNumber = 1,
            int pageSize = 10,
            bool disableTracking = false,
            bool ignoreQueryFilter = false)
        {
            IQueryable<TEntity> query = _dbSet;

            if (ignoreQueryFilter)
            {
                query = query.IgnoreQueryFilters();
            }

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (includes != null)
                query = includes(query);

            if (filter != null)
                query = query.Where(filter);

            if (filterList != null && filterList.Count > 0)
                foreach (var item in filterList)
                    query = query.Where(item);


            if (orderBy != null)
                query = orderBy(query);

            int totalCount = await query.CountAsync();

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var pageData = await query.ToListAsync();
            return new PaginationResponse<TEntity>
            {
                Items = pageData,
                Pagination = new PaginationModel()
                {
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                    TotalItems = totalCount,
                    CurrentPage = pageNumber
                },
            };
        }


        // Many Records - Selection
        public async Task<IEnumerable<TResult>> GetSelectedEntitiesAsync<TResult>(
            Expression<Func<TEntity, TResult>> selectExpression,
            Expression<Func<TEntity, bool>>? filter = null,
            List<Expression<Func<TEntity, bool>>>? filterList = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            bool distinct = false,
            bool disableTracking = false,
            bool ignoreQueryFilter = false
            )
        {
            IQueryable<TEntity> query = _dbSet;

            if (ignoreQueryFilter)
            {
                query = query.IgnoreQueryFilters();
            }

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (filter != null)
                query = query.Where(filter);

            if (filterList != null && filterList.Count > 0)
                foreach (var item in filterList)
                    query = query.Where(item);

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var SelectedItems = query.Select(selectExpression);

            if (distinct)
            {
                SelectedItems = SelectedItems.Distinct();
            }

            var result = await SelectedItems.ToListAsync();

            return result;
        }

        // Many Records / NO pagination - NO selection
        public async Task<IEnumerable<TEntity>> GetAllEntitiesAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            List<Expression<Func<TEntity, bool>>>? filterList = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            bool disableTracking = false,
            bool ignoreQueryFilter = false,
            bool splitQuery = false
        )
        {
            IQueryable<TEntity> query = _dbSet;

            if (ignoreQueryFilter)
            {
                query = query.IgnoreQueryFilters();
            }

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            if (splitQuery)
            {
                query = query.AsSplitQuery();
            }

            // Handle includes with optional IgnoreQueryFilters for related entities
            //if (includes != null)
            //{
            //    query = ApplyIncludesWithIgnoreQueryFilters(query, includes, ignoreQueryFilter);
            //}
            if (includes != null)
            {
                query = includes(query);
            }

            if (filter != null)
                query = query.Where(filter);

            if (filterList != null && filterList.Count > 0)
                foreach (var item in filterList)
                    query = query.Where(item);


            if (orderBy != null)
            {
                query = orderBy(query);
            }


            var result = await query.ToListAsync();

            return result;
        }




        public async Task<int> GetCountAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            List<Expression<Func<TEntity, bool>>>? filterList = null,
            bool disableTracking = false,
            bool ignoreQueryFilter = false
            )
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (ignoreQueryFilter)
            {
                query = query.IgnoreQueryFilters();
            }

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
                query = query.Where(filter);

            if (filterList != null && filterList.Count > 0)
                foreach (var item in filterList)
                    query = query.Where(item);


            return await query.CountAsync();
        }
        public async Task<Dictionary<TKey, TResult>> GetAllEntitiesDictionaryAsync<TKey, TResult>(
        Func<TEntity, TKey> keySelector,
        Func<TEntity, TResult> resultSelector,
        Expression<Func<TEntity, bool>> expression = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
          bool disableTracking = false,
            bool ignoreQueryFilter = false)

        {
            IQueryable<TEntity> query = _dbSet;
            if (ignoreQueryFilter)
            {
                query = query.IgnoreQueryFilters();
            }

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToDictionaryAsync(keySelector: keySelector, elementSelector: resultSelector);
        }


    }
}

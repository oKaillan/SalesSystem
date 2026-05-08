using Microsoft.EntityFrameworkCore;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Responses;
using System.Linq.Expressions;

namespace SalesSystem.Database
{
    public class DAL<T> where T : class
    {
        private readonly SalesSystemContext _context;

        public DAL(SalesSystemContext _context)
        {
            this._context = _context;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<PagedResult<T>> GetAllPagedAsync(int skip, int take)
        {
            var query = _context.Set<T>();

            var count = await query.CountAsync();

            var data = await query
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return new PagedResult<T>
            {
                Data = data,
                TotalCount = count
            };
        }

        public async Task<PagedResult<T>> GetAllPagedWithFilterAsync(
            int skip,
            int take,
            Expression<Func<T, bool>>? filter,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy
            )
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter is not null) 
                query = query.Where(filter);

            if (orderBy is not null)
            {
                query = orderBy(query);
            }

            var count = await query.CountAsync();

            var data = await query
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return new PagedResult<T>
            {
                Data = data,
                TotalCount = count
            };
        }
        public async Task<PagedResult<TResult>> GetAllPagedWithSelectorAsync<TResult>(
    int skip,
    int take,
    Expression<Func<T, bool>>? filter,
    Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
    Expression<Func<T, TResult>> selector,
    params Expression<Func<T, object>>[] includes)
    where TResult : class
        {
            try
            {
                IQueryable<T> query = _context.Set<T>();

                if (filter is not null)
                    query = query.Where(filter);

                if (includes is not null)
                    foreach (var i in includes)
                    {
                        query = query.Include(i);
                    }

                if (orderBy is not null)
                {
                    query = orderBy(query);
                }


                var total = await query.CountAsync();

                var data = await query
                    .Skip(skip)
                    .Take(take)
                    .Select(selector)
                    .ToListAsync();

                return new PagedResult<TResult>
                {
                    TotalCount = total,
                    Data = data
                };
            }
            catch (Exception ex) { throw new Exception($"An error has occurred: {ex.Message}"); }
        }

        public async Task<List<T>> GetAllByAsync(Expression<Func<T, bool>> funcPredicate)
        {
            return await _context.Set<T>().Where(funcPredicate).ToListAsync();
        }

        public async Task<PagedResult<T>> GetAllByPagedAsync(int skip, int take, Expression<Func<T, bool>> functionPredicate)
        {
            IQueryable<T> query = _context.Set<T>();

            var data = await query
                .Where(functionPredicate)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResult<T>
            {
                Data = data,
                TotalCount = count
            };
        }

        public async Task<T?> GetByAsync(Expression<Func<T, bool>> functionPredicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(functionPredicate);
        }

        public async Task CreateAsync(T member)
        {
            await _context.AddAsync(member);
            await _context.SaveChangesAsync();
        }

        public void Update(T member)
        {
            _context.Update(member);
            _context.SaveChanges();
        }

        public void Delete(T member)
        {
            _context.Remove(member);
            _context.SaveChanges();
        }
    }
}
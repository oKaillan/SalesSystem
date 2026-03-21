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

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public List<T> GetAllPaged(int skip, int take)
        {
            try
            {
                var result = _context
                    .Set<T>()
                    .Skip(skip)
                    .Take(take)
                    .ToList();

                return result;
            }
            catch (Exception ex) { throw new Exception($"An error has occurred: {ex.Message}"); }

        }

        public PagedResult<TResult> GetAllPagedWithSelector<TResult>(
            int skip,
            int take,
            Func<T, TResult> selector,
            params Expression<Func<T, object>>[] includes)
            where TResult : class
        {
            try
            {

                IQueryable<T> query = _context.Set<T>();

                if (includes is not null)
                    foreach (var i in includes)
                    {
                        query = query.Include(i);
                    }


                var total = query.Count();

                var data = query
                    .Skip(skip)
                    .Take(take)
                    .Select(selector)
                    .ToList();

                return new PagedResult<TResult>
                {
                    TotalCount = total,
                    Data = data
                };
            }
            catch (Exception ex) { throw new Exception($"An error has occurred: {ex.Message}"); }

        }

        /*
        public List<Product> GetProductsWithInclude()
        {
            return _context.Set<Product>()
                .Include(p => p.Category)
                .ToList();
        }
        */
        public List<T> GetAllBy(Func<T, bool> funcPredicate)
        {
            return _context.Set<T>().Where(funcPredicate).ToList();
        }

        public T? GetBy(Func<T, bool> functionPredicate)
        {
            return _context.Set<T>().FirstOrDefault(functionPredicate);
        }

        /*
        public Product GetProductWithInclude(Func<Product, bool> functionPredicate)
        {
            return _context.Set<Product>()
                .Include(p => p.Category)
                .FirstOrDefault(functionPredicate);
        }
        */
        public void Create(T member)
        {
            _context.Add(member);
            _context.SaveChanges();
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
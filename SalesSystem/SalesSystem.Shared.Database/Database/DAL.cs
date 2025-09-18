using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Linq;

namespace SalesSystem.Database
{
    public class DAL<T> where T : class
    {
        private readonly SalesSystemContext _context = new();

        public DAL(SalesSystemContext _context)
        {
            this._context = _context;
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public List<T> GetAllBy(Func<T, bool> funcPredicate)
        {
            return _context.Set<T>().Where(funcPredicate).ToList();
        }

        public T? GetBy(Func<T, bool> functionPredicate)
        {
            return _context.Set<T>().FirstOrDefault(functionPredicate);
        }

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
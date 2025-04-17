using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Linq;

namespace SalesSystem.Database
{
    internal class DAL<T> where T : class
    {
        public readonly SalesSystemContext context = new();

        public DAL(SalesSystemContext context)
        {
            this.context = context;
        }

        public List<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public List<T> GetAllBy(Func<T, bool> funcPredicate)
        {
            return context.Set<T>().Where(funcPredicate).ToList();
        }

        public T GetBy(Func<T, bool> functionPredicate)
        {
            return context.Set<T>().FirstOrDefault(functionPredicate);
        }

        public void Create(T member)
        {
            context.Add(member);
            context.SaveChanges();
        }

        public void Update(T member)
        {
            context.Update(member);
            context.SaveChanges();
        }

        public void Delete(T member)
        {
            context.Remove(member);
            context.SaveChanges();
        }
    }
}
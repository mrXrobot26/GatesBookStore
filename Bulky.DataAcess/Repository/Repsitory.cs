using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAcess.Repository.IRepsitory;
using BulkyAcess.DataAcess.Data;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAcess.Repository
{
    public class Repsitory<T> : IRepsitory<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> DbSet;
        public Repsitory(ApplicationDbContext db)
        {
            _db = db;
            this.DbSet = _db.Set<T>();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> Filter, bool tracked = false, string? includeProperties = null)
        {

            IQueryable<T> query;
            if (tracked)
            {
                query = DbSet;
            }
            else
            {
                query = DbSet.AsNoTracking();
            }
            query = query.Where(Filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.FirstOrDefault();

        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? Filter, string? includeProperties = null)
        {
            IQueryable<T> query = DbSet;
            if (Filter != null)
            {
                query=query.Where(Filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                   query = query.Include(includeProperty);
                }
            }
            return query.ToList();
        }
        public void RemoveRange(IEnumerable<T> entity)
        {
            DbSet.RemoveRange(entity);
        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);     
        }
    }
}

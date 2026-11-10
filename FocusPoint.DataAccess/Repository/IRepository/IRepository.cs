using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string? includeProperties = null, bool tracked = true);

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, string? includeProperties = null);

        public void Add(T entity);

        public void Remove(T entity);

        public void RemoveRange(IEnumerable<T> entity);
        
    }
}

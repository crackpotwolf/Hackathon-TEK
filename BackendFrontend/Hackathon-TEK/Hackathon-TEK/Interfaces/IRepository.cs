using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hackathon_TEK.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        T Add(T model);
        IEnumerable<T> AddRange(IEnumerable<T> models);
        T Update(T models);
        T Update(int id);
        IEnumerable<T> UpdateRange(IEnumerable<T> models);
        bool Remove(T model);
        bool Remove(int id);
        bool RemoveRange(IEnumerable<T> models);
        bool Delete(T model);
        IQueryable<T> GetListQuery();
        IQueryable<T> GetListQueryWithDeleted();
        IEnumerable<T> GetList();
        IEnumerable<T> GetListWithDeleted();
        T SearchById(int id);
        bool Any(Expression<Func<T, bool>> func);
        T FirstOrDefault(Expression<Func<T, bool>> func);
    }
}
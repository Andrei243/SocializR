using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Common
{
    public interface IRepository<T> where T:IEntity
    {
        T Get(int id);
        void Add(T el);
        void AddRange(IEnumerable<T> elems);
        IQueryable<T> Query { get; }
        void Delete(T el);
        void DeleteRange(IEnumerable<T> elems);
        void Update(T el);

    }
}

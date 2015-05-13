using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALWebApi.Interfaces
{
    public interface IWebApiRepository<T> where T:class
    {
        List<T> All { get; }
    
        T GetById(object id);
        void Add(T entity);
        void Update(object id, T entity);
        void Delete(object id);
    }
}

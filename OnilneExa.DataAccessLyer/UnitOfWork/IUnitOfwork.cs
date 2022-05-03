using OnilneExa.DataAccessLyer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnilneExa.DataAccessLyer.UnitOfWork
{
     public interface IUnitOfwork
    {
        IGenericRepository<T> GenericRepository<T>() where T : class;
        void Save();
    }
}

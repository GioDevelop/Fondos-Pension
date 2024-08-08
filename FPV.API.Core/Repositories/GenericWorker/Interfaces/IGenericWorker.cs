using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FPV.API.Core.Repositories.Generic.Interfaces;
using FPV.API.Core.Repositories.Generic.Interfaces;

namespace FPV.API.Core.Repositories.GenericWorker.Interfaces
{
    public interface IGenericWorker
    {
        IGenericRepository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync();

        Task RollBackChangesAsync();
    }
}

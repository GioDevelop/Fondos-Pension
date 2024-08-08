using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPV.Common.Helper.Interfaces
{
    public interface IUtility
    {
        T CastObject<T, TU>(TU obj) where T : new();
    }
}

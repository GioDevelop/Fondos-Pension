using FPV.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPV.API.Business.interfaces
{
    public interface INotificationServices
    {
        Task<Result<string>> SendMail(string Email, string fundName);
    }
}

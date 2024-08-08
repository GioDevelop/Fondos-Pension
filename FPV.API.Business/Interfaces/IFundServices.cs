using FPV.API.Core.Context;
using FPV.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPV.API.Business.Interfaces
{
    public interface IFundServices
    {
        Task<bool> SubscribeFund(FundDto fund);
        Task<bool> UnsuscribeFund(FundDto fund);
        Task<List<FundTransactionDto>> GetTransactionsByCustomerId(string customerId);
        Task<int> BalanceAccount(string customerId);
        Task<List<FundDto>> GetFunds();
        Task<List<FundDto>> GetFundsNotSuscribe(string customerId);
        Task<List<CustomerFundsDto>> GetMyFunds(string customerId);
    }
}

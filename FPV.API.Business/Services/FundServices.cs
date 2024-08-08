using FPV.API.Core.Context;
using FPV.API.Core.Repositories.GenericWorker.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FPV.Common.Enum;
using FPV.Common.Models;
using FPV.API.Business.Interfaces;
using FPV.Common.Helper;
using FPV.API.Core.Repositories.GenericWorker;
using FPV.Common.Helper.Interfaces;

namespace FPV.API.Business.Services
{
    public class FundServices : IFundServices
    {
        public IGenericWorker _genericWorker;
        public FundServices(IGenericWorker genericWorker, IUtility utility)
        {
            _genericWorker = genericWorker;
        }

        public async Task<bool> SubscribeFund(FundDto fund)
        {
            bool isValid = false;
            try
            {

                List<CustomerFund> customerFund = await _genericWorker.Repository<CustomerFund>().GetListByFilterOrderByIncludingAsync(x => x.FundId == fund.Id && x.CustomerId == AppSettingsApi.Settings.FPVApi.CustomerId);

                if (customerFund.Count>0)
                {
                    throw new Exception("Actualmente ya se encuentra registrado a este fondo");
                }
                else
                {
                    var myMoney = await BalanceAccount(AppSettingsApi.Settings.FPVApi.CustomerId);
                    if (myMoney > Convert.ToInt32(fund.LinkingAmount) )
                    {
                        CustomerFund obCustomerFund = new CustomerFund();

                        obCustomerFund.CustomerId = AppSettingsApi.Settings.FPVApi.CustomerId;
                        obCustomerFund.FundId = fund.Id;
                        obCustomerFund.Id = Guid.NewGuid().ToString();
                        await _genericWorker.Repository<CustomerFund>().Add2(obCustomerFund);


                        FundTransaction objFundTransaction = new FundTransaction();
                        objFundTransaction.CustomerId = AppSettingsApi.Settings.FPVApi.CustomerId;
                        objFundTransaction.FundId = fund.Id;
                        objFundTransaction.Description = $"Redencion nueva al fondo {fund.Name}";
                        objFundTransaction.TransactionTypeId = (int)Enumeraciones.TransactionType.Redención;
                        objFundTransaction.TransactionDate = DateTime.Now;
                        objFundTransaction.Value = fund.LinkingAmount;
                        await _genericWorker.Repository<FundTransaction>().Add(objFundTransaction);

                        await _genericWorker.SaveChangesAsync();
                        isValid = true;
                    }
                    else
                    {
                        throw new Exception($"No tiene saldo disponible para vincularse al fondo <{fund.Name}>");
                    }
                   
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
           
            return isValid;
        }

        public async Task<bool> UnsuscribeFund(FundDto fund)
        {
            bool isValid = false;
            try
            {

                List<CustomerFund> customerFund = await _genericWorker.Repository<CustomerFund?>().GetListByFilterOrderByIncludingAsync(x => x.FundId == fund.Id && x.CustomerId == AppSettingsApi.Settings.FPVApi.CustomerId);

                if (customerFund.Count > 0)
                {
                    CustomerFund obCustomerFund = new CustomerFund();

                    await _genericWorker.Repository<CustomerFund>().DeleteById(customerFund.FirstOrDefault().Id);


                    FundTransaction objFundTransaction = new FundTransaction();
                    objFundTransaction.CustomerId = AppSettingsApi.Settings.FPVApi.CustomerId;
                    objFundTransaction.FundId = fund.Id;
                    objFundTransaction.Description = $"Cancelacion suscripcion al fondo: {fund.Name}";
                    objFundTransaction.TransactionTypeId = (int)Enumeraciones.TransactionType.Acumulacion;
                    objFundTransaction.TransactionDate = DateTime.Now;
                    objFundTransaction.Value = fund.LinkingAmount;
                    await _genericWorker.Repository<FundTransaction?>().Add(objFundTransaction);

                    await _genericWorker.SaveChangesAsync();
                    isValid = true;
                    
                }
                else
                {
                    throw new Exception("Usted no se encuentra suscrito a este fondo");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return isValid;
        }

        public async Task<List<FundTransactionDto>> GetTransactionsByCustomerId(string customerId)
        {
            List<FundTransaction> customerTransactions = new List<FundTransaction>();
            List<FundTransactionDto> customerTransactionsDto = new List<FundTransactionDto>();
            try
            {
                customerTransactions = await _genericWorker.Repository<FundTransaction>().GetListByFilterOrderByIncludingAsync(x => x.CustomerId == customerId, null, x=> x.TransactionType);
                if (customerTransactions != null && customerTransactions.Count > 0)
                {
                    foreach (var itemCustomerTransaction in customerTransactions)
                    {
                        FundTransactionDto objFundTransaction = new FundTransactionDto();
                        objFundTransaction.CustomerId = customerId;
                        objFundTransaction.FundId = itemCustomerTransaction.FundId;
                        objFundTransaction.Description = itemCustomerTransaction.Description;
                        objFundTransaction.Id = itemCustomerTransaction.Id;
                        objFundTransaction.TransactionDate = itemCustomerTransaction.TransactionDate;
                        objFundTransaction.TransactionTypeId = itemCustomerTransaction.TransactionTypeId;
                        objFundTransaction.TransactionType = itemCustomerTransaction.TransactionType.Description;

                        objFundTransaction.Value = itemCustomerTransaction.Value;

                        customerTransactionsDto.Add(objFundTransaction);
                    }
                }
               
                return customerTransactionsDto.OrderByDescending(x => x.TransactionDate).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return null;

        }



        public async Task<List<CustomerFundsDto>> GetMyFunds(string customerId)
        {
            List<CustomerFund> customerFunds = new List<CustomerFund>();
            List<CustomerFundsDto> customerFundsDto = new List<CustomerFundsDto>();
            try
            {
                customerFunds = await _genericWorker.Repository<CustomerFund>().GetListByFilterOrderByIncludingAsync(x => x.CustomerId == customerId,null,x=>x.Fund);
                if (customerFunds != null && customerFunds.Count > 0)
                {
                    foreach (var itemCustomerTransaction in customerFunds)
                    {
                        CustomerFundsDto objFundTransaction = new CustomerFundsDto();
                        objFundTransaction.Fund = new FundDto();
                        objFundTransaction.CustomerId = customerId;
                        objFundTransaction.FundId = itemCustomerTransaction.FundId;

                        objFundTransaction.Id = itemCustomerTransaction.Id;
                        objFundTransaction.Fund.Name = itemCustomerTransaction.Fund.Name;
                        objFundTransaction.Fund.LinkingAmount = itemCustomerTransaction.Fund.LinkingAmount;
                        objFundTransaction.Fund.CategoryId = itemCustomerTransaction.Fund.CategoryId;
                        objFundTransaction.Fund.Id = itemCustomerTransaction.Fund.Id;
                        customerFundsDto.Add(objFundTransaction);
                    }
                }

                return customerFundsDto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return null;

        }






        public async Task<List<FundDto>> GetFundsNotSuscribe(string customerId)
        {
            List<FundDto> listFunds = await GetFunds();
            List<CustomerFundsDto> listFundSuscribe = await GetMyFunds(customerId);
            var fundIdsToRemove =  listFundSuscribe.Select(x => x.Fund.Id).ToList();
            listFunds.RemoveAll(f => fundIdsToRemove.Contains(f.Id));

            return listFunds;
        }



        public async Task<List<FundDto>> GetFunds()
        {
            List<FundDto> fundDtos = new List<FundDto>();
            List<Fund> funds = new List<Fund>();
            var customerTransactionss = await _genericWorker.Repository<Fund>().GetAll();
            funds = customerTransactionss.ToList();
            if (funds.Count>0)
            {
                foreach (var itemCustomerTransaction in funds)
                {
                    FundDto objFundDto = new FundDto();
                    objFundDto.Name = itemCustomerTransaction.Name;
                    objFundDto.CategoryId = itemCustomerTransaction.CategoryId;
                    objFundDto.Id = itemCustomerTransaction.Id;
                    objFundDto.LinkingAmount = itemCustomerTransaction.LinkingAmount;
                    fundDtos.Add(objFundDto);
                }
            }
            return fundDtos;
        }


        public async Task<int> BalanceAccount(string customerId)
        {
            int currentMoney = 0;
            try
            {

                List<FundTransaction> customerFund = await _genericWorker.Repository<FundTransaction>().GetListByFilterOrderByIncludingAsync(x =>  x.CustomerId == customerId);

                if (customerFund.Count > 0)
                {
                    var redemptions = Convert.ToInt32(customerFund.Where(x =>
                        x.TransactionTypeId == (int)Enumeraciones.TransactionType.Redención).Sum(z => z.Value));
                    var acumulation = Convert.ToInt32(customerFund.Where(x =>
                        x.TransactionTypeId == (int)Enumeraciones.TransactionType.Acumulacion).Sum(z => z.Value));

                    currentMoney = acumulation - redemptions;

                }
                else
                {
                    throw new Exception("Usted aún no tiene transacciones");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return currentMoney;
        }
    }
}

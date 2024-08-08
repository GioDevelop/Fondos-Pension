using FPV.API.Business.Interfaces;
using FPV.Common.Helper;
using FPV.Common.Helper.Diagnostics;
using FPV.Common.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FPV.API.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class FundController : ControllerBase
    {
        private readonly List<MessageResult> listMessages;
        private readonly IFundServices _fundServices;

        public FundController(IFundServices fundServices)
        {
            listMessages = new List<MessageResult>();
            _fundServices = fundServices;
        }

        [HttpPost]
        [Route("SubscribeFund")]
        public async Task<Response<bool>> SubscribeFund(FundDto fund)
        {
            ExceptionLogging.LogInfo("SubscribeFund");
            Response<bool> response = new();

            try
            {

                response.ObjectResponse = await _fundServices.SubscribeFund(fund);

                if (response.ObjectResponse)
                {

                    response.statusCode = 200;
                    response.message = new()
                    {
                        new MessageResult($"El fondo {fund.Name} se agregó existosamente ")
                    };
                }
                else
                {
                    response.statusCode = 0;
                    response.message = new()
                    {
                        new MessageResult("Ocurrio un error al suscribirse al fondo")
                    };
                }


            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                listMessages.Add(new MessageResult
                {
                    Message = ex.Message
                });
                return new Response<bool>
                {
                    statusCode = 0,
                    message = new()
                    {
                        new MessageResult($"Ocurrio un error al suscribirse al fondo, Error: {ex.Message}")
                    }
            };
            }
            ExceptionLogging.LogInfo("Se suscribe el producto SubscribeFund");
            return response;
        }


        [HttpPost]
        [Route("UnsuscribeFund")]
        public async Task<Response<bool>> UnsuscribeFund(FundDto fund)
        {
            ExceptionLogging.LogInfo("UnsuscribeFund");
            Response<bool> response = new();

            try
            {

                response.ObjectResponse = await _fundServices.UnsuscribeFund(fund);

                if (response.ObjectResponse)
                {

                    response.statusCode = 200;
                    response.message = new()
                    {
                        new MessageResult($"El fondo {fund.Name} se elimino existosamente ")
                    };
                }
                else
                {
                    response.statusCode = 0;
                    response.message = new()
                    {
                        new MessageResult("Ocurrio un error al suscribirse al fondo")
                    };
                }


            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                listMessages.Add(new MessageResult
                {
                    Message = ex.Message
                });
                return new Response<bool>
                {
                    statusCode = 0,
                    message = new()
                    {
                        new MessageResult($"Ocurrio un error al suscribirse al fondo {ex.Message}")
                    }
                };
            }
            ExceptionLogging.LogInfo("Se cancela suscripcion exitosamente");
            return response;
        }

        [HttpGet]
        [Route("TransactionsByCustomerId")]
        public async Task<Response<List<FundTransactionDto>>> TransactionsByCustomerId(string customerId)
        {
            customerId = AppSettingsApi.Settings.FPVApi.CustomerId;
            ExceptionLogging.LogInfo("GetTransactionsByCustomerId");
            Response<List<FundTransactionDto>> response = new();

            try
            {

                

                if (!string.IsNullOrEmpty(customerId))
                {
                    response.ObjectResponse = await _fundServices.GetTransactionsByCustomerId(customerId);
                    response.statusCode = 200;
                    response.message = new()
                    {
                        new MessageResult($"Las transacciones se obtuvieron exitosamente ")
                    };
                }
                else
                {
                    response.statusCode = 0;
                    response.message = new()
                    {
                        new MessageResult("El customerId es requerido")
                    };
                }


            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                listMessages.Add(new MessageResult
                {
                    Message = ex.Message
                });
                return new Response<List<FundTransactionDto>>
                {
                    statusCode = 0,
                    message = new()
                    {
                        new MessageResult($"Ocurrio un error al obtener las transacciones, Error: {ex.Message}")
                    }
                };
            }
            ExceptionLogging.LogInfo("Se obtuvieron las transacciones Exitosamente ");
            return response;
        }







        [HttpGet]
        [Route("MyFunds")]
        public async Task<Response<List<CustomerFundsDto>>> MyFunds(string customerId)
        {
            customerId = AppSettingsApi.Settings.FPVApi.CustomerId;
            ExceptionLogging.LogInfo("GetTransactionsByCustomerId");
            Response<List<CustomerFundsDto>> response = new();

            try
            {



                if (!string.IsNullOrEmpty(customerId))
                {
                    response.ObjectResponse = await _fundServices.GetMyFunds(customerId);
                    response.statusCode = 200;
                    response.message = new()
                    {
                        new MessageResult($"Las transacciones se obtuvieron exitosamente ")
                    };
                }
                else
                {
                    response.statusCode = 0;
                    response.message = new()
                    {
                        new MessageResult("El customerId es requerido")
                    };
                }


            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                listMessages.Add(new MessageResult
                {
                    Message = ex.Message
                });
                return new Response<List<CustomerFundsDto>>
                {
                    statusCode = 0,
                    message = new()
                    {
                        new MessageResult($"Ocurrio un error al obtener las transacciones, Error: {ex.Message}")
                    }
                };
            }
            ExceptionLogging.LogInfo("Se obtuvieron las transacciones Exitosamente ");
            return response;
        }





        [HttpGet]
        [Route("Funds")]
        public async Task<Response<List<FundDto>>> Funds()
        {
            ExceptionLogging.LogInfo("GetFund");
            Response<List<FundDto>> response = new();

            try
            {
                    response.ObjectResponse = await _fundServices.GetFunds();
                    response.statusCode = 200;
                    response.message = new()
                    {
                        new MessageResult($"Los Fondos se obtuvieron exitosamente ")
                    };
                


            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                listMessages.Add(new MessageResult
                {
                    Message = ex.Message
                });
                return new Response<List<FundDto>>
                {
                    statusCode = 0,
                    message = new()
                    {
                        new MessageResult($"Ocurrio un error al obtener las transacciones, Error: {ex.Message}")
                    }
                };
            }
            ExceptionLogging.LogInfo("Se obtuvieron las transacciones Exitosamente ");
            return response;
        }



        [HttpGet]
        [Route("FundsNotSuscribe")]
        public async Task<Response<List<FundDto>>> FundsNotSuscribe(string customerId)
        {
            customerId = AppSettingsApi.Settings.FPVApi.CustomerId;
            ExceptionLogging.LogInfo("FundsNotSuscribe");
            Response<List<FundDto>> response = new();

            try
            {
                response.ObjectResponse = await _fundServices.GetFundsNotSuscribe(customerId);
                response.statusCode = 200;
                response.message = new()
                {
                    new MessageResult($"Los Fondos se obtuvieron exitosamente ")
                };



            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                listMessages.Add(new MessageResult
                {
                    Message = ex.Message
                });
                return new Response<List<FundDto>>
                {
                    statusCode = 0,
                    message = new()
                    {
                        new MessageResult($"Ocurrio un error al obtener los fondos, Error: {ex.Message}")
                    }
                };
            }
            ExceptionLogging.LogInfo("Se obtuvieron los fondos Exitosamente ");
            return response;
        }


        [HttpGet]
        [Route("BalanceAccount")]
        public async Task<Response<int>> BalanceAccount(string customerId)
        {
            customerId = AppSettingsApi.Settings.FPVApi.CustomerId;
            ExceptionLogging.LogInfo("BalanceAccount");
            Response<int> response = new();

            try
            {

                if (!string.IsNullOrEmpty(customerId))
                {
                    response.ObjectResponse = await _fundServices.BalanceAccount(customerId);
                    response.statusCode = 200;
                    response.message = new()
                    {
                        new MessageResult($"El balance de cuenta se obtuvo exitosamente ")
                    };
                }
                else
                {
                    response.statusCode = 0;
                    response.message = new()
                    {
                        new MessageResult("El customerId es requerido")
                    };
                }


            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                listMessages.Add(new MessageResult
                {
                    Message = ex.Message
                });
                return new Response<int>
                {
                    statusCode = 0,
                    message = new()
                    {
                        new MessageResult($"Ocurrio un error al obtener las transacciones, Error: {ex.Message}")
                    }
                };
            }
            ExceptionLogging.LogInfo("Se obtuvieron las transacciones Exitosamente ");
            return response;
        }

    }
}

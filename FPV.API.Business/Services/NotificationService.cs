using FPV.API.Business.interfaces;
using FPV.Common.Helper.Diagnostics;
using FPV.Common.IntegrationApi.MailingApi.Interfaces;
using FPV.Common.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPV.API.Business.Services
{
    public class NotificationService : INotificationServices
    {
        public IMailingApi _mailingApi;

        public NotificationService(IMailingApi mailingApi)
        {
            _mailingApi = mailingApi;
        }
        public async Task<Result<string>> SendMail(string Email, string fundName)
        {
            Result<string> response = new Result<string>();
            response.IsValid = false;
            try
            {
                Hashtable parameters = new Hashtable();


                try
                {
                    parameters.Add("[Email]", $"{Email}");
                    parameters.Add("[Fund]", $"{fundName}");

                }
                catch (Exception e)
                {
                }

                response.IsValid = await _mailingApi.SendMail(Email, "Auteco.Fund", parameters, "ArchivoSmtp.Auteco", "MailConfiguration.Auteco");
                if (response.IsValid)
                    response.Message = $"Se envio el codigo al correo {Email} exitosamente.";
                else
                    response.Message = $"No se envio el codigo al correo {Email}.";

                
            }
            catch (Exception ex)
            {
                response.IsValid = false;
                ExceptionLogging.LogException(new Exception($"Ocurrio un error enviando el Mail en Notification Services, Error: {ex}"));

            }
            return response;

        }

    }
}

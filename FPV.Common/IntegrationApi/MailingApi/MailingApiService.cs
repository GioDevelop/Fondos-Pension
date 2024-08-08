using Big.Emtelco.EmtelcoPoints.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using FPV.Common.Helper;
using FPV.Common.Helper.Diagnostics;
using FPV.Common.IntegrationApi.MailingApi.Interfaces;

namespace FPV.Common.IntegrationApi.MailingApi
{

    public class MailingApiService : IMailingApi
    {

        
        public async Task<bool> SendMail(string? email, string? EmailTemplateId, Hashtable parameters, string strSmtp, string strMailConfiguration)
        {
            bool isValid = false;
            try
            {
                string msg = string.Empty;
                //envio correo
                MailingRequest objNotificationRequest = new MailingRequest();
                ResponseSendEmail<bool> responseMail = new ResponseSendEmail<bool>();

                objNotificationRequest.parameters = parameters;
                objNotificationRequest.mailTo = email;
                objNotificationRequest.messageId = EmailTemplateId;

                if (!string.IsNullOrEmpty(strSmtp))
                    objNotificationRequest.ConfigSmtp = strSmtp;

                if (!string.IsNullOrEmpty(strMailConfiguration))
                    objNotificationRequest.MailConfiguration = strMailConfiguration;

                responseMail = await SendMail(objNotificationRequest);
                if (responseMail != null)
                {
                    foreach (var item in responseMail.Message)
                    {
                        msg = item.Message;
                    }

                    if (responseMail.Status && !msg.Contains("No fué posible enviar el mensaje"))
                    {
                        ExceptionLogging.LogInfo(msg);
                        isValid = true;
                    }
                    else
                    {
                        ExceptionLogging.LogInfo(string.Format("Error al enviar el correo: {0}. Error: {1} ", email, msg));
                    }
                }
                else
                {
                    ExceptionLogging.LogInfo("Error al conectarse al api de envio de correo.");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
            }
            return isValid;
        }

        public async Task<ResponseSendEmail<bool>> SendMail(MailingRequest request)
        {
            //ExceptionLogging.LogInfo("MailingApi SendMail");
            //ExceptionLogging.LogInfo("BaseUrlEmail: "+ BaseUrlEmail);
            var service = new ServicesHelper<ResponseSendEmail<bool>>(AppSettingsApi.Settings.Mailing.BaseApi, AppSettingsApi.Settings.Mailing.RoutesEmail.SendPendingMails, request, AppSettingsApi.Settings.Mailing.UserName, AppSettingsApi.Settings.Mailing.Password, RestSharp.Method.Post);
            return service.GetResponse();
        }
    }
}

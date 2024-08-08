using Big.Core.EstratecMasivSMS;

using System;
using System.Collections.Generic;
using System.Text;

namespace FPV.Common.Helper.IntegrationSms
{
    public class MyPersonalEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string County { get; set; }
        public string Message { get; set; }
        public string Operator { get; set; }
    }

    /// <summary>
    /// Clase que permite la integracion con el proveedor de SMS
    /// </summary>
    public static class IntegrationSMS
    {
        /// <summary>
        /// Método que realiza el envio del SMS al usuario.
        /// </summary>
        /// <param name="consumer">Entidad de tipo<seealso cref="Consumer"/> </param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResulMessage SendSMS(string phoneNumber, string messageSource)
        {
            //bool actionResult = false;
            try
            {
                ModelRequestSms model = new ModelRequestSms();
                model.to = phoneNumber;
                model.text = messageSource;
                model.username = AppSettingsApi.Settings.EstratecMasivSMS.username;
                model.password = AppSettingsApi.Settings.EstratecMasivSMS.password;
                model.url = AppSettingsApi.Settings.EstratecMasivSMS.url;
                SendSmsNew(model);

            }
            catch (Exception ex)
            {
                //Diagnostics.ExceptionLogging.LogException(new Exception(string.Format("Ocurrió un error al enviar SMS: {0} ", ex.ToString())));
                ExceptionLogging.LogException(new Exception(string.Format("Ocurrió un error al enviar SMS: {0} ", ex.ToString())));
                
            }

            return null;
        }

        public static ResulMessage SendSmsNew(ModelRequestSms model)
        {
            return IntegrationService.SendMessage(model);
        }
    }
}

using FPV.API.Business.interfaces;
using FPV.Common.Helper.Diagnostics;
using FPV.Common.Helper.IntegrationSms;
using FPV.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace FPV.API.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationServices _notificationServices;

        public NotificationController(INotificationServices notificationServices)
        {
            _notificationServices = notificationServices;
        }

        [HttpPost]
        [Route("SendEmails")]
        //[ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<Response<bool>> SendEmails([FromQuery] string email, [FromQuery] string fundName)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                var response2 = await _notificationServices.SendMail(email,fundName);
                response.message = new List<MessageResult>()
                {
                    new MessageResult() { Message = "Se enviaron los correos pendientes Exitosamente" }
                };
                response.Status = true;
            }
            catch (Exception e)
            {
                response.ObjectResponse = false;
                response.Status = false;
                response.message = new List<MessageResult>()
                {
                    new MessageResult() { Message = "Ocurrio un error al enviar los mensajes péndientes." }
                };
            }
            return response;
        }
        
        [HttpPost]
        [Route("SendSms")]
        //[ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<Response<bool>> SendSms(string PhoneNumber, string fundName)
        {
            Response<bool> response = new Response<bool>();

            SendSMS($"Bienvenido al nuevo fondo {fundName}", PhoneNumber, out string sucess);


            return response;
        }

        public static bool SendSMS(string msgSMS, string MobileNumber, out string msg)
        {
            bool isvalid = false;
            string Message = string.Empty;
            msg = string.Empty;
            try
            {
                //Enviar SMS con el OTP
                 IntegrationSMS.SendSMS(MobileNumber, msgSMS);

               
            }
            catch (Exception ex)
            {
                isvalid = false;
                ExceptionLogging.LogException(new Exception($"SendSMS. Error: {ex.Message}"));
            }
            return isvalid;

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FPV.Common.Helper.IntegrationSms
{
    public class MessageSMS
    {
        public static readonly string smsVerificationCodeLogin = "{0} Ya eres un Tecnico Auteco. Comienza esta carrera en www.sociosdelprogreso.com con tu usuario que es tu numero de documento y esta es tu contrasena: {1}";

        public static readonly string smsVerificationCodeRedemption = "Tu codigo de redencion es: {0} Vigencia: {1} Link de PDF: {2}";

        public static readonly string smsRecovertPassword = "{0} Has recuperado tu contrasena con exito, Tu contrasena actual es: {1}";

    }
}

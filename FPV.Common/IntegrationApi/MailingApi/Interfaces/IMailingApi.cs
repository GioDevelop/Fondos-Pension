using Big.Emtelco.EmtelcoPoints.Common.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPV.Common.IntegrationApi.MailingApi.Interfaces
{
    public interface IMailingApi
    {
        Task<bool> SendMail(string? email, string? EmailTemplateId, Hashtable parameters, string strSmtp, string strMailConfiguration);
        Task<ResponseSendEmail<bool>> SendMail(MailingRequest request);

    }
}

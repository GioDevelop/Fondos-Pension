using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FPV.Common.Models;

namespace Big.Emtelco.EmtelcoPoints.Common.Models
{
    public class MailingRequest
    {
        public Hashtable parameters { get; set; } = null!;

        public string mailTo { get; set; } = null!;

        public string messageId { get; set; } = null!;

        public string? ConfigSmtp { get; set; }

        public string? MailConfiguration { get; set; }
    }

    public class ResponseSendEmail<T>
    {
        public bool Status { get; set; }
        public List<MessageResult> Message { get; set; }
        public T ObjectResponse { get; set; }

        public ResponseSendEmail()
        {
            Message = new List<MessageResult>();
        }
    }
}

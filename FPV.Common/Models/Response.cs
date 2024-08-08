using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPV.Common.Models
{
    public class Response<T>
    {
        //public int StatusCode { set; get; }
        public bool Status { get; set; }
        public int statusCode { get; set; }
        public List<MessageResult> message { get; set; }
        public T ObjectResponse { get; set; }

        public Response()
        {
            message = new List<MessageResult>();
        }

        public Response(bool status, List<MessageResult> message, T objectResponse)
        {
            this.Status = status;
            this.message = message;
            this.ObjectResponse = objectResponse;
        }
    }
    public class MessageResult
    {
        public MessageResult()
        {

        }
        public MessageResult(string message)
        {
            Message = message;
        }
        public string Message { get; set; }

    }
}

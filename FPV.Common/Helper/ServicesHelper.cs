using RestSharp.Authenticators;
using RestSharp;
using System.Net;
using Newtonsoft.Json;

using FPV.Common.Helper.Diagnostics;

namespace FPV.Common.Helper
{
    public class ServicesHelper<TR>
    {
        private RestClient client;
        private RestRequest request;
        private RestResponse response;
        private string result;
        private HttpStatusCode statusCode;
        //private TR dataobject;
        private TR dataobject;

        public ServicesHelper(string siteBase, string action, object model, string username, string password, Method method, Dictionary<string, string> parameters = null, Dictionary<string, string> headers = null)
        {
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            this.client = new RestClient(siteBase);
            this.client.Authenticator = new HttpBasicAuthenticator(username, password);
            this.request = new RestRequest(string.Format("{0}/{1}", siteBase, action));
            //this.request.AddJsonBody(model);
            if (model != null)
            {
                //request.AddJsonBody(model);
                var json = JsonConvert.SerializeObject(model);
                this.request.AddParameter("application/json", json, ParameterType.RequestBody);
            }
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    request.AddQueryParameter(item.Key, item.Value);
                }
            }
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.AddHeader(item.Key, item.Value);
                }
            }
            //call service
            Run<TR>(method);
        }

        public void Run<T>(Method method)
        {
            try
            {
                this.request.Method = method;
                this.response = client.Execute(request);

                if (this.response.Content != null)
                {
                    this.result = this.response.Content.ToString();
                    this.statusCode = response.StatusCode;
                    this.dataobject = JsonConvert.DeserializeObject<TR>(this.result);
                }
            }
            catch (System.Exception ex)
            {
                ExceptionLogging.LogException(ex);
            }
        }
        public TR GetResponse()
        {
            return dataobject;
        }
        public string GetResponseRaw()
        {
            return result;
        }

        public HttpStatusCode GetResponseStatusCode()
        {
            return statusCode;
        }

    }
}

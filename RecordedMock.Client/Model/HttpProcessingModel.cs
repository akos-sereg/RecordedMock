using RecordedMock.Client.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace RecordedMock.Client.Model
{
    public class HttpProcessingModel
    {
        /// <summary>
        /// This property represents the type of current instance.
        /// </summary>
        public string Type { get; set; }

        public HttpRequestModel Request { get; set; }

        public HttpResponseModel Response { get; set; }

        public HttpProcessingModel()
        {
            this.Type = typeof(HttpProcessingModel).ToString();
        }

        public HttpProcessingModel(HttpActionExecutedContext actionExecutedContext)
            : this()
        {
            this.Request = new HttpRequestModel();
            this.Request.RecordedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            this.Request.RequestUri = actionExecutedContext.Request.RequestUri.ToString();
            this.Request.QueryString = actionExecutedContext.Request.GetQueryNameValuePairs();
            this.Request.Method = actionExecutedContext.Request.Method.ToString();
            this.Request.Headers = new Dictionary<string, IEnumerable<string>>();
            this.Request.Content = (string)actionExecutedContext.ActionContext.ActionArguments[RecordRequestAttribute.RequestContentKey];

            foreach (var header in actionExecutedContext.Request.Headers)
            {
                this.Request.Headers.Add(header.Key, header.Value);
            }

            this.Response = new HttpResponseModel();
            this.Response.Content = (string)actionExecutedContext.ActionContext.ActionArguments[RecordRequestAttribute.ResponseContentKey];
        }
    }
}

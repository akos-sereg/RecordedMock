using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using RecordedMock.Client.Filters;

namespace RecordedMock.Client.Model
{
    public class HttpRequestModel
    {
        public string RecordedAt { get; set; }

        public string RequestUri { get; set; }

        public IEnumerable<KeyValuePair<string, string>> QueryString { get; set; }

        public string Method { get; set; }

        public Dictionary<string, IEnumerable<string>> Headers { get; set; }

        public string ContentType { get; set; }

        public string Content { get; set; }

        public HttpRequestModel()
        {
        }

        public HttpRequestModel(HttpRequestMessage request)
        {
            this.RecordedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            this.RequestUri = request.RequestUri.ToString();
            this.QueryString = request.GetQueryNameValuePairs();
            this.Method = request.Method.ToString();
            this.Headers = new Dictionary<string, IEnumerable<string>>();

            if (request.Content != null)
            {
                this.ContentType = request.Content.Headers.ContentType.MediaType;
                this.Content = request.Content.ReadAsStringAsync().Result;
            }

            foreach (var header in request.Headers)
            {
                this.Headers.Add(header.Key, header.Value);
            }
        }

        public HttpRequestModel(HttpActionExecutedContext actionExecutedContext)
        {
            this.RecordedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            this.RequestUri = actionExecutedContext.Request.RequestUri.ToString();
            this.QueryString = actionExecutedContext.Request.GetQueryNameValuePairs();
            this.Method = actionExecutedContext.Request.Method.ToString();
            this.Headers = new Dictionary<string, IEnumerable<string>>();
            this.ContentType = (string)actionExecutedContext.ActionContext.ActionArguments[RecordRequestAttribute.RequestContentTypeKey];
            this.Content = (string)actionExecutedContext.ActionContext.ActionArguments[RecordRequestAttribute.RequestContentKey];

            foreach (var header in actionExecutedContext.Request.Headers)
            {
                this.Headers.Add(header.Key, header.Value);
            }
        }
    }
}

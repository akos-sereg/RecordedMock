using RecordedMock.Client.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace RecordedMock.Client.Model
{
    public class HttpResponseModel
    {
        public string ContentType { get; set; }

        public string Content { get; set; }

        public HttpResponseModel()
        {
        }

        public HttpResponseModel(HttpResponseMessage response)
        {
            this.ContentType = response.Content.Headers.ContentType.MediaType;
            this.Content = response.Content.ReadAsStringAsync().Result;
        }

        public HttpResponseModel(HttpActionExecutedContext actionExecutedContext)
        {
            this.ContentType = (string)actionExecutedContext.ActionContext.ActionArguments[RecordRequestAttribute.ResponseContentTypeKey];
            this.Content = (string)actionExecutedContext.ActionContext.ActionArguments[RecordRequestAttribute.ResponseContentKey];
        }
    }
}

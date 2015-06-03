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
            this.Request = new HttpRequestModel(actionExecutedContext);
            this.Response = new HttpResponseModel(actionExecutedContext);
        }
    }
}

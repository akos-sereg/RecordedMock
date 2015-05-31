using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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

        public HttpProcessingModel(HttpRequestMessage request, HttpResponseMessage response)
            : this()
        {
            this.Request = new HttpRequestModel();
            this.Request.RecordedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            this.Request.RequestUri = request.RequestUri.ToString();
            this.Request.QueryString = request.GetQueryNameValuePairs();
            this.Request.Method = request.Method.ToString();
            this.Request.Headers = new Dictionary<string, IEnumerable<string>>();
            this.Request.Content = request.Content.ReadAsStringAsync().Result;

            foreach (var header in request.Headers)
            {
                this.Request.Headers.Add(header.Key, header.Value);
            }

            this.Response = new HttpResponseModel();
            this.Response.Content = response.Content.ReadAsStringAsync().Result;
        }
    }
}

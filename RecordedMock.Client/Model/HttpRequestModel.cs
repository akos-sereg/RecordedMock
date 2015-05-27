using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RecordedMock.Client.Model
{
    public class HttpRequestModel
    {
        public string RequestUri { get; set; }

        public IEnumerable<KeyValuePair<string, string>> QueryString { get; set; }

        public HttpMethod Method { get; set; }

        public HttpRequestHeaders Headers { get; set; }

        public HttpContent Content { get; set; }

        public HttpRequestModel(HttpRequestMessage request)
        {
            this.RequestUri = request.RequestUri.ToString();
            this.QueryString = request.GetQueryNameValuePairs();
            this.Method = request.Method;
            this.Headers = request.Headers;
            this.Content = request.Content;
        }
    }
}

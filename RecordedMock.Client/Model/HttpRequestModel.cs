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
        public string RecordedAt { get; set; }

        public string RequestUri { get; set; }

        public IEnumerable<KeyValuePair<string, string>> QueryString { get; set; }

        public string Method { get; set; }

        public Dictionary<string, IEnumerable<string>> Headers { get; set; }

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
            this.Content = request.Content.ReadAsStringAsync().Result;

            foreach (var header in request.Headers)
            {
                this.Headers.Add(header.Key, header.Value);
            }
        }
    }
}

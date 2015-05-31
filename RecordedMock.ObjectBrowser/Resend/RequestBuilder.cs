using RecordedMock.Client.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RecordedMock.ObjectBrowser.Resend
{
    public class RequestBuilder
    {
        public HttpRequestModel Request { get; set; }

        public RequestBuilder(HttpRequestModel request)
        {
            this.Request = request;
        }

        public HttpRequestMessage Build()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            NameValueCollection queryString = null;
            if (this.Request.QueryString != null)
            {
                queryString = new NameValueCollection();
                foreach (KeyValuePair<string, string> kvPair in this.Request.QueryString)
                {
                    queryString.Add(kvPair.Key, kvPair.Value);
                }
            }

            request.Method = new HttpMethod(this.Request.Method);
            request.RequestUri = new Uri(string.Format("{0}{1}{2}",
                this.Request.RequestUri,
                queryString == null ? string.Empty : "?",
                this.ToQueryString(queryString)));

            if (!string.IsNullOrEmpty(this.Request.Content))
            {
                request.Content = new StringContent(this.Request.Content);

                if (!string.IsNullOrEmpty(this.Request.ContentType))
                {
                    request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(this.Request.ContentType);
                }
            }

            return request;
        }

        public string ToQueryString(NameValueCollection source)
        {
            return String.Join("&", source.AllKeys
                .SelectMany(key => source.GetValues(key)
                    .Select(value => String.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value))))
                .ToArray());
        }
    }
}

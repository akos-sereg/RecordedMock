using RecordedMock.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RecordedMock.ObjectBrowser.Resend
{
    public class HttpProcessingValidator
    {
        public HttpResponseMessage Actual { get; set; }

        public HttpResponseModel Expected { get; set; }

        public HttpProcessingValidator(HttpResponseMessage actual, HttpResponseModel expected)
        {
            this.Actual = actual;
            this.Expected = expected;
        }

        public bool Validate(bool compareContent = true) 
        {
            if (compareContent && 
                (this.Actual.Content.ReadAsStringAsync().Result != this.Expected.Content))
            {
                return false;
            }

            return true;
        }

    }
}

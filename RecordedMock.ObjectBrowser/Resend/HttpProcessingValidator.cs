using RecordedMock.Client.Model;
using RecordedMock.ObjectBrowser.Exception;
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
        public HttpResponseModel Actual { get; set; }

        public HttpResponseModel Expected { get; set; }

        public HttpProcessingValidator(HttpResponseModel actual, HttpResponseModel expected)
        {
            this.Actual = actual;
            this.Expected = expected;
        }

        public bool Validate(bool compareContent = true, bool compareHeaderContentType = true) 
        {
            if (compareContent && 
                (this.Actual.Content != this.Expected.Content))
            {
                throw new HttpProcessingValidationException("Content mismatch.");
            }

            if (compareHeaderContentType &&
                (this.Actual.ContentType != this.Expected.ContentType)) 
            {
                throw new HttpProcessingValidationException("Header.ContentType mismatch");
            }

            return true;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordedMock.ObjectBrowser.Model
{
    public class Display
    {
        private HttpProcessingTestCase testCase;

        public Display(HttpProcessingTestCase testCase)
        {
            this.testCase = testCase;
        }

        #region Derived properties for display

        public string RequestedAuthority
        {
            get
            {
                try
                {
                    Uri requestedUri = new Uri(this.testCase.RecordedProcessing.Request.RequestUri);
                    return requestedUri.Authority;
                }
                catch
                {
                    return "(unable to parse url)";
                }
            }
        }

        public string RequestedPath
        {
            get
            {
                try
                {
                    Uri requestedUri = new Uri(this.testCase.RecordedProcessing.Request.RequestUri);
                    return requestedUri.LocalPath;
                }
                catch
                {
                    return "(unable to parse url)";
                }
            }
        }

        public string RequestedQueryString
        {
            get
            {
                try
                {
                    Uri requestedUri = new Uri(this.testCase.RecordedProcessing.Request.RequestUri);
                    return requestedUri.Query;
                }
                catch
                {
                    return "(unable to parse url)";
                }
            }
        }

        #endregion
    }
}

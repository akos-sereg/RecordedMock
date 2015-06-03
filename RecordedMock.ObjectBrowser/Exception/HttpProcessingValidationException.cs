using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordedMock.ObjectBrowser.Exception
{
    public class HttpProcessingValidationException : ApplicationException
    {
        public HttpProcessingValidationException(string message)
            : base(message)
        {
        }
    }
}

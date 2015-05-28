using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordedMock.Client.Exception
{
    public class RecordedMockException : ApplicationException
    {
        public RecordedMockException(string message) : base(message) { }
    }
}

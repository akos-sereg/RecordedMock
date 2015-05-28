using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordedMock.Client.Model
{
    public class InvocationModel
    {
        public object[] Arguments { get; set; }

        public object ReturnValue { get; set; }

        public Exception Exception { get; set; }
    }
}

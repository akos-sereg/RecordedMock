using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordedMock.Client.Model
{
    public class InvocationModel
    {
        public string Namespace { get; set; }
        
        public string Class { get; set; }

        public string MethodName { get; set; }

        public object[] Arguments { get; set; }

        public object ReturnValue { get; set; }

        public System.Exception Exception { get; set; }
    }
}

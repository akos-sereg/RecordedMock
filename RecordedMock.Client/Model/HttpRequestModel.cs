using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}

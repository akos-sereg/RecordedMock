using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordedMock.SampleWebApi.Infrastructure
{
    public interface IDataAccessConfiguration
    {
        Dictionary<string, string> Configuration { get; }
    }
}

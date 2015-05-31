using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordedMock.Client.Proxy
{
    public class RecordingMock
    {
        public static T Create<T>(Object service, string recordingFilePath, int maxDumpSizeInMbs)
            where T : class
        {
            return new ProxyGenerator().CreateInterfaceProxyWithTarget<T>((T)service, new RecordingInterceptor(recordingFilePath, maxDumpSizeInMbs));
        }
    }
}

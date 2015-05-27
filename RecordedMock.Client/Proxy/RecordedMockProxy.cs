using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordedMock.Client.Proxy
{
    public class RecordedMockProxy
    {
        public static T Create<T>(Object service)
            where T : class
        {
            return new ProxyGenerator().CreateInterfaceProxyWithTarget<T>((T)service, new RecordingInterceptor());
        }
    }
}

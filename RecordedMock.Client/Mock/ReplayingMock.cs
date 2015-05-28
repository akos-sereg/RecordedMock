using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordedMock.Client.Mock
{
    public class ReplayingMock
    {
        public static T Create<T>(Object service, string mockFilePath)
            where T : class
        {
            return new ProxyGenerator().CreateInterfaceProxyWithTarget<T>((T)service, new ReplayingInterceptor(mockFilePath));
        }
    }
}

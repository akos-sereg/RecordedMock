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
        /// <summary>
        /// Creates an instance of RecordingMock service. The service's behaviour will be the same as your <see cref="service"/>, but all interactions will be logged.
        /// </summary>
        /// <typeparam name="T">Interface of your service</typeparam>
        /// <param name="service">Your service implementation</param>
        /// <param name="recordingFilePath">Recorded interactions will be logged here</param>
        /// <param name="maxDumpSizeInMbs">Maximum allowed dump size of <see cref="recordingFilePath"/></param>
        /// <param name="isDeterministic">Should be set to true, if your service's behaviour is deterministic: for same input (arguments), it provides the same output (return value or exception). 
        /// If this configuration property is set to true, invocations in object dump will be unique.</param>
        /// <returns></returns>
        public static T Create<T>(Object service, string recordingFilePath, int maxDumpSizeInMbs, bool isDeterministic)
            where T : class
        {
            return new ProxyGenerator().CreateInterfaceProxyWithTarget<T>((T)service, new RecordingInterceptor(recordingFilePath, maxDumpSizeInMbs, isDeterministic));
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecordedMock.Client.Proxy;
using System.IO;
using Moq;
using Castle.DynamicProxy;

namespace RecordedMock.ClientTest
{
    [TestClass]
    public class RecordingInterceptorTest
    {
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Interceptor_ThrowsException_Works()
        {
            // Arrange
            Mock<IInvocation> invocation = new Mock<IInvocation>();
            invocation.Setup(x => x.Proceed()).Throws(new ApplicationException());

            // Act
            RecordingInterceptor interceptor = new RecordingInterceptor(Path.GetTempFileName(), 10);
            interceptor.Intercept(invocation.Object);
        }

        [TestMethod]
        public void Interceptor_ReturnNormally_Works()
        {
            // Arrange
            Mock<IInvocation> invocation = new Mock<IInvocation>();

            // Act
            RecordingInterceptor interceptor = new RecordingInterceptor(Path.GetTempFileName(), 10);
            interceptor.Intercept(invocation.Object);
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecordedMock.Client.Proxy;
using System.IO;
using Moq;
using Castle.DynamicProxy;
using System.Reflection;

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
            invocation.SetupGet(x => x.InvocationTarget).Returns(new Object());
            invocation.SetupGet(x => x.Method).Returns(new Mock<MethodInfo>().Object);
            invocation.Setup(x => x.Proceed()).Throws(new ApplicationException());

            // Act
            RecordingInterceptor interceptor = new RecordingInterceptor(Path.GetTempFileName(), 10, true);
            interceptor.Intercept(invocation.Object);
        }

        [TestMethod]
        public void Interceptor_ReturnNormally_Works()
        {
            // Arrange
            Mock<IInvocation> invocation = new Mock<IInvocation>();
            invocation.SetupGet(x => x.InvocationTarget).Returns(new Object());
            invocation.SetupGet(x => x.Method).Returns(new Mock<MethodInfo>().Object);

            // Act
            RecordingInterceptor interceptor = new RecordingInterceptor(Path.GetTempFileName(), 10, true);
            interceptor.Intercept(invocation.Object);
        }
    }
}

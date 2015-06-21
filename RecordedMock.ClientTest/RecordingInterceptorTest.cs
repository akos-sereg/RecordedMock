using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecordedMock.Client.Proxy;
using System.IO;
using Moq;
using Castle.DynamicProxy;
using System.Reflection;
using System.Threading;
using System.Collections.Generic;
using RecordedMock.Client.Model;
using Newtonsoft.Json;

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
            invocation.SetupGet(x => x.ReturnValue).Returns("Operation result");
            string tempFilename = Path.GetTempFileName();

            // Act
            RecordingInterceptor interceptor = new RecordingInterceptor(tempFilename, 10, true);
            interceptor.Intercept(invocation.Object);

            // Assert
            Thread.Sleep(500); // Interceptor is saving invocation data asynchronously, but we do not have an awaitable async task here
            StringAssert.Contains(File.ReadAllText(tempFilename), "Operation result");
        }

        [TestMethod]
        public void Interceptor_RecordingUniqueInvocationsInDeterministicMode_Works()
        {
            // Arrange
            Mock<IInvocation> invocation = new Mock<IInvocation>();
            invocation.SetupGet(x => x.Arguments).Returns(new [] { "Param #1", "Param #2" });
            invocation.SetupGet(x => x.InvocationTarget).Returns(new Object());
            invocation.SetupGet(x => x.Method).Returns(new Mock<MethodInfo>().Object);
            invocation.SetupGet(x => x.ReturnValue).Returns("Operation result");
            string tempFilename = Path.GetTempFileName();

            // Act
            RecordingInterceptor interceptor = new RecordingInterceptor(tempFilename, 10, true);
            interceptor.Intercept(invocation.Object);
            interceptor.Intercept(invocation.Object);

            // Assert
            Thread.Sleep(500); // Interceptor is saving invocation data asynchronously, but we do not have an awaitable async task here
            List<InvocationModel> invocations = JsonConvert.DeserializeObject<List<InvocationModel>>(string.Format("[ {0} ]", File.ReadAllText(tempFilename)));
            Assert.AreEqual(1, invocations.Count, "Second invocation should not be recorded, as RecordingInterceptor is configured for deterministic service");
        }
    }
}

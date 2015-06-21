using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecordedMock.Client.Proxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordedMock.ClientTest.Proxy
{
    [TestClass]
    public class RecordingMockTest
    {
        [TestMethod]
        public void Factory_RetainsBehaviour_Works() 
        {
            // Arrange
            ISampleService service = new SampleService();
            ISampleService recordingService = RecordingMock.Create<ISampleService>(service, Path.GetTempFileName(), 1, true);

            // Act & Assert
            Assert.AreEqual(service.DummyText(), recordingService.DummyText());
        }
    }

    public interface ISampleService
    {
        string DummyText();
    }

    public class SampleService : ISampleService 
    {
        public string DummyText()
        {
            return "Dummy Text";
        }
    }
}

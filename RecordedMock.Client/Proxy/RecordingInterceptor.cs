using Castle.DynamicProxy;
using Newtonsoft.Json;
using RecordedMock.Client.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordedMock.Client.Proxy
{
    public class RecordingInterceptor : IInterceptor
    {
        public string RecordingFilePath { get; set; }

        public RecordingInterceptor(string recordingFilePath)
        {
            this.RecordingFilePath = recordingFilePath;
        }

        public void Intercept(IInvocation invocation)
        {
            InvocationModel invocationModel = new InvocationModel();
            invocationModel.Arguments = invocation.Arguments;

            try
            {
                invocation.Proceed();
                invocationModel.ReturnValue = invocation.ReturnValue;
            }
            catch (Exception error)
            {
                invocationModel.Exception = error;
                throw error;
            }
            finally
            {
                try
                {
                    this.StoreInvocation(invocationModel);
                }
                catch { }
            }
        }

        private void StoreInvocation(InvocationModel invocationModel)
        {
            long length;

            try
            {
                length = new FileInfo(this.RecordingFilePath).Length;
            }
            catch (FileNotFoundException)
            {
                length = 0;
            }

            File.AppendAllText(
                this.RecordingFilePath,
                string.Format("{0}{1}",
                    length == 0 ? string.Empty : ", ",
                    JsonConvert.SerializeObject(invocationModel)));
        }
    }
}

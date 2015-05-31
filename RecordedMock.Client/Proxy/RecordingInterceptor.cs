using Castle.DynamicProxy;
using Newtonsoft.Json;
using RecordedMock.Client.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RecordedMock.Client.Proxy
{
    public class RecordingInterceptor : IInterceptor
    {
        public string RecordingFilePath { get; set; }

        public int MaxDumpSize { get; set; }

        private Object lockObject = new Object();

        public RecordingInterceptor(string recordingFilePath, int maxDumpSizeInMbs)
        {
            this.RecordingFilePath = recordingFilePath;
            this.MaxDumpSize = maxDumpSizeInMbs;
        }

        public void Intercept(IInvocation invocation)
        {
            InvocationModel invocationModel = new InvocationModel();
            invocationModel.Arguments = invocation.Arguments;
            invocationModel.Namespace = invocation.InvocationTarget.GetType().Namespace;
            invocationModel.Class = invocation.InvocationTarget.GetType().Name;
            invocationModel.MethodName = invocation.Method.Name;

            try
            {
                invocation.Proceed();
                invocationModel.ReturnValue = invocation.ReturnValue;
            }
            catch (System.Exception error)
            {
                invocationModel.Exception = error;
                throw error;
            }
            finally
            {
                Task.Run(() => { this.StoreInvocation(invocationModel); }).ConfigureAwait(false);   
            }
        }

        private void StoreInvocation(InvocationModel invocationModel)
        {
            long length;

            lock (this.lockObject)
            {
                try
                {
                    length = new FileInfo(this.RecordingFilePath).Length;
                }
                catch (FileNotFoundException)
                {
                    length = 0;
                }

                if (length > (this.MaxDumpSize * 1024 * 1024)) 
                {
                    return;
                }

                try
                {
                    File.AppendAllText(
                        this.RecordingFilePath,
                        string.Format("{0}{1}",
                            length == 0 ? string.Empty : ", ",
                            JsonConvert.SerializeObject(invocationModel)));
                }
                catch { }
            }
        }
    }
}

using Castle.DynamicProxy;
using Newtonsoft.Json;
using RecordedMock.Client.Exception;
using RecordedMock.Client.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordedMock.Client.Mock
{
    public class ReplayingInterceptor : IInterceptor
    {
        public string MockFilePath { get; set; }

        public List<InvocationModel> RecordedInvocations;

        public ReplayingInterceptor(string mockFilePath)
        {
            this.MockFilePath = mockFilePath;

            string serializedObjects = string.Format("[ {0} ]", File.ReadAllText(this.MockFilePath));
            this.RecordedInvocations = JsonConvert.DeserializeObject<List<InvocationModel>>(serializedObjects);
        }

        public void Intercept(IInvocation invocation)
        {
            string inputArguments = JsonConvert.SerializeObject(invocation.Arguments);
            InvocationModel similarInvocation = this.RecordedInvocations.FirstOrDefault(x => JsonConvert.SerializeObject(x.Arguments) == inputArguments);

            if (similarInvocation == null)
            {
                throw new RecordedMockException(string.Format("No return value candidate found for arguments: {0}", inputArguments));
            }

            Debug.WriteLine(string.Format("Return value found for input: {0}. Returning with object: {1}", inputArguments, JsonConvert.SerializeObject(similarInvocation.ReturnValue)));

            invocation.ReturnValue = similarInvocation.ReturnValue;
        }
    }
}

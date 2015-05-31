using Newtonsoft.Json;
using RecordedMock.Client.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace RecordedMock.Client.Filters
{
    public class RecordRequestAttribute : ActionFilterAttribute
    {
        public string DumpFilePath { get; set; }

        public int MaxDumpSize { get; set; }

        private Object lockObject = new Object();

        public RecordRequestAttribute(string dumpFilePath, int maxDumpSizeInMbs)
        {
            this.DumpFilePath = dumpFilePath;
            this.MaxDumpSize = maxDumpSizeInMbs;
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                long length;

                lock (this.lockObject)
                {
                    try
                    {
                        length = new FileInfo(this.DumpFilePath).Length;
                    }
                    catch (FileNotFoundException)
                    {
                        length = 0;
                    }

                    // Stop logging, if log size exceeded maximum size
                    if (length > (this.MaxDumpSize * 1024 * 1024))
                    {
                        return;
                    }

                    File.AppendAllText(
                        this.DumpFilePath,
                        string.Format("{0}{1}",
                            length == 0 ? string.Empty : ", ",
                            JsonConvert.SerializeObject(new HttpProcessingModel(actionExecutedContext.Request, actionExecutedContext.Response))));
                }
            }
            catch (System.Exception error)
            {
                Debug.WriteLine(error.Message);
            }    
        }
    }
}

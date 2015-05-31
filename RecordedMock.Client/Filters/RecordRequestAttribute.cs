using Newtonsoft.Json;
using RecordedMock.Client.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace RecordedMock.Client.Filters
{
    public class RecordRequestAttribute : ActionFilterAttribute
    {
        public const string RequestContentTypeKey = "RecordedMock.Client.Filters.Request.ContentType";

        public const string RequestContentKey = "RecordedMock.Client.Filters.Request.Content";

        public const string ResponseContentTypeKey = "RecordedMock.Client.Filters.Response.ContentType";

        public const string ResponseContentKey = "RecordedMock.Client.Filters.Response.Content";

        public string DumpFilePath { get; set; }

        public int MaxDumpSize { get; set; }

        private Object lockObject = new Object();

        public RecordRequestAttribute(string dumpFilePath, int maxDumpSizeInMbs)
        {
            this.DumpFilePath = dumpFilePath;
            this.MaxDumpSize = maxDumpSizeInMbs;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // Request.Content (StreamContent) should be read before processing request, and should be replaced with StringContent, as we are unable to 
            // rewind StreamContent in OnActionExecuted method, as Request.Content is already read at that time.
            var contentType = actionContext.Request.Content.Headers.ContentType;
            var contentInString = actionContext.Request.Content.ReadAsStringAsync().Result;
            actionContext.Request.Content = new StringContent(contentInString);
            actionContext.Request.Content.Headers.ContentType = contentType;

            actionContext.ActionArguments[RequestContentTypeKey] = contentType == null ? null : contentType.ToString();
            actionContext.ActionArguments[RequestContentKey] = contentInString;
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            // Response.Content should be read before returning. Store method is executed asynchronously, so most probably Response.Content is already disposed 
            // by the time we are reading it.
            var contentType = actionExecutedContext.Response.Content.Headers.ContentType;
            var contentInString = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;
            actionExecutedContext.Response.Content = new StringContent(contentInString);
            actionExecutedContext.Response.Content.Headers.ContentType = contentType;

            actionExecutedContext.ActionContext.ActionArguments[ResponseContentTypeKey] = contentType == null ? null : contentType.ToString();
            actionExecutedContext.ActionContext.ActionArguments[ResponseContentKey] = contentInString;

            // Fire and forget
            Task.Run(() => { this.StoreRequestProcessing(actionExecutedContext); }).ConfigureAwait(false);
        }

        private void StoreRequestProcessing(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                HttpProcessingModel processingModel = new HttpProcessingModel(actionExecutedContext);
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
                            JsonConvert.SerializeObject(processingModel)));
                }
            }
            catch (System.Exception error) 
            {
                Debug.WriteLine(error.Message);
            }
        }
    }
}

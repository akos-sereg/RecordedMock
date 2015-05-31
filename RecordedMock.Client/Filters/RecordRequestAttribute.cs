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

        public RecordRequestAttribute(string dumpFilePath)
        {
            this.DumpFilePath = dumpFilePath;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                long length;

                try
                {
                    length = new FileInfo(this.DumpFilePath).Length;
                }
                catch (FileNotFoundException)
                {
                    length = 0;
                }

                File.AppendAllText(
                    this.DumpFilePath, 
                    string.Format("{0}{1}", 
                        length == 0 ? string.Empty : ", ",
                        JsonConvert.SerializeObject(new HttpRequestModel(actionContext.Request))));
            }
            catch (System.Exception error)
            {
                Debug.WriteLine(error.Message);
            }
        }
    }
}

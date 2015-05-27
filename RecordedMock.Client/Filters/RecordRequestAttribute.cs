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
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                string file = @"c:\Users\Akos\dump.txt";
                long length;

                try
                {
                    length = new FileInfo(file).Length;
                }
                catch (FileNotFoundException)
                {
                    length = 0;
                }

                File.AppendAllText(
                    file, 
                    string.Format("{0}{1}", 
                        length == 0 ? string.Empty : ", ",
                        JsonConvert.SerializeObject(new HttpRequestModel(actionContext.Request))));
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
            }
        }
    }
}

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
                File.AppendAllText(@"c:\Users\Akos\dump.txt", JsonConvert.SerializeObject(new HttpRequestModel(actionContext.Request)));
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
            }
        }
    }
}

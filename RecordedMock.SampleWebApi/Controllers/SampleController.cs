using RecordedMock.Client.Filters;
using RecordedMock.Client.Mock;
using RecordedMock.Client.Proxy;
using RecordedMock.SampleWebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RecordedMock.SampleWebApi.Controllers
{
    [RecordRequest(@"C:\Users\Akos\dump.json", 10)]
    public class SampleController : ApiController
    {
        private IDataAccess dataAccess;

        public SampleController(IDataAccess dataAccess) 
        {
            this.dataAccess = dataAccess;
        }

        [HttpGet]
        public string DummyAction()
        {
            return this.dataAccess.GetObjectFromDatabase();
        }
    }
}
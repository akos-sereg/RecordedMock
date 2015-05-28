using RecordedMock.Client.Filters;
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
    public class SampleController : ApiController
    {
        private IDataAccess dataAccess;

        public SampleController() 
        {
            this.dataAccess = RecordedImplementation.Create<IDataAccess>(new DataAccess(), @"c:\Users\Akos\mock-DataAccess.json");
        }

        [HttpGet]
        [RecordRequest]
        public string DummyAction()
        {
            return this.dataAccess.GetObjectFromDatabase();
        }
    }
}
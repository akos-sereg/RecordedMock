using RecordedMock.Client.Filters;
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
        [HttpGet]
        [RecordRequest]
        public string DummyAction()
        {
            return "hello world";
        }
    }
}
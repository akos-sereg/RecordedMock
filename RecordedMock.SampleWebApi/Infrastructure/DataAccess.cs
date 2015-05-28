using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecordedMock.SampleWebApi.Infrastructure
{
    public class DataAccess : IDataAccess 
    {
        public string GetObjectFromDatabase()
        {
            return "hello world";
        }
    }
}
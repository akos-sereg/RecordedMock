using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecordedMock.SampleWebApi.Infrastructure
{
    public interface IDataAccess
    {
        string GetObjectFromDatabase();
    }
}
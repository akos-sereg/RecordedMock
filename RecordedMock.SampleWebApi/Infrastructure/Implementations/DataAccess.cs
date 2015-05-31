using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecordedMock.SampleWebApi.Infrastructure.Implementations
{
    public class DataAccess : IDataAccess 
    {
        public IDataAccessConfiguration ConfigurationHolder { get; set; }

        public DataAccess(IDataAccessConfiguration configuration)
        {
            this.ConfigurationHolder = configuration;
        }

        public string GetObjectFromDatabase()
        {
            return string.Format("Dummy Object, from db host {0}", this.ConfigurationHolder.Configuration["Host"]);
        }
    }
}
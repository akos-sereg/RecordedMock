using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecordedMock.SampleWebApi.Infrastructure.Implementations
{
    public class DataAccessConfiguration : IDataAccessConfiguration
    {
        public Dictionary<string, string> Configuration
        {
            get {
                return new Dictionary<string, string> {
                    { "Host",     "127.0.0.1" },
                    { "User",     "user" },
                    { "Password", "passwd" }
                };
            }
        }
    }
}
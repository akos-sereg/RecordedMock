using Ninject;
using Ninject.WebApi.DependencyResolver;
using RecordedMock.Client.Mock;
using RecordedMock.Client.Proxy;
using RecordedMock.SampleWebApi.Infrastructure;
using RecordedMock.SampleWebApi.Infrastructure.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RecordedMock.SampleWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            // Dependencies
            IKernel kernel = new StandardKernel();
            kernel.Bind<IDataAccessConfiguration>().To<DataAccessConfiguration>();

            IDataAccess recordingDataAccess = RecordingMock.Create<IDataAccess>(kernel.Get<DataAccess>(), @"C:\Users\Akos\mock-DataAccess.json");
            //IDataAccess replayingDataAccess = ReplayingMock.Create<IDataAccess>(@"C:\Users\Akos\mock-DataAccess.json");

            kernel.Bind<IDataAccess>().ToMethod(context => recordingDataAccess);

            //Register Resolver for Web Api
            var resolver = new NinjectDependencyResolver(kernel);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
}
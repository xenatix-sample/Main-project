using System.Web.Http;
using System.Web.Http.Batch;
using Autofac.Integration.WebApi;
using Axis.Helpers.Infrastructure;
using Axis.PresentationEngine.Helpers.Routes;

namespace Axis.PresentationEngine
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            HttpBatchHandler batchHandler = new DefaultHttpBatchHandler(GlobalConfiguration.DefaultServer)
            {
                ExecutionOrder = BatchExecutionOrder.NonSequential
            };

            var server = new HttpServer(config);

            config.Routes.MapHttpBatchRoute(
                routeName: "batch",
                routeTemplate: "data/batch",
                batchHandler: new DefaultHttpBatchHandler(server));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "data/{controller}/{action}"
                );

            var routePublisher = EngineContext.Current.Resolve<IHttpRoutePublisher>();
            routePublisher.RegisterHttpRoutes(config.Routes);
        }
    }
}

using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.DataProvider.ReportingServices;

namespace Axis.DataEngine.Plugins.ReportingServices
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<ReportingServicesDataProvider>().As<IReportingServicesDataProvider>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        public int Order
        {
            get { return 6; }
        }
    }
}

using Autofac;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.Plugins.ReportingServices.Respository;

namespace Axis.Plugins.ReportingServices.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<ReportingServicesRepository>().As<IReportingServicesRepository>();
        }

        public int Order
        {
            get { return 6; }
        }
    }
}

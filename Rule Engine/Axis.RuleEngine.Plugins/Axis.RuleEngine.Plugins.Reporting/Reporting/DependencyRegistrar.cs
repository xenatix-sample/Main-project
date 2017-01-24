using Autofac;
using Autofac.Integration.WebApi;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.RuleEngine.Reporting;
using Axis.Service.Reporting;
using System.Reflection;


namespace Axis.RuleEngine.Plugins.Reporting
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<ReportingRuleEngine>().As<IReportingRuleEngine>();
            builder.RegisterType<ReportingService>().As<IReportingService>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        public int Order
        {
            get { return 7; }
        }
    }
}

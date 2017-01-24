using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.RuleEngine.Scheduling;
using Axis.RuleEngine.Scheduling.GroupScheduling;
using Axis.RuleEngine.Scheduling.Resource;
using Axis.Service.Scheduling;
using Axis.Service.Scheduling.GroupScheduling;

namespace Axis.RuleEngine.Plugins.Scheduling
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<AppointmentRuleEngine>().As<IAppointmentRuleEngine>();
            builder.RegisterType<ResourceRuleEngine>().As<IResourceRuleEngine>();
            builder.RegisterType<GroupSchedulingRuleEngine>().As<IGroupSchedulingRuleEngine>();

            builder.RegisterType<AppointmentService>().As<IAppointmentService>();
            builder.RegisterType<ResourceService>().As<IResourceService>();
            builder.RegisterType<GroupSchedulingService>().As<IGroupSchedulingService>();

            builder.RegisterType<GroupSchedulingSearchRuleEngine>().As<IGroupSchedulingSearchRuleEngine>();
            builder.RegisterType<GroupSchedulingSearchService>().As<IGroupSchedulingSearchService>();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        public int Order
        {
            get { return 5; }
        }
    }
}

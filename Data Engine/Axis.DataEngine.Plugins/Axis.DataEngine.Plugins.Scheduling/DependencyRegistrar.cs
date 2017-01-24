using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Axis.DataProvider.Scheduling;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.DataEngine.Plugins.Scheduling.Resource;
using Axis.DataProvider.Scheduling.GroupScheduling;

namespace Axis.DataEngine.Plugins.Scheduling
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<AppointmentDataProvider>().As<IAppointmentDataProvider>();
            builder.RegisterType<RecurrenceDataProvider>().As<IRecurrenceDataProvider>();
            builder.RegisterType<ResourceDataProvider>().As<IResourceDataProvider>();
            builder.RegisterType<GroupSchedulingSearchDataProvider>().As<IGroupSchedulingSearchDataProvider>();
            builder.RegisterType<GroupSchedulingDataProvider>().As<IGroupSchedulingDataProvider>();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        public int Order
        {
            get { return 5; }
        }
    }
}

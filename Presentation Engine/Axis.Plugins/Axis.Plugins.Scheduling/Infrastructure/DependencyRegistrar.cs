using Autofac;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.Plugins.Scheduling.Repository.Appointment;
using Axis.Plugins.Scheduling.Repository.GroupScheduling;
using Axis.Plugins.Scheduling.Repository.Resource;

namespace Axis.Plugins.Scheduling.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<AppointmentRepository>().As<IAppointmentRepository>();
            builder.RegisterType<ResourceRepository>().As<IResourceRepository>();
            builder.RegisterType<GroupSchedulingRepository>().As<IGroupSchedulingRepository>();
            builder.RegisterType<GroupSchedulingSearchRepository>().As<IGroupSchedulingSearchRepository>();
        }

        public int Order
        {
            get { return 3; }
        }
    }
}
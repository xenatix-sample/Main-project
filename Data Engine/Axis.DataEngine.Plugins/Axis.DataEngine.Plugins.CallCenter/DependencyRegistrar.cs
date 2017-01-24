using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.DataProvider.CallCenter.CallCenterSummary;
using Axis.DataProvider.Common;
using Axis.DataProvider.CallCenter.CallerInformation;
using Axis.DataProvider.CallCenter;

namespace Axis.DataEngine.Plugins.CallCenter
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<CallCenterSummaryDataProvider>().As<ICallCenterSummaryDataProvider>();
            builder.RegisterType<CallerInformationDataProvider>().As<ICallerInformationDataProvider>();
            builder.RegisterType<ServiceRecordingDataProvider>().As<IServiceRecordingDataProvider>();
            builder.RegisterType<CallCenterProgressNoteDataProvider>().As<ICallCenterProgressNoteDataProvider>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        public int Order
        {
            get { return 5; }
        }
    }
}

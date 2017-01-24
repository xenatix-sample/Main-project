using Autofac;
using Autofac.Integration.WebApi;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.RuleEngine.CallCenter.CallCenterSummary;
using Axis.Service.CallCenter.CallCenterSummary;
using Axis.RuleEngine.CallCenter.CallerInformation;
using Axis.Service.CallCenter.CallerInformation;
using System.Reflection;
using Axis.Service.CallCenter;
using Axis.RuleEngine.CallCenter;

namespace Axis.RuleEngine.Plugins.CallCenter
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<CallCenterSummaryService>().As<ICallCenterSummaryService>();
            builder.RegisterType<CallCenterSummaryRuleEngine>().As<ICallCenterSummaryRuleEngine>();
            builder.RegisterType<CallerInformationService>().As<ICallerInformationService>();
            builder.RegisterType<CallerInformationRuleEngine>().As<ICallerInformationRuleEngine>();
            

            builder.RegisterType<CallCenterProgressNoteService>().As<ICallCenterProgressNoteService>();
            builder.RegisterType<CallCenterProgressNoteRuleEngine>().As<ICallCenterProgressNoteRuleEngine>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        public int Order
        {
            get { return 6; }
        }
    }
}

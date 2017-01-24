using Autofac;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.Plugins.CallCenter.Repository;
using Axis.Plugins.CallCenter.Repository.LawLiaisonSummary;
using Axis.Plugins.CallCenter.Repository.CrisisLineSummary;
using Axis.Plugins.CallCenter.Repository.CallerInformation;

namespace Axis.Plugins.CallCenter.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<LawLiaisonSummaryRepository>().As<ILawLiaisonSummaryRepository>();
            builder.RegisterType<CrisisLineSummaryRepository>().As<ICrisisLineSummaryRepository>();
            builder.RegisterType<CallerInformationRepository>().As<ICallerInformationRepository>();
            builder.RegisterType<CallCenterProgressNoteRepository>().As<ICallCenterProgressNoteRepository>();
        }

        public int Order
        {
            get { return 6; }
        }
    }
}

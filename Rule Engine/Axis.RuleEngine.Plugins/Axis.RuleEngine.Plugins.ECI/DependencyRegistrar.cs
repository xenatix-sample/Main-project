using Autofac;
using Autofac.Integration.WebApi;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.RuleEngine.ECI;
using System.Reflection;
using Axis.RuleEngine.ECI.Demographic;
using Axis.RuleEngine.ECI.EligibilityDetermination;
using Axis.Service.ECI;
using Axis.Service.ECI.Demographic;
using Axis.Service.ECI.EligibilityDetermination;


namespace Axis.RuleEngine.Plugins.ECI
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<ScreeningRuleEngine>().As<IScreeningRuleEngine>();
            builder.RegisterType<EligibilityDeterminationRuleEngine>().As<IEligibilityDeterminationRuleEngine>();
            builder.RegisterType<EligibilityCalculationRuleEngine>().As<IEligibilityCalculationRuleEngine>();
            builder.RegisterType<ScreeningService>().As<IScreeningService>();
            builder.RegisterType<EligibilityDeterminationService>().As<IEligibilityDeterminationService>();
            builder.RegisterType<IFSPService>().As<IIFSPService>();
            builder.RegisterType<IFSPRuleEngine>().As<IIFSPRuleEngine>();
            builder.RegisterType<EligibilityCalculationService>().As<IEligibilityCalculationService>();
            builder.RegisterType<ProgressNoteRuleEngine>().As<IProgressNoteRuleEngine>();
            builder.RegisterType<ProgressNoteService>().As<IProgressNoteService>();
            builder.RegisterType<ECIDemographicRuleEngine>().As<IECIDemographicRuleEngine>();
            builder.RegisterType<ECIDemographicService>().As<IECIDemographicService>();

            builder.RegisterType<ECIAdditionalDemographicService>().As<IECIAdditionalDemographicService>();
            builder.RegisterType<ECIAdditionalDemographicRuleEngine>().As<IECIAdditionalDemographicRuleEngine>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        public int Order
        {
            get { return 4; }
        }
    }
}

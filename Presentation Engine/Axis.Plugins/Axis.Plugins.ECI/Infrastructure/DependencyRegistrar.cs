using Autofac;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.Plugins.ECI.Repository.ECIDemographic;
using Axis.Plugins.ECI.Repository.EligibilityDetermination;
using Axis.Plugins.ECI.Repository.Registration;
using Axis.Plugins.Screening.Repository;

namespace Axis.Plugins.ECI.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<ScreeningRepository>().As<IScreeningRepository>();
            builder.RegisterType<EligibilityDeterminationRepository>().As<IEligibilityDeterminationRepository>();
            builder.RegisterType<EligibilityCalculationRepository>().As<IEligibilityCalculationRepository>();
            builder.RegisterType<IFSPRepository>().As<IIFSPRepository>();
            builder.RegisterType<ECIRegistrationRepository>().As<IECIRegistrationRepository>();
            builder.RegisterType<ECIDemographicRepository>().As<IECIDemographicRepository>();
            builder.RegisterType<ProgressNoteRepository>().As<IProgressNoteRepository>();
            builder.RegisterType<ECIAdditionalDemographicRepository>().As<IECIAdditionalDemographicRepository>();
        }

        public int Order
        {
            get { return 4; }
        }
    }
}

using Autofac;
using Autofac.Integration.WebApi;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using System.Reflection;
using Axis.DataProvider.ECI.EligibilityDetermination;
using Axis.DataProvider.ECI.IFSP;
using Axis.DataProvider.ECI.Screening;
using Axis.DataProvider.ECI;
using Axis.DataProvider.ECI.Demographic;


namespace Axis.DataEngine.Plugins.ECI
{
    /// <summary>
    /// To register dependencies for registration plugins
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<EligibilityDeterminationDataProvider>().As<IEligibilityDeterminationDataProvider>();
            builder.RegisterType<ScreeningDataProvider>().As<IScreeningDataProvider>();
            builder.RegisterType<IFSPDataProvider>().As<IIFSPDataProvider>();
            builder.RegisterType<EligibilityCalculationDataProvider>().As<IEligibilityCalculationDataProvider>();
            builder.RegisterType<ProgressNoteDataProvider>().As<IProgressNoteDataProvider>();
            builder.RegisterType<ECIDemographicDataProvider>().As<IECIDemographicDataProvider>();
         
            builder.RegisterType<NoteHeaderDataProvider>().As<INoteHeaderDataProvider>();
            builder.RegisterType<DischargeDataProvider>().As<IDischargeDataProvider>();
            builder.RegisterType<ProgressNoteAssessmentDataProvider>().As<IProgressNoteAssessmentDataProvider>();
            builder.RegisterType<ECIAdditionalDemographicDataProvider>().As<IECIAdditionalDemographicDataProvider>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        public int Order
        {
            get { return 4; }
        }
    }
}

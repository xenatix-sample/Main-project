using Autofac;
using Autofac.Integration.WebApi;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using System.Reflection;
using Axis.DataProvider.Clinical.Allergy;
using Axis.DataProvider.Clinical.Vital;
using Axis.DataProvider.Clinical;
using Axis.DataProvider.Clinical.ReviewOfSystems;
using Axis.DataProvider.Clinical.ChiefComplaint;
using Axis.DataProvider.Clinical.Assessment;
using Axis.DataProvider.Clinical.PresentIllness;
using Axis.DataProvider.Clinical.SocialRelationship;
using Axis.DataProvider.Clinical.MedicalHistory;

namespace Axis.DataEngine.Plugins.Clinical
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<VitalDataProvider>().As<IVitalDataProvider>();
            builder.RegisterType<AllergyDataProvider>().As<IAllergyDataProvider>();
            builder.RegisterType<NoteDataProvider>().As<INoteDataProvider>();
            builder.RegisterType<ReviewOfSystemsDataProvider>().As<IReviewOfSystemsDataProvider>();
            builder.RegisterType<ChiefComplaintDataProvider>().As<IChiefComplaintDataProvider>();
            builder.RegisterType<ClinicalAssessmentDataProvider>().As<IClinicalAssessmentDataProvider>();
            builder.RegisterType<MedicalHistoryDataProvider>().As<IMedicalHistoryDataProvider>();

            builder.RegisterType<PresentIllnessDataProvider>().As<IPresentIllnessDataProvider>();
            builder.RegisterType<SocialRelationshipHistoryDataProvider>().As<ISocialRelationshipHistoryDataProvider>();
            builder.RegisterType<SocialRelationshipDataProvider>().As<ISocialRelationshipDataProvider>();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        public int Order
        {
            get { return 5; }
        }
    }
}

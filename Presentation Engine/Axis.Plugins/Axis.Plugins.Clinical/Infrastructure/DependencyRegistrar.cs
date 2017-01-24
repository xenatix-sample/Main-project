using Autofac;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.Plugins.Clinical.Repository.Allergy;
using Axis.Plugins.Clinical.Repository.ReviewOfSystems;
using Axis.Plugins.Clinical.Repository.ChiefComplaint;
using Axis.Plugins.Clinical.Repository.Vital;
using Axis.Plugins.Clinical.Repository.Assessment;
using Axis.Plugins.Clinical.Repository.SocialRelationship;
using Axis.Plugins.Clinical.Repository.PresentIllness;
using Axis.Plugins.Clinical.Repository.MedicalHistory;

namespace Axis.Plugins.Clinical.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<VitalRepository>().As<IVitalRepository>();
            builder.RegisterType<AllergyRepository>().As<IAllergyRepository>();
            builder.RegisterType<NoteRepository>().As<INoteRepository>();
            builder.RegisterType<ReviewOfSystemsRepository>().As<IReviewOfSystemsRepository>();
            builder.RegisterType<ChiefComplaintRepository>().As<IChiefComplaintRepository>();
            builder.RegisterType<ClinicalAssessmentRepository>().As<IClinicalAssessmentRepository>();
            builder.RegisterType<SocialRelationshipHistoryRepository>().As<ISocialRelationshipHistoryRepository>();
            builder.RegisterType<SocialRelationshipRepository>().As<ISocialRelationshipRepository>();
            builder.RegisterType<PresentIllnessRepository>().As<IPresentIllnessRepository>();

            builder.RegisterType<MedicalHistoryRepository>().As<IMedicalHistoryRepository>();
        }

        public int Order
        {
            get { return 5; }
        }
    }
}

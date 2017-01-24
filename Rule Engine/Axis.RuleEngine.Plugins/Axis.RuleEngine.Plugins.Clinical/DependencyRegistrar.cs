using Autofac;
using Autofac.Integration.WebApi;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.RuleEngine.Clinical.Vital;
using Axis.Service.Clinical.Vital;
using System.Reflection;
using Axis.RuleEngine.Clinical.Allergy;
using Axis.Service.Clinical.Allergy;
using Axis.Service.Clinical;
using Axis.RuleEngine.Clinical;
using Axis.Service.Clinical.ReviewOfSystems;
using Axis.RuleEngine.Clinical.ReviewOfSystems;
using Axis.RuleEngine.Clinical.ChiefComplaint;
using Axis.Service.Clinical.ChiefComplaint;
using Axis.RuleEngine.Clinical.Assessment;
using Axis.RuleEngine.Clinical.MedicalHistory;
using Axis.Service.Clinical.Assessment;
using Axis.RuleEngine.Clinical.PresentIllness;
using Axis.Service.Clinical.PresentIllness;
using Axis.Service.Clinical.SocialRelationship;
using Axis.RuleEngine.Clinical.SocialRelationship;
using Axis.Service.Clinical.MedicalHistory;

namespace Axis.RuleEngine.Plugins.Clinical
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<VitalRuleEngine>().As<IVitalRuleEngine>();
            builder.RegisterType<VitalService>().As<IVitalService>();
            builder.RegisterType<AllergyRuleEngine>().As<IAllergyRuleEngine>();
            builder.RegisterType<AllergyService>().As<IAllergyService>();
            builder.RegisterType<NoteRuleEngine>().As<INoteRuleEngine>();
            builder.RegisterType<NoteService>().As<INoteService>();

            builder.RegisterType<ReviewOfSystemsRuleEngine>().As<IReviewOfSystemsRuleEngine>();
            builder.RegisterType<ReviewOfSystemsService>().As<IReviewOfSystemsService>();
            builder.RegisterType<ChiefComplaintRuleEngine>().As<IChiefComplaintRuleEngine>();
            builder.RegisterType<ChiefComplaintService>().As<IChiefComplaintService>();
            builder.RegisterType<SocialRelationshipHistoryRuleEngine>().As<ISocialRelationshipHistoryRuleEngine>();
            builder.RegisterType<SocialRelationshipHistoryService>().As<ISocialRelationshipHistoryService>();


            builder.RegisterType<SocialRelationshipRuleEngine>().As<ISocialRelationshipRuleEngine>();
            builder.RegisterType<SocialRelationshipService>().As<ISocialRelationshipService>();

            builder.RegisterType<AssessmentRuleEngine>().As<IAssessmentRuleEngine>();
            builder.RegisterType<AssessmentService>().As<IAssessmentService>();

            builder.RegisterType<PresentIllnessRuleEngine>().As<IPresentIllnessRuleEngine>();
            builder.RegisterType<PresentIllnessService>().As<IPresentIllnessService>();

            builder.RegisterType<MedicalHistoryRuleEngine>().As<IMedicalHistoryRuleEngine>();
            builder.RegisterType<MedicalHistoryService>().As<IMedicalHistoryService>();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        public int Order
        {
            get { return 5; }
        }
    }
}

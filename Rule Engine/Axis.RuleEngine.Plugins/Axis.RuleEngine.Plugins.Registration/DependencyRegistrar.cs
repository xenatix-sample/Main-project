using Autofac;
using Autofac.Integration.WebApi;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.RuleEngine.Registration;
using Axis.Service.Registration;
using System.Reflection;
using Axis.RuleEngine.Registration.Consent;
using Axis.Service.Registration.Consent;
using Axis.RuleEngine.Registration.Common;
using Axis.Service.Registration.Common;
using Axis.RuleEngine.Registration.Referrals.Followup;
using Axis.RuleEngine.Registration.Referrals.Disposition;
using Axis.RuleEngine.Registration.Referrals;
using Axis.Service.Registration.Referrals;
using Axis.Service.Registration.Referrals.Followup;
using Axis.Service.Registration.Referrals.Information;
using Axis.Service.Registration.Referrals.Disposition;
using Axis.RuleEngine.Registration.Referrals.Forwarded;
using Axis.Service.Registration.Referrals.Forwarded;
using Axis.Service.Registration.Referrals.ClientInformation;
using Axis.RuleEngine.Registration.Referrals.Common;
using Axis.RuleEngine.Registration.Referrals.Requestor;
using Axis.Service.Registration.Referrals.Common;
using Axis.Service.Registration.Referrals.Requestor;
using Axis.RuleEngine.Registration.Referrals.Information;
using Axis.RuleEngine.Registration.Referrals.ClientInformation;
using Axis.RuleEngine.Registration.Referral;
using Axis.Service.Registration.Referral;
using Axis.RuleEngine.Registration.Admission;
using Axis.Service.Registration.Admission;

namespace Axis.RuleEngine.Plugins.Registration
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<RegistrationRuleEngine>().As<IRegistrationRuleEngine>();
            builder.RegisterType<RegistrationService>().As<IRegistrationService>();
            builder.RegisterType<QuickRegistrationRuleEngine>().As<IQuickRegistrationRuleEngine>();
            builder.RegisterType<QuickRegistrationService>().As<IQuickRegistrationService>();

            builder.RegisterType<AdditionalDemographicRuleEngine>().As<IAdditionalDemographicRuleEngine>();
            builder.RegisterType<AdditionalDemographicService>().As<IAdditionalDemographicService>();
            builder.RegisterType<FinancialAssessmentRuleEngine>().As<IFinancialAssessmentRuleEngine>();
            builder.RegisterType<FinancialAssessmentService>().As<IFinancialAssessmentService>();

            builder.RegisterType<FinancialSummaryRuleEngine>().As<IFinancialSummaryRuleEngine>();
            builder.RegisterType<FinancialSummaryService>().As<IFinancialSummaryService>();

            builder.RegisterType<ConsentRuleEngine>().As<IConsentRuleEngine>();
            builder.RegisterType<ConsentService>().As<IConsentService>();
            builder.RegisterType<EmergencyContactRuleEngine>().As<IEmergencyContactRuleEngine>();
            builder.RegisterType<EmergencyContactService>().As<IEmergencyContactService>();
            builder.RegisterType<ContactBenefitRuleEngine>().As<IContactBenefitRuleEngine>();
            builder.RegisterType<ContactBenefitService>().As<IContactBenefitService>();
            builder.RegisterType<CollateralRuleEngine>().As<ICollateralRuleEngine>();
            builder.RegisterType<CollateralService>().As<ICollateralService>();
            builder.RegisterType<ContactPhonesRuleEngine>().As<IContactPhonesRuleEngine>();
            builder.RegisterType<ContactPhonesService>().As<IContactPhonesService>();
            builder.RegisterType<ReferralRuleEngine>().As<IReferralRuleEngine>();
            builder.RegisterType<ReferralService>().As<IReferralService>();


            builder.RegisterType<ContactEmailRuleEngine>().As<IContactEmailRuleEngine>();
            builder.RegisterType<ContactEmailService>().As<IContactEmailService>();

            builder.RegisterType<ContactAddressRuleEngine>().As<IContactAddressRuleEngine>();
            builder.RegisterType<ContactAddressService>().As<IContactAddressService>();

            builder.RegisterType<ContactAddressRuleEngine>().As<IContactAddressRuleEngine>();
            builder.RegisterType<ContactAddressService>().As<IContactAddressService>();

            builder.RegisterType<PatientProfileRuleEngine>().As<IPatientProfileRuleEngine>();
            builder.RegisterType<PatientProfileService>().As<IPatientProfileService>();

            builder.RegisterType<ReferralDispositionRuleEngine>().As<IReferralDispositionRuleEngine>();
            builder.RegisterType<ReferralDispositionService>().As<IReferralDispositionService>();

            builder.RegisterType<ReferralSearchRuleEngine>().As<IReferralSearchRuleEngine>();
            builder.RegisterType<ReferralSearchService>().As<IReferralSearchService>();

            builder.RegisterType<ReferralFollowupRuleEngine>().As<IReferralFollowupRuleEngine>();
            builder.RegisterType<ReferralFollowupService>().As<IReferralFollowupService>();

            builder.RegisterType<ReferralForwardedRuleEngine>().As<IReferralForwardedRuleEngine>();
            builder.RegisterType<ReferralForwardedService>().As<IReferralForwardedService>();

            builder.RegisterType<ReferralReferredInformationRuleEngine>().As<IReferralReferredInformationRuleEngine>();
            builder.RegisterType<ReferralReferredInformationService>().As<IReferralReferredInformationService>();

            builder.RegisterType<ReferralClientInformationService>().As<IReferralClientInformationService>();
            builder.RegisterType<ReferralClientInformationRuleEngine>().As<IReferralClientInformationRuleEngine>();

            builder.RegisterType<ReferralAddressRuleEngine>().As<IReferralAddressRuleEngine>();
            builder.RegisterType<ReferralEmailRuleEngine>().As<IReferralEmailRuleEngine>();
            builder.RegisterType<ReferralPhoneRuleEngine>().As<IReferralPhoneRuleEngine>();
            builder.RegisterType<ReferralDemographicsRuleEngine>().As<IReferralDemographicsRuleEngine>();
            builder.RegisterType<ReferralHeaderRuleEngine>().As<IReferralHeaderRuleEngine>();

            builder.RegisterType<ReferralAddressService>().As<IReferralAddressService>();
            builder.RegisterType<ReferralEmailService>().As<IReferralEmailService>();
            builder.RegisterType<ReferralPhoneService>().As<IReferralPhoneService>();
            builder.RegisterType<ReferralDemographicsService>().As<IReferralDemographicsService>();
            builder.RegisterType<ReferralHeaderService>().As<IReferralHeaderService>();

            builder.RegisterType<ContactPhotoRuleEngine>().As<IContactPhotoRuleEngine>();
            builder.RegisterType<ContactPhotoService>().As<IContactPhotoService>();

            builder.RegisterType<ReferralConcernDetailRuleEngine>().As<IReferralConcernDetailRuleEngine>();
            builder.RegisterType<ReferralConcernDetailService>().As<IReferralConcernDetailService>();
            
            builder.RegisterType<ReferralAdditionalDetailRuleEngine>().As<IReferralAdditionalDetailRuleEngine>();
            builder.RegisterType<ReferralAdditionalDetailService>().As<IReferralAdditionalDetailService>();

            builder.RegisterType<ContactAliasRuleEngine>().As<IContactAliasRuleEngine>();
            builder.RegisterType<ContactAliasService>().As<IContactAliasService>();

            builder.RegisterType<ContactRaceRuleEngine>().As<IContactRaceRuleEngine>();
            builder.RegisterType<ContactRaceService>().As<IContactRaceService>();

            builder.RegisterType<AdmissionRuleEngine>().As<IAdmissionRuleEngine>();
            builder.RegisterType<AdmissionService>().As<IAdmissionService>();


            builder.RegisterType<ContactDischargeNoteRuleEngine>().As<IContactDischargeNoteRuleEngine>();
            builder.RegisterType<ContactDischargeNoteService>().As<IContactDischargeNoteService>();
            builder.RegisterType<SelfPayRuleEngine>().As<ISelfPayRuleEngine>();
            builder.RegisterType<BenefitsAssistanceRuleEngine>().As<IBenefitsAssistanceRuleEngine>();
            builder.RegisterType<SelfPayService>().As<ISelfPayService>();
            builder.RegisterType<BenefitsAssistanceService>().As<IBenefitsAssistanceService>();
            builder.RegisterType<ContactRelationshipRuleEngine>().As<IContactRelationshipRuleEngine>();
            builder.RegisterType<ContactRelationshipService>().As<IContactRelationshipService>();
            builder.RegisterType<LettersService>().As<ILettersService>();
            builder.RegisterType<LettersRuleEngine>().As<ILettersRuleEngine>();
            builder.RegisterType<IntakeFormsService>().As<IIntakeFormsService>();
            builder.RegisterType<IntakeFormsRuleEngine>().As<IIntakeFormsRuleEngine>();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        public int Order
        {
            get { return 2; }
        }
    }
}

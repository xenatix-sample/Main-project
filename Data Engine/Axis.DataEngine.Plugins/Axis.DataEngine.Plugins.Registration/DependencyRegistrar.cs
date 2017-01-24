using Autofac;
using Autofac.Integration.WebApi;
using Axis.DataProvider.Common;
using Axis.DataProvider.Registration;
using Axis.DataProvider.Registration.Common;
using Axis.DataProvider.Registration.Consent;
using Axis.DataProvider.Registration.Referrals.Disposition;
using Axis.DataProvider.Registration.Referrals.Followup;
using Axis.DataProvider.Registration.Referrals;
using Axis.DataProvider.Registration.Referrals.Forwarded;
using Axis.DataProvider.Registration.Referrals.Information;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using System.Reflection;
using Axis.DataProvider.Registration.Referrals.Requestor;
using Axis.DataProvider.Registration.Referrals.Common;
using Axis.DataProvider.Registration.Referrals.ClientInformation;
using Axis.DataProvider.Registration.Referral;

namespace Axis.DataEngine.Plugins.Registration
{
    /// <summary>
    /// To register dependencies for registration plugins
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<RegistrationDataProvider>().As<IRegistrationDataProvider>();
            builder.RegisterType<AdditionalDemographicDataProvider>().As<IAdditionalDemographicDataProvider>();
            builder.RegisterType<FinancialAssessmentDataProvider>().As<IFinancialAssessmentDataProvider>();
            builder.RegisterType<QuickRegistrationDataProvider>().As<IQuickRegistrationDataProvider>();
            builder.RegisterType<ContactAddressDataProvider>().As<IContactAddressDataProvider>();
            builder.RegisterType<ContactPhoneDataProvider>().As<IContactPhoneDataProvider>();
            builder.RegisterType<ContactEmailDataProvider>().As<IContactEmailDataProvider>();
            builder.RegisterType<ConsentDataProvider>().As<IConsentDataProvider>();
            builder.RegisterType<EmergencyContactDataProvider>().As<IEmergencyContactDataProvider>();
            builder.RegisterType<ContactBenefitDataProvider>().As<IContactBenefitDataProvider>();
            builder.RegisterType<CollateralDataProvider>().As<ICollateralDataProvider>();
            builder.RegisterType<ReferralDataProvider>().As<IReferralDataProvider>();
            builder.RegisterType<PatientProfileDataProvider>().As<IPatientProfileDataProvider>();
            builder.RegisterType<ReferralFollowupDataProvider>().As<IReferralFollowupDataProvider>();
            builder.RegisterType<ReferralSearchDataProvider>().As<IReferralSearchDataProvider>();
            builder.RegisterType<ReferralForwardedDataProvider>().As<IReferralForwardedDataProvider>();
            builder.RegisterType<ReferralReferredInformationDataProvider>().As<IReferralReferredInformationDataProvider>();
            builder.RegisterType<ReferralDispositionDataProvider>().As<IReferralDispositionDataProvider>();
            builder.RegisterType<ReferralClientInformationDataProvider>().As<IReferralClientInformationDataProvider>();


            builder.RegisterType<ReferralHeaderDataProvider>().As<IReferralHeaderDataProvider>();
            builder.RegisterType<ReferralDemographicsDataProvider>().As<IReferralDemographicsDataProvider>();
            builder.RegisterType<ReferralAddressDataProvider>().As<IReferralAddressDataProvider>();
            builder.RegisterType<ReferralPhoneDataProvider>().As<IReferralPhoneDataProvider>();
            builder.RegisterType<ReferralEmailDataProvider>().As<IReferralEmailDataProvider>();

            builder.RegisterType<ReferralClientAdditionalDetailsDataProvider>().As<IReferralClientAdditionalDetailsDataProvider>();
            builder.RegisterType<ReferralClientConcernDataProvider>().As<IReferralClientConcernDataProvider>();
            builder.RegisterType<ReferralClientDemographicsDataProvider>().As<IReferralClientDemographicsDataProvider>();

            builder.RegisterType<ReferralOriginDataProvider>().As<IReferralOriginDataProvider>();
            builder.RegisterType<ContactPhotoDataProvider>().As<IContactPhotoDataProvider>();

            builder.RegisterType<ContactClientIdentifierDataProvider>().As<IContactClientIdentifierDataProvider>();

            builder.RegisterType<ReferralAdditionalDetailDataProvider>().As<IReferralAdditionalDetailDataProvider>();
            builder.RegisterType<ReferralConcernDetailDataProvider>().As<IReferralConcernDetailDataProvider>();
            builder.RegisterType<ContactAliasDataProvider>().As<IContactAliasDataProvider>();
            builder.RegisterType<ContactRaceDataProvider>().As<IContactRaceDataProvider>();
            builder.RegisterType<AdmissionDataProvider>().As<IAdmissionDataProvider>();
            builder.RegisterType<ContactPresentingProblemProvider>().As<IContactPresentingProblemProvider>();
            builder.RegisterType<FinancialSummaryDataProvider>().As<IFinancialSummaryDataProvider>();
            
            builder.RegisterType<ContactDischargeNoteDataProvider>().As<IContactDischargeNoteDataProvider>();
            builder.RegisterType<SelfPayDataProvider>().As<ISelfPayDataProvider>();
            builder.RegisterType<BenefitsAssistanceDataProvider>().As<IBenefitsAssistanceDataProvider>();
            builder.RegisterType<ContactRelationshipDataProvider>().As<IContactRelationshipDataProvider>();
            builder.RegisterType<LettersDataProvider>().As<ILettersDataProvider>();
            builder.RegisterType<IntakeFormsDataProvider>().As<IIntakeFormsDataProvider>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        public int Order
        {
            get { return 2; }
        }
    }
}
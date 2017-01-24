using Autofac;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.Plugins.Registration.Repository;
using Axis.Plugins.Registration.Repository.Common;
using Axis.Plugins.Registration.Repository.FinancialAssessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Plugins.Registration.Repository.PatientProfile;
using Axis.Plugins.Registration.Repository.Referrals.Followup;
using Axis.Plugins.Registration.Repository.Referrals.Disposition;
using Axis.Plugins.Registration.Repository.Referrals;
using Axis.Plugins.Registration.Repository.Referrals.Information;
using Axis.Plugins.Registration.Repository.Referrals.ClientInformation;
using Axis.Plugins.Registration.Repository.Referrals.Common;
using Axis.Plugins.Registration.Repository.Referrals.Forwarded;
using Axis.Plugins.Registration.Repository.Referrals.Requestor;
using Axis.Plugins.Registration.Repository.Referral;
using Axis.Plugins.Registration.Repository.Admission;

namespace Axis.Plugins.Registration.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<RegistrationRepository>().As<IRegistrationRepository>();
            builder.RegisterType<AdditionalDemographicRepository>().As<IAdditionalDemographicRepository>();
            builder.RegisterType<ClientSearchRepository>().As<IClientSearchRepository>();
            builder.RegisterType<FinancialAssessmentRepository>().As<IFinancialAssessmentRepository>();
            builder.RegisterType<ContactBenefitRepository>().As<IContactBenefitRepository>();
            builder.RegisterType<ConsentRepository>().As<IConsentRepository>();
            builder.RegisterType<EmergencyContactRepository>().As<IEmergencyContactRepository>();
            builder.RegisterType<CollateralRepository>().As<ICollateralRepository>();
            builder.RegisterType<ContactEmailRepository>().As<IContactEmailRepository>();
            builder.RegisterType<ContactPhoneRepository>().As<IContactPhoneRepository>();
            builder.RegisterType<ContactPhotoRepository>().As<IContactPhotoRepository>();
            
            builder.RegisterType<PatientProfileRepository>().As<IPatientProfileRepository>();
            builder.RegisterType<ContactAddressRepository>().As<IContactAddressRepository>();
            builder.RegisterType<ReferralRepository>().As<IReferralRepository>();
            builder.RegisterType<ReferralFollowupRepository>().As<IReferralFollowupRepository>();            
            builder.RegisterType<ReferralReferredInformationRepository>().As<IReferralReferredInformationRepository>();
            builder.RegisterType<ReferralSearchRepository>().As<IReferralSearchRepository>();
            
            builder.RegisterType<ReferralDispositionRepository>().As<IReferralDispositionRepository>();
            builder.RegisterType<ReferralClientInformationRepository>().As<IReferralClientInformationRepository>();

            builder.RegisterType<ReferralAddressRepository>().As<IReferralAddressRepository>();
            builder.RegisterType<ReferralEmailRepository>().As<IReferralEmailRepository>();
            builder.RegisterType<ReferralPhoneRepository>().As<IReferralPhoneRepository>();
            builder.RegisterType<ReferralDemographicsRepository>().As<IReferralDemographicsRepository>();
            builder.RegisterType<ReferralHeaderRepository>().As<IReferralHeaderRepository>();
            builder.RegisterType<ReferralForwardedRepository>().As<IReferralForwardedRepository>();
            builder.RegisterType<ReferralAdditionalDetailRepository>().As<IReferralAdditionalDetailRepository>();
            builder.RegisterType<ReferralConcernDetailRepository>().As<IReferralConcernDetailRepository>();
            builder.RegisterType<ContactAliasRepository>().As<IContactAliasRepository>();
            builder.RegisterType<ContactRaceRepository>().As<IContactRaceRepository>();

            builder.RegisterType<ContactDischargeNoteRepository>().As<IContactDischargeNoteRepository>();
            builder.RegisterType<AdmissionRepository>().As<IAdmissionRepository>();
            builder.RegisterType<FinancialSummaryRepository>().As<IFinancialSummaryRepository>();
            builder.RegisterType<SelfPayRepository>().As<ISelfPayRepository>();
            builder.RegisterType<BenefitsAssistanceRepository>().As<IBenefitsAssistanceRepository>();
            builder.RegisterType<LettersRepository>().As<ILettersRepository>();
            builder.RegisterType<IntakeFormsRepository>().As<IIntakeFormsRepository>();

            builder.RegisterType<ContactRelationshipRepository>().As<IContactRelationshipRepository>();            
        }

        public int Order
        {
            get { return 2; }
        }
    }
}

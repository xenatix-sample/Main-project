using Autofac;
using Autofac.Integration.WebApi;
using Axis.Configuration;
using Axis.Constant;
using Axis.Data.Repository;
using Axis.Data.Repository.Context;
using Axis.DataProvider.Account;
using Axis.DataProvider.Account.UserProfile;
using Axis.DataProvider.Account.UserSecurity;
using Axis.DataProvider.Admin;
using Axis.DataProvider.Admin.DivisionProgram;
using Axis.DataProvider.Admin.UserDirectReports;
using Axis.DataProvider.Admin.UserPhoto;
using Axis.DataProvider.Assessment;
using Axis.DataProvider.BusinessAdmin.ClientMerge;
using Axis.DataProvider.BusinessAdmin.Company;
using Axis.DataProvider.BusinessAdmin.Division;
using Axis.DataProvider.BusinessAdmin.OrganizationStructure;
using Axis.DataProvider.BusinessAdmin.Payors;
using Axis.DataProvider.BusinessAdmin.PlanAddresses;
using Axis.DataProvider.BusinessAdmin.Program;
using Axis.DataProvider.BusinessAdmin.ProgramUnit;
using Axis.DataProvider.BusinessAdmin.ServiceDefinition;
using Axis.DataProvider.BusinessAdmin.ServiceDetails;
using Axis.DataProvider.Common;
using Axis.DataProvider.Common.ClientAudit;
using Axis.DataProvider.Common.WorkFlowHeader;
using Axis.DataProvider.Common.Lookups.FamilyRelationship;
using Axis.DataProvider.Common.Lookups.GroupService;
using Axis.DataProvider.Common.Lookups.GroupType;
using Axis.DataProvider.Common.Lookups.Services;
using Axis.DataProvider.Common.Photo;
using Axis.DataProvider.Common.SecurityQuestion;
using Axis.DataProvider.Consents;
using Axis.DataProvider.Logging;
using Axis.DataProvider.Manifest;
using Axis.DataProvider.MessageTemplate;
using Axis.DataProvider.RecordedServices.VoidServices;
using Axis.DataProvider.Security;
using Axis.DataProvider.Settings;
using Axis.DataProvider.SynchronizationService;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.Logging;
using Axis.NotificationService.Email;
using System.Data.Entity;
using System.Reflection;

//using Axis.DataProvider.RecordedServices.VoidServices;

namespace Axis.DataEngine.Service.DependencyInjection
{
    /// <summary>
    /// To register dependencies
    /// </summary>
    public class DependencyRegistration : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            // Added logger's constructor parameter
            builder.RegisterType<Logger>().As<ILogger>().WithParameter("enableLogging", (ApplicationSettings.EnableLogging && ApplicationSettings.LoggingMode == (int)LoggingMode.File)).InstancePerLifetimeScope();

            //services
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<XenatixContext>().As<DbContext>();
            builder.RegisterType<AccountDataProvider>().As<IAccountDataProvider>();
            builder.RegisterType<ForgotPasswordDataProvider>().As<IForgotPasswordDataProvider>();
            builder.RegisterType<EmailService>().As<IEmailService>();
            builder.RegisterType<MessageTemplateDataProvider>().As<IMessageTemplateDataProvider>();
            builder.RegisterType<PhotoDataProvider>().As<IPhotoDataProvider>();
            builder.RegisterType<ClientAuditDataProvider>().As<IClientAuditDataProvider>();
            builder.RegisterType<WorkFlowHeaderDataProvider>().As<IWorkFlowHeaderDataProvider>();
            builder.RegisterType<LoggingDataProvider>().As<ILoggingDataProvider>().SingleInstance();
            builder.RegisterType<SecurityDataProvider>().As<ISecurityDataProvider>().SingleInstance();
            builder.RegisterType<ManifestDataProvider>().As<IManifestDataProvider>();
            builder.RegisterType<AdminDataProvider>().As<IAdminDataProvider>();
            builder.RegisterType<AddressDataProvider>().As<IAddressDataProvider>();
            builder.RegisterType<EmailDataProvider>().As<IEmailDataProvider>();
            builder.RegisterType<SettingsDataProvider>().As<ISettingsDataProvider>();
            builder.RegisterType<UserProfileDataProvider>().As<IUserProfileDataProvider>();
            builder.RegisterType<UserSecurityQuestionDataProvider>().As<IUserSecurityQuestionDataProvider>();
            builder.RegisterType<LookupDataProvider>().As<ILookupDataProvider>();

            //Register all lookup data provider
            builder.RegisterType<AddressTypeDataProvider>().As<IAddressTypeDataProvider>();
            builder.RegisterType<CitizenshipDataProvider>().As<ICitizenshipDataProvider>();
            builder.RegisterType<ClientTypeDataProvider>().As<IClientTypeDataProvider>();
            builder.RegisterType<ContactMethodDataProvider>().As<IContactMethodDataProvider>();
            builder.RegisterType<ContactTypeDataProvider>().As<IContactTypeDataProvider>();
            builder.RegisterType<ClientIdentifierTypeDataProvider>().As<IClientIdentifierTypeDataProvider>();
            builder.RegisterType<DOBStatusDataProvider>().As<IDOBStatusDataProvider>();
            builder.RegisterType<EducationStatusDataProvider>().As<IEducationStatusDataProvider>();
            builder.RegisterType<EmploymentStatusDataProvider>().As<IEmploymentStatusDataProvider>();
            builder.RegisterType<EthnicityDataProvider>().As<IEthnicityDataProvider>();
            builder.RegisterType<GenderDataProvider>().As<IGenderDataProvider>();
            builder.RegisterType<PayorTypeDataProvider>().As<IPayorTypeDataProvider>();
            builder.RegisterType<LegalStatusDataProvider>().As<ILegalStatusDataProvider>();
            builder.RegisterType<LetterOutcomeDataProvider>().As<ILetterOutcomeDataProvider>();
            builder.RegisterType<LivingArrangementDataProvider>().As<ILivingArrangementDataProvider>();
            builder.RegisterType<MaritalStatusDataProvider>().As<IMaritalStatusDataProvider>();
            builder.RegisterType<PhonePermissionDataProvider>().As<IPhonePermissionDataProvider>();
            builder.RegisterType<RaceDataProvider>().As<IRaceDataProvider>();
            builder.RegisterType<ReferralSourceDataProvider>().As<IReferralSourceDataProvider>();
            builder.RegisterType<ReligionDataProvider>().As<IReligionDataProvider>();
            builder.RegisterType<SmokingStatusDataProvider>().As<ISmokingStatusDataProvider>();
            builder.RegisterType<SSNStatusDataProvider>().As<ISSNStatusDataProvider>();
            builder.RegisterType<SuffixDataProvider>().As<ISuffixDataProvider>();
            builder.RegisterType<LanguageDataProvider>().As<ILanguageDataProvider>();
            builder.RegisterType<VeteranStatusDataProvider>().As<IVeteranStatusDataProvider>();
            builder.RegisterType<PresentingProblemTypeDataProvider>().As<IPresentingProblemTypeDataProvider>();
            builder.RegisterType<PrefixTypeDataProvider>().As<IPrefixTypeDataProvider>();
            builder.RegisterType<EligibilityTypeDataProvider>().As<IEligibilityTypeDataProvider>();
            builder.RegisterType<DurationDataProvider>().As<IDurationDataProvider>();
            builder.RegisterType<EligibilityCategoryDataProvider>().As<IEligibilityCategoryDataProvider>();
            builder.RegisterType<SignatureStatusDataProvider>().As<ISignatureStatusDataProvider>();

            builder.RegisterType<RegistrationTypeDataProvider>().As<IRegistrationTypeDataProvider>();
            builder.RegisterType<CountyDataProvider>().As<ICountyDataProvider>();
            builder.RegisterType<StateProvinceDataProvider>().As<IStateProvinceDataProvider>();
            builder.RegisterType<PhoneDataProvider>().As<IPhoneDataProvider>();
            builder.RegisterType<MailPermissionTypeDataProvider>().As<IMailPermissionTypeDataProvider>();
            builder.RegisterType<SchoolDistrictDataProvider>().As<ISchoolDistrictDataProvider>();
            builder.RegisterType<EmailPermissionDataProvider>().As<IEmailPermissionDataProvider>();
            builder.RegisterType<PhoneTypeDataProvider>().As<IPhoneTypeDataProvider>();

            builder.RegisterType<CategoryDataProvider>().As<ICategoryDataProvider>();
            builder.RegisterType<FinanceFrequencyDataProvider>().As<IFinanceFrequencyDataProvider>();
            builder.RegisterType<ExpirationReasonDataProvider>().As<IExpirationReasonDataProvider>();
            builder.RegisterType<PayorDataProvider>().As<IPayorDataProvider>();
            builder.RegisterType<PolicyHolderDataProvider>().As<IPolicyHolderDataProvider>();
            builder.RegisterType<SecurityQuestionDataProvider>().As<ISecurityQuestionDataProvider>();
            builder.RegisterType<RelationshipTypeDataProvider>().As<IRelationshipTypeDataProvider>();
            builder.RegisterType<CollateralTypeDataProvider>().As<ICollateralTypeDataProvider>();
            builder.RegisterType<LivingStatusDataProvider>().As<ILivingStatusDataProvider>();
            builder.RegisterType<CredentialDataProvider>().As<ICredentialDataProvider>();
            builder.RegisterType<AssessmentDataProvider>().As<IAssessmentDataProvider>();

            builder.RegisterType<ScreeningNameDataProvider>().As<IScreeningNameDataProvider>();
            builder.RegisterType<ScreeningTypeDataProvider>().As<IScreeningTypeDataProvider>();
            builder.RegisterType<ScreeningResultDataProvider>().As<IScreeningResultDataProvider>();
            builder.RegisterType<ServiceDurationDataProvider>().As<IServiceDurationDataProvider>();
            builder.RegisterType<ServiceCoordinatorDataProvider>().As<IServiceCoordinatorDataProvider>();
            builder.RegisterType<IFSPTypeDataProvider>().As<IFSPTypeDataProvider>();
            builder.RegisterType<ReceiveCorrespondenceDataProvider>().As<IReceiveCorrespondenceDataProvider>();
            builder.RegisterType<ReasonForDelayDataProvider>().As<IReasonForDelayDataProvider>();

            builder.RegisterType<ProgramDataProvider>().As<IProgramDataProvider>();
            builder.RegisterType<FacilityDataProvider>().As<IFacilityDataProvider>();
            builder.RegisterType<DayOfWeekDataProvider>().As<IDayOfWeekDataProvider>();
            builder.RegisterType<MonthDataProvider>().As<IMonthDataProvider>();
            builder.RegisterType<WeekOfMonthDataProvider>().As<IWeekOfMonthDataProvider>();
            builder.RegisterType<EntityTypeDataProvider>().As<IEntityTypeDataProvider>();
            builder.RegisterType<DocumentTypeDataProvider>().As<IDocumentTypeDataProvider>();
            builder.RegisterType<ScheduleTypeDataProvider>().As<IScheduleTypeDataProvider>();
            builder.RegisterType<UsersDataProvider>().As<IUsersDataProvider>();
            builder.RegisterType<ReportDataProvider>().As<IReportDataProvider>();
            builder.RegisterType<BPMethodDataProvider>().As<IBPMethodDataProvider>();
            builder.RegisterType<NoteTypeDataProvider>().As<INoteTypeDataProvider>();
            builder.RegisterType<NoteStatusDataProvider>().As<INoteStatusDataProvider>();
            builder.RegisterType<AllergyLookupDataProvider>().As<IAllergyLookupDataProvider>();
            builder.RegisterType<DrugLookupDataProvider>().As<IDrugLookupDataProvider>();
            builder.RegisterType<MedicalConditionLookupDataProvider>().As<IMedicalConditionLookupDataProvider>();
            builder.RegisterType<FamilyRelationshipDataProvider>().As<IFamilyRelationshipDataProvider>();
            builder.RegisterType<ReferralStatusDataProvider>().As<IReferralStatusDataProvider>();

            builder.RegisterType<ReferralTypeDataProvider>().As<IReferralTypeDataProvider>();
            builder.RegisterType<ReferralResourceTypeDataProvider>().As<IReferralResourceTypeDataProvider>();
            builder.RegisterType<ResourceTypeDataProvider>().As<IResourceTypeDataProvider>();
            builder.RegisterType<ReferralDispositionTypeDataProvider>().As<IReferralDispositionTypeDataProvider>();
            builder.RegisterType<ReferralDispositionOutcomeTypeDataProvider>().As<IReferralDispositionOutcomeTypeDataProvider>();
            builder.RegisterType<ReferralCategoryDataProvider>().As<IReferralCategoryDataProvider>();
            builder.RegisterType<Axis.DataProvider.Common.Lookups.Assesssment.AssessmentDataProvider>().As<Axis.DataProvider.Common.Lookups.Assesssment.IAssessmentDataProvider>();
            builder.RegisterType<UserFacilityDataProvider>().As<IUserFacilityDataProvider>();
            builder.RegisterType<ImmunizationStatusDataProvider>().As<IImmunizationStatusDataProvider>();
            builder.RegisterType<ReferralConcernTypeDataProvider>().As<IReferralConcernTypeDataProvider>();
            builder.RegisterType<ReferralPriorityDataProvider>().As<IReferralPriorityDataProvider>();
            builder.RegisterType<DischargeReasonDataProvider>().As<IDischargeReasonDataProvider>();
            builder.RegisterType<ReferralOrganizationDataProvider>().As<IReferralOrganizationDataProvider>();
            builder.RegisterType<ReferralCategorySourceDataProvider>().As<IReferralCategorySourceDataProvider>();

            builder.RegisterType<ReferralDispositionStatusTypeDataProvider>().As<IReferralDispositionStatusTypeDataProvider>();
            builder.RegisterType<ProgramClientIdentifierDataProvider>().As<IProgramClientIdentifierDataProvider>();

            builder.RegisterType<ServiceItemDataProvider>().As<IServiceItemDataProvider>();
            builder.RegisterType<AttendanceStatusDataProvider>().As<IAttendanceStatusDataProvider>();
            builder.RegisterType<DeliveryMethodDataProvider>().As<IDeliveryMethodDataProvider>();
            builder.RegisterType<ServiceStatusDataProvider>().As<IServiceStatusDataProvider>();
            builder.RegisterType<ServiceLocationDataProvider>().As<IServiceLocationDataProvider>();
            builder.RegisterType<RecipientCodeDataProvider>().As<IRecipientCodeDataProvider>();
            builder.RegisterType<ConversionStatusDataProvider>().As<IConversionStatusDataProvider>();
            builder.RegisterType<ContactTypeCallCenterDataProvider>().As<IContactTypeCallCenterDataProvider>();

            builder.RegisterType<SuicidalHomicidalDataProvider>().As<ISuicidalHomicidalDataProvider>();
            builder.RegisterType<CallStatusDataProvider>().As<ICallStatusDataProvider>();
            builder.RegisterType<ProgramUnitDataProvider>().As<IProgramUnitDataProvider>();
            builder.RegisterType<CCPriorityDataProvider>().As<ICCPriorityDataProvider>();
            builder.RegisterType<StaffManagementDataProvider>().As<IStaffManagementDataProvider>();
            builder.RegisterType<UserDetailDataProvider>().As<IUserDetailDataProvider>();
            builder.RegisterType<UserRoleDataProvider>().As<IUserRoleDataProvider>();
            builder.RegisterType<UserCredentialDataProvider>().As<IUserCredentialDataProvider>();
            builder.RegisterType<CallTypeDataProvider>().As<ICallTypeDataProvider>();
            builder.RegisterType<ClientStatusDataProvider>().As<IClientStatusDataProvider>();
            builder.RegisterType<BehavioralCategoryDataProvider>().As<IBehavioralCategoryDataProvider>();
            builder.RegisterType<CancelReasonDataProvider>().As<ICancelReasonDataProvider>();
            builder.RegisterType<DivisionProgramDataProvider>().As<IDivisionProgramDataProvider>();
            builder.RegisterType<UserSchedulingDataProvider>().As<IUserSchedulingDataProvider>();
            builder.RegisterType<UserPhotoDataProvider>().As<IUserPhotoDataProvider>();
            builder.RegisterType<UserDirectReportsDataProvider>().As<IUserDirectReportsDataProvider>();
            builder.RegisterType<UserSecurityDataProvider>().As<IUserSecurityDataProvider>();
            builder.RegisterType<ReferralAgencyDataProvider>().As<IReferralAgencyDataProvider>();
            builder.RegisterType<AdvancedDirectiveTypeDataProvider>().As<IAdvancedDirectiveTypeDataProvider>();
            builder.RegisterType<GroupTypeDataProvider>().As<IGroupTypeDataProvider>();
            builder.RegisterType<ServiceDataProvider>().As<IServiceDataProvider>();
            builder.RegisterType<PayorExpirationReasonDataProvider>().As<IPayorExpirationReasonDataProvider>();
            builder.RegisterType<GroupServiceDataProvider>().As<IGroupServiceDataProvider>();
            builder.RegisterType<VoidServiceDataProvider>().As<IVoidServiceDataProvider>();
            builder.RegisterType<ServiceTypeDataProvider>().As<IServiceTypeDataProvider>();
            builder.RegisterType<VoidRecordedServiceReasonDataProvider>().As<IVoidRecordedServiceReasonDataProvider>();
            builder.RegisterType<OrganizationsDataProvider>().As<IOrganizationsDataProvider>();
            builder.RegisterType<ServiceRecordingSourceDataProvider>().As<IServiceRecordingSourceDataProvider>();
            builder.RegisterType<AssessmentDataProvider>().As<IAssessmentDataProvider>();
            builder.RegisterType<ConsentsDataProvider>().As<IConsentsDataProvider>();
            builder.RegisterType<DocumentTypeGroupDataProvider>().As<IDocumentTypeGroupDataProvider>();
            builder.RegisterType<UserIdentifierTypeDataProvider>().As<IUserIdentifierTypeDataProvider>();
            builder.RegisterType<ConfirmationStatementDataProvider>().As<IConfirmationStatementDataProvider>();
            builder.RegisterType<RoleManagementDataProvider>().As<IRoleManagementDataProvider>();
            builder.RegisterType<TrackingFieldDataProvider>().As<ITrackingFieldDataProvider>();
            builder.RegisterType<ServicesDataProvider>().As<IServicesDataProvider>();
            builder.RegisterType<SynchronizationServiceDataProvider>().As<ISynchronizationServiceDataProvider>();
            builder.RegisterType<ClientMergeDataProvider>().As<IClientMergeDataProvider>();
            builder.RegisterType<PayorsDataProvider>().As<IPayorsDataProvider>();
            builder.RegisterType<OrganizationIdentifiersDataProvider>().As<IOrganizationIdentifiersDataProvider>();
            builder.RegisterType<OrganizationStructureDataProvider>().As<IOrganizationStructureDataProvider>();
            builder.RegisterType<CompanyDataProvider>().As<ICompanyDataProvider>();
            builder.RegisterType<DivisionDataProvider>().As<IDivisionDataProvider>();
            builder.RegisterType<ProgramsDataProvider>().As<IProgramsDataProvider>();
            builder.RegisterType<ProgramUnitsDataProvider>().As<IProgramUnitsDataProvider>();

            builder.RegisterType<PayorPlansDataProvider>().As<IPayorPlansDataProvider>();
            builder.RegisterType<PlanAddressesDataProvider>().As<IPlanAddressesDataProvider>();
            builder.RegisterType<CauseOfDeathDataProvider>().As<ICauseOfDeathDataProvider>();
            builder.RegisterType<ProvidersDataProvider>().As<IProvidersDataProvider>();
            builder.RegisterType<AdmissionReasonDataProvider>().As<IAdmissionReasonDataProvider>();
            builder.RegisterType<ServiceDefinitionDataProvider>().As<IServiceDefinitionDataProvider>();
            builder.RegisterType<ServiceConfigTypeDataProvider>().As<IServiceConfigTypeDataProvider>();
            builder.RegisterType<ServiceWorkflowTypeDataProvider>().As<IServiceWorkflowTypeDataProvider>();
            builder.RegisterType<ServiceDetailsDataProvider>().As<IServiceDetailsDataProvider>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        public int Order
        {
            get { return 1; }
        }
    }
}
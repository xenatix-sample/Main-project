using Autofac;
using Autofac.Integration.WebApi;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.Logging;
using Axis.RuleEngine.Account;
using Axis.RuleEngine.Account.ForgotPassword;
using Axis.RuleEngine.Account.UserProfile;
using Axis.RuleEngine.Admin;
using Axis.RuleEngine.Admin.DivisionProgram;
using Axis.RuleEngine.Admin.UserDirectReports;
using Axis.RuleEngine.Admin.UserPhoto;
using Axis.RuleEngine.Assessment;
using Axis.RuleEngine.BusinessAdmin.ClientMerge;
using Axis.RuleEngine.BusinessAdmin.Company;
using Axis.RuleEngine.BusinessAdmin.Division;
using Axis.RuleEngine.BusinessAdmin.HealthRecords;
using Axis.RuleEngine.BusinessAdmin.OrganizationStructure;
using Axis.RuleEngine.BusinessAdmin.PayorPlans;
using Axis.RuleEngine.BusinessAdmin.Payors;
using Axis.RuleEngine.BusinessAdmin.PlanAddresses;
using Axis.RuleEngine.BusinessAdmin.Program;
using Axis.RuleEngine.BusinessAdmin.ProgramUnit;
using Axis.RuleEngine.BusinessAdmin.ServiceDefinition;
using Axis.RuleEngine.BusinessAdmin.ServiceDetails;
using Axis.RuleEngine.Common.ClientAudit;
using Axis.RuleEngine.Common.WorkflowHeader;
using Axis.RuleEngine.Common.Photo;
using Axis.RuleEngine.Common.Provider;
using Axis.RuleEngine.Common.ServiceRecording;
using Axis.RuleEngine.Consents;
using Axis.RuleEngine.Logging;
using Axis.RuleEngine.Lookup;
using Axis.RuleEngine.Manifest;
using Axis.RuleEngine.RecordedServices.VoidService;
using Axis.RuleEngine.Security;
using Axis.RuleEngine.Settings;
using Axis.Service.Account;
using Axis.Service.Account.ForgotPassword;
using Axis.Service.Account.UserProfile;
using Axis.Service.Admin;
using Axis.Service.Admin.DivisionProgram;
using Axis.Service.Admin.UserDirectReports;
using Axis.Service.Admin.UserPhoto;
using Axis.Service.Assessment;
using Axis.Service.BusinessAdmin.ClientMerge;
using Axis.Service.BusinessAdmin.Company;
using Axis.Service.BusinessAdmin.Division;
using Axis.Service.BusinessAdmin.HealthRecords;
using Axis.Service.BusinessAdmin.OrganizationStructure;
using Axis.Service.BusinessAdmin.PayorPlans;
using Axis.Service.BusinessAdmin.Payors;
using Axis.Service.BusinessAdmin.PlanAddresses;
using Axis.Service.BusinessAdmin.Program;
using Axis.Service.BusinessAdmin.ProgramUnit;
using Axis.Service.BusinessAdmin.ServiceDefinition;
using Axis.Service.BusinessAdmin.ServiceDetails;
using Axis.Service.Common.ClientAudit;
using Axis.Service.Common.WorkFlowHeader;
using Axis.Service.Common.Photo;
using Axis.Service.Common.Provider;
using Axis.Service.Common.ServiceRecording;
using Axis.Service.Consents;
using Axis.Service.Logging;
using Axis.Service.Lookup;
using Axis.Service.Manifest;
using Axis.Service.RecordedServices.VoidService;
using Axis.Service.Security;
using Axis.Service.Settings;
using System.Reflection;

namespace Axis.RuleEngine.Service.DependencyInjection
{
    public class DependencyRegistration : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            // Added logger constructor parameter
            builder.RegisterType<Logger>().As<ILogger>().WithParameter("enableLogging", (ApplicationSettings.EnableLogging && ApplicationSettings.LoggingMode == (int)LoggingMode.File)).InstancePerLifetimeScope();

            builder.RegisterType<AccountRuleEngine>().As<IAccountRuleEngine>();
            builder.RegisterType<AccountService>().As<IAccountService>();

            builder.RegisterType<ForgotPasswordRuleEngine>().As<IForgotPasswordRuleEngine>();
            builder.RegisterType<ForgotPasswordService>().As<IForgotPasswordService>();

            builder.RegisterType<LoggingRuleEngine>().As<ILoggingRuleEngine>();
            builder.RegisterType<LoggingService>().As<ILoggingService>();

            builder.RegisterType<SecurityRuleEngine>().As<ISecurityRuleEngine>();
            builder.RegisterType<SecurityService>().As<ISecurityService>();

            builder.RegisterType<ManifestRuleEngine>().As<IManifestRuleEngine>();
            builder.RegisterType<ManifestService>().As<IManifestService>();

            builder.RegisterType<AdminRuleEngine>().As<IAdminRuleEngine>();
            builder.RegisterType<AdminService>().As<IAdminService>();

            builder.RegisterType<SettingsRuleEngine>().As<ISettingsRuleEngine>();
            builder.RegisterType<SettingsService>().As<ISettingsService>();

            builder.RegisterType<LookupRuleEngine>().As<ILookupRuleEngine>();
            builder.RegisterType<LookupService>().As<ILookupService>();

            builder.RegisterType<AssessmentRuleEngine>().As<IAssessmentRuleEngine>();
            builder.RegisterType<AssessmentService>().As<IAssessmentService>();

            builder.RegisterType<UserProfileRuleEngine>().As<IUserProfileRuleEngine>();
            builder.RegisterType<UserProfileService>().As<IUserProfileService>();

            builder.RegisterType<PhotoRuleEngine>().As<IPhotoRuleEngine>();
            builder.RegisterType<PhotoService>().As<IPhotoService>();

            builder.RegisterType<ClientAuditRuleEngine>().As<IClientAuditRuleEngine>();
            builder.RegisterType<ClientAuditService>().As<IClientAuditService>();

            builder.RegisterType<WorkflowHeaderRuleEngine>().As<IWorkflowHeaderRuleEngine>();
            builder.RegisterType<WorkFlowHeaderService>().As<IWorkFlowHeaderService>();

            builder.RegisterType<StaffManagementRuleEngine>().As<IStaffManagementRuleEngine>();
            builder.RegisterType<StaffManagementService>().As<IStaffManagementService>();

            builder.RegisterType<UserDetailRuleEngine>().As<IUserDetailRuleEngine>();
            builder.RegisterType<UserDetailService>().As<IUserDetailService>();

            builder.RegisterType<UserRoleRuleEngine>().As<IUserRoleRuleEngine>();
            builder.RegisterType<UserRoleService>().As<IUserRoleService>();

            builder.RegisterType<UserCredentialRuleEngine>().As<IUserCredentialRuleEngine>();
            builder.RegisterType<UserCredentialService>().As<IUserCredentialService>();

            builder.RegisterType<DivisionProgramRuleEngine>().As<IDivisionProgramRuleEngine>();
            builder.RegisterType<DivisionProgramService>().As<IDivisionProgramService>();

            builder.RegisterType<UserSchedulingRuleEngine>().As<IUserSchedulingRuleEngine>();
            builder.RegisterType<UserSchedulingService>().As<IUserSchedulingService>();

            builder.RegisterType<UserPhotoRuleEngine>().As<IUserPhotoRuleEngine>();
            builder.RegisterType<UserPhotoService>().As<IUserPhotoService>();

            builder.RegisterType<UserDirectReportsRuleEngine>().As<IUserDirectReportsRuleEngine>();
            builder.RegisterType<UserDirectReportsService>().As<IUserDirectReportsService>();

            builder.RegisterType<UserSecurityRuleEngine>().As<IUserSecurityRuleEngine>();
            builder.RegisterType<UserSecurityService>().As<IUserSecurityService>();

            builder.RegisterType<ServiceRecordingService>().As<IServiceRecordingService>();
            builder.RegisterType<ServiceRecordingRuleEngine>().As<IServiceRecordingRuleEngine>();

            builder.RegisterType<VoidServiceRuleEngine>().As<IVoidServiceRuleEngine>();
            builder.RegisterType<VoidRecordedService>().As<IVoidRecordedService>();

            builder.RegisterType<ConsentsRuleEngine>().As<IConsentsRuleEngine>();
            builder.RegisterType<ConsentsService>().As<IConsentsService>();

            builder.RegisterType<RoleManagementRuleEngine>().As<IRoleManagementRuleEngine>();
            builder.RegisterType<RoleManagementService>().As<IRoleManagementService>();

            builder.RegisterType<ClientMergeRuleEngine>().As<IClientMergeRuleEngine>();
            builder.RegisterType<ClientMergeService>().As<IClientMergeService>();

            builder.RegisterType<ProvidersRuleEngine>().As<IProvidersRuleEngine>();
            builder.RegisterType<ProvidersService>().As<IProvidersService>();

            builder.RegisterType<HealthRecordsRuleEngine>().As<IHealthRecordsRuleEngine>();
            builder.RegisterType<HealthRecordsService>().As<IHealthRecordsService>();

            builder.RegisterType<PayorsRuleEngine>().As<IPayorsRuleEngine>();
            builder.RegisterType<PayorsService>().As<IPayorsService>();

            builder.RegisterType<PayorPlansRuleEngine>().As<IPayorPlansRuleEngine>();
            builder.RegisterType<PayorPlansService>().As<IPayorPlansService>();

            builder.RegisterType<PlanAddressesRuleEngine>().As<IPlanAddressesRuleEngine>();
            builder.RegisterType<PlanAddressesService>().As<IPlanAddressesService>();

            builder.RegisterType<OrganizationStructureRuleEngine>().As<IOrganizationStructureRuleEngine>();
            builder.RegisterType<OrganizationStructureService>().As<IOrganizationStructureService>();

            builder.RegisterType<CompanyRuleEngine>().As<ICompanyRuleEngine>();
            builder.RegisterType<CompanyService>().As<ICompanyService>();

            builder.RegisterType<DivisionRuleEngine>().As<IDivisionRuleEngine>();
            builder.RegisterType<DivisionService>().As<IDivisionService>();

            builder.RegisterType<ProgramRuleEngine>().As<IProgramRuleEngine>();
            builder.RegisterType<ProgramService>().As<IProgramService>();

            builder.RegisterType<ProgramUnitsRuleEngine>().As<IProgramUnitsRuleEngine>();
            builder.RegisterType<ProgramUnitsService>().As<IProgramUnitsService>();

            builder.RegisterType<ServiceDefinitionRuleEngine>().As<IServiceDefinitionRuleEngine>();
            builder.RegisterType<ServiceDefinitionService>().As<IServiceDefinitionService>();

            builder.RegisterType<ServiceDetailsRuleEngine>().As<IServiceDetailsRuleEngine>();
            builder.RegisterType<ServiceDetailsService>().As<IServiceDetailsService>();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        public int Order
        {
            get { return 1; }
        }
    }
}
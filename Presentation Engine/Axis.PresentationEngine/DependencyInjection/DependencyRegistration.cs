using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Helpers.Caching;
using Axis.Helpers.Http;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.Logging;
using Axis.Plugins.CallCenter.Repository.ServiceRecording;
using Axis.PluginsEngine;
using Axis.PresentationEngine.Areas.Account.Repository;
using Axis.PresentationEngine.Areas.Account.Repository.ForgotPassword;
using Axis.PresentationEngine.Areas.Account.Respository;
using Axis.PresentationEngine.Areas.Admin.Respository;
using Axis.PresentationEngine.Areas.Admin.Respository.DivisionProgram;
using Axis.PresentationEngine.Areas.Admin.Respository.Provider;
using Axis.PresentationEngine.Areas.Admin.Respository.UserPhoto;
using Axis.PresentationEngine.Areas.Assessment.Repository;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.ClientMerge;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.Company;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.Division;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.HealthRecords;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.OrganizationStructure;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.PayorPlans;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.Payors;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.PlanAddresses;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.Program;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.ProgramUnit;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.ServiceDefinition;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.ServiceDetails;
using Axis.PresentationEngine.Areas.Cache.Repository;
using Axis.PresentationEngine.Areas.Configuration.Repository;
using Axis.PresentationEngine.Areas.Consents.Repository;
using Axis.PresentationEngine.Areas.Logging.Repository;
using Axis.PresentationEngine.Areas.Lookup.Repository;
using Axis.PresentationEngine.Areas.RecordedServices.Repository;
using Axis.PresentationEngine.Areas.Security.Repository;
using Axis.PresentationEngine.Helpers.Bundles;
using Axis.PresentationEngine.Helpers.Repositories;
using Axis.PresentationEngine.Helpers.Routes;
using Axis.PresentationEngine.Repository;
using System.Linq;
using System.Web;

namespace Axis.PresentationEngine.DependencyInjection
{
    public class DependencyRegistration : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //HTTP context and other related stuff
            builder.Register(c =>
                //register FakeHttpContext when HttpContext is not available
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerRequest();

            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerRequest();

            //cache manager
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("axis_cache_static").SingleInstance();

            // Added logger's constructor parameter
            builder.RegisterType<Logger>().As<ILogger>().WithParameter("enableLogging", (ApplicationSettings.EnableLogging && ApplicationSettings.LoggingMode == (int)LoggingMode.File)).InstancePerLifetimeScope();
            builder.RegisterType<LoggingRepository>().As<ILoggingRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AccountRepository>().As<IAccountRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ForgotPasswordRepository>().As<IForgotPasswordRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SecurityRepository>().As<ISecurityRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CacheRepository>().As<ICacheRepository>();
            builder.RegisterType<AdminRepository>().As<IAdminRepository>();
            builder.RegisterType<ConfigurationRepository>().As<IConfigurationRepository>();
            builder.RegisterType<LookupRepository>().As<ILookupRepository>();
            builder.RegisterType<UserProfileRepository>().As<IUserProfileRepository>();
            builder.RegisterType<AssessmentRepository>().As<IAssessmentRepository>();
            builder.RegisterType<PhotoRepository>().As<IPhotoRepository>();
            builder.RegisterType<ClientAuditRepository>().As<IClientAuditRepository>();
            builder.RegisterType<WorkflowHeaderRepository>().As<IWorkflowHeaderRepository>();
            builder.RegisterType<StaffManagementRepository>().As<IStaffManagementRepository>();
            builder.RegisterType<UserDetailRepository>().As<IUserDetailRepository>();
            builder.RegisterType<UserRoleRepository>().As<IUserRoleRepository>();
            builder.RegisterType<UserCredentialRepository>().As<IUserCredentialRepository>();
            builder.RegisterType<UserHeaderRepository>().As<IUserHeaderRepository>();
            builder.RegisterType<DivisionProgramRepository>().As<IDivisionProgramRepository>();
            builder.RegisterType<UserSchedulingRepository>().As<IUserSchedulingRepository>();
            builder.RegisterType<UserPhotoRepository>().As<IUserPhotoRepository>();
            builder.RegisterType<UserDirectReportsRepository>().As<IUserDirectReportsRepository>();
            builder.RegisterType<UserSecurityRepository>().As<IUserSecurityRepository>();
            builder.RegisterType<RoutePublisher>().As<IRoutePublisher>().SingleInstance();
            builder.RegisterType<HttpRoutePublisher>().As<IHttpRoutePublisher>().SingleInstance();
            builder.RegisterType<BundlePublisher>().As<IBundlePublisher>().SingleInstance();
            builder.RegisterType<AppDomainTypeFinder>().As<ITypeFinder>().SingleInstance();
            builder.RegisterType<ServiceRecordingRepository>().As<IServiceRecordingRepository>();
            builder.RegisterType<VoidServiceRepository>().As<IVoidServiceRepository>();
            builder.RegisterType<ConsentsRepository>().As<IConsentsRepository>();
            builder.RegisterType<RoleManagementRepository>().As<IRoleManagementRepository>();
            builder.RegisterType<ClientMergeRepository>().As<IClientMergeRepository>();
            builder.RegisterType<ProvidersRepository>().As<IProvidersRepository>();
            builder.RegisterType<HealthRecordsRepository>().As<IHealthRecordsRepository>();
            builder.RegisterType<PayorsRepository>().As<IPayorsRepository>();
            builder.RegisterType<PayorPlansRepository>().As<IPayorPlansRepository>();
            builder.RegisterType<PlanAddressesRepository>().As<IPlanAddressesRepository>();
            builder.RegisterType<OrganizationStructureRepository>().As<IOrganizationStructureRepository>();
            builder.RegisterType<CompanyRepository>().As<ICompanyRepository>();
            builder.RegisterType<DivisionRepository>().As<IDivisionRepository>();
            builder.RegisterType<ProgramRepository>().As<IProgramRepository>();
            builder.RegisterType<ProgramUnitsRepository>().As<IProgramUnitsRepository>();
            builder.RegisterType<ServiceDefinitionRepository>().As<IServiceDefinitionRepository>();
            builder.RegisterType<ServiceDetailsRepository>().As<IServiceDetailsRepository>();
            
            builder.RegisterType<PluginFinder>().As<IPluginFinder>().InstancePerLifetimeScope();
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();

            var allAssemblies = typeFinder.GetAssemblies().ToArray();
            builder.RegisterControllers(allAssemblies);
            builder.RegisterApiControllers(allAssemblies);
        }

        public int Order
        {
            get { return 1; }
        }
    }
}
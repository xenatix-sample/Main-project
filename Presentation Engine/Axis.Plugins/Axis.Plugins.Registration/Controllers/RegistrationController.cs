using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Axis.Helpers.Infrastructure;
using Axis.Model.Common;
using Axis.Plugins.Registration.Repository;
using Axis.Plugins.Registration.Model;
using Axis.Plugins.Registration.Repository.FinancialAssessment;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.PresentationEngine.Helpers.Repositories;
using Newtonsoft.Json;
using Axis.Plugins.Registration.Repository.Referral;

namespace Axis.Plugins.Registration.Controllers
{
    /// <summary>
    /// Registration Controller Class
    /// </summary>
    [PresentationEngine.Helpers.Filters.Authorization(AllowAnonymous = true)]
    public class RegistrationController : BaseController
    {
        #region Private Variable

        /// <summary>
        ///  Private variable for registration repository
        /// </summary>
        private readonly IRegistrationRepository registrationRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="registrationRepository"></param>
        public RegistrationController(IRegistrationRepository registrationRepository)
        {
            this.registrationRepository = registrationRepository;
        }

        /// <summary>
        /// constructor
        /// </summary>
        public RegistrationController()
        {

        }

        #endregion

        #region Public Action Methods

        /// <summary>
        /// Main Registration Screen Method
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Main()
        {
            return View();
        }

        /// <summary>
        /// Registration Screen Navigation Menu Method
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Navigation()
        {
            return View("../Shared/_RegistrationNavigation");
        }

        protected enum NavigationValidationState
        {
            Valid,
            Invalid,
            Warning,
            Unsigned
        };

        [HttpGet]
        async public Task<ActionResult> NavigationValidationStates(long contactId, List<string> workflowActions)
        {
            Dictionary<string, Task<NavigationValidationState>> validationTasks = new Dictionary<string, Task<NavigationValidationState>>();

            var states = new Dictionary<string, string>();

            var contactDemographic = await registrationRepository.GetContactDemographics(contactId);
            if ((contactDemographic.DataItems != null) && (contactDemographic.DataItems.Count == 1))
            {
                var contactTypeLookups =
                    EngineContext.Current.Resolve<ILookupRepository>().GetLookupsByType(LookupType.ContactType);

                // DCC: Will refactor this into a command pattern, I promise! ;-)

                if (workflowActions != null)
                {
                    if (workflowActions.Contains("registration.additional"))
                        validationTasks.Add("registration.additional", GetAdditionalValidationState(contactId));
                    if (workflowActions.Contains("registration.referral"))
                        validationTasks.Add("registration.referral", GetReferralValidationState(contactId));

                    if (workflowActions.Contains("registration.emergcontacts"))
                        validationTasks.Add("registration.emergcontacts", GetEmergencyValidationState(contactId,
                            (int) contactTypeLookups.DataItems[0][LookupType.ContactType.ToString()].First(
                                x => x.Name == "Emergency").ID));
                    if (workflowActions.Contains("registration.benefits"))
                        validationTasks.Add("registration.benefits",
                            GetBenefitValidationState(contactId));
                    if (workflowActions.Contains("registration.financial"))
                        validationTasks.Add("registration.financial", GetFinancialValidationState(contactId));
                    if (workflowActions.Contains("registration.collateral"))
                        validationTasks.Add("registration.collateral", GetCollateralValidationState(contactId,
                            (int) contactTypeLookups.DataItems[0][LookupType.ContactType.ToString()].First(
                                x => x.Name == "Collateral").ID));
                    if (workflowActions.Contains("registration.consent"))
                        validationTasks.Add("registration.consent", GetConsentValidationState(contactId));
                }

                states.Add("registration.demographics", NavigationValidationState.Valid.ToString().ToLower());
                validationTasks.ToList().ForEach(async delegate(KeyValuePair<string, Task<NavigationValidationState>> validationTask)
                {
                    states.Add(validationTask.Key, (await validationTask.Value).ToString().ToLower());
                });
            }

            await Task.WhenAll(validationTasks.Select(x => x.Value));

            return Content(JsonConvert.SerializeObject(states), "text/javascript");
        }

        #region Async Validation State Helpers

        private async Task<NavigationValidationState> GetAdditionalValidationState(long contactId)
        {
            NavigationValidationState returnValue = NavigationValidationState.Invalid;
            var additionalDemographic = await EngineContext.Current.Resolve<IAdditionalDemographicRepository>()
                .GetAdditionalDemographicAsync(contactId);
            if ((additionalDemographic.DataItems != null) && (additionalDemographic.DataItems.Count > 0) && additionalDemographic.DataItems[0].AdditionalDemographicID > 0)
                returnValue = NavigationValidationState.Valid;
            return returnValue;
        }

        private async Task<NavigationValidationState> GetReferralValidationState(long contactId)
        {
            NavigationValidationState returnValue = NavigationValidationState.Warning;
            var referral = await EngineContext.Current.Resolve<IReferralAdditionalDetailRepository>()
                .GetReferralAdditionalDetail(contactId);
            if ((referral.DataItems != null) && (referral.DataItems.Count > 0) && referral.DataItems[0].ReferralHeaderID > 0)
                returnValue = NavigationValidationState.Valid;
            return returnValue;
        }

        private async Task<NavigationValidationState> GetEmergencyValidationState(long contactId, int contactTypeId)
        {
            NavigationValidationState returnValue = NavigationValidationState.Warning;
            var emergContacts = await EngineContext.Current.Resolve<IEmergencyContactRepository>()
                .GetEmergencyContactsAsync(contactId, contactTypeId);
            if ((emergContacts.DataItems != null) && (emergContacts.DataItems.Count > 0))
                returnValue = NavigationValidationState.Valid;
            return returnValue;
        }

        private async Task<NavigationValidationState> GetBenefitValidationState(long contactId)
        {
            NavigationValidationState returnValue = NavigationValidationState.Warning;
            var benefits = await EngineContext.Current.Resolve<IContactBenefitRepository>()
                .GetContactBenefitsAsync(contactId);
            if ((benefits.DataItems != null) && (benefits.DataItems.Count > 0))
                returnValue = NavigationValidationState.Valid;
            return returnValue;
        }

        private async Task<NavigationValidationState> GetFinancialValidationState(long contactId)
        {
            NavigationValidationState returnValue = NavigationValidationState.Warning;
            var financial = await EngineContext.Current.Resolve<IFinancialAssessmentRepository>()
                .GetFinancialAssessmentDetailsAsync(contactId, 0);
            if ((financial.DataItems != null) && (financial.DataItems.Count > 0))
                returnValue = NavigationValidationState.Valid;
            return returnValue;
        }

        private async Task<NavigationValidationState> GetCollateralValidationState(long contactId, int contactTypeId)
        {
            NavigationValidationState returnValue = NavigationValidationState.Warning;
            var collateral = await EngineContext.Current.Resolve<ICollateralRepository>()
                .GetCollateralsAsync(contactId, contactTypeId,false);
            if ((collateral.DataItems != null) && (collateral.DataItems.Count > 0))
                returnValue = NavigationValidationState.Valid;
            return returnValue;
        }

        private async Task<NavigationValidationState> GetConsentValidationState(long contactId)
        {
            NavigationValidationState returnValue = NavigationValidationState.Warning;
            var consent = await EngineContext.Current.Resolve<IConsentRepository>()
                .GetConsentSignatureAsync(contactId);
            if ((consent.DataItems != null) && (consent.DataItems.Count > 0) && (!consent.DataItems[0].SignatureId.HasValue))
                returnValue = NavigationValidationState.Unsigned;
            else if ((consent.DataItems != null) && (consent.DataItems.Count > 0) && (consent.DataItems[0].SignatureId.HasValue))
                returnValue = NavigationValidationState.Valid;

            return returnValue;
        }

        #endregion

        /// <summary>
        /// Index Method
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Add Action Method
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Update Action Method
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Update()
        {
            return View();
        }

        /// <summary>
        /// Load patient header
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult PatientHeader()
        {
            var strView = "../Shared/_PatientHeader";
            return View(strView);
        }

        /// <summary>
        /// Load ECI patient header
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ECIPatientHeader()
        {
            var strView = "../Shared/_ECIPatientHeader";
            return View(strView);
        }

        #endregion
    }
}
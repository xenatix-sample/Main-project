using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Axis.Helpers.Infrastructure;
using Axis.Model.Security;
using Axis.PresentationEngine.Areas.Admin.Respository;
using Axis.PresentationEngine.Areas.Admin.Respository.DivisionProgram;
using Axis.PresentationEngine.Areas.Admin.Respository.UserPhoto;
using Axis.PresentationEngine.Helpers.Controllers;
using Newtonsoft.Json;

namespace Axis.PresentationEngine.Areas.Admin.Controllers
{
    public class UserDetailController : BaseController
    {
        #region Private Variables

        private readonly IUserDetailRepository _userDetailRepository;

        #endregion

        #region Constructors

        public UserDetailController()
        {

        }

        public UserDetailController(IUserDetailRepository userDetailRepository)
        {
            _userDetailRepository = userDetailRepository;
        }

        #endregion

        #region Action Results

        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Public Methods

        [HttpGet]
        async public Task<ActionResult> NavigationValidationStates(int userID, List<string> workflowActions)
        {
            Dictionary<string, Task<NavigationValidationState>> validationTasks = new Dictionary<string, Task<NavigationValidationState>>();

            var states = new Dictionary<string, string>();

            var userDetails = await _userDetailRepository.GetUserAsync(userID);
            if ((userDetails.DataItems != null) && (userDetails.DataItems.Count == 1))
            {
                if (workflowActions != null)
                {
                    if (workflowActions.Contains("siteadministration.staffmanagement.user.roles"))
                        validationTasks.Add("siteadministration.staffmanagement.user.roles", GetUserRolesValidationState(userID));
                    if (workflowActions.Contains("siteadministration.staffmanagement.user.credentials"))
                        validationTasks.Add("siteadministration.staffmanagement.user.credentials", GetUserCredentialsValidationState(userID));
                    if (workflowActions.Contains("siteadministration.staffmanagement.user.divisionprogram"))
                        validationTasks.Add("siteadministration.staffmanagement.user.divisionprogram", GetDivisionProgramValidationState(userID));
                    if (workflowActions.Contains("siteadministration.staffmanagement.user.scheduling"))
                        validationTasks.Add("siteadministration.staffmanagement.user.scheduling", GetSchedulingValidationState(userID));
                    if (workflowActions.Contains("siteadministration.staffmanagement.user.blockedtime"))
                        validationTasks.Add("siteadministration.staffmanagement.user.blockedtime", GetBlockedTimeValidationState(userID));
                    if (workflowActions.Contains("siteadministration.staffmanagement.user.directreports"))
                        validationTasks.Add("siteadministration.staffmanagement.user.directreports", GetDirectReportsValidationState(userID));
                    if (workflowActions.Contains("siteadministration.staffmanagement.user.profile"))
                        validationTasks.Add("siteadministration.staffmanagement.user.profile", GetUserProfileValidationState());
                    if (workflowActions.Contains("siteadministration.staffmanagement.user.photos"))
                        validationTasks.Add("siteadministration.staffmanagement.user.photos", GetUserPhotosValidationState(userID));
                    if (workflowActions.Contains("siteadministration.staffmanagement.user.additionaldetails"))
                        validationTasks.Add("siteadministration.staffmanagement.user.additionaldetails", GetAdditionalDetailsValidationState(userID));
                }

                states.Add("siteadministration.staffmanagement.user.details", NavigationValidationState.Valid.ToString().ToLower());
                validationTasks.ToList().ForEach(async delegate (KeyValuePair<string, Task<NavigationValidationState>> validationTask)
                {
                    states.Add(validationTask.Key, (await validationTask.Value).ToString().ToLower());
                });
            }

            await Task.WhenAll(validationTasks.Select(x => x.Value));

            return Content(JsonConvert.SerializeObject(states), "text/javascript");
        }

        #endregion

        #region Enum

        protected enum NavigationValidationState
        {
            Valid,
            Invalid,
            Warning,
            Unsigned
        };

        #endregion

        #region Async Validation State Helpers

        private async Task<NavigationValidationState> GetUserRolesValidationState(int userID)
        {
            var userRoles = await EngineContext.Current.Resolve<IUserRoleRepository>()
                .GetUserRolesAsync(userID);

            bool hasAssignedRoles = false;
            if ((userRoles.DataItems != null) && (userRoles.DataItems.Count > 0))
            {
                foreach (UserRoleModel userRole in userRoles.DataItems)
                {
                    if (userRole.HasRole)
                        hasAssignedRoles = true;
                }
            }

            var returnValue = hasAssignedRoles ? NavigationValidationState.Valid : NavigationValidationState.Invalid;
            return returnValue;
        }

        private async Task<NavigationValidationState> GetUserCredentialsValidationState(int userID)
        {
            NavigationValidationState returnValue = NavigationValidationState.Warning;
            var userCredentials = await EngineContext.Current.Resolve<IUserCredentialRepository>()
                .GetUserCredentialsAsync(userID, false);
            if ((userCredentials.DataItems != null) && (userCredentials.DataItems.Count > 0))
                returnValue = NavigationValidationState.Valid;

            return returnValue;
        }

        private async Task<NavigationValidationState> GetDivisionProgramValidationState(int userID)
        {
            NavigationValidationState returnValue = NavigationValidationState.Warning;
            var divisionProgram = await EngineContext.Current.Resolve<IDivisionProgramRepository>()
                .GetDivisionProgramsAsync(userID, false);
            if ((divisionProgram.DataItems != null) && (divisionProgram.DataItems.Count > 0))
                returnValue = NavigationValidationState.Valid;

            return returnValue;
        }

        private async Task<NavigationValidationState> GetSchedulingValidationState(int userID)
        {
            NavigationValidationState returnValue = NavigationValidationState.Warning;
            var userSchedule = await EngineContext.Current.Resolve<IUserSchedulingRepository>()
               .GetUserFacilitiesAsync(userID, false);
            if ((userSchedule.DataItems != null) && (userSchedule.DataItems.Count > 0))
                returnValue = NavigationValidationState.Valid;

            return returnValue;
        }

        private async Task<NavigationValidationState> GetBlockedTimeValidationState(int userID)
        {
            NavigationValidationState returnValue = NavigationValidationState.Warning;
            await PlaceHolderTask();
            return returnValue;
        }

        private async Task<NavigationValidationState> GetDirectReportsValidationState(int userID)
        {
            NavigationValidationState returnValue = NavigationValidationState.Warning;
            var directReports = await EngineContext.Current.Resolve<IUserDirectReportsRepository>()
                .GetUsersAsync(userID, false);
            if ((directReports.DataItems != null) && (directReports.DataItems.Count > 0))
                returnValue = NavigationValidationState.Valid;
            return returnValue;
        }

        private async Task<NavigationValidationState> GetUserProfileValidationState()
        {
            await PlaceHolderTask();
            return NavigationValidationState.Valid;
        }

        private async Task<NavigationValidationState> GetUserPhotosValidationState(int userID)
        {
            NavigationValidationState returnValue = NavigationValidationState.Warning;
            var userPhotos = await EngineContext.Current.Resolve<IUserPhotoRepository>()
                .GetUserPhotoAsync(userID, false);
            if ((userPhotos.DataItems != null) && (userPhotos.DataItems.Count > 0))
                returnValue = NavigationValidationState.Valid;

            return returnValue;
        }

        private async Task<NavigationValidationState> GetAdditionalDetailsValidationState(int userID)
        {
            NavigationValidationState returnValue = NavigationValidationState.Warning;
            var result = await EngineContext.Current.Resolve<IUserDetailRepository>()
                .GetCoSignaturesAsync(userID);
            if ((result.DataItems != null) && (result.DataItems.Count > 0) &&
                result.DataItems[0].CoSignatures != null && result.DataItems[0].CoSignatures.Count > 0)
                returnValue = NavigationValidationState.Valid;
            else
            {
                var result2 = await EngineContext.Current.Resolve<IUserDetailRepository>().GetUserIdentifierDetailsAsync(userID);
                if ((result2.DataItems != null) && (result2.DataItems.Count > 0) &&
                    result2.DataItems[0].UserDetails != null && result2.DataItems[0].UserDetails.Count > 0)
                    returnValue = NavigationValidationState.Valid;
                else
                {
                    var result3 = await EngineContext.Current.Resolve<IUserDetailRepository>().GetUserAdditionalDetailsAsync(userID);
                    if ((result3.DataItems != null) && (result3.DataItems.Count > 0) &&
                        result3.DataItems[0].UserDetails != null && result3.DataItems[0].UserDetails.Count > 0)
                        returnValue = NavigationValidationState.Valid;
                }
            }

            return returnValue;
        }

        #region Task Placeholders

        private Task<int> PlaceHolderTask()
        {
            return Task.Run(() => Nothing());
        }

        private int Nothing()
        {
            return 0;
        }

        #endregion

        #endregion
    }
}
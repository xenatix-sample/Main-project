using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Axis.Helpers.Infrastructure;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Account.Repository;
using Axis.PresentationEngine.Helpers.Controllers;
using Newtonsoft.Json;
using Axis.PresentationEngine.Areas.Account.Respository;
using Axis.PresentationEngine.Areas.Admin.Respository;
using Axis.PresentationEngine.Areas.Admin.Respository.DivisionProgram;
using Axis.PresentationEngine.Areas.Admin.Respository.UserPhoto;

namespace Axis.PresentationEngine.Areas.Account.Controllers
{
    public class UserProfileController : BaseController
    {
        #region Class Variables

        private readonly IUserProfileRepository _userProfileRepository;

        #endregion

        #region Constructors

        public UserProfileController(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        #endregion

        #region Action Results

        public ActionResult UserProfile()
        {
            return View();
        }

        public ActionResult UserProfileMain()
        {
            return View();
        }

        public ActionResult MyProfileMain()
        {
            var strView = "../Shared/_MyProfileMain";
            return View(strView);
        }

        #endregion

        #region Json Results

        /// <summary>
        /// Load the user profile data from the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetUserProfile(bool isMyProfile)
        {
            return Json(_userProfileRepository.GetUserProfile(isMyProfile), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUserProfileByID(int userID, bool isMyProfile)
        {
            return Json(_userProfileRepository.GetUserProfileByID(userID, isMyProfile), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Save the user profile data to the database
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveUserProfile(UserProfileViewModel userProfile, bool isMyProfile)
        {
            //need to create repo method
            return Json(_userProfileRepository.SaveUserProfile(userProfile, isMyProfile), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Public Methods

        [HttpGet]
        async public Task<ActionResult> NavigationValidationStates(List<string> workflowActions)
        {
            Dictionary<string, Task<NavigationValidationState>> validationTasks = new Dictionary<string, Task<NavigationValidationState>>();

            var states = new Dictionary<string, string>();
            if (workflowActions != null)
            {
                if (workflowActions.Contains("myprofile.nav.profile"))
                    validationTasks.Add("myprofile.nav.profile", GetMyProfileValidationState());
                if (workflowActions.Contains("myprofile.nav.security"))
                    validationTasks.Add("myprofile.nav.security", GetSecurityDigitalSignatureValidationState());
                if (workflowActions.Contains("myprofile.nav.digitalsignature"))
                    validationTasks.Add("myprofile.nav.digitalsignature", GetSecurityDigitalSignatureValidationState());
                if (workflowActions.Contains("myprofile.nav.credentials"))
                    validationTasks.Add("myprofile.nav.credentials", GetCredentialsValidationState());
                if (workflowActions.Contains("myprofile.nav.divisionprograms"))
                    validationTasks.Add("myprofile.nav.divisionprograms", GetDivisionProgramsValidationState());
                if (workflowActions.Contains("myprofile.nav.scheduling"))
                    validationTasks.Add("myprofile.nav.scheduling", GetSchedulingValidationState());
                if (workflowActions.Contains("myprofile.nav.directreports"))
                    validationTasks.Add("myprofile.nav.directreports", GetDirectReportsValidationState());
                if (workflowActions.Contains("myprofile.nav.userphotos"))
                    validationTasks.Add("myprofile.nav.userphotos", GetUserPhotosValidationState());
            }

            validationTasks.ToList().ForEach(async delegate(KeyValuePair<string, Task<NavigationValidationState>> validationTask)
            {
                states.Add(validationTask.Key, (await validationTask.Value).ToString().ToLower());
            });

            await Task.WhenAll(validationTasks.Select(x => x.Value));

            return Content(JsonConvert.SerializeObject(states), "text/javascript");
        }

        #endregion

        #region Async Validation State Helpers

        private async Task<NavigationValidationState> GetMyProfileValidationState()
        {
            //This screen will always return as valid because this data is required to enter a new user
            await PlaceHolderTask();
            return NavigationValidationState.Valid;
        }

        private async Task<NavigationValidationState> GetSecurityDigitalSignatureValidationState()
        {
            NavigationValidationState returnValue = NavigationValidationState.Invalid;
            var myProfileData = await EngineContext.Current.Resolve<IUserProfileRepository>()
               .GetUserProfileAsync(true);
            if ((myProfileData.DataItems != null) && (myProfileData.DataItems.Count > 0))
            {
                var securityQuestions = await EngineContext.Current.Resolve<IUserSecurityRepository>()
                   .GetUserSecurityAsync(myProfileData.DataItems[0].UserID);
                if ((securityQuestions.DataItems != null) && (securityQuestions.DataItems.Count > 0))
                    returnValue = NavigationValidationState.Valid;
            }
            return returnValue;
        }

        private async Task<NavigationValidationState> GetCredentialsValidationState()
        {
            NavigationValidationState returnValue = NavigationValidationState.Warning;
            var myProfileData = await EngineContext.Current.Resolve<IUserProfileRepository>()
              .GetUserProfileAsync(true);
            if ((myProfileData.DataItems != null) && (myProfileData.DataItems.Count > 0))
            {
                var credentials = await EngineContext.Current.Resolve<IUserCredentialRepository>()
                    .GetUserCredentialsAsync(myProfileData.DataItems[0].UserID,true);
                if ((credentials.DataItems != null) && (credentials.DataItems.Count > 0))
                    returnValue = NavigationValidationState.Valid;
            }

            return returnValue;
        }

        private async Task<NavigationValidationState> GetDivisionProgramsValidationState()
        {
            NavigationValidationState returnValue = NavigationValidationState.Warning;
            var myProfileData = await EngineContext.Current.Resolve<IUserProfileRepository>()
                .GetUserProfileAsync(true);
            if ((myProfileData.DataItems != null) && (myProfileData.DataItems.Count > 0))
            {
                var divisionPrograms = await EngineContext.Current.Resolve<IDivisionProgramRepository>()
                    .GetDivisionProgramsAsync(myProfileData.DataItems[0].UserID,true);
                if ((divisionPrograms.DataItems != null) && (divisionPrograms.DataItems.Count > 0))
                    returnValue = NavigationValidationState.Valid;
            }

            return returnValue;
        }

        private async Task<NavigationValidationState> GetSchedulingValidationState()
        {
            NavigationValidationState returnValue = NavigationValidationState.Warning;
            var myProfileData = await EngineContext.Current.Resolve<IUserProfileRepository>()
                .GetUserProfileAsync(true);
            if ((myProfileData.DataItems != null) && (myProfileData.DataItems.Count > 0))
            {
            var userSchedule = await EngineContext.Current.Resolve<IUserSchedulingRepository>()
               .GetUserFacilitiesAsync(myProfileData.DataItems[0].UserID,true);
            if ((userSchedule.DataItems != null) && (userSchedule.DataItems.Count > 0))
                returnValue = NavigationValidationState.Valid;
            }
            
            return returnValue;
        }

        private async Task<NavigationValidationState> GetDirectReportsValidationState()
        {
            NavigationValidationState returnValue = NavigationValidationState.Warning;
            var myProfileData = await EngineContext.Current.Resolve<IUserProfileRepository>()
               .GetUserProfileAsync(true);
            if ((myProfileData.DataItems != null) && (myProfileData.DataItems.Count > 0))
            {
                var userID = myProfileData.DataItems[0].UserID;
                var directReports = await EngineContext.Current.Resolve<IUserDirectReportsRepository>()
                    .GetUsersAsync(userID,true);
                if ((directReports.DataItems != null) && (directReports.DataItems.Count > 0))
                    returnValue = NavigationValidationState.Valid;
            }
            return returnValue;
        }

        private async Task<NavigationValidationState> GetUserPhotosValidationState()
        {
            NavigationValidationState returnValue = NavigationValidationState.Warning;
            var myProfileData = await EngineContext.Current.Resolve<IUserProfileRepository>()
                .GetUserProfileAsync(true);
            if ((myProfileData.DataItems != null) && (myProfileData.DataItems.Count > 0))
            {
                var userPhotos = await EngineContext.Current.Resolve<IUserPhotoRepository>()
                    .GetUserPhotoAsync(myProfileData.DataItems[0].UserID,true);
                if ((userPhotos.DataItems != null) && (userPhotos.DataItems.Count > 0))
                    returnValue = NavigationValidationState.Valid;
            }

            return returnValue;
        }

        #endregion

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

        #region Enum

        protected enum NavigationValidationState
        {
            Valid,
            Invalid,
            Warning,
            Unsigned
        };

        #endregion
    }
}
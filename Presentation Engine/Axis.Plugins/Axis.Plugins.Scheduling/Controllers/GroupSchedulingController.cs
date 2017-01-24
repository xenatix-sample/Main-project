using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;
using Axis.Helpers.Infrastructure;
using Axis.Plugins.Scheduling.Repository.Appointment;
using Axis.Plugins.Scheduling.Repository.GroupScheduling;
using Newtonsoft.Json;

namespace Axis.Plugins.Scheduling.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class GroupSchedulingController : BaseController
    {
        #region Private Variables

        private readonly IGroupSchedulingRepository _groupSchedulingRepository;

        #endregion

        #region Constructors

        public GroupSchedulingController(IGroupSchedulingRepository groupSchedulingRepository)
        {
            _groupSchedulingRepository = groupSchedulingRepository;
        }

        #endregion

        #region Action Results

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult GroupSchedule()
        {
            return View();
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult GroupNote()
        {
            return View();
        }

        public ActionResult GroupSchedulingMain()
        {
            return View();
        }
        
        #endregion

        #region Public Methods

        [HttpGet]
        async public Task<ActionResult> NavigationValidationStates(long groupID, List<string> workflowActions)
        {
            Dictionary<string, Task<NavigationValidationState>> validationTasks = new Dictionary<string, Task<NavigationValidationState>>();
            var states = new Dictionary<string, string>();

            var groupDetails = await _groupSchedulingRepository.GetAppointmentByGroupIDAsync(groupID);
            if (groupDetails.DataItems != null)
            {
                if (workflowActions != null)
                {
                    var appointmentID = groupDetails.DataItems[0].AppointmentID;
                    if (workflowActions.Contains("scheduling.groupscheduling.details.groupnote"))
                        validationTasks.Add("scheduling.groupscheduling.details.groupnote", GetGroupNoteValidationState(appointmentID, groupID));
                }

                states.Add("scheduling.groupscheduling.details.groupschedule", NavigationValidationState.Valid.ToString().ToLower());
                validationTasks.ToList().ForEach(async delegate(KeyValuePair<string, Task<NavigationValidationState>> validationTask)
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

        private async Task<NavigationValidationState> GetGroupNoteValidationState(long appointmentID, long groupID)
        {
            NavigationValidationState returnValue = NavigationValidationState.Warning;
            var groupNoteDetails = await EngineContext.Current.Resolve<IAppointmentRepository>()
                .GetAppointmentNoteAsync(appointmentID, null, groupID, null);
            if ((groupNoteDetails.DataItems != null) && (groupNoteDetails.DataItems.Count > 0))
                returnValue = NavigationValidationState.Valid;

            return returnValue;
        }

        #endregion
    }
}
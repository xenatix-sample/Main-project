using System;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Plugins.Scheduling.Models;
using Axis.Plugins.Scheduling.Translator;
using Axis.Service;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace Axis.Plugins.Scheduling.Repository.Appointment
{
    /// <summary>
    ///
    /// </summary>
    public class AppointmentRepository : IAppointmentRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "appointment/";

        /// <summary>
        /// constructor
        /// </summary>
        public AppointmentRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="token">The token</param>
        public AppointmentRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the appointments.
        /// </summary>
        /// <param name="scheduleDate">The date for which to retrieve appointments.</param>
        /// <returns></returns>
        async public Task<Response<AppointmentViewModel>> GetAppointmentsByDate(DateTime scheduleDate)
        {
            const string apiUrl = baseRoute + "GetAppointmentsByDate";
            var param = new NameValueCollection { { "scheduleDate", scheduleDate.ToShortDateString() } };
            var model = await communicationManager.GetAsync<Response<AppointmentModel>>(param, apiUrl);
            return model.ToViewModel();
        }

        /// <summary>
        /// Gets the appointments by week
        /// </summary>
        /// <param name="startDate">The week start date for which to retrieve appointments.</param>
        /// <returns></returns>
        async public Task<Response<CalendarViewModel>> GetResourceAppointmentsByWeek(int resourceId, int resourceTypeId, DateTime startDate)
        {
            const string apiUrl = baseRoute + "GetResourceAppointmentsByWeek";
            var param = new NameValueCollection();
            param.Add("resourceId", resourceId.ToString());
            param.Add("resourceTypeId", resourceTypeId.ToString());
            param.Add("startDate", startDate.ToShortDateString());
            var model = await communicationManager.GetAsync<Response<CalendarModel>>(param, apiUrl);
            var viewModel = model.CloneResponse<CalendarViewModel>();
            viewModel.DataItems = model.DataItems.Select(x => CalendarViewModel.ToViewModel(x)).ToList();
            return viewModel;
        }

        /// <summary>
        /// Gets the contacts by appointment ID.
        /// </summary>
        /// <param name="appointmentId">The appointment ID for which to retrieve contacts.</param>
        /// <returns></returns>
        async public Task<Response<AppointmentContactViewModel>> GetContactsByAppointment(long appointmentId)
        {
            const string apiUrl = baseRoute + "GetContactsByAppointment";
            var param = new NameValueCollection { { "appointmentId", appointmentId.ToString() } };
            var model = await communicationManager.GetAsync<Response<AppointmentContactModel>>(param, apiUrl);
            return model.ToViewModel();
        }

        /// <summary>
        /// Get appointment
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<AppointmentViewModel> GetAppointments(long contactId)
        {
            const string apiUrl = baseRoute + "GetAppointments";
            var param = new NameValueCollection { { "contactId", contactId.ToString() } };
            var viewModel = communicationManager.Get<Response<AppointmentModel>>(param, apiUrl);
            return viewModel.ToViewModel();
        }

        /// <summary>
        /// Get the appointment
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        public Response<AppointmentViewModel> GetAppointment(long appointmentId)
        {
            const string apiUrl = baseRoute + "GetAppointment";
            var param = new NameValueCollection { { "appointmentId", appointmentId.ToString() } };
            var response = communicationManager.Get<Response<AppointmentModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the appointment by resource.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        public Response<AppointmentViewModel> GetAppointmentByResource(int resourceId, short resourceTypeId)
        {
            const string apiUrl = baseRoute + "GetAppointmentByResource";

            var param = new NameValueCollection();
            param.Add("resourceId", resourceId.ToString());
            param.Add("resourceTypeID", resourceTypeId.ToString());

            var response = communicationManager.Get<Response<AppointmentModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// GetBlockedTimeAppointments
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        public Response<AppointmentViewModel> GetBlockedTimeAppointments(int resourceId, short resourceTypeId)
        {
            const string apiUrl = baseRoute + "GetBlockedTimeAppointments";

            var param = new NameValueCollection();
            param.Add("resourceId", resourceId.ToString());
            param.Add("resourceTypeID", resourceTypeId.ToString());

            var response = communicationManager.Get<Response<AppointmentModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the appointment resource.
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        async public Task<Response<AppointmentResourceViewModel>> GetAppointmentResource(long appointmentId)
        {
            const string apiUrl = baseRoute + "GetAppointmentResource";
            var param = new NameValueCollection { { "appointmentId", appointmentId.ToString() } };
            var response = await communicationManager.GetAsync<Response<AppointmentResourceModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the appointment resource by contact.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<AppointmentResourceViewModel> GetAppointmentResourceByContact(long contactId)
        {
            const string apiUrl = baseRoute + "GetAppointmentResourceByContact";
            var param = new NameValueCollection { { "contactId", contactId.ToString() } };
            var response = communicationManager.Get<Response<AppointmentResourceModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the length of the appointment.
        /// </summary>
        /// <param name="appointmentTypeID">The appointment type identifier.</param>
        /// <returns></returns>
        public Response<AppointmentLengthViewModel> GetAppointmentLength(long appointmentTypeID)
        {
            const string apiUrl = baseRoute + "GetAppointmentLength";
            var param = new NameValueCollection { { "appointmentTypeID", appointmentTypeID.ToString() } };
            var response = communicationManager.Get<Response<AppointmentLengthModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the type of the appointment.
        /// </summary>
        /// <param name="programId">The program identifier.</param>
        /// <returns></returns>
        public Response<AppointmentTypeViewModel> GetAppointmentType(long programId)
        {
            const string apiUrl = baseRoute + "GetAppointmentType";
            var param = new NameValueCollection { { "programId", programId.ToString() } };
            var response = communicationManager.Get<Response<AppointmentTypeModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Adds the appointment
        /// </summary>
        /// <param name="appointment">appointment</param>
        /// <returns></returns>
        public Response<AppointmentViewModel> AddAppointment(AppointmentViewModel appointment)
        {
            const string apiUrl = baseRoute + "AddAppointment";
            return
                communicationManager.Post<AppointmentModel, Response<AppointmentModel>>(appointment.ToModel(), apiUrl)
                    .ToViewModel();
        }

        /// <summary>
        /// Adds the appointment contact.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        public Response<AppointmentContactViewModel> AddAppointmentContact(AppointmentContactViewModel appointmentContact)
        {
            const string apiUrl = baseRoute + "AddAppointmentContact";
            return
                communicationManager.Post<AppointmentContactModel, Response<AppointmentContactModel>>(appointmentContact.ToModel(), apiUrl)
                    .ToViewModel();
        }

        /// <summary>
        /// Adds the appointment resource.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        public Response<AppointmentResourceViewModel> AddAppointmentResource(List<AppointmentResourceViewModel> appointmentResourceDetails)
        {
            const string apiUrl = baseRoute + "AddAppointmentResource";

            var appointmentResourceDetailsRequest = new List<AppointmentResourceModel>();

            appointmentResourceDetails.ForEach(delegate(AppointmentResourceViewModel item)
            {
                appointmentResourceDetailsRequest.Add(item.ToModel());
            });

            return
                communicationManager.Post<List<AppointmentResourceModel>, Response<AppointmentResourceModel>>(appointmentResourceDetailsRequest, apiUrl)
                    .ToViewModel();
        }

        /// <summary>
        /// Updates the appointment
        /// </summary>
        /// <param name="appointment">appointment</param>
        /// <returns></returns>
        public Response<AppointmentViewModel> UpdateAppointment(AppointmentViewModel appointment)
        {
            const string apiUrl = baseRoute + "UpdateAppointment";
            return
                communicationManager.Put<AppointmentModel, Response<AppointmentModel>>(appointment.ToModel(), apiUrl)
                    .ToViewModel();
        }

        /// <summary>
        /// Updates the appointment contact.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        public Response<AppointmentContactViewModel> UpdateAppointmentContact(AppointmentContactViewModel appointmentContact)
        {
            const string apiUrl = baseRoute + "UpdateAppointmentContact";
            return
                communicationManager.Put<AppointmentContactModel, Response<AppointmentContactModel>>(appointmentContact.ToModel(), apiUrl)
                    .ToViewModel();
        }

        /// <summary>
        /// Updates the appointment resource.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        public Response<AppointmentResourceViewModel> UpdateAppointmentResource(List<AppointmentResourceViewModel> appointmentResourceDetails)
        {
            const string apiUrl = baseRoute + "UpdateAppointmentResource";

            var appointmentResourceDetailsRequest = new List<AppointmentResourceModel>();

            appointmentResourceDetails.ForEach(delegate(AppointmentResourceViewModel item)
            {
                appointmentResourceDetailsRequest.Add(item.ToModel());
            });
            return
                communicationManager.Put<List<AppointmentResourceModel>, Response<AppointmentResourceModel>>(appointmentResourceDetailsRequest, apiUrl)
                    .ToViewModel();
        }

        /// <summary>
        /// Deletes appointment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<AppointmentViewModel> DeleteAppointment(long id, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteAppointment";
            var requestId = new NameValueCollection { { "id", id.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Delete<Response<AppointmentViewModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Deletes appointments by recurrene
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<AppointmentViewModel> DeleteAppointmentsByRecurrence(long id)
        {
            const string apiUrl = baseRoute + "DeleteAppointmentsByRecurrence";
            var requestId = new NameValueCollection { { "id", id.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Delete<Response<AppointmentViewModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Add appt note
        /// </summary>
        /// <param name="appointmentNote"></param>
        /// <returns></returns>
        public Response<AppointmentNoteViewModel> AddAppointmentNote(AppointmentNoteViewModel appointmentNote)
        {
            const string apiUrl = baseRoute + "AddAppointmentNote";
            return communicationManager.Post<AppointmentNoteModel, Response<AppointmentNoteModel>>(appointmentNote.ToModel(), apiUrl).ToViewModel(); 
        }


        /// <summary>
        /// Get appt note
        /// </summary>
        /// <param name="appointmentID"></param>
        /// <param name="resourceID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public Response<AppointmentNoteViewModel> GetAppointmentNote(long appointmentID, long? contactID, long? groupID, long? userID)
        {
            const string apiUrl = baseRoute + "GetAppointmentNote";
            var param = new NameValueCollection { 
                { "appointmentID", appointmentID.ToString(CultureInfo.InvariantCulture) },
                { "contactID", contactID.ToString() },
                { "groupID", groupID.ToString() },
                { "userID", userID.ToString() }
            };
            var response = communicationManager.Get<Response<AppointmentNoteModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Get appt note
        /// </summary>
        /// <param name="appointmentID"></param>
        /// <param name="contactID"></param>
        /// <param name="groupID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<Response<AppointmentNoteViewModel>> GetAppointmentNoteAsync(long appointmentID, long? contactID, long? groupID, long? userID)
        {
            const string apiUrl = baseRoute + "GetAppointmentNote";
            var param = new NameValueCollection { 
                { "appointmentID", appointmentID.ToString(CultureInfo.InvariantCulture) },
                { "contactID", contactID.ToString() },
                { "groupID", groupID.ToString() },
                { "userID", userID.ToString() }
            };

            return (await communicationManager.GetAsync<Response<AppointmentNoteModel>>(param, apiUrl)).ToViewModel();
        }

        /// <summary>
        /// Delete appt note
        /// </summary>
        /// <param name="appointmentNoteID"></param>
        /// <returns></returns>
        public Response<AppointmentNoteViewModel> DeleteAppointmentNote(long appointmentNoteID, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteAppointmentNote";
            var requestXMLValueNvc = new NameValueCollection
            {
                { "appointmentNoteID", appointmentNoteID.ToString(CultureInfo.InvariantCulture) },
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return communicationManager.Delete<Response<AppointmentNoteViewModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// UPdate appt note
        /// </summary>
        /// <param name="appointmentNote"></param>
        /// <returns></returns>
        public Response<AppointmentNoteViewModel> UpdateAppointmentNote(AppointmentNoteViewModel appointmentNote)
        {
            const string apiUrl = baseRoute + "UpdateAppointmentNote";
            return
                communicationManager.Put<AppointmentNoteModel, Response<AppointmentNoteModel>>(appointmentNote.ToModel(), apiUrl)
                    .ToViewModel();
        }

        /// <summary>
        /// Update the appointment resource no show flag.
        /// </summary>
        /// <param name="appointmentResource"></param>
        /// <returns></returns>
        public Response<AppointmentResourceViewModel> UpdateAppointmentNoShow(AppointmentResourceViewModel appointmentResource)
        {
            const string apiUrl = baseRoute + "UpdateAppointmentNoShow";
            return
                communicationManager.Put<AppointmentResourceModel, Response<AppointmentResourceModel>>(appointmentResource.ToModel(), apiUrl)
                    .ToViewModel();
        }

        /// <summary>
        /// Add appt status detail
        /// </summary>
        /// <param name="appointmentStatus"></param>
        /// <returns></returns>
        public Response<AppointmentStatusDetailViewModel> AddAppointmentStatusDetail(AppointmentStatusDetailViewModel appointmentStatus)
        {
            const string apiUrl = baseRoute + "AddAppointmentStatusDetail";
            return
                communicationManager.Post<AppointmentStatusDetailModel, Response<AppointmentStatusDetailModel>>(appointmentStatus.ToModel(), apiUrl)
                    .ToViewModel();
        }

        /// <summary>
        /// Update appt status detail
        /// </summary>
        /// <param name="appointmentStatus"></param>
        /// <returns></returns>
        public Response<AppointmentStatusDetailViewModel> UpdateAppointmentStatusDetail(AppointmentStatusDetailViewModel appointmentStatus)
        {
            const string apiUrl = baseRoute + "UpdateAppointmentStatusDetail";
            return
                communicationManager.Put<AppointmentStatusDetailModel, Response<AppointmentStatusDetailModel>>(appointmentStatus.ToModel(), apiUrl)
                    .ToViewModel();
        }

        /// <summary>
        /// Get appt status detail
        /// </summary>
        /// <param name="appointmentResourceID"></param>
        /// <returns></returns>
        public Response<AppointmentStatusDetailViewModel> GetAppointmentStatusDetail(long appointmentResourceID)
        {
            const string apiUrl = baseRoute + "GetAppointmentStatusDetail";
            var param = new NameValueCollection { { "AppointmentResourceID", appointmentResourceID.ToString() } };
            var response = communicationManager.Get<Response<AppointmentStatusDetailModel>>(param, apiUrl);
            return response.ToViewModel();
        }


        public Response<AppointmentResourceViewModel> DeleteAppointmentResource(long id, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteAppointmentResource";
            var requestId = new NameValueCollection { { "id", id.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Delete<Response<AppointmentResourceViewModel>>(requestId, apiUrl);
        }

        public Response<AppointmentViewModel> CancelAppointment(AppointmentViewModel model)
        {
            const string apiUrl = baseRoute + "CancelAppointment";
            return
                communicationManager.Put<AppointmentModel, Response<AppointmentModel>>(model.ToModel(), apiUrl)
                    .ToViewModel();
        }

    }
}
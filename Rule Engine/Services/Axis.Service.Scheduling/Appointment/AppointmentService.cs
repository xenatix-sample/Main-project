using System;
using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Security;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;

namespace Axis.Service.Scheduling
{
    /// <summary>
    /// Appointment service class to call web api methods
    /// </summary>
    public class AppointmentService : IAppointmentService
    {
        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "appointment/";

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentService" /> class.
        /// </summary>
        public AppointmentService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Gets the appointments.
        /// </summary>
        /// <param name="scheduleDate">The date for which to retrieve appointments.</param>
        /// <returns></returns>
        async public Task<Response<AppointmentModel>> GetAppointmentsByDate(DateTime scheduleDate)
        {
            const string apiUrl = BaseRoute + "GetAppointmentsByDate";
            var requestXMLValueNvc = new NameValueCollection { { "scheduleDate", scheduleDate.ToShortDateString() } };
            return await _communicationManager.GetAsync<Response<AppointmentModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Gets the appointments by week
        /// </summary>
        /// <param name="startDate">The week start date for which to retrieve appointments.</param>
        /// <returns></returns>
        async public Task<Response<CalendarModel>> GetResourceAppointmentsByWeek(int resourceId, int resourceTypeId, DateTime startDate)
        {
            const string apiUrl = BaseRoute + "GetResourceAppointmentsByWeek";
            var param = new NameValueCollection();
            param.Add("resourceId", resourceId.ToString());
            param.Add("resourceTypeId", resourceTypeId.ToString());
            param.Add("startDate", startDate.ToShortDateString());
            return await _communicationManager.GetAsync<Response<CalendarModel>>(param, apiUrl);
        }

        /// <summary>
        /// Gets the contacts by appointment ID.
        /// </summary>
        /// <param name="appointmentId">The appointment ID for which to retrieve contacts.</param>
        /// <returns></returns>
        async public Task<Response<AppointmentContactModel>> GetContactsByAppointment(long appointmentId)
        {
            const string apiUrl = BaseRoute + "GetContactsByAppointment";
            var requestXMLValueNvc = new NameValueCollection { { "appointmentId", appointmentId.ToString() } };
            return await _communicationManager.GetAsync<Response<AppointmentContactModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Gets the appointments.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<AppointmentModel> GetAppointments(long contactId)
        {
            const string apiUrl = BaseRoute + "GetAppointments";
            var requestXMLValueNvc = new NameValueCollection { { "contactId", contactId.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<AppointmentModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Get the appointment
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        public Response<AppointmentModel> GetAppointment(long appointmentId)
        {
            const string apiUrl = BaseRoute + "GetAppointment";
            var requestXMLValueNvc = new NameValueCollection { { "appointmentId", appointmentId.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<AppointmentModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Gets the appointment by resource.
        /// </summary>
        /// <param name="resourceID">The resource identifier.</param>
        /// <param name="resourceTypeID">The resource type identifier.</param>
        /// <returns></returns>
        public Response<AppointmentModel> GetAppointmentByResource(int resourceId, short resourceTypeId)
        {
            const string apiUrl = BaseRoute + "GetAppointmentByResource";
            var requestXMLValueNvc = new NameValueCollection();
            requestXMLValueNvc.Add("resourceId", resourceId.ToString(CultureInfo.InvariantCulture));
            requestXMLValueNvc.Add("resourceTypeID", resourceTypeId.ToString(CultureInfo.InvariantCulture));

            return _communicationManager.Get<Response<AppointmentModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Gets the appointment resource.
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        async public Task<Response<AppointmentResourceModel>> GetAppointmentResource(long appointmentId)
        {
            const string apiUrl = BaseRoute + "GetAppointmentResource";
            var requestXMLValueNvc = new NameValueCollection { { "appointmentId", appointmentId.ToString(CultureInfo.InvariantCulture) } };
            return await _communicationManager.GetAsync<Response<AppointmentResourceModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Gets the appointment resource by contact.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<AppointmentResourceModel> GetAppointmentResourceByContact(long contactId)
        {
            const string apiUrl = BaseRoute + "GetAppointmentResourceByContact";
            var requestXMLValueNvc = new NameValueCollection { { "contactId", contactId.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<AppointmentResourceModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Gets the length of the appointment.
        /// </summary>
        /// <param name="appointmentTypeID">The appointment type identifier.</param>
        /// <returns></returns>
        public Response<AppointmentLengthModel> GetAppointmentLength(long appointmentTypeID)
        {
            const string apiUrl = BaseRoute + "GetAppointmentLength";
            var requestXMLValueNvc = new NameValueCollection { { "appointmentTypeID", appointmentTypeID.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<AppointmentLengthModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Gets the type of the appointment.
        /// </summary>
        /// <param name="programId">The program identifier.</param>
        /// <returns></returns>
        public Response<AppointmentTypeModel> GetAppointmentType(long programId)
        {
            const string apiUrl = BaseRoute + "GetAppointmentType";
            var requestXMLValueNvc = new NameValueCollection { { "programId", programId.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<AppointmentTypeModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Adds the appointment
        /// </summary>
        /// <param name="appointment">appointment</param>
        /// <returns></returns>
        public Response<AppointmentModel> AddAppointment(AppointmentModel appointment)
        {
            const string apiUrl = BaseRoute + "AddAppointment";
            return _communicationManager.Post<AppointmentModel, Response<AppointmentModel>>(appointment, apiUrl);
        }

        /// <summary>
        /// Adds the appointment contact.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        public Response<AppointmentContactModel> AddAppointmentContact(AppointmentContactModel appointmentContact)
        {
            const string apiUrl = BaseRoute + "AddAppointmentContact";
            return _communicationManager.Post<AppointmentContactModel, Response<AppointmentContactModel>>(appointmentContact, apiUrl);
        }

        /// <summary>
        /// Adds the appointment resource.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        public Response<AppointmentResourceModel> AddAppointmentResource(List<AppointmentResourceModel> appointmentResourceDetails)
        {
            const string apiUrl = BaseRoute + "AddAppointmentResource";
            return _communicationManager.Post<List<AppointmentResourceModel>, Response<AppointmentResourceModel>>(appointmentResourceDetails, apiUrl);
        }

        /// <summary>
        /// Update the appointment
        /// </summary>
        /// <param name="appointment">appointment</param>
        /// <returns></returns>
        public Response<AppointmentModel> UpdateAppointment(AppointmentModel appointment)
        {
            const string apiUrl = BaseRoute + "UpdateAppointment";
            return _communicationManager.Put<AppointmentModel, Response<AppointmentModel>>(appointment, apiUrl);
        }

        /// <summary>
        /// Updates the appointment contact.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        public Response<AppointmentContactModel> UpdateAppointmentContact(AppointmentContactModel appointmentContact)
        {
            const string apiUrl = BaseRoute + "UpdateAppointmentContact";
            return _communicationManager.Put<AppointmentContactModel, Response<AppointmentContactModel>>(appointmentContact, apiUrl);
        }

        /// <summary>
        /// Updates the appointment resource.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        public Response<AppointmentResourceModel> UpdateAppointmentResource(List<AppointmentResourceModel> appointmentResourceDetails)
        {
            const string apiUrl = BaseRoute + "UpdateAppointmentResource";
            return _communicationManager.Put<List<AppointmentResourceModel>, Response<AppointmentResourceModel>>(appointmentResourceDetails, apiUrl);
        }

        /// <summary>
        /// Delete the appointment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<AppointmentModel> DeleteAppointment(long id, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteAppointment";
            var requestXMLValueNvc = new NameValueCollection
            {
                { "Id", id.ToString(CultureInfo.InvariantCulture) },
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return _communicationManager.Delete<Response<AppointmentModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Delete the appointment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<AppointmentModel> DeleteAppointmentsByRecurrence(long id)
        {
            const string apiUrl = BaseRoute + "DeleteAppointmentsByRecurrence";
            var requestXMLValueNvc = new NameValueCollection
            {
                { "Id", id.ToString(CultureInfo.InvariantCulture) }
            };
            return _communicationManager.Delete<Response<AppointmentModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Add Appt note
        /// </summary>
        /// <param name="appointmentNote"></param>
        /// <returns></returns>
        public Response<AppointmentNoteModel> AddAppointmentNote(AppointmentNoteModel appointmentNote)
        {
            const string apiUrl = BaseRoute + "AddAppointmentNote";
            return _communicationManager.Post<AppointmentNoteModel, Response<AppointmentNoteModel>>(appointmentNote, apiUrl);
        }

        /// <summary>
        /// Get appointment note
        /// </summary>
        /// <param name="appointmentID"></param>
        /// <param name="resourceID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public Response<AppointmentNoteModel> GetAppointmentNote(long appointmentID, long? contactID, long? groupID, long? userID)
        {
            const string apiUrl = BaseRoute + "GetAppointmentNote";
            var param = new NameValueCollection { 
                { "appointmentID", appointmentID.ToString(CultureInfo.InvariantCulture) },
                { "contactID", contactID.ToString() },
                { "groupID", groupID.ToString() },
                { "userID", userID.ToString() }
            };
            return _communicationManager.Get<Response<AppointmentNoteModel>>(param, apiUrl);
        }

        /// <summary>
        /// Delete appt note
        /// </summary>
        /// <param name="appointmentNoteID"></param>
        /// <returns></returns>
        public Response<AppointmentNoteModel> DeleteAppointmentNote(long appointmentNoteID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteAppointmentNote";
            var requestXMLValueNvc = new NameValueCollection
            {
                { "appointmentNoteID", appointmentNoteID.ToString(CultureInfo.InvariantCulture) },
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return _communicationManager.Delete<Response<AppointmentNoteModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Update appt note
        /// </summary>
        /// <param name="appointmentNote"></param>
        /// <returns></returns>
        public Response<AppointmentNoteModel> UpdateAppointmentNote(AppointmentNoteModel appointmentNote)
        {
            const string apiUrl = BaseRoute + "UpdateAppointmentNote";
            return _communicationManager.Put<AppointmentNoteModel, Response<AppointmentNoteModel>>(appointmentNote, apiUrl);
        }

        /// <summary>
        /// Update the appt resource no show flag.
        /// </summary>
        /// <param name="appointmentResource"></param>
        /// <returns></returns>
        public Response<AppointmentResourceModel> UpdateAppointmentNoShow(AppointmentResourceModel appointmentResource)
        {
            const string apiUrl = BaseRoute + "UpdateAppointmentNoShow";
            return _communicationManager.Post<AppointmentResourceModel, Response<AppointmentResourceModel>>(appointmentResource, apiUrl);
        }

        /// <summary>
        /// Get appt status
        /// </summary>
        /// <returns></returns>
        public Response<AppointmentStatusModel> GetAppointmentStatus()
        {
            const string apiUrl = BaseRoute + "GetAppointmentStatus";
            return _communicationManager.Get<Response<AppointmentStatusModel>>(apiUrl);
        }

        /// <summary>
        /// Add appt status detail
        /// </summary>
        /// <param name="appointmentStatus"></param>
        /// <returns></returns>
        public Response<AppointmentStatusDetailModel> AddAppointmentStatusDetail(AppointmentStatusDetailModel appointmentStatus)
        {
            const string apiUrl = BaseRoute + "AddAppointmentStatusDetail";
            return _communicationManager.Post<AppointmentStatusDetailModel, Response<AppointmentStatusDetailModel>>(appointmentStatus, apiUrl);
        }

        /// <summary>
        /// Update appt status detail
        /// </summary>
        /// <param name="appointmentStatus"></param>
        /// <returns></returns>
        public Response<AppointmentStatusDetailModel> UpdateAppointmentStatusDetail(AppointmentStatusDetailModel appointmentStatus)
        {
            const string apiUrl = BaseRoute + "UpdateAppointmentStatusDetail";
            return _communicationManager.Put<AppointmentStatusDetailModel, Response<AppointmentStatusDetailModel>>(appointmentStatus, apiUrl);
        }

        /// <summary>
        ///  Get appt status detail
        /// </summary>
        /// <param name="appointmentResourceID"></param>
        /// <returns></returns>
        public Response<AppointmentStatusDetailModel> GetAppointmentStatusDetail(int appointmentResourceID)
        {
            const string apiUrl = BaseRoute + "GetAppointmentStatusDetail";
            var param = new NameValueCollection { 
                { "AppointmentResourceID", appointmentResourceID.ToString(CultureInfo.InvariantCulture) }
            };
            return _communicationManager.Get<Response<AppointmentStatusDetailModel>>(param, apiUrl);
        }

        /// <summary>
        /// DeleteAppointmentResource
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<AppointmentResourceModel> DeleteAppointmentResource(long id, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteAppointmentResource";
            var requestXMLValueNvc = new NameValueCollection
            {
                {"id", id.ToString(CultureInfo.InvariantCulture) },
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return _communicationManager.Delete<Response<AppointmentResourceModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Cancels the appointment
        /// </summary>
        /// <param name="appointment">appointment</param>
        /// <returns></returns>
        public Response<AppointmentModel> CancelAppointment(AppointmentModel appointment)
        {
            const string apiUrl = BaseRoute + "CancelAppointment";
            return _communicationManager.Put<AppointmentModel, Response<AppointmentModel>>(appointment, apiUrl);
        }

        /// <summary>
        /// Gets the block time appts.
        /// </summary>
        /// <param name="resourceID">The resource identifier.</param>
        /// <param name="resourceTypeID">The resource type identifier.</param>
        /// <returns></returns>
        public Response<AppointmentModel> GetBlockedTimeAppointments(int resourceId, short resourceTypeId)
        {
            const string apiUrl = BaseRoute + "GetBlockedTimeAppointments";
            var requestXMLValueNvc = new NameValueCollection();
            requestXMLValueNvc.Add("resourceId", resourceId.ToString(CultureInfo.InvariantCulture));
            requestXMLValueNvc.Add("resourceTypeID", resourceTypeId.ToString(CultureInfo.InvariantCulture));

            var tmp = _communicationManager.Get<Response<AppointmentModel>>(requestXMLValueNvc, apiUrl);
            return tmp;
        }
    }
}
using System;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Scheduling;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Scheduling
{
    /// <summary>
    ///
    /// </summary>
    public class AppointmentController : BaseApiController
    {
        /// <summary>
        /// The appointment data provider
        /// </summary>
        private readonly IAppointmentDataProvider appointmentDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentController"/> class.
        /// </summary>
        /// <param name="appointmentDataProvider">The appointment data provider.</param>
        public AppointmentController(IAppointmentDataProvider appointmentDataProvider)
        {
            this.appointmentDataProvider = appointmentDataProvider;
        }

        /// <summary>
        /// Gets the appointments by date.
        /// </summary>
        /// <param name="scheduleDate">The date for which to retrieve appointments.</param>
        /// <returns></returns>
        [HttpGet]
        async public Task<IHttpActionResult> GetAppointmentsByDate(DateTime scheduleDate)
        {
            return new HttpResult<Response<AppointmentModel>>(await appointmentDataProvider.GetAppointmentsByDate(scheduleDate), Request);
        }

        /// <summary>
        /// Gets the appointments by week
        /// </summary>
        /// <param name="startDate">The week start date for which to retrieve appointments.</param>
        /// <returns></returns>
        [HttpGet]
        async public Task<IHttpActionResult> GetResourceAppointmentsByWeek(int resourceId, int resourceTypeId, DateTime startDate)
        {
            return new HttpResult<Response<CalendarModel>>(await appointmentDataProvider.GetResourceAppointmentsByWeek(resourceId, resourceTypeId, startDate), Request);
        }

        /// <summary>
        /// Gets the contacts by appointment ID.
        /// </summary>
        /// <param name="appointmentId">The appointment ID for which to retrieve contacts.</param>
        /// <returns></returns>
        [HttpGet]
        async public Task<IHttpActionResult> GetContactsByAppointment(long appointmentId)
        {
            return new HttpResult<Response<AppointmentContactModel>>(await appointmentDataProvider.GetContactsByAppointment(appointmentId), Request);
        }

        /// <summary>
        /// Gets the appointments for contact
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAppointments(long contactId)
        {
            return new HttpResult<Response<AppointmentModel>>(appointmentDataProvider.GetAppointments(contactId), Request);
        }

        /// <summary>
        /// Gets the appointment.
        /// </summary>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAppointment(long appointmentId)
        {
            return new HttpResult<Response<AppointmentModel>>(appointmentDataProvider.GetAppointment(appointmentId), Request);
        }

        /// <summary>
        /// Gets the appointment by resource.
        /// </summary>
        /// <param name="resourceID">The resource identifier.</param>
        /// <param name="resourceTypeID">The resource type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAppointmentByResource(int resourceID, short resourceTypeID)
        {
            return new HttpResult<Response<AppointmentModel>>(appointmentDataProvider.GetAppointmentByResource(resourceID, resourceTypeID), Request);
        }

        /// <summary>
        /// Gets the appointment resource.
        /// </summary>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <returns></returns>
        [HttpGet]
        async public Task<IHttpActionResult> GetAppointmentResource(long appointmentId)
        {
            return new HttpResult<Response<AppointmentResourceModel>>(await appointmentDataProvider.GetAppointmentResource(appointmentId), Request);
        }

        /// <summary>
        /// Gets the appointment resource by contact.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAppointmentResourceByContact(long contactId)
        {
            return new HttpResult<Response<AppointmentResourceModel>>(appointmentDataProvider.GetAppointmentResourceByContact(contactId), Request);
        }

        /// <summary>
        /// Gets the length of the appointment.
        /// </summary>
        /// <param name="appointmentTypeID">The appointment type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAppointmentLength(long appointmentTypeID)
        {
            return new HttpResult<Response<AppointmentLengthModel>>(appointmentDataProvider.GetAppointmentLength(appointmentTypeID), Request);
        }

        /// <summary>
        /// Gets the type of the appointment.
        /// </summary>
        /// <param name="programId">The program identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAppointmentType(long programId)
        {
            return new HttpResult<Response<AppointmentTypeModel>>(appointmentDataProvider.GetAppointmentType(programId), Request);
        }

        /// <summary>
        /// Get appt status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAppointmentStatus()
        {
            return new HttpResult<Response<AppointmentStatusModel>>(appointmentDataProvider.GetAppointmentStatus(), Request);
        }

        /// <summary>
        /// Adds the appointment.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddAppointment(AppointmentModel appointment)
        {
            return new HttpResult<Response<AppointmentModel>>(appointmentDataProvider.AddAppointment(appointment), Request);
        }

        /// <summary>
        /// Adds the appointment contact.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddAppointmentContact(AppointmentContactModel appointmentContact)
        {
            return new HttpResult<Response<AppointmentContactModel>>(appointmentDataProvider.AddAppointmentContact(appointmentContact), Request);
        }

        /// <summary>
        /// Add appt status detail
        /// </summary>
        /// <param name="appointmentStatusDetail"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddAppointmentStatusDetail(AppointmentStatusDetailModel appointmentStatusDetail)
        {
            return new HttpResult<Response<AppointmentStatusDetailModel>>(appointmentDataProvider.AddAppointmentStatusDetail(appointmentStatusDetail), Request);
        }

        /// <summary>
        /// Update appt status detail
        /// </summary>
        /// <param name="appointmentStatusDetail"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateAppointmentStatusDetail(AppointmentStatusDetailModel appointmentStatusDetail)
        {
            return new HttpResult<Response<AppointmentStatusDetailModel>>(appointmentDataProvider.UpdateAppointmentStatusDetail(appointmentStatusDetail), Request);
        }

        /// <summary>
        /// Get appt status detail
        /// </summary>
        /// <param name="appointmentStatusDetail"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAppointmentStatusDetail(int appointmentResourceID)
        {
            return new HttpResult<Response<AppointmentStatusDetailModel>>(appointmentDataProvider.GetAppointmentStatusDetail(appointmentResourceID), Request);
        }

        /// <summary>
        /// Adds the appointment resource.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddAppointmentResource(List<AppointmentResourceModel> appointmentResourceDetails)
        {
            return new HttpResult<Response<AppointmentResourceModel>>(appointmentDataProvider.AddAppointmentResource(appointmentResourceDetails), Request);
        }

        /// <summary>
        /// Updates the appointment.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateAppointment(AppointmentModel appointment)
        {
            return new HttpResult<Response<AppointmentModel>>(appointmentDataProvider.UpdateAppointment(appointment), Request);
        }

        /// <summary>
        /// Updates the appointment contact.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateAppointmentContact(AppointmentContactModel appointmentContact)
        {
            return new HttpResult<Response<AppointmentContactModel>>(appointmentDataProvider.UpdateAppointmentContact(appointmentContact), Request);
        }

        /// <summary>
        /// Updates the appointment resource.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateAppointmentResource(List<AppointmentResourceModel> appointmentResourceDetails)
        {
            return new HttpResult<Response<AppointmentResourceModel>>(appointmentDataProvider.UpdateAppointmentResource(appointmentResourceDetails), Request);
        }

        /// <summary>
        /// Deletes the appointment.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteAppointment(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<AppointmentModel>>(appointmentDataProvider.DeleteAppointment(id, modifiedOn), Request);
        }

        /// <summary>
        /// Deletes the appointment by recurrence.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteAppointmentsByRecurrence(long id)
        {
            return new HttpResult<Response<AppointmentModel>>(appointmentDataProvider.DeleteAppointmentsByRecurrence(id), Request);
        }

        /// <summary>
        /// Adds the appointment note.
        /// </summary>
        /// <param name="appointmentNote">The appointment note.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddAppointmentNote(AppointmentNoteModel appointmentNote)
        {
            return new HttpResult<Response<AppointmentNoteModel>>(appointmentDataProvider.AddAppointmentNote(appointmentNote), Request);
        }

        /// <summary>
        /// Updates the appointment.
        /// </summary>
        /// <param name="appointmentNote">The appointment note.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateAppointmentNote(AppointmentNoteModel appointmentNote)
        {
            return new HttpResult<Response<AppointmentNoteModel>>(appointmentDataProvider.UpdateAppointmentNote(appointmentNote), Request);
        }

        /// <summary>
        /// Get appt note
        /// </summary>
        /// <param name="appointmentID"></param>
        /// <param name="resourceID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAppointmentNote(long appointmentID, long? contactID, long? groupID, long? userID)
        {
            return new HttpResult<Response<AppointmentNoteModel>>(appointmentDataProvider.GetAppointmentNote(appointmentID, contactID, groupID, userID), Request);
        }

        /// <summary>
        /// Deletes the appointment note.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteAppointmentNote(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<AppointmentNoteModel>>(appointmentDataProvider.DeleteAppointmentNote(id, modifiedOn), Request);
        }

        /// <summary>
        /// DeleteAppointmentResource
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteAppointmentResource(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<AppointmentResourceModel>>(appointmentDataProvider.DeleteAppointmentResource(id, modifiedOn), Request);
        }

        /// <summary>
        /// Update appt no show flag
        /// </summary>
        /// <param name="appointmentResource"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateAppointmentNoShow(AppointmentResourceModel appointmentResource)
        {
            return new HttpResult<Response<AppointmentResourceModel>>(appointmentDataProvider.UpdateAppointmentNoShow(appointmentResource), Request);
        }

        /// <summary>
        /// Updates the appointment.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult CancelAppointment(AppointmentModel appointment)
        {
            return new HttpResult<Response<AppointmentModel>>(appointmentDataProvider.CancelAppointment(appointment), Request);
        }

        /// <summary>
        /// Gets block time appts.
        /// </summary>
        /// <param name="resourceID">The resource identifier.</param>
        /// <param name="resourceTypeID">The resource type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetBlockedTimeAppointments(int resourceID, short resourceTypeID)
        {
            return new HttpResult<Response<AppointmentModel>>(appointmentDataProvider.GetBlockedTimeAppointments(resourceID, resourceTypeID), Request);
        }
    }
}
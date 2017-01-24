using System;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Service.Scheduling;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Axis.RuleEngine.Scheduling
{
    /// <summary>
    ///
    /// </summary>
    public class AppointmentRuleEngine : IAppointmentRuleEngine
    {
        /// <summary>
        /// The appointment service
        /// </summary>
        private readonly IAppointmentService appointmentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentRuleEngine"/> class.
        /// </summary>
        /// <param name="appointmentService">The appointment service.</param>
        public AppointmentRuleEngine(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        /// <summary>
        /// Gets the appointments.
        /// </summary>
        /// <param name="scheduleDate">The date for which to retrieve appointments.</param>
        /// <returns></returns>
        async public Task<Response<AppointmentModel>> GetAppointmentsByDate(DateTime scheduleDate)
        {
            return await appointmentService.GetAppointmentsByDate(scheduleDate);
        }

        /// <summary>
        /// Gets the appointments by week
        /// </summary>
        /// <param name="startDate">The week start date for which to retrieve appointments.</param>
        /// <returns></returns>
        async public Task<Response<CalendarModel>> GetResourceAppointmentsByWeek(int resourceId, int resourceTypeId, DateTime startDate)
        {
            return await appointmentService.GetResourceAppointmentsByWeek(resourceId, resourceTypeId, startDate);
        }

        /// <summary>
        /// Gets the contacts by appointment ID.
        /// </summary>
        /// <param name="appointmentId">The appointment ID for which to retrieve contacts.</param>
        /// <returns></returns>
        async public Task<Response<AppointmentContactModel>> GetContactsByAppointment(long appointmentId)
        {
            return await appointmentService.GetContactsByAppointment(appointmentId);
        }

        /// <summary>
        /// Gets the appointments.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<AppointmentModel> GetAppointments(long contactId)
        {
            return appointmentService.GetAppointments(contactId);
        }

        /// <summary>
        /// Get the appointment
        /// </summary>
        /// <param name="appintmentId">The appintment identifier.</param>
        /// <returns></returns>
        public Response<AppointmentModel> GetAppointment(long appointmentId)
        {
            return appointmentService.GetAppointment(appointmentId);
        }

        /// <summary>
        /// Gets the appointment by resource.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        public Response<AppointmentModel> GetAppointmentByResource(int resourceId, short resourceTypeId)
        {
            return appointmentService.GetAppointmentByResource(resourceId, resourceTypeId);
        }

        /// <summary>
        /// Gets the appointment resource.
        /// </summary>
        /// <param name="appintmentId">The appintment identifier.</param>
        /// <returns></returns>
        async public Task<Response<AppointmentResourceModel>> GetAppointmentResource(long appointmentId)
        {
            return await appointmentService.GetAppointmentResource(appointmentId);
        }

        /// <summary>
        /// Gets the appointment resource by contact.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<AppointmentResourceModel> GetAppointmentResourceByContact(long contactId)
        {
            return appointmentService.GetAppointmentResourceByContact(contactId);
        }

        /// <summary>
        /// Gets the length of the appointment.
        /// </summary>
        /// <param name="appointmentTypeID">The appointment type identifier.</param>
        /// <returns></returns>
        public Response<AppointmentLengthModel> GetAppointmentLength(long appointmentTypeID)
        {
            return appointmentService.GetAppointmentLength(appointmentTypeID);
        }

        /// <summary>
        /// Gets the type of the appointment.
        /// </summary>
        /// <param name="programId">The program identifier.</param>
        /// <returns></returns>
        public Response<AppointmentTypeModel> GetAppointmentType(long programId)
        {
            return appointmentService.GetAppointmentType(programId);
        }

        /// <summary>
        /// Adds the appointment
        /// </summary>
        /// <param name="appointment">appointment</param>
        /// <returns></returns>
        public Response<AppointmentModel> AddAppointment(AppointmentModel appointment)
        {
            return appointmentService.AddAppointment(appointment);
        }

        /// <summary>
        /// Adds the appointment contact.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        public Response<AppointmentContactModel> AddAppointmentContact(AppointmentContactModel appointmentContact)
        {
            return appointmentService.AddAppointmentContact(appointmentContact);
        }

        /// <summary>
        /// Adds the appointment resource.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        public Response<AppointmentResourceModel> AddAppointmentResource(List<AppointmentResourceModel> appointmentResourceDetails)
        {
            return appointmentService.AddAppointmentResource(appointmentResourceDetails);
        }

        /// <summary>
        /// Update the appointment
        /// </summary>
        /// <param name="appointment">appointment</param>
        /// <returns></returns>
        public Response<AppointmentModel> UpdateAppointment(AppointmentModel appointment)
        {
            return appointmentService.UpdateAppointment(appointment);
        }

        /// <summary>
        /// Updates the appointment contact.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        public Response<AppointmentContactModel> UpdateAppointmentContact(AppointmentContactModel appointmentContact)
        {
            return appointmentService.UpdateAppointmentContact(appointmentContact);
        }

        /// <summary>
        /// Updates the appointment resource.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        public Response<AppointmentResourceModel> UpdateAppointmentResource(List<AppointmentResourceModel> appointmentResourceDetails)
        {
            return appointmentService.UpdateAppointmentResource(appointmentResourceDetails);
        }

        /// <summary>
        /// Delete the appointment
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public Response<AppointmentModel> DeleteAppointment(long id, DateTime modifiedOn)
        {
            return appointmentService.DeleteAppointment(id, modifiedOn);
        }
        /// <summary>
        /// Delete the appointment by recurrence
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public Response<AppointmentModel> DeleteAppointmentsByRecurrence(long id)
        {
            return appointmentService.DeleteAppointmentsByRecurrence(id);
        }


        /// <summary>
        /// Add appt note
        /// </summary>
        /// <param name="appointmentNote"></param>
        /// <returns></returns>
        public Response<AppointmentNoteModel> AddAppointmentNote(AppointmentNoteModel appointmentNote)
        {
            return appointmentService.AddAppointmentNote(appointmentNote);
        }


        /// <summary>
        /// Get appt note
        /// </summary>
        /// <param name="appointmentID"></param>
        /// <param name="resourceID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public Response<AppointmentNoteModel> GetAppointmentNote(long appointmentID, long? contactID, long? groupID, long? userID)
        {
            return appointmentService.GetAppointmentNote(appointmentID, contactID, groupID, userID);
        }

        /// <summary>
        /// Delete appt note
        /// </summary>
        /// <param name="appointmentNoteID"></param>
        /// <returns></returns>
        public Response<AppointmentNoteModel> DeleteAppointmentNote(long appointmentNoteID, DateTime modifiedOn)
        {
            return appointmentService.DeleteAppointmentNote(appointmentNoteID, modifiedOn);
        }

        /// <summary>
        /// UPdate appt note
        /// </summary>
        /// <param name="appointmentNote"></param>
        /// <returns></returns>
        public Response<AppointmentNoteModel> UpdateAppointmentNote(AppointmentNoteModel appointmentNote)
        {
            return appointmentService.UpdateAppointmentNote(appointmentNote);
        }

        /// <summary>
        /// Update the appointment resource no show flag.
        /// </summary>
        /// <param name="appointmentResource"></param>
        /// <returns></returns>
        public Response<AppointmentResourceModel> UpdateAppointmentNoShow(AppointmentResourceModel appointmentResource)
        {
            return appointmentService.UpdateAppointmentNoShow(appointmentResource);
        }

        /// <summary>
        /// Add appt status detail
        /// </summary>
        /// <param name="appointmentStatus"></param>
        /// <returns></returns>
        public Response<AppointmentStatusDetailModel> AddAppointmentStatusDetail(AppointmentStatusDetailModel appointmentStatus)
        {
            return appointmentService.AddAppointmentStatusDetail(appointmentStatus);
        }

        /// <summary>
        /// Update appt status detail
        /// </summary>
        /// <param name="appointmentStatus"></param>
        /// <returns></returns>
        public Response<AppointmentStatusDetailModel> UpdateAppointmentStatusDetail(AppointmentStatusDetailModel appointmentStatus)
        {
            return appointmentService.UpdateAppointmentStatusDetail(appointmentStatus);
        }

        /// <summary>
        /// Get appt status detail
        /// </summary>
        /// <param name="appointmentResourceID"></param>
        /// <returns></returns>
        public Response<AppointmentStatusDetailModel> GetAppointmentStatusDetail(int appointmentResourceID)
        {
            return appointmentService.GetAppointmentStatusDetail(appointmentResourceID);
        }

        /// <summary>
        /// Get appt status
        /// </summary>
        /// <returns></returns>
        public Response<AppointmentStatusModel> GetAppointmentStatus()
        {
            return appointmentService.GetAppointmentStatus();
        }

        /// <summary>
        /// DeleteAppointmentResource
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<AppointmentResourceModel> DeleteAppointmentResource(long id, DateTime modifiedOn)
        {
            return appointmentService.DeleteAppointmentResource(id, modifiedOn);
        }

        /// <summary>
        /// Cancels the appointment
        /// </summary>
        /// <param name="appointment">appointment</param>
        /// <returns></returns>
        public Response<AppointmentModel> CancelAppointment(AppointmentModel appointment)
        {
            return appointmentService.CancelAppointment(appointment);
        }

        /// <summary>
        /// GetBlockedTimeAppointments
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="resourceTypeId"></param>
        /// <returns></returns>
        public Response<AppointmentModel> GetBlockedTimeAppointments(int resourceId, short resourceTypeId)
        {
            return appointmentService.GetBlockedTimeAppointments(resourceId, resourceTypeId);
        }

    }

}
using Axis.Model.Common;
using Axis.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Axis.DataProvider.Scheduling
{
    /// <summary>
    /// Interface for appointment
    /// </summary>
    public interface IAppointmentDataProvider
    {
        /// <summary>
        /// Gets the appointments by date
        /// </summary>
        /// <param name="scheduleDate">The date for which to retrieve appointments.</param>
        /// <returns></returns>
        Task<Response<AppointmentModel>> GetAppointmentsByDate(DateTime scheduleDate);

        /// <summary>
        /// Gets the appointments by week
        /// </summary>
        /// <param name="startDate">The week start date for which to retrieve appointments.</param>
        /// <returns></returns>
        Task<Response<CalendarModel>> GetResourceAppointmentsByWeek(int resourceId, int resourceTypeId, DateTime startDate);

        /// <summary>
        /// Gets the contacts by appointment ID.
        /// </summary>
        /// <param name="appointmentId">The appointment ID for which to retrieve contacts.</param>
        /// <returns></returns>
        Task<Response<AppointmentContactModel>> GetContactsByAppointment(long appointmentId);

        /// <summary>
        /// Gets the appointments.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        Response<AppointmentModel> GetAppointments(long contactId);

        /// <summary>
        /// Get the appointment
        /// </summary>
        /// <param name="appiontmentId">The appintment identifier.</param>
        /// <returns></returns>
        Response<AppointmentModel> GetAppointment(long appiontmentId);

        /// <summary>
        /// Gets the appointment by resource.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        Response<AppointmentModel> GetAppointmentByResource(int resourceId, short resourceTypeId);

        /// <summary>
        /// Gets the appointment by resource.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        Response<AppointmentModel> GetAppointmentByResourceID(int resourceId, short resourceTypeId);

        /// <summary>
        /// Gets the appointment resource.
        /// </summary>
        /// <param name="appiontmentId">The appintment identifier.</param>
        /// <returns></returns>
        Task<Response<AppointmentResourceModel>> GetAppointmentResource(long appiontmentId);

        /// <summary>
        /// Gets the appointment resource by contact.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        Response<AppointmentResourceModel> GetAppointmentResourceByContact(long contactId);

        /// <summary>
        /// Gets the length of the appointment.
        /// </summary>
        /// <param name="appointmentTypeID">The appointment type identifier.</param>
        /// <returns></returns>
        Response<AppointmentLengthModel> GetAppointmentLength(long appointmentTypeID);

        /// <summary>
        /// Gets the type of the appointment.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        Response<AppointmentTypeModel> GetAppointmentType(long programId);

        /// <summary>
        /// Gets the type of the appointmenttype Mapping.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        Response<AppointmentTypeModel> GetAppointmentTypeMapping();

        /// <summary>
        /// Get appt status
        /// </summary>
        /// <returns></returns>
        Response<AppointmentStatusModel> GetAppointmentStatus();

        /// <summary>
        /// Adds the appointment
        /// </summary>
        /// <param name="appointment">appointment</param>
        /// <returns></returns>
        Response<AppointmentModel> AddAppointment(AppointmentModel appointment);

        /// <summary>
        /// Adds the appointment contact.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        Response<AppointmentContactModel> AddAppointmentContact(AppointmentContactModel appointmentContact);

        /// <summary>
        /// Add appt status detail
        /// </summary>
        /// <param name="appointmentStatus"></param>
        /// <returns></returns>
        Response<AppointmentStatusDetailModel> AddAppointmentStatusDetail(AppointmentStatusDetailModel appointmentStatus);

        /// <summary>
        /// Update appt status detail
        /// </summary>
        /// <param name="appointmentStatus"></param>
        /// <returns></returns>
        Response<AppointmentStatusDetailModel> UpdateAppointmentStatusDetail(AppointmentStatusDetailModel appointmentStatus);

        /// <summary>
        ///  Get appt status detail
        /// </summary>
        /// <param name="appointmentResourceID"></param>
        /// <returns></returns>
        Response<AppointmentStatusDetailModel> GetAppointmentStatusDetail(int appointmentResourceID);

        /// <summary>
        /// Adds the appointment resource.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        Response<AppointmentResourceModel> AddAppointmentResource(List<AppointmentResourceModel> appointmentResourceDetails);

        /// <summary>
        /// Update the appointment
        /// </summary>
        /// <param name="appointment">appointment</param>
        /// <returns></returns>
        Response<AppointmentModel> UpdateAppointment(AppointmentModel appointment);

        /// <summary>
        /// Updates the appointment contact.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        Response<AppointmentContactModel> UpdateAppointmentContact(AppointmentContactModel appointmentContact);

        /// <summary>
        /// Updates the appointment resource.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        Response<AppointmentResourceModel> UpdateAppointmentResource(List<AppointmentResourceModel> appointmentResourceDetails);

        /// <summary>
        /// Delete the appointment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<AppointmentModel> DeleteAppointment(long id, DateTime modifiedOn);

        /// <summary>
        /// Add Appt note
        /// </summary>
        /// <param name="appointmentNote"></param>
        /// <returns></returns>
        Response<AppointmentNoteModel> AddAppointmentNote(AppointmentNoteModel appointmentNote);

        /// <summary>
        /// Get appointment note
        /// </summary>
        /// <param name="appointmentID"></param>
        /// <param name="resourceID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        Response<AppointmentNoteModel> GetAppointmentNote(long appointmentID, long? contactID, long? groupID, long? userID);

        /// <summary>
        /// Delete appt note
        /// </summary>
        /// <param name="appointmentNoteID"></param>
        /// <returns></returns>
        Response<AppointmentNoteModel> DeleteAppointmentNote(long appointmentNoteID, DateTime modifiedOn);

        /// <summary>
        /// Update appt note
        /// </summary>
        /// <param name="appointmentNote"></param>
        /// <returns></returns>
        Response<AppointmentNoteModel> UpdateAppointmentNote(AppointmentNoteModel appointmentNote);

        /// <summary>
        /// Update the appt resource no show flag.
        /// </summary>
        /// <param name="appointmentResource"></param>
        /// <returns></returns>
        Response<AppointmentResourceModel> UpdateAppointmentNoShow(AppointmentResourceModel appointmentResource);

        Response<AppointmentModel> DeleteAppointmentsByRecurrence(long recurrenceID);

        Response<AppointmentResourceModel> DeleteAppointmentResource(long id, DateTime modifiedOn);

        /// <summary>
        /// Cancels the appointment
        /// </summary>
        /// <param name="appointment">appointment</param>
        /// <returns></returns>
        Response<AppointmentModel> CancelAppointment(AppointmentModel appointment);

        Response<AppointmentModel> GetBlockedTimeAppointments(int resourceId, short resourceTypeId);
    }
}
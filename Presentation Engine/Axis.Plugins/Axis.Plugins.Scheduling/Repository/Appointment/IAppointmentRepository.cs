using System;
using Axis.Model.Common;
using Axis.Plugins.Scheduling.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Axis.Plugins.Scheduling.Repository.Appointment
{
    public interface IAppointmentRepository
    {
        /// <summary>
        /// Gets the appointments.
        /// </summary>
        /// <param name="scheduleDate">The date for which to retrieve appointments.</param>
        /// <returns></returns>
        Task<Response<AppointmentViewModel>> GetAppointmentsByDate(DateTime scheduleDate);

        /// <summary>
        /// Gets the appointments by week
        /// </summary>
        /// <param name="startDate">The week start date for which to retrieve appointments.</param>
        /// <returns></returns>
        Task<Response<CalendarViewModel>> GetResourceAppointmentsByWeek(int resourceId, int resourceTypeId, DateTime startDate);

        /// <summary>
        /// Gets the contacts by appointment ID.
        /// </summary>
        /// <param name="appointmentId">The appointment ID for which to retrieve contacts.</param>
        /// <returns></returns>
        Task<Response<AppointmentContactViewModel>> GetContactsByAppointment(long appointmentId);

        Response<AppointmentViewModel> GetAppointments(long contactId);

        /// <summary>
        /// Get the appointment
        /// </summary>
        /// <param name="appintmentId">The appintment identifier.</param>
        /// <returns></returns>
        Response<AppointmentViewModel> GetAppointment(long appointmentId);

        /// <summary>
        /// Gets the appointment by resource.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        Response<AppointmentViewModel> GetAppointmentByResource(int resourceId, short resourceTypeId);

        /// <summary>
        /// GetBlockedTimeAppointments
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="resourceTypeId"></param>
        /// <returns></returns>
        Response<AppointmentViewModel> GetBlockedTimeAppointments(int resourceId, short resourceTypeId);

        /// <summary>
        /// Gets the appointment resource.
        /// </summary>
        /// <param name="appintmentId">The appintment identifier.</param>
        /// <returns></returns>
        Task<Response<AppointmentResourceViewModel>> GetAppointmentResource(long appointmentId);

        /// <summary>
        /// Gets the appointment resource by contact.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        Response<AppointmentResourceViewModel> GetAppointmentResourceByContact(long contactId);

        /// <summary>
        /// Gets the length of the appointment.
        /// </summary>
        /// <param name="appointmentTypeID">The appointment type identifier.</param>
        /// <returns></returns>
        Response<AppointmentLengthViewModel> GetAppointmentLength(long appointmentTypeID);

        /// <summary>
        /// Gets the type of the appointment.
        /// </summary>
        /// <param name="programId">The program identifier.</param>
        /// <returns></returns>
        Response<AppointmentTypeViewModel> GetAppointmentType(long programId);

        /// <summary>
        /// Adds the appointment
        /// </summary>
        /// <param name="appointment">appointment</param>
        /// <returns></returns>
        Response<AppointmentViewModel> AddAppointment(AppointmentViewModel appointment);

        /// <summary>
        /// Adds the appointment contact.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        Response<AppointmentContactViewModel> AddAppointmentContact(AppointmentContactViewModel appointmentContact);

        /// <summary>
        /// Adds the appointment resource.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        Response<AppointmentResourceViewModel> AddAppointmentResource(List<AppointmentResourceViewModel> appointmentResourceDetails);

        /// <summary>
        /// Update the appointment
        /// </summary>
        /// <param name="appointment">appointment</param>
        /// <returns></returns>
        Response<AppointmentViewModel> UpdateAppointment(AppointmentViewModel appointment);

        /// <summary>
        /// Updates the appointment contact.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        Response<AppointmentContactViewModel> UpdateAppointmentContact(AppointmentContactViewModel appointmentContact);

        /// <summary>
        /// Updates the appointment resource.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        Response<AppointmentResourceViewModel> UpdateAppointmentResource(List<AppointmentResourceViewModel> appointmentResourceDetails);

        /// <summary>
        /// Delete the appointment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<AppointmentViewModel> DeleteAppointment(long id, DateTime modifiedOn);

        /// <summary>
        /// Delete the appointment by recurrence
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<AppointmentViewModel> DeleteAppointmentsByRecurrence(long id);

        /// <summary>
        /// Add Appt note
        /// </summary>
        /// <param name="appointmentNote"></param>
        /// <returns></returns>
        Response<AppointmentNoteViewModel> AddAppointmentNote(AppointmentNoteViewModel appointmentNote);

        /// <summary>
        /// Get appointment note
        /// </summary>
        /// <param name="appointmentID"></param>
        /// <param name="resourceID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        Response<AppointmentNoteViewModel> GetAppointmentNote(long appointmentID, long? contactID, long? groupID, long? userID);

        /// <summary>
        /// Get apppointment note
        /// </summary>
        /// <param name="appointmentID"></param>
        /// <param name="contactID"></param>
        /// <param name="groupID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<Response<AppointmentNoteViewModel>> GetAppointmentNoteAsync(long appointmentID, long? contactID,
            long? groupID, long? userID);

        /// <summary>
        /// Delete appt note
        /// </summary>
        /// <param name="appointmentNoteID"></param>
        /// <returns></returns>
        Response<AppointmentNoteViewModel> DeleteAppointmentNote(long appointmentNoteID, DateTime modifiedOn);

        /// <summary>
        /// Update appt note
        /// </summary>
        /// <param name="appointmentNote"></param>
        /// <returns></returns>
        Response<AppointmentNoteViewModel> UpdateAppointmentNote(AppointmentNoteViewModel appointmentNote);

        /// <summary>
        /// Update the appt resource no show flag.
        /// </summary>
        /// <param name="appointmentResource"></param>
        /// <returns></returns>
        Response<AppointmentResourceViewModel> UpdateAppointmentNoShow(AppointmentResourceViewModel appointmentResource);

        /// <summary>
        /// Add appt status detail
        /// </summary>
        /// <param name="appointmentStatus"></param>
        /// <returns></returns>
        Response<AppointmentStatusDetailViewModel> AddAppointmentStatusDetail(AppointmentStatusDetailViewModel appointmentStatus);

        /// <summary>
        /// Update appt status detail
        /// </summary>
        /// <param name="appointmentStatus"></param>
        /// <returns></returns>
        Response<AppointmentStatusDetailViewModel> UpdateAppointmentStatusDetail(AppointmentStatusDetailViewModel appointmentStatus);

        /// <summary>
        ///  Get appt status detail
        /// </summary>
        /// <param name="appointmentResourceID"></param>
        /// <returns></returns>
        Response<AppointmentStatusDetailViewModel> GetAppointmentStatusDetail(long appointmentResourceID);

        /// <summary>
        /// Delete appt resource
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<AppointmentResourceViewModel> DeleteAppointmentResource(long id, DateTime modifiedOn);

        Response<AppointmentViewModel> CancelAppointment(AppointmentViewModel model);
    }
}
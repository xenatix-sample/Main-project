using System;
using Axis.Plugins.Scheduling.Models;
using Axis.Plugins.Scheduling.Repository.Appointment;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Web.Http;
using Axis.Model.Common;

namespace Axis.Plugins.Scheduling.ApiControllers
{
    /// <summary>
    ///
    /// </summary>
    public class AppointmentController : BaseApiController
    {
        /// <summary>
        /// The appointment repository
        /// </summary>
        private readonly IAppointmentRepository appointmentRepository;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="appointmentRepository">The appointment repository.</param>
        public AppointmentController(IAppointmentRepository appointmentRepository)
        {
            this.appointmentRepository = appointmentRepository;
        }

        /// <summary>
        /// Gets the appointments.
        /// </summary>
        /// <param name="scheduleDate">The date for which to retrieve appointments.</param>
        /// <returns></returns>
        [HttpGet]
        async public Task<Response<AppointmentViewModel>> GetAppointmentsByDate(DateTime scheduleDate)
        {
            return await appointmentRepository.GetAppointmentsByDate(scheduleDate);
        }

        /// <summary>
        /// Gets the appointments by week
        /// </summary>
        /// <param name="startDate">The week start date for which to retrieve appointments.</param>
        /// <returns></returns>
        [HttpGet]
        async public Task<Response<CalendarViewModel>> GetResourceAppointmentsByWeek(int resourceId, int resourceTypeId, DateTime startDate)
        {
            return await appointmentRepository.GetResourceAppointmentsByWeek(resourceId, resourceTypeId, startDate);
        }

        /// <summary>
        /// Gets the contacts by appointment ID.
        /// </summary>
        /// <param name="appointmentId">The appointment ID for which to retrieve contacts.</param>
        /// <returns></returns>
        [HttpGet]
        async public Task<Response<AppointmentContactViewModel>> GetContactsByAppointment(long appointmentId)
        {
            return await appointmentRepository.GetContactsByAppointment(appointmentId);
        }

        /// <summary>
        /// Gets appointments
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<AppointmentViewModel> GetAppointments(long contactId)
        {
            return appointmentRepository.GetAppointments(contactId);
        }

        /// <summary>
        /// Gets the appointment.
        /// </summary>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<AppointmentViewModel> GetAppointment(long appointmentId)
        {
            return appointmentRepository.GetAppointment(appointmentId);
        }

        /// <summary>
        /// Gets the appointment by resource.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<AppointmentViewModel> GetAppointmentByResource(int resourceId, short resourceTypeId)
        {
            return appointmentRepository.GetAppointmentByResource(resourceId, resourceTypeId);
        }

        /// <summary>
        /// Gets the appointment by resource.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<AppointmentViewModel> GetBlockedTimeAppointments(int resourceId, short resourceTypeId)
        {
            return appointmentRepository.GetBlockedTimeAppointments(resourceId, resourceTypeId);
        }

        /// <summary>
        /// Gets the appointment resource.
        /// </summary>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <returns></returns>
        [HttpGet]
        async public Task<Response<AppointmentResourceViewModel>> GetAppointmentResource(long appointmentId)
        {
            return await appointmentRepository.GetAppointmentResource(appointmentId);
        }

        /// <summary>
        /// Gets the appointment resource by contact.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<AppointmentResourceViewModel> GetAppointmentResourceByContact(long contactId)
        {
            return appointmentRepository.GetAppointmentResourceByContact(contactId);
        }

        /// <summary>
        /// Gets the length of the appointment.
        /// </summary>
        /// <param name="appointmentTypeID">The appointment type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<AppointmentLengthViewModel> GetAppointmentLength(long? appointmentTypeID)
        {
            return appointmentRepository.GetAppointmentLength(appointmentTypeID ?? 0);
        }

        /// <summary>
        /// Gets the type of the appointment.
        /// </summary>
        /// <param name="programId">The program identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<AppointmentTypeViewModel> GetAppointmentType(long? programId)
        {
            return appointmentRepository.GetAppointmentType(programId ?? 0);
        }

        /// <summary>
        /// Adds appointment
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<AppointmentViewModel> AddAppointment(AppointmentViewModel appointment)
        {
            return appointmentRepository.AddAppointment(appointment);
        }

        /// <summary>
        /// Adds the appointment contact.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<AppointmentContactViewModel> AddAppointmentContact(AppointmentContactViewModel appointmentContact)
        {
            return appointmentRepository.AddAppointmentContact(appointmentContact);
        }

        /// <summary>
        /// Adds the appointment resource.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<AppointmentResourceViewModel> AddAppointmentResource(AppointmentResourceViewModel appointmentResourceDetail)
        {
            List<AppointmentResourceViewModel> appointmentResourceDetails = new List<AppointmentResourceViewModel>();
            appointmentResourceDetails.Add(appointmentResourceDetail);
            return appointmentRepository.AddAppointmentResource(appointmentResourceDetails);
        }

        /// <summary>
        /// Update appointment
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<AppointmentViewModel> UpdateAppointment(AppointmentViewModel appointment)
        {
            return appointmentRepository.UpdateAppointment(appointment);
        }

        /// <summary>
        /// Updates the appointment contact.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<AppointmentContactViewModel> UpdateAppointmentContact(AppointmentContactViewModel appointmentContact)
        {
            return appointmentRepository.UpdateAppointmentContact(appointmentContact);
        }

        /// <summary>
        /// Updates the appointment resource.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<AppointmentResourceViewModel> UpdateAppointmentResource(AppointmentResourceViewModel appointmentResourceDetail)
        {
            List<AppointmentResourceViewModel> appointmentResourceDetails = new List<AppointmentResourceViewModel>();
            appointmentResourceDetails.Add(appointmentResourceDetail);
            return appointmentRepository.UpdateAppointmentResource(appointmentResourceDetails);
        }

        /// <summary>
        /// Delete appointment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public Response<AppointmentViewModel> DeleteAppointment(long id, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return appointmentRepository.DeleteAppointment(id, modifiedOn);
        }

        /// <summary>
        /// Deletes the appointment by recurrence.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public Response<AppointmentViewModel> DeleteAppointmentsByRecurrence(long id)
        {
            return appointmentRepository.DeleteAppointmentsByRecurrence(id);
        }

        /// <summary>
        /// Add appt note
        /// </summary>
        /// <param name="appointmentNote"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<AppointmentNoteViewModel> AddAppointmentNote(AppointmentNoteViewModel appointmentNote)
        {
            return appointmentRepository.AddAppointmentNote(appointmentNote);
        }

        /// <summary>
        /// Update appt note
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns></returns>
        [HttpPut]
        public Response<AppointmentNoteViewModel> UpdateAppointmentNote(AppointmentNoteViewModel appointment)
        {
            return appointmentRepository.UpdateAppointmentNote(appointment);
        }

        /// <summary>
        /// Delete appt note
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public Response<AppointmentNoteViewModel> DeleteAppointmentNote(long id, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return appointmentRepository.DeleteAppointmentNote(id, modifiedOn);
        }

        /// <summary>
        /// Get appt note
        /// </summary>
        /// <param name="appointmentID"></param>
        /// <param name="resourceID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        [HttpGet]
        public Response<AppointmentNoteViewModel> GetAppointmentNote(long appointmentID, long? contactID = null, long? groupID = null, long? userID = null)
        {
            return appointmentRepository.GetAppointmentNote(appointmentID, contactID, groupID, userID);
        }

        /// <summary>
        /// UPdate appt no show flag
        /// </summary>
        /// <param name="appointmentResource"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<AppointmentResourceViewModel> UpdateAppointmentNoShow(AppointmentResourceViewModel appointmentResource)
        {
            return appointmentRepository.UpdateAppointmentNoShow(appointmentResource);
        }

        /// <summary>
        /// Update appt status detail
        /// </summary>
        /// <param name="appointmentStatus"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<AppointmentStatusDetailViewModel> UpdateAppointmentStatusDetail(AppointmentStatusDetailViewModel appointmentStatus)
        {
            return appointmentRepository.UpdateAppointmentStatusDetail(appointmentStatus);
        }

        /// <summary>
        /// Add appt status detail
        /// </summary>
        /// <param name="appointmentStatus"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<AppointmentStatusDetailViewModel> AddAppointmentStatusDetail(AppointmentStatusDetailViewModel appointmentStatus)
        {
            return appointmentRepository.AddAppointmentStatusDetail(appointmentStatus);
        }

        /// <summary>
        /// Get appt status detail
        /// </summary>
        /// <param name="appointmentResourceID"></param>
        /// <returns></returns>
        [HttpGet]
        public Response<AppointmentStatusDetailViewModel> GetAppointmentStatusDetail(long appointmentResourceID)
        {
            return appointmentRepository.GetAppointmentStatusDetail(appointmentResourceID);
        }

        /// <summary>
        /// Delete appt resource
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public Response<AppointmentResourceViewModel> DeleteAppointmentResource(long id, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return appointmentRepository.DeleteAppointmentResource(id, modifiedOn);
        }

        /// <summary>
        /// Cancels the Appointment.
        /// </summary>
        /// <param name="model">AppointmentCancel Model.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<AppointmentViewModel> CancelAppointment(AppointmentViewModel model)
        {
            return appointmentRepository.CancelAppointment(model);
        }

    }
}
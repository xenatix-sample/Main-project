using System;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Scheduling;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Scheduling
{
    /// <summary>
    ///
    /// </summary>
    public class AppointmentController : BaseApiController
    {
        /// <summary>
        /// The appointment rule engine
        /// </summary>
        private readonly IAppointmentRuleEngine appointmentRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentController"/> class.
        /// </summary>
        /// <param name="appointmentRuleEngine">The appointment rule engine.</param>
        public AppointmentController(IAppointmentRuleEngine appointmentRuleEngine)
        {
            this.appointmentRuleEngine = appointmentRuleEngine;
        }

        /// <summary>
        /// Gets the appointments by date.
        /// </summary>
        /// <param name="scheduleDate">The date for which to retrieve appointments.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Read)]
        [HttpGet]
        async public Task<IHttpActionResult> GetAppointmentsByDate(DateTime scheduleDate)
        {
            return new HttpResult<Response<AppointmentModel>>(await appointmentRuleEngine.GetAppointmentsByDate(scheduleDate), Request);
        }

        /// <summary>
        /// Gets the appointments by week
        /// </summary>
        /// <param name="startDate">The week start date for which to retrieve appointments.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Read)]
        [HttpGet]
        async public Task<IHttpActionResult> GetResourceAppointmentsByWeek(int resourceId, int resourceTypeId, DateTime startDate)
        {
            return new HttpResult<Response<CalendarModel>>(await appointmentRuleEngine.GetResourceAppointmentsByWeek(resourceId, resourceTypeId, startDate), Request);
        }

        /// <summary>
        /// Gets the contacts by appointment ID.
        /// </summary>
        /// <param name="appointmentId">The appointment ID for which to retrieve contacts.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Read)]
        [HttpGet]
        async public Task<IHttpActionResult> GetContactsByAppointment(long appointmentId)
        {
            return new HttpResult<Response<AppointmentContactModel>>(await appointmentRuleEngine.GetContactsByAppointment(appointmentId), Request);
        }

        /// <summary>
        /// Get Appointment for contact
        /// </summary>
        /// <param name="contactId">Contact Id</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetAppointments(long contactId)
        {
            return new HttpResult<Response<AppointmentModel>>(appointmentRuleEngine.GetAppointments(contactId), Request);
        }

        /// <summary>
        /// Gets the appointment.
        /// </summary>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetAppointment(long appointmentId)
        {
            return new HttpResult<Response<AppointmentModel>>(appointmentRuleEngine.GetAppointment(appointmentId), Request);
        }

        /// <summary>
        /// Gets the appointment by resource.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAppointmentByResource(int resourceId, short resourceTypeId)
        {
            return new HttpResult<Response<AppointmentModel>>(appointmentRuleEngine.GetAppointmentByResource(resourceId, resourceTypeId), Request);
        }

        /// <summary>
        /// Gets the appointment resource.
        /// </summary>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Read)]
        [HttpGet]
        async public Task<IHttpActionResult> GetAppointmentResource(long appointmentId)
        {
            return new HttpResult<Response<AppointmentResourceModel>>(await appointmentRuleEngine.GetAppointmentResource(appointmentId), Request);
        }

        /// <summary>
        /// Gets the appointment resource by contact.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetAppointmentResourceByContact(long contactId)
        {
            return new HttpResult<Response<AppointmentResourceModel>>(appointmentRuleEngine.GetAppointmentResourceByContact(contactId), Request);
        }

        /// <summary>
        /// Gets the length of the appointment.
        /// </summary>
        /// <param name="appointmentTypeID">The appointment type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAppointmentLength(long appointmentTypeID)
        {
            return new HttpResult<Response<AppointmentLengthModel>>(appointmentRuleEngine.GetAppointmentLength(appointmentTypeID), Request);
        }

        /// <summary>
        /// Gets the type of the appointment.
        /// </summary>
        /// <param name="programId">The program identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAppointmentType(long programId)
        {
            return new HttpResult<Response<AppointmentTypeModel>>(appointmentRuleEngine.GetAppointmentType(programId), Request);
        }

        /// <summary>
        /// Adds the appointment.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddAppointment(AppointmentModel appointment)
        {
            return new HttpResult<Response<AppointmentModel>>(appointmentRuleEngine.AddAppointment(appointment), Request);
        }

        /// <summary>
        /// Adds the appointment contact.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddAppointmentContact(AppointmentContactModel appointmentContact)
        {
            return new HttpResult<Response<AppointmentContactModel>>(appointmentRuleEngine.AddAppointmentContact(appointmentContact), Request);
        }

        /// <summary>
        /// Adds the appointment resource.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddAppointmentResource(List<AppointmentResourceModel> appointmentResourceDetails)
        {
            return new HttpResult<Response<AppointmentResourceModel>>(appointmentRuleEngine.AddAppointmentResource(appointmentResourceDetails), Request);
        }

        /// <summary>
        /// Updates the appointment.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateAppointment(AppointmentModel appointment)
        {
            return new HttpResult<Response<AppointmentModel>>(appointmentRuleEngine.UpdateAppointment(appointment), Request);
        }

        /// <summary>
        /// Updates the appointment contact.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateAppointmentContact(AppointmentContactModel appointmentContact)
        {
            return new HttpResult<Response<AppointmentContactModel>>(appointmentRuleEngine.UpdateAppointmentContact(appointmentContact), Request);
        }

        /// <summary>
        /// Updates the appointment resource.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateAppointmentResource(List<AppointmentResourceModel> appointmentResourceDetails)
        {
            return new HttpResult<Response<AppointmentResourceModel>>(appointmentRuleEngine.UpdateAppointmentResource(appointmentResourceDetails), Request);
        }

        /// <summary>
        /// Deletes the appointment.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteAppointment(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<AppointmentModel>>(appointmentRuleEngine.DeleteAppointment(id, modifiedOn), Request);
        }

        /// <summary>
        /// Deletes the appointment by recurrence.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteAppointmentsByRecurrence(long id)
        {
            return new HttpResult<Response<AppointmentModel>>(appointmentRuleEngine.DeleteAppointmentsByRecurrence(id), Request);
        }

        /// <summary>
        /// Adds the appointment note.
        /// </summary>
        /// <param name="appointmentNote">The appointment note.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddAppointmentNote(AppointmentNoteModel appointmentNote)
        {
            return new HttpResult<Response<AppointmentNoteModel>>(appointmentRuleEngine.AddAppointmentNote(appointmentNote), Request);
        }

        /// <summary>
        /// Updates the appointment.
        /// </summary>
        /// <param name="appointmentNote">The appointment note.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateAppointmentNote(AppointmentNoteModel appointmentNote)
        {
            return new HttpResult<Response<AppointmentNoteModel>>(appointmentRuleEngine.UpdateAppointmentNote(appointmentNote), Request);
        }

        /// <summary>
        /// Get appt note
        /// </summary>
        /// <param name="appointmentID"></param>
        /// <param name="resourceID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetAppointmentNote(long appointmentID, long? contactID, long? groupID, long? userID)
        {
            return new HttpResult<Response<AppointmentNoteModel>>(appointmentRuleEngine.GetAppointmentNote(appointmentID, contactID, groupID, userID), Request);
        }

        /// <summary>
        /// Deletes the appointment note.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteAppointmentNote(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<AppointmentNoteModel>>(appointmentRuleEngine.DeleteAppointmentNote(id, modifiedOn), Request);
        }

        /// <summary>
        /// Delete appt resource
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteAppointmentResource(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<AppointmentResourceModel>>(appointmentRuleEngine.DeleteAppointmentResource(id, modifiedOn), Request);

        }

        /// <summary>
        /// Update appt no show flag
        /// </summary>
        /// <param name="appointmentResource"></param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateAppointmentNoShow(AppointmentResourceModel appointmentResource)
        {
            return new HttpResult<Response<AppointmentResourceModel>>(appointmentRuleEngine.UpdateAppointmentNoShow(appointmentResource), Request);
        }

        /// <summary>
        /// Add appt status detail
        /// </summary>
        /// <param name="appointmentStatusDetail"></param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddAppointmentStatusDetail(AppointmentStatusDetailModel appointmentStatusDetail)
        {
            return new HttpResult<Response<AppointmentStatusDetailModel>>(appointmentRuleEngine.AddAppointmentStatusDetail(appointmentStatusDetail), Request);
        }

        /// <summary>
        /// Update appt status detail
        /// </summary>
        /// <param name="appointmentStatusDetail"></param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateAppointmentStatusDetail(AppointmentStatusDetailModel appointmentStatusDetail)
        {
            return new HttpResult<Response<AppointmentStatusDetailModel>>(appointmentRuleEngine.UpdateAppointmentStatusDetail(appointmentStatusDetail), Request);
        }

        /// <summary>
        /// Get appt status detail
        /// </summary>
        /// <param name="appointmentStatusDetail"></param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetAppointmentStatusDetail(int appointmentResourceID)
        {
            return new HttpResult<Response<AppointmentStatusDetailModel>>(appointmentRuleEngine.GetAppointmentStatusDetail(appointmentResourceID), Request);
        }

        /// <summary>
        /// Get appt status
        /// </summary>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetAppointmentStatus()
        {
            return new HttpResult<Response<AppointmentStatusModel>>(appointmentRuleEngine.GetAppointmentStatus(), Request);
        }

        /// <summary>
        /// Updates the appointment.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Delete)]
        [HttpPut]
        public IHttpActionResult CancelAppointment(AppointmentModel appointment)
        {
            return new HttpResult<Response<AppointmentModel>>(appointmentRuleEngine.CancelAppointment(appointment), Request);
        }

        /// <summary>
        /// Gets block time appts.
        /// </summary>
        /// <param name="resourceID">The resource identifier.</param>
        /// <param name="resourceTypeID">The resource type identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetBlockedTimeAppointments(int resourceID, short resourceTypeID)
        {
            return new HttpResult<Response<AppointmentModel>>(appointmentRuleEngine.GetBlockedTimeAppointments(resourceID, resourceTypeID), Request);
        }
    }
}
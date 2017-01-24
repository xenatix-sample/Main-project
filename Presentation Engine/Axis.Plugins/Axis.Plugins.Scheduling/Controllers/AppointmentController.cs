using Axis.Plugins.Scheduling.Models;
using Axis.Plugins.Scheduling.Repository.Appointment;
using Axis.PresentationEngine.Helpers.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Axis.Plugins.Scheduling.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class AppointmentController : BaseController
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

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Mains this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult MainScheduling()
        {
            return View();
        }

        /// <summary>
        /// Schedules this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Schedule()
        {
            return View();
        }
        public ActionResult Calendar()
        {
            return View();
        }

        /// <summary>
        /// Registration Screen Navigation Menu Method
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Navigation()
        {
            return View("../Shared/_SchedulingNavigation");
        }

        /// <summary>
        /// Gets the appointments.
        /// </summary>
        /// <param name="scheduleDate">The date for which to retrieve appointments.</param>
        /// <returns></returns>
        [HttpGet]
        async public Task<JsonResult> GetAppointmentsByDate(DateTime scheduleDate)
        {
            return Json(await appointmentRepository.GetAppointmentsByDate(scheduleDate), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the appointments by week
        /// </summary>
        /// <param name="startDate">The week start date for which to retrieve appointments.</param>
        /// <returns></returns>
        [HttpGet]
        async public Task<JsonResult> GetResourceAppointmentsByWeek(int resourceId, int resourceTypeId, DateTime startDate)
        {
            return Json(await appointmentRepository.GetResourceAppointmentsByWeek(resourceId, resourceTypeId, startDate), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the contacts by appointment ID.
        /// </summary>
        /// <param name="appointmentId">The appointment ID for which to retrieve contacts.</param>
        /// <returns></returns>
        [HttpGet]
        async public Task<JsonResult> GetContactsByAppointment(long appointmentId)
        {
            return Json(await appointmentRepository.GetContactsByAppointment(appointmentId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets appointments
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAppointments(long contactId)
        {
            return Json(appointmentRepository.GetAppointments(contactId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the appointment.
        /// </summary>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAppointment(long appointmentId)
        {
            return Json(appointmentRepository.GetAppointment(appointmentId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the appointment by resource.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAppointmentByResource(int resourceId, short resourceTypeId)
        {
            return Json(appointmentRepository.GetAppointmentByResource(resourceId, resourceTypeId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetBlockedTimeAppointments
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetBlockedTimeAppointments(int resourceId, short resourceTypeId)
        {
            return Json(appointmentRepository.GetBlockedTimeAppointments(resourceId, resourceTypeId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the appointment resource.
        /// </summary>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <returns></returns>
        [HttpGet]
        async public Task<JsonResult> GetAppointmentResource(long appointmentId)
        {
            return Json(await appointmentRepository.GetAppointmentResource(appointmentId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the appointment resource by contact.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public JsonResult GetAppointmentResourceByContact(long contactId)
        {
            return Json(appointmentRepository.GetAppointmentResourceByContact(contactId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the length of the appointment.
        /// </summary>
        /// <param name="appointmentTypeID">The appointment type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAppointmentLength(long? appointmentTypeID)
        {
            return Json(appointmentRepository.GetAppointmentLength(appointmentTypeID ?? 0), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the type of the appointment.
        /// </summary>
        /// <param name="programId">The program identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAppointmentType(long? programId)
        {
            return Json(appointmentRepository.GetAppointmentType(programId ?? 0), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds appointment
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddAppointment(AppointmentViewModel appointment)
        {
            return Json(appointmentRepository.AddAppointment(appointment));
        }

        /// <summary>
        /// Adds the appointment contact.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddAppointmentContact(AppointmentContactViewModel appointmentContact)
        {
            return Json(appointmentRepository.AddAppointmentContact(appointmentContact));
        }

        /// <summary>
        /// Adds the appointment resource.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddAppointmentResource(AppointmentResourceViewModel appointmentResourceDetail)
        {
            List<AppointmentResourceViewModel> appointmentResourceDetails = new List<AppointmentResourceViewModel>();
            appointmentResourceDetails.Add(appointmentResourceDetail);
            return Json(appointmentRepository.AddAppointmentResource(appointmentResourceDetails));
        }

        /// <summary>
        /// Update appointment
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns></returns>
        [HttpPut]
        public JsonResult UpdateAppointment(AppointmentViewModel appointment)
        {
            return Json(appointmentRepository.UpdateAppointment(appointment));
        }

        /// <summary>
        /// Updates the appointment contact.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        [HttpPut]
        public JsonResult UpdateAppointmentContact(AppointmentContactViewModel appointmentContact)
        {
            return Json(appointmentRepository.UpdateAppointmentContact(appointmentContact));
        }

        /// <summary>
        /// Updates the appointment resource.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        [HttpPut]
        public JsonResult UpdateAppointmentResource(AppointmentResourceViewModel appointmentResourceDetail)
        {
            List<AppointmentResourceViewModel> appointmentResourceDetails = new List<AppointmentResourceViewModel>();
            appointmentResourceDetails.Add(appointmentResourceDetail);
            return Json(appointmentRepository.UpdateAppointmentResource(appointmentResourceDetails));
        }

        /// <summary>
        /// Delete appointment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public JsonResult DeleteAppointment(long id, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return Json(appointmentRepository.DeleteAppointment(id, modifiedOn));
        }
    }
}
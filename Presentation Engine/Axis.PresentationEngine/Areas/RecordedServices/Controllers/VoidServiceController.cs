using Axis.Model.Common;
using Axis.PresentationEngine.Areas.RecordedServices.Models;
using Axis.PresentationEngine.Areas.RecordedServices.Repository;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.RecordedServices.Controllers
{
    public class VoidServiceController : BaseController
    {
        /// <summary>
        /// The void service password repository
        /// </summary>
        private IVoidServiceRepository _voidServiceRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForgotPasswordController" /> class.
        /// </summary>
        /// <param name="forgotPasswordRepository">The forgot password repository.</param>
        public VoidServiceController(IVoidServiceRepository voidServiceRepository)
        {
            this._voidServiceRepository = voidServiceRepository;
        }

         // GET: VoidService/Index
        /// <summary>
        /// Initiates the view.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// void recorded service.
        /// </summary>
        /// <param name="voidService">The voidService.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VoidRecordedService(VoidServiceViewModel voidService)
        {
            return Json(_voidServiceRepository.VoidService(voidService), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// void recorded service.
        /// </summary>
        /// <param name="voidService">The voidService.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VoidServiceCallCenter(VoidServiceViewModel voidService)
        {
            return Json(_voidServiceRepository.VoidServiceCallCenter(voidService), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get void recorded service.
        /// </summary>
        /// <param name="serviceRecordingID">The serviceRecordingID.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetVoidRecordedService(int serviceRecordingID)
        {
            return Json(_voidServiceRepository.GetVoidService(serviceRecordingID), JsonRequestBehavior.AllowGet);
        }
	}
}
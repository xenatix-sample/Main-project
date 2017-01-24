using System;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Admin.Respository;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.PresentationEngine.Helpers.Filters;
using System.Web.Mvc;
using Axis.PresentationEngine.Areas.Admin.Model;
using System.Linq;

namespace Axis.PresentationEngine.Areas.Admin.Controllers
{
    public class AdminController : BaseController
    {
        #region Class Variables

        private IAdminRepository adminRepository;

        #endregion

        #region Constructors

        public AdminController()
        {

        }

        public AdminController(IAdminRepository adminRepository)
        {
            this.adminRepository = adminRepository;
        }

        #endregion

        #region ActionResults

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        [Authorization(AllowAnonymous = false)]
        public ActionResult SiteAdminMain()
        {
            return View();
        }

        [Authorization(AllowAnonymous = false)]
        public ActionResult SiteAdminIndex()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetUsers(string userSch)
        {
            return Json(adminRepository.GetUsers(userSch), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorization(AllowAnonymous = false)]
        public JsonResult AddUser(UserViewModel user)
        {
            return Json(adminRepository.AddUser(user), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorization(AllowAnonymous = false)]
        public JsonResult UpdateUser(UserViewModel user)
        {
            return Json(adminRepository.UpdateUser(user), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorization(AllowAnonymous = false)]
        public JsonResult RemoveUser(UserViewModel user)
        {
            return Json(adminRepository.RemoveUser(user), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorization(AllowAnonymous = false)]
        public JsonResult ActivateUser(UserViewModel user)
        {
            return Json(adminRepository.ActivateUser(user), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorization(AllowAnonymous = false)]
        public JsonResult UnlockUser(UserViewModel user)
        {
            return Json(adminRepository.UnlockUser(user), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorization(AllowAnonymous = false)]
        public JsonResult GetUserRoles(int userID)
        {
            return Json(adminRepository.GetUserRoles(userID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorization(AllowAnonymous = false)]
        public JsonResult GetUserCredentials(UserViewModel userView)
        {
            return Json(adminRepository.GetUserCredentials(userView), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorization(AllowAnonymous = false)]
        public JsonResult AddUserCredential(UserCredentialViewModel userCredentialView)
        {
            return Json(adminRepository.AddUserCredential(userCredentialView), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorization(AllowAnonymous = false)]
        public JsonResult UpdateUserCredential(UserCredentialViewModel userCredentialView)
        {
            return Json(adminRepository.UpdateUserCredential(userCredentialView), JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        [Authorization(AllowAnonymous = false)]
        public JsonResult DeleteUserCredential(int userCredentialID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return Json(adminRepository.DeleteUserCredential(userCredentialID, modifiedOn), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorization(AllowAnonymous = false)]
        public JsonResult VerifySecurityDetails(ResetPasswordViewModel resetPassword)
        {
            resetPassword.RequestorIPAddress = Request.UserHostAddress;
            var response = adminRepository.VerifySecurityDetails(resetPassword);
            if (response.ResultCode == 0 && response.DataItems != null && response.DataItems.Count > 0)
            {
                TempData["email"] = response.DataItems.FirstOrDefault().Email;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
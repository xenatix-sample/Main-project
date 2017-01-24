using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Account.Repository.ForgotPassword;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.PresentationEngine.Helpers.Filters;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.Account.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class ForgotPasswordController : BaseController
    {
        /// <summary>
        /// The forgot password repository
        /// </summary>
        private IForgotPasswordRepository forgotPasswordRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForgotPasswordController" /> class.
        /// </summary>
        /// <param name="forgotPasswordRepository">The forgot password repository.</param>
        public ForgotPasswordController(IForgotPasswordRepository forgotPasswordRepository)
        {
            this.forgotPasswordRepository = forgotPasswordRepository;
        }

        // GET: Account/ForgotPassword
        /// <summary>
        /// Initiates the reset password.
        /// </summary>
        /// <returns></returns>
        [Authorization(AllowAnonymous = true)]
        public ActionResult InitiateResetPassword()
        {
            return View();
        }

        /// <summary>
        /// Sends the reset link.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorization(AllowAnonymous = true)]
        public JsonResult SendResetLink(string email)
        {
            var requestorIPAddress = Request.UserHostAddress;
            var response = forgotPasswordRepository.SendResetLink(email, requestorIPAddress);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Verifies the reset identifier.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorization(AllowAnonymous = true)]
        public JsonResult VerifyResetIdentifier(ResetPasswordViewModel resetPassword)
        {
            resetPassword.RequestorIPAddress = Request.UserHostAddress;
            var response = forgotPasswordRepository.VerifyResetIdentifier(resetPassword);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Securities the question.
        /// </summary>
        /// <param name="resetIdentifier">The reset link.</param>
        /// <returns></returns>
        [Authorization(AllowAnonymous = true)]
        public ActionResult SecurityQuestion(string resetIdentifier)
        {
            return View();
        }

        /// <summary>
        /// Verifies the security details.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorization(AllowAnonymous = true)]
        public JsonResult VerifySecurityDetails(ResetPasswordViewModel resetPassword)
        {
            resetPassword.RequestorIPAddress = Request.UserHostAddress;
            var response = forgotPasswordRepository.VerifySecurityDetails(resetPassword);
            if (response.ResultCode == 0 && response.DataItems != null && response.DataItems.Count > 0)
            {
                TempData["email"] = response.DataItems.FirstOrDefault().Email;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Secondaries the authentication.
        /// </summary>
        /// <returns></returns>
        [Authorization(AllowAnonymous = true)]
        public ActionResult SecondaryAuthentication()
        {
            return View();
        }

        /// <summary>
        /// Verifies the otp.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorization(AllowAnonymous = true)]
        public JsonResult VerifyOTP(ResetPasswordViewModel resetPassword)
        {
            var response = forgotPasswordRepository.VerifyOTP(resetPassword);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="resetIdentifier">The reset identifier.</param>
        /// <returns></returns>
        [Authorization(AllowAnonymous = true)]
        public ActionResult ResetPassword(string resetIdentifier)
        {
            return View();
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorization(AllowAnonymous = true)]
        public JsonResult ResetPassword(ResetPasswordViewModel resetPassword)
        {
            resetPassword.RequestorIPAddress = Request.UserHostAddress;
            resetPassword.Email = Convert.ToString(TempData["email"]);
            if (Session["UserID"] != null)
            {
                resetPassword.UserID = Convert.ToInt32(Session["UserID"]);
                resetPassword.ResetIdentifier = Guid.NewGuid();
            }
            else resetPassword.UserID = 0;
            var response = forgotPasswordRepository.ResetPassword(resetPassword);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the security questions.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorization(AllowAnonymous = true)]
        public JsonResult GetSecurityQuestions()
        {
            var response = forgotPasswordRepository.GetSecurityQuestions();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
using Axis.Constant;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.SecurityQuestion;
using Axis.RuleEngine.Account.ForgotPassword;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class ForgotPasswordController : BaseApiController
    {
        /// <summary>
        /// The forgot password rule engine
        /// </summary>
        private IForgotPasswordRuleEngine forgotPasswordRuleEngine = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForgotPasswordController" /> class.
        /// </summary>
        /// <param name="forgotPasswordRuleEngine">The forgot password rule engine.</param>
        public ForgotPasswordController(IForgotPasswordRuleEngine forgotPasswordRuleEngine)
        {
            this.forgotPasswordRuleEngine = forgotPasswordRuleEngine;
        }

        /// <summary>
        /// Sends the reset link.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="requestorIPAddress">The requestor ip address.</param>
        /// <returns></returns>
        // Removed the Authorization Attribute As When a User requests for Password Reset there is no Authorization Permission for the User
        [HttpPost]
        public IHttpActionResult SendResetLink(string email, string requestorIPAddress)
        {
            return new HttpResult<Response<ResetPasswordModel>>(forgotPasswordRuleEngine.SendResetLink(email, requestorIPAddress), Request);
        }

        /// <summary>
        /// Verifies the reset identifier.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult VerifyResetIdentifier(ResetPasswordModel resetPassword)
        {
            return new HttpResult<Response<ResetPasswordModel>>(forgotPasswordRuleEngine.VerifyResetIdentifier(resetPassword), Request);
        }

        /// <summary>
        /// Verifies the security details.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult VerifySecurityDetails(ResetPasswordModel resetPassword)
        {
            return new HttpResult<Response<ResetPasswordModel>>(forgotPasswordRuleEngine.VerifySecurityDetails(resetPassword), Request);
        }

        /// <summary>
        /// Verifies the otp.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult VerifyOTP(ResetPasswordModel resetPassword)
        {
            return new HttpResult<Response<ResetPasswordModel>>(forgotPasswordRuleEngine.VerifyOTP(resetPassword), Request);
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ResetPassword(ResetPasswordModel resetPassword)
        {
            return new HttpResult<Response<ResetPasswordModel>>(forgotPasswordRuleEngine.ResetPassword(resetPassword), Request);
        }

        /// <summary>
        /// Gets the security questions.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetSecurityQuestions()
        {
            return new HttpResult<Response<SecurityQuestionModel>>(forgotPasswordRuleEngine.GetSecurityQuestions(), Request);
        }
    }
}
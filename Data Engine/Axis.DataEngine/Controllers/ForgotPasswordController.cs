using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Account;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.SecurityQuestion;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class ForgotPasswordController : BaseApiController
    {
        /// <summary>
        /// The forgot password data provider
        /// </summary>
        private IForgotPasswordDataProvider forgotPasswordDataProvider = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForgotPasswordController" /> class.
        /// </summary>
        /// <param name="forgotPasswordDataProvider">The forgot password data provider.</param>
        public ForgotPasswordController(IForgotPasswordDataProvider forgotPasswordDataProvider)
        {
            this.forgotPasswordDataProvider = forgotPasswordDataProvider;
        }

        /// <summary>
        /// Sends the reset link.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="requestorIPAddress">The requestor ip address.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SendResetLink(string email, string requestorIPAddress)
        {
            var sendResetPasswordResult = forgotPasswordDataProvider.SendResetLink(email, requestorIPAddress);

            return new HttpResult<Response<ResetPasswordModel>>(sendResetPasswordResult, Request);
        }

        /// <summary>
        /// Verifies the reset identifier.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        public IHttpActionResult VerifyResetIdentifier(ResetPasswordModel resetPassword)
        {
            var verifyResetIdentifierResult = forgotPasswordDataProvider.VerifyResetIdentifier(resetPassword);

            return new HttpResult<Response<ResetPasswordModel>>(verifyResetIdentifierResult, Request);
        }

        /// <summary>
        /// Verifies the security details.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult VerifySecurityDetails(ResetPasswordModel resetPassword)
        {
            var verifySecurityDetailsResult = forgotPasswordDataProvider.VerifySecurityDetails(resetPassword);

            return new HttpResult<Response<ResetPasswordModel>>(verifySecurityDetailsResult, Request);
        }

        /// <summary>
        /// Verifies the otp.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult VerifyOTP(ResetPasswordModel resetPassword)
        {
            var verifyOTPResult = forgotPasswordDataProvider.VerifyOTP(resetPassword);

            return new HttpResult<Response<ResetPasswordModel>>(verifyOTPResult, Request);
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ResetPassword(ResetPasswordModel resetPassword)
        {
            var resetPasswordResult = forgotPasswordDataProvider.ResetPassword(resetPassword);

            return new HttpResult<Response<ResetPasswordModel>>(resetPasswordResult, Request);
        }

        /// <summary>
        /// Gets the security questions.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetSecurityQuestions()
        {
            var resetPasswordResult = forgotPasswordDataProvider.GetSecurityQuestions();

            return new HttpResult<Response<SecurityQuestionModel>>(resetPasswordResult, Request);
        }
    }
}
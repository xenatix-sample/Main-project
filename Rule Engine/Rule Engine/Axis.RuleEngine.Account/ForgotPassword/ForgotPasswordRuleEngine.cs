using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.SecurityQuestion;
using Axis.Service.Account.ForgotPassword;

namespace Axis.RuleEngine.Account.ForgotPassword
{
    /// <summary>
    ///
    /// </summary>
    public class ForgotPasswordRuleEngine : IForgotPasswordRuleEngine
    {
        /// <summary>
        /// The forgot password service
        /// </summary>
        private IForgotPasswordService forgotPasswordService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForgotPasswordRuleEngine" /> class.
        /// </summary>
        /// <param name="forgotPasswordService">The forgot password service.</param>
        public ForgotPasswordRuleEngine(IForgotPasswordService forgotPasswordService)
        {
            this.forgotPasswordService = forgotPasswordService;
        }

        /// <summary>
        /// Sends the reset link.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="requestorIPAddress">The requestor ip address.</param>
        /// <returns></returns>
        public Response<ResetPasswordModel> SendResetLink(string email, string requestorIPAddress)
        {
            return forgotPasswordService.SendResetLink(email, requestorIPAddress);
        }

        /// <summary>
        /// Verifies the reset identifier.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        public Response<ResetPasswordModel> VerifyResetIdentifier(ResetPasswordModel resetPassword)
        {
            return forgotPasswordService.VerifyResetIdentifier(resetPassword);
        }

        /// <summary>
        /// Verifies the security details.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        public Response<ResetPasswordModel> VerifySecurityDetails(ResetPasswordModel resetPassword)
        {
            return forgotPasswordService.VerifySecurityDetails(resetPassword);
        }

        /// <summary>
        /// Verifies the otp.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        public Response<ResetPasswordModel> VerifyOTP(ResetPasswordModel resetPassword)
        {
            return forgotPasswordService.VerifyOTP(resetPassword);
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        public Response<ResetPasswordModel> ResetPassword(ResetPasswordModel resetPassword)
        {
            return forgotPasswordService.ResetPassword(resetPassword);
        }

        /// <summary>
        /// Gets the security questions.
        /// </summary>
        /// <returns></returns>
        public Response<SecurityQuestionModel> GetSecurityQuestions()
        {
            return forgotPasswordService.GetSecurityQuestions();
        }
    }
}
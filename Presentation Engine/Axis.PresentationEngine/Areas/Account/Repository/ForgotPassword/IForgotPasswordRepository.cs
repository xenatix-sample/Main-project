using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Helpers.Model;

namespace Axis.PresentationEngine.Areas.Account.Repository.ForgotPassword
{
    /// <summary>
    ///
    /// </summary>
    public interface IForgotPasswordRepository
    {
        /// <summary>
        /// Sends the reset link.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="requestorIPAddress">The requestor ip address.</param>
        /// <returns></returns>
        Response<ResetPasswordViewModel> SendResetLink(string email, string requestorIPAddress);

        /// <summary>
        /// Verifies the reset identifier.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        Response<ResetPasswordViewModel> VerifyResetIdentifier(ResetPasswordViewModel resetPassword);

        /// <summary>
        /// Verifies the security details.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        Response<ResetPasswordViewModel> VerifySecurityDetails(ResetPasswordViewModel resetPassword);

        /// <summary>
        /// Verifies the otp.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        Response<ResetPasswordViewModel> VerifyOTP(ResetPasswordViewModel resetPassword);

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        Response<ResetPasswordViewModel> ResetPassword(ResetPasswordViewModel resetPassword);

        /// <summary>
        /// Gets the security questions.
        /// </summary>
        /// <returns></returns>
        Response<SecurityQuestionViewModel> GetSecurityQuestions();
    }
}
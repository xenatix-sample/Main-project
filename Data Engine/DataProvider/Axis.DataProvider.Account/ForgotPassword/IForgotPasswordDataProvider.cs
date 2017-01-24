using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.SecurityQuestion;

namespace Axis.DataProvider.Account
{
    /// <summary>
    ///
    /// </summary>
    public interface IForgotPasswordDataProvider
    {
        /// <summary>
        /// Sends the reset link.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="requestorIPAddress">The requestor ip address.</param>
        /// <returns></returns>
        Response<ResetPasswordModel> SendResetLink(string email, string requestorIPAddress);

        /// <summary>
        /// Verifies the reset identifier.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        Response<ResetPasswordModel> VerifyResetIdentifier(ResetPasswordModel resetPassword);

        /// <summary>
        /// Verifies the security details.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        Response<ResetPasswordModel> VerifySecurityDetails(ResetPasswordModel resetPassword);

        /// <summary>
        /// Verifies the otp.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        Response<ResetPasswordModel> VerifyOTP(ResetPasswordModel resetPassword);

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        Response<ResetPasswordModel> ResetPassword(ResetPasswordModel resetPassword);

        /// <summary>
        /// Gets the security questions.
        /// </summary>
        /// <returns></returns>
        Response<SecurityQuestionModel> GetSecurityQuestions();
    }
}
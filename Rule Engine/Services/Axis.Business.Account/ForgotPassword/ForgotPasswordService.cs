using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.SecurityQuestion;
using System.Collections.Specialized;

namespace Axis.Service.Account.ForgotPassword
{
    /// <summary>
    ///
    /// </summary>
    public class ForgotPasswordService : IForgotPasswordService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "forgotPassword/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ForgotPasswordService" /> class.
        /// </summary>
        public ForgotPasswordService()
        {
            communicationManager = new CommunicationManager();
        }

        /// <summary>
        /// Sends the reset link.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="requestorIPAddress">The requestor ip address.</param>
        /// <returns></returns>
        public Response<ResetPasswordModel> SendResetLink(string email, string requestorIPAddress)
        {
            var apiUrl = baseRoute + "sendResetLink";
            var param = new NameValueCollection();
            param.Add("email", email);
            param.Add("requestorIPAddress", requestorIPAddress);
            return communicationManager.Post<Response<ResetPasswordModel>>(param, apiUrl);
        }

        /// <summary>
        /// Verifies the reset identifier.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        public Response<ResetPasswordModel> VerifyResetIdentifier(ResetPasswordModel resetPassword)
        {
            var apiUrl = baseRoute + "verifyResetIdentifier";

            return communicationManager.Post<ResetPasswordModel, Response<ResetPasswordModel>>(resetPassword, apiUrl);
        }

        /// <summary>
        /// Verifies the security details.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        public Response<ResetPasswordModel> VerifySecurityDetails(ResetPasswordModel resetPassword)
        {
            var apiUrl = baseRoute + "verifySecurityDetails";

            return communicationManager.Post<ResetPasswordModel, Response<ResetPasswordModel>>(resetPassword, apiUrl);
        }

        /// <summary>
        /// Verifies the otp.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        public Response<ResetPasswordModel> VerifyOTP(ResetPasswordModel resetPassword)
        {
            var apiUrl = baseRoute + "verifyOTP";

            return communicationManager.Post<ResetPasswordModel, Response<ResetPasswordModel>>(resetPassword, apiUrl);
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        public Response<ResetPasswordModel> ResetPassword(ResetPasswordModel resetPassword)
        {
            var apiUrl = baseRoute + "resetPassword";

            return communicationManager.Post<ResetPasswordModel, Response<ResetPasswordModel>>(resetPassword, apiUrl);
        }

        /// <summary>
        /// Gets the security questions.
        /// </summary>
        /// <returns></returns>
        public Response<SecurityQuestionModel> GetSecurityQuestions()
        {
            var apiUrl = baseRoute + "getSecurityQuestions";

            return communicationManager.Get<Response<SecurityQuestionModel>>(apiUrl);
        }
    }
}
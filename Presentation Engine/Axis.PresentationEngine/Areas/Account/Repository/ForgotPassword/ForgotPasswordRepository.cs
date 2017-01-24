using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.SecurityQuestion;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Account.Translator;
using Axis.PresentationEngine.Helpers.Model;
using Axis.Service;
using System.Collections.Specialized;

namespace Axis.PresentationEngine.Areas.Account.Repository.ForgotPassword
{
    /// <summary>
    ///
    /// </summary>
    public class ForgotPasswordRepository : IForgotPasswordRepository
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
        /// Initializes a new instance of the <see cref="ForgotPasswordRepository" /> class.
        /// </summary>
        public ForgotPasswordRepository()
        {
            communicationManager = new CommunicationManager();
        }

        /// <summary>
        /// Sends the reset link.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="requestorIPAddress"></param>
        /// <returns></returns>
        public Response<ResetPasswordViewModel> SendResetLink(string email, string requestorIPAddress)
        {
            var apiUrl = baseRoute + "sendResetLink";

            var param = new NameValueCollection();
            param.Add("email", email ?? string.Empty);
            param.Add("requestorIPAddress", requestorIPAddress ?? string.Empty);

            var response = communicationManager.Post<Response<ResetPasswordModel>>(param, apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Verifies the reset identifier.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        public Response<ResetPasswordViewModel> VerifyResetIdentifier(ResetPasswordViewModel resetPassword)
        {
            var apiUrl = baseRoute + "verifyResetIdentifier";
            var response = communicationManager.Post<ResetPasswordModel, Response<ResetPasswordModel>>(resetPassword.ToEntity(), apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Verifies the security details.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        public Response<ResetPasswordViewModel> VerifySecurityDetails(ResetPasswordViewModel resetPassword)
        {
            var apiUrl = baseRoute + "verifySecurityDetails";
            var response = communicationManager.Post<ResetPasswordModel, Response<ResetPasswordModel>>(resetPassword.ToEntity(), apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Verifies the otp.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        public Response<ResetPasswordViewModel> VerifyOTP(ResetPasswordViewModel resetPassword)
        {
            var apiUrl = baseRoute + "verifyOTP";
            var response = communicationManager.Post<ResetPasswordModel, Response<ResetPasswordModel>>(resetPassword.ToEntity(), apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        public Response<ResetPasswordViewModel> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            var apiUrl = baseRoute + "resetPassword";
            var response = communicationManager.Post<ResetPasswordModel, Response<ResetPasswordModel>>(resetPassword.ToEntity(), apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Gets the security questions.
        /// </summary>
        /// <returns></returns>
        public Response<SecurityQuestionViewModel> GetSecurityQuestions()
        {
            var apiUrl = baseRoute + "getSecurityQuestions";
            var response = communicationManager.Get<Response<SecurityQuestionModel>>(apiUrl);
            return response.ToModel();
        }
    }
}
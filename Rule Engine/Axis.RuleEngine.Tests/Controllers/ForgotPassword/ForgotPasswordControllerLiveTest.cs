using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;

namespace Axis.RuleEngine.Tests.Controllers.ForgotPassword
{
    /// <summary>
    /// live test case for forgot password
    /// </summary>
    [TestClass]
    public class ForgotPasswordControllerLiveTest
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager = null;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "ForgotPassword/";

        /// <summary>
        /// The reset password model
        /// </summary>
        private ResetPasswordModel resetPasswordModel = null;

        #endregion Class Variables

        #region Test Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];

            resetPasswordModel = new ResetPasswordModel
           {
               ResetPasswordID = 1,
               UserID = 1,
               Email = "sss@ss.com",
               Phone = "3453453455",
               PhoneID = 1,
               OTPCode = "345",
               SecurityQuestionID = 1,
               SecurityAnswer = "gdgdfg",
               NewPassword = "234",
               RequestorIPAddress = "",
               ExpiresOn = DateTime.Now
           };
        }

        /// <summary>
        /// Sends the reset link_ success.
        /// </summary>
        [TestMethod]
        public void SendResetLink_Success()
        {
            //Arrenge
            var url = baseRoute + "SendResetLink";

            //Act
            var response = communicationManager.Post<ResetPasswordModel, Response<ResetPasswordModel>>(resetPasswordModel, url);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.DataItems);
            Assert.IsTrue(response.RowAffected > 0);
        }

        /// <summary>
        /// Sends the reset link_ failed.
        /// </summary>
        [TestMethod]
        public void SendResetLink_Failed()
        {
            //Arrenge
            var url = baseRoute + "SendResetLink";

            //Act
            var response = communicationManager.Post<ResetPasswordModel, Response<ResetPasswordModel>>(resetPasswordModel, url);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.DataItems);
            Assert.IsTrue(response.RowAffected == 0);
        }

        /// <summary>
        /// Verifies the security details_ success.
        /// </summary>
        [TestMethod]
        public void VerifySecurityDetails_Success()
        {
            //Arrenge
            var url = baseRoute + "VerifySecurityDetails";

            //Act
            var response = communicationManager.Post<ResetPasswordModel, Response<ResetPasswordModel>>(resetPasswordModel, url);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.DataItems);
            Assert.IsTrue(response.RowAffected > 0);
        }

        /// <summary>
        /// Verifies the security details_ failed.
        /// </summary>
        [TestMethod]
        public void VerifySecurityDetails_Failed()
        {
            //Arrenge
            var url = baseRoute + "VerifySecurityDetails";

            //Act
            var response = communicationManager.Post<ResetPasswordModel, Response<ResetPasswordModel>>(resetPasswordModel, url);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.DataItems);
            Assert.IsTrue(response.RowAffected == 0);
        }

        /// <summary>
        /// Verifies the ot p_ success.
        /// </summary>
        [TestMethod]
        public void VerifyOTP_Success()
        {
            //Arrenge
            var url = baseRoute + "VerifyOTP";

            //Act
            var response = communicationManager.Post<ResetPasswordModel, Response<ResetPasswordModel>>(resetPasswordModel, url);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.DataItems);
            Assert.IsTrue(response.RowAffected > 0);
        }

        /// <summary>
        /// Verifies the ot p_ failed.
        /// </summary>
        [TestMethod]
        public void VerifyOTP_Failed()
        {
            //Arrenge
            var url = baseRoute + "VerifyOTP";

            //Act
            var response = communicationManager.Post<ResetPasswordModel, Response<ResetPasswordModel>>(resetPasswordModel, url);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.DataItems);
            Assert.IsTrue(response.RowAffected == 0);
        }

        /// <summary>
        /// Resets the password_ success.
        /// </summary>
        [TestMethod]
        public void ResetPassword_Success()
        {
            //Arrenge
            var url = baseRoute + "ResetPassword";

            //Act
            var response = communicationManager.Post<ResetPasswordModel, Response<ResetPasswordModel>>(resetPasswordModel, url);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.DataItems);
            Assert.IsTrue(response.RowAffected > 0);
        }

        /// <summary>
        /// Resets the password_ failed.
        /// </summary>
        [TestMethod]
        public void ResetPassword_Failed()
        {
            //Arrenge
            var url = baseRoute + "ResetPassword";

            //Act
            var response = communicationManager.Post<ResetPasswordModel, Response<ResetPasswordModel>>(resetPasswordModel, url);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.DataItems);
            Assert.IsTrue(response.RowAffected == 0);
        }

        #endregion Test Methods
    }
}
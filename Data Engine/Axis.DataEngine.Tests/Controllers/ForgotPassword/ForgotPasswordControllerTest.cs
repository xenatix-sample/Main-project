using Axis.DataEngine.Helpers.Results;
using Axis.DataEngine.Service.Controllers;
using Axis.DataProvider.Account;
using Axis.Model.Account;
using Axis.Model.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Axis.DataEngine.Tests.Controllers.ForgotPassword
{
    /// <summary>
    /// mock test cases for forgot password
    /// </summary>
    [TestClass]
    public class ForgotPasswordControllerTest
    {
        /// <summary>
        /// The forgot password data provider
        /// </summary>
        private IForgotPasswordDataProvider forgotPasswordDataProvider;

        /// <summary>
        /// The user identifier
        /// </summary>
        private int userID = 1;

        /// <summary>
        /// The reset password model
        /// </summary>
        private ResetPasswordModel resetPasswordModel = null;

        /// <summary>
        /// The empty reset password model
        /// </summary>
        private ResetPasswordModel emptyResetPasswordModel = null;

        /// <summary>
        /// Forgots the password_ success.
        /// </summary>
        private void ForgotPassword_Success()
        {
            Mock<IForgotPasswordDataProvider> mock = new Mock<IForgotPasswordDataProvider>();
            forgotPasswordDataProvider = mock.Object;

            var resetPasswordList = new List<ResetPasswordModel>();
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
            resetPasswordList.Add(resetPasswordModel);

            var allUsers = new Response<ResetPasswordModel>()
            {
                DataItems = resetPasswordList,
                RowAffected = 1
            };

            //Get User
            Response<ResetPasswordModel> resetPasswordResponse = new Response<ResetPasswordModel>();
            resetPasswordResponse.DataItems = resetPasswordList.Where(x => x.UserID.Equals(userID)).ToList();
            resetPasswordResponse.RowAffected = 1;

            //Authenticate
            mock.Setup(r => r.SendResetLink(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(resetPasswordResponse);

            mock.Setup(r => r.VerifySecurityDetails(It.IsAny<ResetPasswordModel>()))
              .Returns(resetPasswordResponse);

            mock.Setup(r => r.VerifyOTP(It.IsAny<ResetPasswordModel>()))
              .Returns(resetPasswordResponse);

            mock.Setup(r => r.ResetPassword(It.IsAny<ResetPasswordModel>()))
              .Returns(resetPasswordResponse);
        }

        /// <summary>
        /// Forgots the password_ failed.
        /// </summary>
        private void ForgotPassword_Failed()
        {
            Mock<IForgotPasswordDataProvider> mock = new Mock<IForgotPasswordDataProvider>();
            forgotPasswordDataProvider = mock.Object;

            var resetPasswordList = new List<ResetPasswordModel>();
            resetPasswordModel = new ResetPasswordModel
            {
                ResetPasswordID = 1,
                UserID = 0,
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
            resetPasswordList.Add(resetPasswordModel);

            var allUsers = new Response<ResetPasswordModel>()
            {
                DataItems = null,
                RowAffected = 0
            };

            //Get User
            Response<ResetPasswordModel> resetPasswordResponse = new Response<ResetPasswordModel>();
            resetPasswordResponse.DataItems = resetPasswordList.Where(x => x.UserID.Equals(userID)).ToList();
            resetPasswordResponse.RowAffected = 0;

            mock.Setup(r => r.SendResetLink(It.IsAny<string>(),It.IsAny<string>()))
                .Returns(resetPasswordResponse);

            mock.Setup(r => r.VerifySecurityDetails(It.IsAny<ResetPasswordModel>()))
              .Returns(resetPasswordResponse);

            mock.Setup(r => r.VerifyOTP(It.IsAny<ResetPasswordModel>()))
              .Returns(resetPasswordResponse);

            mock.Setup(r => r.ResetPassword(It.IsAny<ResetPasswordModel>()))
              .Returns(resetPasswordResponse);
        }

        /// <summary>
        /// Sends the reset link_ success.
        /// </summary>
        [TestMethod]
        public void SendResetLink_Success()
        {
            //Arrenge
            ForgotPassword_Success();
            ForgotPasswordController forgotPasswordController = new ForgotPasswordController(forgotPasswordDataProvider);

            //Act
            var getForgotPasswordResult = forgotPasswordController.SendResetLink("sss@ss.com","127.0.0.1");
            var response = getForgotPasswordResult as HttpResult<Response<ResetPasswordModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.RowAffected > 0);
        }

        /// <summary>
        /// Sends the reset link_ failed.
        /// </summary>
        [TestMethod]
        public void SendResetLink_Failed()
        {
            //Arrenge
            ForgotPassword_Failed();
            ForgotPasswordController forgotPasswordController = new ForgotPasswordController(forgotPasswordDataProvider);

            //Act
            var getForgotPasswordResult = forgotPasswordController.SendResetLink(string.Empty,string.Empty);
            var response = getForgotPasswordResult as HttpResult<Response<ResetPasswordModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsTrue(response.Value.DataItems.Count == 0);
            Assert.IsTrue(response.Value.RowAffected == 0);
        }

        /// <summary>
        /// Verifies the security details_ success.
        /// </summary>
        [TestMethod]
        public void VerifySecurityDetails_Success()
        {
            //Arrenge
            ForgotPassword_Success();
            ForgotPasswordController forgotPasswordController = new ForgotPasswordController(forgotPasswordDataProvider);

            //Act
            var getForgotPasswordResult = forgotPasswordController.VerifySecurityDetails(resetPasswordModel);
            var response = getForgotPasswordResult as HttpResult<Response<ResetPasswordModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.RowAffected > 0);
        }

        /// <summary>
        /// Verifies the security details_ failed.
        /// </summary>
        [TestMethod]
        public void VerifySecurityDetails_Failed()
        {
            //Arrenge
            ForgotPassword_Failed();
            ForgotPasswordController forgotPasswordController = new ForgotPasswordController(forgotPasswordDataProvider);

            //Act
            var getForgotPasswordResult = forgotPasswordController.VerifySecurityDetails(emptyResetPasswordModel);
            var response = getForgotPasswordResult as HttpResult<Response<ResetPasswordModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsTrue(response.Value.DataItems.Count == 0);
            Assert.IsTrue(response.Value.RowAffected == 0);
        }

        /// <summary>
        /// Verifies the OTP  success.
        /// </summary>
        [TestMethod]
        public void VerifyOTP_Success()
        {
            //Arrenge
            ForgotPassword_Success();
            ForgotPasswordController forgotPasswordController = new ForgotPasswordController(forgotPasswordDataProvider);

            //Act
            var getForgotPasswordResult = forgotPasswordController.VerifyOTP(resetPasswordModel);
            var response = getForgotPasswordResult as HttpResult<Response<ResetPasswordModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.RowAffected > 0);
        }

        /// <summary>
        /// Verifies the OTP failed.
        /// </summary>
        [TestMethod]
        public void VerifyOTP_Failed()
        {
            //Arrenge
            ForgotPassword_Failed();
            ForgotPasswordController forgotPasswordController = new ForgotPasswordController(forgotPasswordDataProvider);

            //Act
            var getForgotPasswordResult = forgotPasswordController.VerifyOTP(emptyResetPasswordModel);
            var response = getForgotPasswordResult as HttpResult<Response<ResetPasswordModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsTrue(response.Value.DataItems.Count == 0);
            Assert.IsTrue(response.Value.RowAffected == 0);
        }

        /// <summary>
        /// Resets the ResetPassword success.
        /// </summary>
        [TestMethod]
        public void ResetPassword_Success()
        {
            //Arrenge
            ForgotPassword_Success();
            ForgotPasswordController forgotPasswordController = new ForgotPasswordController(forgotPasswordDataProvider);

            //Act
            var getForgotPasswordResult = forgotPasswordController.ResetPassword(resetPasswordModel);
            var response = getForgotPasswordResult as HttpResult<Response<ResetPasswordModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.RowAffected > 0);
        }

        /// <summary>
        /// Resets the ResetPassword failed.
        /// </summary>
        [TestMethod]
        public void ResetPassword_Failed()
        {
            //Arrenge
            ForgotPassword_Failed();
            ForgotPasswordController forgotPasswordController = new ForgotPasswordController(forgotPasswordDataProvider);

            //Act
            var getForgotPasswordResult = forgotPasswordController.ResetPassword(emptyResetPasswordModel);
            var response = getForgotPasswordResult as HttpResult<Response<ResetPasswordModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsTrue(response.Value.DataItems.Count == 0);
            Assert.IsTrue(response.Value.RowAffected == 0);
        }
    }
}
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Model.Account;
using Moq;
using System.Collections.Generic;
using Axis.Model.Common;
using System.Linq;
using Axis.RuleEngine.Account.ForgotPassword;
using Axis.RuleEngine.Service.Controllers;
using Axis.RuleEngine.Helpers.Results;

namespace Axis.RuleEngine.Tests.Controllers.ForgotPassword
{
    [TestClass]
    public class ForgotPasswordControllerTest
    {

        private IForgotPasswordRuleEngine forgotPasswordRuleEngine;
        private int userID = 1;
        private ResetPasswordModel resetPasswordModel = null;
        private ResetPasswordModel emptyResetPasswordModel = null;


        private void ForgotPassword_Success()
        {
            Mock<IForgotPasswordRuleEngine> mock = new Mock<IForgotPasswordRuleEngine>();
            forgotPasswordRuleEngine = mock.Object;

            var resetPasswordList = new List<ResetPasswordModel>();
            resetPasswordModel = new ResetPasswordModel
            {
                ResetPasswordID =1,
                UserID =1,
                Email = "sss@ss.com",
                Phone  = "3453453455",
                PhoneID = 1,
                OTPCode = "345",
                SecurityQuestionID = 1, 
                SecurityAnswer ="gdgdfg",
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

        private void ForgotPassword_Failed()
        {
            Mock<IForgotPasswordRuleEngine> mock = new Mock<IForgotPasswordRuleEngine>();
            forgotPasswordRuleEngine = mock.Object;

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
        /// Sends the reset link_ success.
        /// </summary>
        [TestMethod]
        public void SendResetLink_Success()
        {
            //Arrenge
            ForgotPassword_Success();
            ForgotPasswordController forgotPasswordController = new ForgotPasswordController(forgotPasswordRuleEngine);

            //Act
            var getForgotPasswordResult = forgotPasswordController.SendResetLink("sss@ss.com","127.0.0.1");
            var response = getForgotPasswordResult as HttpResult<Response<ResetPasswordModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.RowAffected > 0);
        }

        [TestMethod]
        public void SendResetLink_Failed()
        {

            //Arrenge
            ForgotPassword_Failed();
            ForgotPasswordController forgotPasswordController = new ForgotPasswordController(forgotPasswordRuleEngine);

            //Act
            var getForgotPasswordResult = forgotPasswordController.SendResetLink(string.Empty, string.Empty);
            var response = getForgotPasswordResult as HttpResult<Response<ResetPasswordModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsTrue(response.Value.DataItems.Count == 0);
            Assert.IsTrue(response.Value.RowAffected == 0);


        }

        [TestMethod]
        public void VerifySecurityDetails_Success()
        {
            //Arrenge
            ForgotPassword_Success();
            ForgotPasswordController forgotPasswordController = new ForgotPasswordController(forgotPasswordRuleEngine);

            //Act
            var getForgotPasswordResult = forgotPasswordController.VerifySecurityDetails(resetPasswordModel);
            var response = getForgotPasswordResult as HttpResult<Response<ResetPasswordModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.RowAffected > 0);
        }

        [TestMethod]
        public void VerifySecurityDetails_Failed()
        {

            //Arrenge
            ForgotPassword_Failed();
            ForgotPasswordController forgotPasswordController = new ForgotPasswordController(forgotPasswordRuleEngine);

            //Act
            var getForgotPasswordResult = forgotPasswordController.VerifySecurityDetails(emptyResetPasswordModel);
            var response = getForgotPasswordResult as HttpResult<Response<ResetPasswordModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsTrue(response.Value.DataItems.Count == 0);
            Assert.IsTrue(response.Value.RowAffected == 0);

        }

        [TestMethod]
        public void VerifyOTP_Success()
        {
            //Arrenge
            ForgotPassword_Success();
            ForgotPasswordController forgotPasswordController = new ForgotPasswordController(forgotPasswordRuleEngine);

            //Act
            var getForgotPasswordResult = forgotPasswordController.VerifyOTP(resetPasswordModel);
            var response = getForgotPasswordResult as HttpResult<Response<ResetPasswordModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.RowAffected > 0);
        }

        [TestMethod]
        public void VerifyOTP_Failed()
        {

            //Arrenge
            ForgotPassword_Failed();
            ForgotPasswordController forgotPasswordController = new ForgotPasswordController(forgotPasswordRuleEngine);

            //Act
            var getForgotPasswordResult = forgotPasswordController.VerifyOTP(emptyResetPasswordModel);
            var response = getForgotPasswordResult as HttpResult<Response<ResetPasswordModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsTrue(response.Value.DataItems.Count == 0);
            Assert.IsTrue(response.Value.RowAffected == 0);
        }

        [TestMethod]
        public void ResetPassword_Success()
        {
            //Arrenge
            ForgotPassword_Success();
            ForgotPasswordController forgotPasswordController = new ForgotPasswordController(forgotPasswordRuleEngine);

            //Act
            var getForgotPasswordResult = forgotPasswordController.ResetPassword(resetPasswordModel);
            var response = getForgotPasswordResult as HttpResult<Response<ResetPasswordModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.RowAffected > 0);
        }

        [TestMethod]
        public void ResetPassword_Failed()
        {

            //Arrenge
            ForgotPassword_Failed();
            ForgotPasswordController forgotPasswordController = new ForgotPasswordController(forgotPasswordRuleEngine);

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


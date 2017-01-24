using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;
using Axis.Model.Account;

namespace Axis.DataEngine.Tests.Controllers.Login
{
    [TestClass]
    public class AccountControllerLiveTest
    {
        private CommunicationManager communicationManager;
        private const string baseRoute = "Account/";
        private string serverIP = "10.20.10.4";
        private UserModel userModel = null;

        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        private UserModel GetUserModel_Success()
        {
            return userModel = new UserModel
            {
                UserName = "achoudhary",
                Password = "Welcome1"
            };
        }

        private UserModel GetUserModel_Failed()
        {
            return userModel = new UserModel
            {
                UserName = "",
                Password = ""
            };

        }


        [TestMethod]
        public void Authenticate_Success()
        {
            // Arrange
            var url = baseRoute + "Authenticate";

            //Act
            var response = communicationManager.Post<UserModel, Response<UserModel>>(GetUserModel_Success(), url);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one user must exists.");
        }

        [TestMethod]
        public void Authenticate_Failed()
        {
            // Arrange
            var url = baseRoute + "Authenticate";

            //Act
            var response = communicationManager.Post<UserModel, Response<UserModel>>(GetUserModel_Failed(), url);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Atleast one user must exists.");
        }

        [TestMethod]
        public void SetLoginData_Success()
        {
            // Arrange
            var url = baseRoute + "SetLoginData";

            //Act
            var response = communicationManager.Post<UserModel, Response<UserModel>>(GetUserModel_Success(), url);

            //Assert
            Assert.IsNotNull(response);
        }


        [TestMethod]
        public void SyncUser_Success()
        {
            // Arrange
            var url = baseRoute + "SyncUser";

            //Act
            var response = communicationManager.Post<UserModel, int>(GetUserModel_Success(), url);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response > 0);
        }


        [TestMethod]
        public void LogAccessToken_Success()
        {
            // Arrange
            var url = baseRoute + "LogAccessToken";
            var accessTokenModel = new AccessTokenModel
            {
                UserName = "achoudhary",
                Token = ConfigurationManager.AppSettings["UnitTestUrl"],
                ClientIP = serverIP
            };

            //Act
            var response = communicationManager.Post<AccessTokenModel, int>(accessTokenModel, url);

            //Assert
            Assert.IsTrue(response > 0);
        }


        [TestMethod]
        public void IsValidServerIP_Success()
        {
            // Arrange
            var url = baseRoute + "IsValidServerIP";
            var param = new NameValueCollection();
            param.Add("IPAddress", serverIP);

            //Act
            var response = communicationManager.Get<Response<ServerResourceModel>>(param, url);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(0, response.ResultCode);

        }

        [TestMethod]
        public void IsValidServerIP_Failed()
        {
            // Arrange
            var url = baseRoute + "IsValidServerIP";
            var param = new NameValueCollection();
            param.Add("IPAddress", "");

            //Act
            var response = communicationManager.Get<Response<ServerResourceModel>>(param, url);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(-1, response.ResultCode);

        }

        [TestMethod]
        public void GetTokenIssueExpireDate_Success()
        {
            // Arrange
            var url = baseRoute + "GetTokenIssueExpireDate";

            //Act
            var response = communicationManager.Get<Response<AccessTokenModel>>(url);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.DataItems.Count > 0);
        }
    }
}

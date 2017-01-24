using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using Axis.Model.Account;
using Axis.Model.Common;

namespace Axis.RuleEngine.Tests.Controllers.Login
{
    [TestClass]
    public class UnitTest1
    {
        private CommunicationManager communicationManager;
        private const string baseRoute = "Account/";

        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        [TestMethod]
        public void Authenticate_Success()
        {
            // Arrange
            var url = baseRoute + "Authenticate";
            var userModel = new UserModel
            {
                UserName = "achoudhary",
                Password = "Welcome1"
            };

            //Act
            var response = communicationManager.Post<UserModel, AuthenticationModel>(userModel, url);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsAuthenticated  == true);
        }

        [TestMethod]
        public void Authenticate_Failed()
        {
            // Arrange
            var url = baseRoute + "Authenticate";
            var userModel = new UserModel
            {
                UserName = "",
                Password = ""
            };

            //Act
            var response = communicationManager.Post<UserModel, AuthenticationModel>(userModel, url);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsAuthenticated == false);
        }
    }
}

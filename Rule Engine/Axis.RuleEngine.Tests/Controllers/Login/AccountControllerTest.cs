using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.RuleEngine.Account;
using Moq;
using System.Collections.Generic;
using Axis.Model.Account;
using Axis.Model.Common;
using System.Linq;
using Axis.RuleEngine.Service;
using Axis.RuleEngine.Helpers.Results;
using System.Configuration;

namespace Axis.RuleEngine.Tests.Controllers.Login
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class AccountControllerTest
    {
        /// <summary>
        /// The account rule engine
        /// </summary>
        private IAccountRuleEngine accountRuleEngine;
        /// <summary>
        /// The user name
        /// </summary>
        private string userName = "achoudhary";
        /// <summary>
        /// The server ip
        /// </summary>
        private string serverIP = "10.20.10.4";
        /// <summary>
        /// The user model
        /// </summary>
        private UserModel userModel = null;


        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            Mock<IAccountRuleEngine> mock = new Mock<IAccountRuleEngine>();
            accountRuleEngine = mock.Object;

            var userList = new List<UserModel>();
            userModel = new UserModel
            {
                UserName = "achoudhary",
                Password = "Welcome1"
            };
            userList.Add(userModel);

            var allUsers = new Response<UserModel>()
            {
                DataItems = userList
            };

            //Get User
            Response<UserModel> userResponse = new Response<UserModel>();
            userResponse.DataItems = userList.Where(x => x.UserName.Contains(userName)).ToList();

            mock.Setup(r => r.Authenticate(It.IsAny<UserModel>()))
                .Returns(userResponse);

            mock.Setup(r => r.SetLoginData(It.IsAny<UserModel>()))
               .Returns(userResponse);

            mock.Setup(r => r.SyncUser(It.IsAny<UserModel>()));


            var accessTokenModel = new List<AccessTokenModel>();
            accessTokenModel.Add(new AccessTokenModel()
            {
                UserName = "achoudhary",
            });

            Response<AccessTokenModel> response = new Response<AccessTokenModel>();
            response.DataItems = accessTokenModel.Where(x => x.UserName.Contains(userName)).ToList();

            mock.Setup(r => r.LogAccessToken(It.IsAny<AccessTokenModel>()));

            var serverResourceModel = new List<ServerResourceModel>();
            serverResourceModel.Add(new ServerResourceModel()
            {
                ServerIP = serverIP,
                ServerResourceID = 4,
                ServerResourceType = "1"

            });
            Response<ServerResourceModel> serverResponse = new Response<ServerResourceModel>();
            serverResponse.DataItems = serverResourceModel.Where(x => x.ServerIP.Contains(serverIP)).ToList();


            mock.Setup(r => r.IsValidServerIP(It.IsAny<string>())).Returns(serverResponse);


            mock.Setup(r => r.GetTokenIssueExpireDate()).Returns(response);


        }
        /// <summary>
        /// Authenticate_s the success.
        /// </summary>
        [TestMethod]
        public void Authenticate_Success()
        {
            // Arrange
            AccountController accountController = new AccountController(accountRuleEngine);

            //Act
            var getUserResult = accountController.Authenticate(userModel);
            var response = getUserResult as HttpResult<AuthenticationModel>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.User);

        }

        /// <summary>
        /// Authenticate_s the failed.
        /// </summary>
        [TestMethod]
        public void Authenticate_Failed()
        {
            // Arrange
            userModel = new UserModel
            {
                UserName = "",
                Password = ""
            };
            AccountController accountController = new AccountController(accountRuleEngine);

            //Act
            var getUserResult = accountController.Authenticate(userModel);
            var response = getUserResult as HttpResult<AuthenticationModel>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsTrue(response.Value.User.UserID == 0);
        }
    }
}

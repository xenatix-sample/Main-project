using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Axis.DataProvider.Account;
using System.Collections.Generic;
using Axis.Model.Account;
using Axis.Model.Common;
using System.Linq;
using Axis.DataEngine.Service.Controllers;
using Axis.DataEngine.Helpers.Results;
using System.Configuration;

namespace Axis.DataEngine.Tests.Controllers.Login
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class AccountControllerTest
    {

        /// <summary>
        /// The account data provider
        /// </summary>
        private IAccountDataProvider accountDataProvider;
        /// <summary>
        /// The user name
        /// </summary>
        private string userName = string.Empty;
        /// <summary>
        /// The server ip
        /// </summary>
        private string serverIP = "10.20.10.4";
        /// <summary>
        /// The user model
        /// </summary>
        private UserModel userModel = null;
        /// <summary>
        /// The empty user model
        /// </summary>
        private UserModel emptyUserModel = null;


        /// <summary>
        /// Mock_s the success.
        /// </summary>
        private void Mock_Success()
        {
            Mock<IAccountDataProvider> mock = new Mock<IAccountDataProvider>();
            accountDataProvider = mock.Object;

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

            //Authenticate
            mock.Setup(r => r.Authenticate(It.IsAny<UserModel>()))
                .Returns(userResponse);

            //SetLoginData
            mock.Setup(r => r.SetLoginData(It.IsAny<UserModel>()))
               .Returns(userResponse);

            //SyncUser
            mock.Setup(r => r.SyncUser(It.IsAny<UserModel>()));


            var accessTokenModel = new List<AccessTokenModel>();
            accessTokenModel.Add(new AccessTokenModel()
            {
                UserName = "achoudhary",
                Token = ConfigurationManager.AppSettings["UnitTestToken"],
                AccessTokenID = 1
            });

            Response<AccessTokenModel> response = new Response<AccessTokenModel>();
            response.DataItems = accessTokenModel.Where(x => x.UserName.Contains(userName)).ToList();

            //LogAccessToken
            mock.Setup(r => r.GetValidUserInfoByToken(It.IsAny<AccessTokenModel>())).Returns((userModel));


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

            //IsValidServerIP
            mock.Setup(r => r.IsValidServerIP(It.IsAny<string>())).Returns(serverResponse);


            //GetTokenIssueExpireDate
            mock.Setup(r => r.GetTokenIssueExpireDate()).Returns(response);
        }

        /// <summary>
        /// Mock_s the failed.
        /// </summary>
        private void Mock_Failed()
        {
            //Failed Mock
            Mock<IAccountDataProvider> mock_Failed = new Mock<IAccountDataProvider>();
            accountDataProvider = mock_Failed.Object;

            var emptyUserList = new List<UserModel>();

            var allUserResponse = new Response<UserModel>()
            {
                DataItems = emptyUserList
            };

            //Get User
            Response<UserModel> emptyUserResponse = new Response<UserModel>();
            emptyUserResponse.DataItems = emptyUserList.Where(x => x.UserName.Contains(userName)).ToList();

            //Authenticate
            mock_Failed.Setup(r => r.Authenticate(It.IsAny<UserModel>()))
                .Returns(emptyUserResponse);

            //SetLoginData
            mock_Failed.Setup(r => r.SetLoginData(It.IsAny<UserModel>()))
               .Returns(emptyUserResponse);

            
            var emptyAccessTokenModel = new List<AccessTokenModel>();

            Response<AccessTokenModel> emptyResponse = new Response<AccessTokenModel>();
            emptyResponse.DataItems = emptyAccessTokenModel.Where(x => x.UserName.Contains(userName)).ToList();

          

            var emptyServerResourceModel = new List<ServerResourceModel>();

            Response<ServerResourceModel> emptyServerResponse = new Response<ServerResourceModel>();
            emptyServerResponse.DataItems = emptyServerResourceModel.Where(x => x.ServerIP.Contains(serverIP)).ToList();

            //IsValidServerIP
            mock_Failed.Setup(r => r.IsValidServerIP(It.IsAny<string>())).Returns(emptyServerResponse);


            //GetTokenIssueExpireDate
            mock_Failed.Setup(r => r.GetTokenIssueExpireDate()).Returns(emptyResponse);
        }


        /// <summary>
        /// Authenticate_s the success.
        /// </summary>
        [TestMethod]
        public void Authenticate_Success()
        {
            //Arrenge
            Mock_Success();
            AccountController accountController = new AccountController(accountDataProvider);

            //Act
            var getUserResult = accountController.Authenticate(userModel);
            var response = getUserResult as HttpResult<Response<UserModel>>;
            var user = response.Value.DataItems;

            //Assert
            Assert.IsNotNull(user);
            Assert.IsTrue(user.Count > 0);
        }

        /// <summary>
        /// Authenticate_s the failed.
        /// </summary>
        [TestMethod]
        public void Authenticate_Failed()
        {

            //Arrenge
            Mock_Failed();
            AccountController accountController = new AccountController(accountDataProvider);

            //Act
            var getUserResult = accountController.Authenticate(emptyUserModel);
            var response = getUserResult as HttpResult<Response<UserModel>>;
            var user = response.Value.DataItems;

            //Assert
            Assert.IsNotNull(user);
            Assert.IsTrue(user.Count == 0);

        }

        /// <summary>
        /// Sets the login data_ success.
        /// </summary>
        [TestMethod]
        public void SetLoginData_Success()
        {
            // Arrange
            Mock_Success();
            AccountController accountController = new AccountController(accountDataProvider);

            //Act
            var getUserResult = accountController.SetLoginData(userModel);
            var response = getUserResult as HttpResult<Response<UserModel>>;
            var user = response.Value.DataItems;

            //Assert
            Assert.IsNotNull(user);
            Assert.IsTrue(user.Count > 0);
        }

        /// <summary>
        /// Sets the login data_ failed.
        /// </summary>
        [TestMethod]
        public void SetLoginData_Failed()
        {
            // Arrange
            Mock_Failed();
            AccountController accountController = new AccountController(accountDataProvider);

            //Act
            var getUserResult = accountController.SetLoginData(emptyUserModel);
            var response = getUserResult as HttpResult<Response<UserModel>>;
            var user = response.Value.DataItems;

            //Assert
            Assert.IsNotNull(user);
            Assert.IsTrue(user.Count == 0);

        }

        /// <summary>
        /// Synchronizes the user_ success.
        /// </summary>
        [TestMethod]
        public void SyncUser_Success()
        {
            // Arrange
            Mock_Success();
            AccountController accountController = new AccountController(accountDataProvider);

            //Act
            var getUserResult = accountController.SyncUser(userModel);
            var response = getUserResult as HttpResult<int>;
            var userValue = response.Value;

            //Assert
            Assert.IsNotNull(userValue);
            Assert.IsTrue(userValue > 0);
        }

        /// <summary>
        /// Logs the access token_ success.
        /// </summary>
        [TestMethod]
        public void LogAccessToken_Success()
        {
            // Arrange
            Mock_Success();
            var userModel = new AccessTokenModel
            {
                UserName = "achoudhary",
                Token = ConfigurationManager.AppSettings["UnitTestToken"],
                AccessTokenID = 1
            };

            AccountController accountController = new AccountController(accountDataProvider);

            //Act
            var getUserResult = accountController.LogAccessToken(userModel);
            var response = getUserResult as HttpResult<int>;
            var userValue = response.Value;

            //Assert
            Assert.IsNotNull(userValue);
            Assert.IsTrue(userValue > 0);
        }


        /// <summary>
        /// Determines whether [is valid server i p_ success].
        /// </summary>
        [TestMethod]
        public void IsValidServerIP_Success()
        {
            // Arrange
            Mock_Success();
            AccountController accountController = new AccountController(accountDataProvider);

            //Act
            var getUserResult = accountController.IsValidServerIP(serverIP);
            var response = getUserResult as HttpResult<Response<ServerResourceModel>>;
            var serverValue = response.Value.DataItems;

            //Assert
            Assert.IsNotNull(serverValue);
            Assert.IsTrue(serverValue.Count > 0);
        }

        /// <summary>
        /// Determines whether [is valid server i p_ failed].
        /// </summary>
        [TestMethod]
        public void IsValidServerIP_Failed()
        {
            // Arrange
            Mock_Failed();
            AccountController accountController = new AccountController(accountDataProvider);

            //Act
            var getUserResult = accountController.IsValidServerIP("");
            var response = getUserResult as HttpResult<Response<ServerResourceModel>>;
            var serverValue = response.Value;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsTrue(response.Value.DataItems.Count == 0);
        }

        /// <summary>
        /// Gets the token issue expire date_ success.
        /// </summary>
        [TestMethod]
        public void GetTokenIssueExpireDate_Success()
        {
            // Arrange
            Mock_Success();
            AccountController accountController = new AccountController(accountDataProvider);

            //Act
            var getUserResult = accountController.GetTokenIssueExpireDate();
            var response = getUserResult as HttpResult<Response<AccessTokenModel>>;
            var serverValue = response.Value.DataItems;

            //Assert
            Assert.IsNotNull(serverValue);
            Assert.IsTrue(serverValue.Count > 0);
        }

        /// <summary>
        /// Gets the token issue expire date_ failed.
        /// </summary>
        [TestMethod]
        public void GetTokenIssueExpireDate_Failed()
        {
            // Arrange
            Mock_Failed();
            AccountController accountController = new AccountController(accountDataProvider);

            //Act
            var getUserResult = accountController.GetTokenIssueExpireDate();
            var response = getUserResult as HttpResult<Response<AccessTokenModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsTrue(response.Value.DataItems.Count == 0);
        }

        /// <summary>
        /// Gets the valid user information by token_ success.
        /// </summary>
        [TestMethod]
        public void GetValidUserInfoByToken_Success()
        {
            Mock_Success();
            var accessTokenModel = new AccessTokenModel
            {
                UserName = "achoudhary",
                Token = ConfigurationManager.AppSettings["UnitTestToken"],
                AccessTokenID = 1
            };
            AccountController accountController = new AccountController(accountDataProvider);

            var getUserRolesResult = accountController.GetValidUserInfoByToken(accessTokenModel);
            var response = getUserRolesResult as HttpResult<UserModel>;

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
        }

        /// <summary>
        /// Gets the valid user information by token_ failed.
        /// </summary>
        [TestMethod]
        public void GetValidUserInfoByToken_Failed()
        {
            //Arrange
            Mock_Failed();
            var accessTokenModel = new AccessTokenModel
            {
                UserName = "achoudhary",
                Token = ConfigurationManager.AppSettings["UnitTestToken"],
                AccessTokenID = 1
            };

            //Act
            AccountController accountController = new AccountController(accountDataProvider);
            var getUserRolesResult = accountController.GetValidUserInfoByToken(accessTokenModel);
            var response = getUserRolesResult as HttpResult<UserModel>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNull(response.Value);
        }


    }
}

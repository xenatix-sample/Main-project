using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Collections.Specialized;
using Axis.Model.Common.Assessment;
using Axis.Model.Common;
using System.Configuration;
using System.Collections.Generic;
using Axis.Model.Account;
using System.Globalization;

namespace Axis.RuleEngine.Tests.Controllers.Account.UserSecurity
{
    [TestClass]
    public class UserSecurityLiveTest
    {
        private CommunicationManager communicationManager;
        private const string baseRoute = "UserSecurity/";       

        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        /// <summary>
        /// Get user security questions success
        /// </summary>
        [TestMethod]
        public void GetUserSecurityQuestions_Success()
        {
            //Arrange
            int userId = 1;
            var url = baseRoute + "GetUserSecurityQuestions";
            var param = new NameValueCollection();
            param.Add("userID", userId.ToString(CultureInfo.InvariantCulture));

            //Act
            var response = communicationManager.Get<Response<UserSecurityQuestionAnswerModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.DataItems, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one security questions must exists.");
        }


        /// <summary>
        /// Get user security questions - failed
        /// </summary>
        [TestMethod]
        public void GetUserSecurityQuestions_Failed()
        {
            //Arrange
            int userId = -1;
            var url = baseRoute + "GetUserSecurityQuestions";
            var param = new NameValueCollection();
            param.Add("userID", userId.ToString(CultureInfo.InvariantCulture));

            //Act
            var response = communicationManager.Get<Response<UserSecurityQuestionAnswerModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.DataItems, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "security questions exists.");
        }

        /// <summary>
        /// Save/update user security questions success
        /// </summary>
        [TestMethod]
        public void SaveUserSecurityQuestions_Success()
        {
            //Arrange
            var url = baseRoute + "SaveUserSecurityQuestions";
            List<UserSecurityQuestionAnswerModel> securityQuestions = new List<UserSecurityQuestionAnswerModel>();
            securityQuestions.Add(new UserSecurityQuestionAnswerModel() { UserID = 1, UserSecurityQuestionID = 1, SecurityQuestionID = 1, SecurityAnswer = "Answer 1", ForceRollback = true });
            securityQuestions.Add(new UserSecurityQuestionAnswerModel() { UserID = 1, UserSecurityQuestionID = 0, SecurityQuestionID = 2, SecurityAnswer = "Answer 2", ForceRollback = true });

            //Act
            var response = communicationManager.Post<List<UserSecurityQuestionAnswerModel>, Response<UserSecurityQuestionAnswerModel>>(securityQuestions, url);

            // Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Security questions not created");
        }


        /// <summary>
        /// Save/update user security questions - failed
        /// </summary>
        [TestMethod]
        public void SaveUserSecurityQuestions_Failed()
        {
            //Arrange
            var url = baseRoute + "SaveUserSecurityQuestions";
            List<UserSecurityQuestionAnswerModel> securityQuestions = new List<UserSecurityQuestionAnswerModel>();
            securityQuestions.Add(new UserSecurityQuestionAnswerModel() { UserID = -1, UserSecurityQuestionID = -1, SecurityQuestionID = 1, SecurityAnswer = "Answer 1",ForceRollback=true });
            securityQuestions.Add(new UserSecurityQuestionAnswerModel() { UserID = -1, UserSecurityQuestionID = 0, SecurityQuestionID = -2, SecurityAnswer = "Answer 2", ForceRollback = true });

            //Act
            var response = communicationManager.Post<List<UserSecurityQuestionAnswerModel>, Response<UserSecurityQuestionAnswerModel>>(securityQuestions, url);

            // Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Security questions created");

        }
    }
}

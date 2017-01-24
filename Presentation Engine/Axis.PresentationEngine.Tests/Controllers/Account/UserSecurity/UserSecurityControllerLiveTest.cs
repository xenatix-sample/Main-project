using System.Collections.Generic;
using System.Configuration;
using Axis.Model.Address;
using Axis.Model.Email;
using Axis.Model.Phone;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Account.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.PresentationEngine.Areas.Account.Respository;
using Axis.Model.Account;

namespace Axis.PresentationEngine.Tests.Controllers.Account.UserSecurity
{
    [TestClass]
    public class UserSecurityControllerLiveTest
    {
        #region Test Methods

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "UserSecurity/";

        /// <summary>
        /// The User Security manager
        /// </summary>
        private UserSecurityRepository _repository;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()        {
            
            _repository = new UserSecurityRepository(ConfigurationManager.AppSettings["UnitTestToken"]);
        }


        /// <summary>
        /// Get user security questions success
        /// </summary>
        [TestMethod]
        public void GetUserSecurityQuestions_Success()
        {
            //Arrange
            int userId = 1;            

            //Act
            var response = _repository.GetUserSecurityQuestions(userId);

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

            //Act
            var response = _repository.GetUserSecurityQuestions(userId);

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.DataItems, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Security questions exists for invalid data.");
        }

        /// <summary>
        /// Save/update user security questions success
        /// </summary>
        [TestMethod]
        public void SaveUserSecurityQuestions_Success()
        {
            //Arrange           
            List<UserSecurityQuestionAnswerViewModel> securityQuestions = new List<UserSecurityQuestionAnswerViewModel>();
            securityQuestions.Add(new UserSecurityQuestionAnswerViewModel() { UserID = 1, UserSecurityQuestionID = 1, SecurityQuestionID = 1, SecurityAnswer = "Answer 1", ForceRollback = true });
            securityQuestions.Add(new UserSecurityQuestionAnswerViewModel() { UserID = 1, UserSecurityQuestionID = 0, SecurityQuestionID = 2, SecurityAnswer = "Answer 2", ForceRollback = true });

            //Act
            var response = _repository.SaveUserSecurityQuestions(securityQuestions);

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
            List<UserSecurityQuestionAnswerViewModel> securityQuestions = new List<UserSecurityQuestionAnswerViewModel>();
            securityQuestions.Add(new UserSecurityQuestionAnswerViewModel() { UserID = -1, UserSecurityQuestionID = -1, SecurityQuestionID = 1, SecurityAnswer = "Answer 1", ForceRollback = true });
            securityQuestions.Add(new UserSecurityQuestionAnswerViewModel() { UserID = -1, UserSecurityQuestionID = 0, SecurityQuestionID = -2, SecurityAnswer = "Answer 2", ForceRollback = true });

            //Act
            var response = _repository.SaveUserSecurityQuestions(securityQuestions);

            // Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Security questions created for invalid data");

        }

        #endregion
    }
}

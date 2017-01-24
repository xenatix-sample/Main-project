using System;
using System.Collections.Generic;
using System.Configuration;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Admin.ApiControllers;
using Axis.PresentationEngine.Areas.Admin.Model;
using Axis.PresentationEngine.Areas.Admin.Respository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axis.PresentationEngine.Tests.Controllers.Admin.UserCredential
{
    [TestClass]
    public class UserCredentialLiveTest
    {
        #region Class Variables

        private UserCredentialController _controller;
        private readonly int _userID = 1;
        private readonly bool isMyProfile = true;
        #endregion

        [TestInitialize]
        public void Initialize()
        {
            _controller = new UserCredentialController(new UserCredentialRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        #region Test Methods

        [TestMethod]
        public void GetUserCredentials_Success()
        {
            var response = _controller.GetUserCredentials(_userID, isMyProfile);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            //ToDo: We need to setup a user that should always have credentials assigned to them...then I can uncomment the assert and add a _Failure test method
            //Assert.IsTrue(response.DataItems.Count > 0, "At least one user detail record must exist");
        }

        [TestMethod]
        public void SaveUserCredentials_Success()
        {
            List<UserCredentialViewModel> credentials = new List<UserCredentialViewModel>()
            {
                new UserCredentialViewModel()
                {
                    UserID = _userID,
                    CredentialID = 1,
                    LicenseNbr = "12345",
                    StateIssuedByID = 5,
                    LicenseIssueDate = DateTime.Now,
                    LicenseExpirationDate = DateTime.Now.AddYears(3)
                }
            };

            var userCredentialModel = new UserViewModel()
            {
                UserID = _userID,
                Credentials = credentials,
                ForceRollback = true
            };

            var response = _controller.SaveUserCredentials(userCredentialModel, isMyProfile);
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "User credentials could not be saved");
            Assert.IsTrue(response.RowAffected > 0, "No records were saved");
        }

        [TestMethod]
        public void SaveUserCredentials_Failure()
        {
            List<UserCredentialViewModel> credentials = new List<UserCredentialViewModel>()
            {
                new UserCredentialViewModel()
                {
                    UserID = _userID,
                    CredentialID = -1,
                    LicenseNbr = "12345",
                    StateIssuedByID = 5,
                    LicenseIssueDate = DateTime.Now,
                    LicenseExpirationDate = DateTime.Now.AddYears(3)
                }
            };

            var userCredentialModel = new UserViewModel()
            {
                UserID = _userID,
                Credentials = credentials,
                ForceRollback = true
            };

            var response = _controller.SaveUserCredentials(userCredentialModel, isMyProfile);
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected <= 2, "User credentials were saved with invalid data");//This covers the rows affected by the auditing
        }

        #endregion
    }
}

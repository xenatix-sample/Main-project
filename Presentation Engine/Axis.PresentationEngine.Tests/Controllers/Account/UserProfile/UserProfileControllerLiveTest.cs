using System.Collections.Generic;
using System.Configuration;
using Axis.Model.Address;
using Axis.Model.Email;
using Axis.Model.Phone;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Account.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axis.PresentationEngine.Tests.Controllers.Account.UserProfile
{
    [TestClass]
    public class UserProfileControllerLiveTest
    {
        private const string BaseRoute = "userProfile/";
        private UserProfileRepository _repository;
        private readonly int _userID = 1;
        private readonly bool isMyProfile = true;
        [TestInitialize]
        public void Initialize()
        {
            _repository = new UserProfileRepository(ConfigurationManager.AppSettings["UnitTestToken"]);
        }

        [TestMethod]
        public void GetUserProfile_Success()
        {
            var response = _repository.GetUserProfile(isMyProfile);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "At least one record must exist");
        }

        [TestMethod]
        public void GetUserProfileByID_Success()
        {
            var response = _repository.GetUserProfileByID(_userID, isMyProfile);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "At least one record must exist");
        }

        [TestMethod]
        public void GetUserProfileByID_Failure()
        {
            var response = _repository.GetUserProfileByID(-1, isMyProfile);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Data returned for invalid user id");
        }

        [TestMethod]
        public async void GetUserProfileAsync_Success()
        {
            var response = await _repository.GetUserProfileAsync(isMyProfile);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "At least one record must exist");
        }

        [TestMethod]
        public void SaveUserProfile_Success()
        {
            var emailModel = new UserEmailModel() { EmailID = 1, Email = "arunc.choudhary@xenatix.com", UserEmailID = 1, IsPrimary = true, ForceRollback = true };
            var model = new UserProfileViewModel()
            {
                UserID = _userID,
                IsTemporaryPassword = false,
                Emails = new List<UserEmailModel>() { emailModel },
                Phones = new List<UserPhoneModel>(),
                Addresses = new List<UserAddressModel>(),
                ForceRollback = true
            };
            var response = _repository.SaveUserProfile(model, isMyProfile);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "User profile could not be saved");
        }

        [TestMethod]
        public void SaveUserProfile_Failure()
        {
            var model = new UserProfileViewModel()
            {
                UserID = -999,
                IsTemporaryPassword = false,
                Emails = new List<UserEmailModel>(),
                Phones = new List<UserPhoneModel>(),
                Addresses = new List<UserAddressModel>(),
                ForceRollback = true
            };
            var response = _repository.SaveUserProfile(model, isMyProfile);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected == 0, "User profile was saved with invalid data");
        }
    }
}

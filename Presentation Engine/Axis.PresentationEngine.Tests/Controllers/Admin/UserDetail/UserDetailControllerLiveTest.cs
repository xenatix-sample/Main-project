using System.Configuration;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Admin.ApiControllers;
using Axis.PresentationEngine.Areas.Admin.Respository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Helpers;

namespace Axis.PresentationEngine.Tests.Controllers.Admin.UserDetail
{
    [TestClass]
    public class UserDetailControllerLiveTest
    {
        private const string BaseRoute = "userDetail/";
        private UserDetailController _controller;
        private readonly int _userID = 1;

        [TestInitialize]
        public void Initialize()
        {
            _controller = new UserDetailController(new UserDetailRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        [TestMethod]
        public void GetUserDetails_Success()
        {
            var response = _controller.GetUser(_userID);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "At least one user detail record must exist");
        }

        [TestMethod]
        public void GetUserDetails_Failure()
        {
            var response = _controller.GetUser(-1);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Data exists for invalid user id");
        }

        [TestMethod]
        public void AddUser_Success()
        {
            var model = new UserViewModel()
            {
                UserName = Axis.Helpers.CommonHelper.GenerateRandomString(10),
                FirstName = "Test",
                LastName = "User",
                MiddleName = "M",
                EffectiveToDate = null,
                GenderID = 1,
                PrimaryEmail = "tusertest@xenatix.com",
                IsActive = false,
                ForceRollback = true
            };

            var response = _controller.AddUser(model);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "User detail could not be created");
        }

        [TestMethod]
        public void AddUser_Failure()
        {
            var model = new UserViewModel()
            {
                UserName = null,
                FirstName = null,
                LastName = null,
                MiddleName = null,
                EffectiveToDate = null,
                IsActive = false,
                ForceRollback = true
            };

            var response = _controller.AddUser(model);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "User detail was created with invalid data.");
        }

        [TestMethod]
        public void UpdateUser_Success()
        {
            var model = new UserViewModel()
            {
                UserID = _userID,
                UserName = "achoudhary",
                FirstName = "Arun",
                LastName = "Choudhary",
                MiddleName = "M",
                EffectiveToDate = null,
                GenderID = 1,
                PrimaryEmail = "arun.choudhary@xenatix.com",
                EmailID = 1,
                IsActive = true,
                ForceRollback = true
            };

            var response = _controller.UpdateUser(model);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "User detail could not be updated");
        }

        [TestMethod]
        public void UpdateUser_Failure()
        {
            var model = new UserViewModel()
            {
                UserID = -1,
                UserName = "achoudhary",
                FirstName = "Arun",
                LastName = "Choudhary",
                MiddleName = "M",
                EffectiveToDate = null,
                GenderID = 1,
                EmailID = 1,
                IsActive = false,
                ForceRollback = true
            };

            var response = _controller.UpdateUser(model);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "User detail was updated with invalid data.");
        }
    }
}

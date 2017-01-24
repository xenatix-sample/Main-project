using System.Configuration;
using Axis.PresentationEngine.Areas.Admin.ApiControllers;
using Axis.PresentationEngine.Areas.Admin.Respository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axis.PresentationEngine.Tests.Controllers.Admin.StaffManagement
{
    [TestClass]
    public class StaffManagementControllerLive
    {
        private const string BaseRoute = "staffManagement/";
        private StaffManagementController _controller = null;
        private readonly int _userID = 1;
        private readonly string _invalidSearchText = "abc1230987";

        [TestInitialize]
        public void Initialize()
        {
            _controller = new StaffManagementController(new StaffManagementRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        [TestMethod]
        public void GetStaff_Success()
        {
            var response = _controller.GetStaff(string.Empty);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "At least one record must exist");
        }

        [TestMethod]
        public void GetStaff_Failure()
        {
            var response = _controller.GetStaff(_invalidSearchText);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Data exists for invalid search text");
        }

        [TestMethod]
        public void DeleteUser_Success()
        {
            var response = _controller.DeleteUser(-999);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "User could not be deleted");
        }

        [TestMethod]
        public void DeleteUser_Failure()
        {
            var response = _controller.DeleteUser(-999);
            bool dataAffected = response.RowAffected > 0;

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(!dataAffected, "Invalid user was successfully deleted");
        }

        [TestMethod]
        public void ActivateUser_Success()
        {
            var response = _controller.ActivateUser(_userID);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "User could not be activated");
        }

        [TestMethod]
        public void ActivateUser_Failure()
        {
            var response = _controller.ActivateUser(-1);
            bool dataAffected = response.RowAffected > 0;

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(!dataAffected, "Invalid user was successfully activated");
        }

        [TestMethod]
        public void UnlockUser_Success()
        {
            var response = _controller.UnlockUser(_userID);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "User could not be unlocked");
        }

        [TestMethod]
        public void UnlockUser_Failure()
        {
            var response = _controller.UnlockUser(-1);
            bool dataAffected = response.RowAffected > 0;

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(!dataAffected, "Invalid user was successfully unlocked");
        }
    }
}

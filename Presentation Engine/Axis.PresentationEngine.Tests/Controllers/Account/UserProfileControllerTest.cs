using Axis.PresentationEngine.Areas.Account.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;


namespace Axis.PresentationEngine.Tests._controllers
{
    [TestClass]
    public class UserProfileControllerTest
    {
        #region Class Variables

        private UserProfileRepository _rep;
        private readonly bool isMyProfile = false;
        #endregion

        #region Test Methods

        [TestInitialize]
        public void Initialize()
        {
            _rep = new UserProfileRepository();
        }

        #region ActionResults

        [TestMethod]
        public void Get_ADUserProfile()
        {
            var result = _rep.GetUserProfile(isMyProfile);
            Assert.AreEqual(result.DataItems[0].ADFlag, true);
            Assert.AreEqual(result.DataItems[0].ADUserPasswordResetMessage, "Please change your password using the same process for changing your Windows password.");
        }

        [TestMethod]
        public void Get_NonADUserProfile()
        {
            var result = _rep.GetUserProfile(isMyProfile);
            Assert.AreEqual(result.DataItems, null);
        }

        #endregion

        #endregion
    }
}

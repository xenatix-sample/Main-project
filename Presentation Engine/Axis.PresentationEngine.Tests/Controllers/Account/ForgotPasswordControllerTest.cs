using Axis.PresentationEngine.Areas.Account.Repository.ForgotPassword;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Axis.PresentationEngine.Tests._controllers
{
    [TestClass]
    public class ForgotPasswordControllerTest
    {
        #region Class Variables

        private ForgotPasswordRepository _rep;

        #endregion

        #region Test Methods

        [TestInitialize]
        public void Initialize()
        {
            _rep = new ForgotPasswordRepository();
        }

        #region ActionResults

        [TestMethod]
        public void SendResetLink_ADUser()
        {
            var result = _rep.SendResetLink("Test.ADUser@xenatix.com", "localhost");
            Assert.AreEqual(result.DataItems[0].ADFlag, true);
            Assert.AreEqual(result.DataItems[0].ADUserPasswordResetMessage, "Please change your password using the same process for changing your Windows password.");
        }

        [TestMethod]
        public void SendResetLink_NonADUser()
        {
            var result = _rep.SendResetLink("Test.ActiveUser@xenatix.com", "localhost");
            Assert.AreEqual(result.DataItems, null);
        }

        #endregion

        #endregion
    }
}

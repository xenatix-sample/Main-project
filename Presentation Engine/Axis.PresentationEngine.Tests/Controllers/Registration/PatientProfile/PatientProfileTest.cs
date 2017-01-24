using Axis.Plugins.Registration.ApiControllers;
using Axis.Plugins.Registration.Repository.PatientProfile;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace Axis.PresentationEngine.Tests.Controllers.Registration.PatientProfile
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class PatientProfileTest
    {
        #region Variables

        
        /// <summary>
        /// The contact identifier
        /// </summary>
        private long contactId = 1;
      
        /// <summary>
        /// The token
        /// </summary>
        private string token = ConfigurationManager.AppSettings["UnitTestToken"];


        #endregion

        #region Public Method

        /// <summary>
        /// Gets the Patient Profile success.
        /// </summary>
        [TestMethod]
        public void GetPatientProfile_Success()
        {
            // Arrange
            var controller = new PatientProfileController(new PatientProfileRepository(token));

            // Act
            var modelResponse = controller.GetPatientProfile(contactId).Result;

            // Assert
            Assert.IsNotNull(modelResponse);
            Assert.IsNotNull(modelResponse.DataItems);
            Assert.IsTrue(modelResponse.DataItems.Count > 0);
        }

        /// <summary>
        /// Gets the Patient Profile failed.
        /// </summary>
        [TestMethod]
        public void GetPatientProfile_Failed()
        {
            // Arrange
            var controller = new PatientProfileController(new PatientProfileRepository(token));

            // Act
            var modelResponse = controller.GetPatientProfile(0).Result;

            //Assert
            Assert.IsNotNull(modelResponse);
            Assert.IsNotNull(modelResponse.DataItems);
            Assert.IsTrue(modelResponse.DataItems.Count == 0);
        }
      
        #endregion
    }
}
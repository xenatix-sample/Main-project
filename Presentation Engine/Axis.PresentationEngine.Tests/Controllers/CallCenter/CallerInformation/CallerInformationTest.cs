using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.CallCenter.ApiControllers;
using Axis.Plugins.CallCenter.Repository.CallerInformation;
using System.Configuration;
using Axis.Plugins.CallCenter;

namespace Axis.PresentationEngine.Tests.Controllers.CallCenter.CallerInformation
{
    [TestClass]
    public class CallerInformationTest
    {
        /// <summary>
        /// The caller header identifier
        /// </summary>
        private long callCenterHeaderID = 1;

        /// <summary>
        /// The controller
        /// </summary>
        private CallerInformationController controller = null;

        /// <summary>
        /// The request model
        /// </summary>
        private CallCenterProgressNoteViewModel requestModel = null;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            controller = new CallerInformationController(new CallerInformationRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
            requestModel = new CallCenterProgressNoteViewModel
            {
                CallCenterHeaderID = 0,
                CallCenterTypeID = 1,
                ClientContactID = 3,
                CallerContactID = 1,
                ProviderID = 1,
                ClientStatusID = 4,
                ClientProviderID = 1,
                Disposition = "Testing",
                IsActive = false,
                Comments = "",
                ReasonCalled = "Testing",
                CallStartTime = System.DateTime.Now,
                CallStatusID = 1,
                ProgramUnitID = 1,
                CountyID = 2,
                ModifiedOn = System.DateTime.Now,
                ModifiedBy = 1,
                SuicideHomicideID = 3

            };

        }

        /// <summary>
        /// Get Caller information Success unit test.
        /// </summary>
        [TestMethod]
        public void GetCallerInformation_Success()
        {
            // Act
            var response = controller.GetCallerInformation(callCenterHeaderID);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one caller information must exist.");
        }

        /// <summary>
        /// Get caller information failed unit test.
        /// </summary>
        [TestMethod]
        public void GetCallerInformation_Failed()
        {
            // Act
            var response = controller.GetCallerInformation(-1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "caller information should not exist");
        }

        /// <summary>
        /// Add caller information success unit test.
        /// </summary>
        [TestMethod]
        public void AddCallerInformation_Success()
        {
            // Act
            var response = controller.AddCallerInformation(requestModel);
            requestModel.ForceRollback = true;
            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Result Code should be 0");
        }

        /// <summary>
        /// Add Caller information success unit test.
        /// </summary>
        [TestMethod]
        public void AddCallerInformation_Failed()
        {
            // Arrange
            requestModel.CallCenterHeaderID = -1;
            requestModel.ForceRollback = true;
            // Act
            var response = controller.AddCallerInformation(requestModel);

            // Assert
            var rowAffected = response.RowAffected;
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Caller information not created");
        }

        /// <summary>
        /// Update Service Recording success unit test.
        /// </summary>
        [TestMethod]
        public void UpdateCallerInformation_Success()
        {
            // Arrange
            requestModel.CallCenterHeaderID = callCenterHeaderID;
            requestModel.Comments = "update caller comment";
            requestModel.ForceRollback = true;

            // Act
            var response = controller.UpdateCallerInformation(requestModel);

            // Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Caller information could not be updated.");
            Assert.IsTrue(response.RowAffected > 0, "Caller information could not be updated.");
        }

        /// <summary>
        /// Update caller information failed unit test.
        /// </summary>
        [TestMethod]
        public void UpdateCallerInformation_Failed()
        {
            // Arrange
            requestModel.CallCenterHeaderID = -1;
            requestModel.Comments = "update test";
            requestModel.ForceRollback = true;

            // Act
            var response = controller.UpdateCallerInformation(requestModel);

            // Assert
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.RowAffected == 0, "Caller information updated.");
        }

    }
}

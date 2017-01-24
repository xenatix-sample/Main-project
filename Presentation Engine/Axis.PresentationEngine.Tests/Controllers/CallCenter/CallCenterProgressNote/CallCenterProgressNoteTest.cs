using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.CallCenter;
using System.Configuration;
using Axis.Plugins.CallCenter.Repository;
using Axis.Plugins.CallCenter.ApiControllers;

namespace Axis.PresentationEngine.Tests.Controllers.CallCenter.CallCenterProgressNote
{
    [TestClass]
    public class CallCenterProgressNoteTest
    {
        /// <summary>
        /// The caller header identifier
        /// </summary>
        private long callCenterHeaderID = 1;

        /// <summary>
        /// The controller
        /// </summary>
        private CallCenterProgressNoteController controller = null;

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
            controller = new CallCenterProgressNoteController(new CallCenterProgressNoteRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
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
        /// Get call center progress note Success unit test.
        /// </summary>
        [TestMethod]
        public void GetCallCenterProgressNote_Success()
        {
            // Act
            var response = controller.GetCallCenterProgressNote(callCenterHeaderID);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one call center progress note must exist.");
        }

        /// <summary>
        /// Get call center progress note failed unit test.
        /// </summary>
        [TestMethod]
        public void GetCallCenterProgressNote_Failed()
        {
            // Act
            var response = controller.GetCallCenterProgressNote(-1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "call center progress note exists for invalid data");
        }

        /// <summary>
        /// Add call center progress note success unit test.
        /// </summary>
        [TestMethod]
        public void AddCallCenterProgressNote_Success()
        {
            //Arrange
            requestModel.CallCenterHeaderID = callCenterHeaderID;

            // Act
            var response = controller.AddCallCenterProgressNote(requestModel);
            requestModel.ForceRollback = true;
            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Result Code should be 0");
        }

        /// <summary>
        /// Add call center progress note failed unit test.
        /// </summary>
        [TestMethod]
        public void AddCallerInformation_Failed()
        {
            // Arrange
            requestModel.CallCenterHeaderID = -1;
            requestModel.ForceRollback = true;
            // Act
            var response = controller.AddCallCenterProgressNote(requestModel);

            // Assert
            var rowAffected = response.RowAffected;
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "call center progress note information not created");
        }

        /// <summary>
        /// Update call center progress note success unit test.
        /// </summary>
        [TestMethod]
        public void UpdateCallCenterProgressNote_Success()
        {
            // Arrange
            requestModel.CallCenterHeaderID = callCenterHeaderID;
            requestModel.Comments = "update progress note comment";
            requestModel.ForceRollback = true;

            // Act
            var response = controller.UpdateCallCenterProgressNote(requestModel);

            // Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0 && response.RowAffected > 0, "call center progress note could not be updated.");
        }

        /// <summary>
        /// Update call center progress note failed unit test.
        /// </summary>
        [TestMethod]
        public void UpdateCallCenterProgressNote_Failed()
        {
            // Arrange
            requestModel.CallCenterHeaderID = -1;
            requestModel.Comments = "update test";
            requestModel.ForceRollback = true;

            // Act
            var response = controller.UpdateCallCenterProgressNote(requestModel);

            // Assert
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.RowAffected == 0, "call center progress note updated.");
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;
using System.Globalization;
using Axis.Model.Clinical;
using Axis.Model.Clinical.PresentIllness;

namespace Axis.RuleEngine.Tests.Controllers.Clinical.PresentIllness
{
    /// <summary>
    /// Test for PresentIllness
    /// </summary>
    [TestClass]
    public class PresentIllnessDetailLiveTest
    {
        private CommunicationManager _communicationManager;
        private const string baseRoute = "PresentIllnessDetail/";
        private long _defaultContactID = 1;
        private long _defaultDeleteID = 10003;

        [TestInitialize]
        public void Initialize()
        {
            _communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            _communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        /// <summary>
        /// The test method for GetHPIDetail success
        /// </summary>
        [TestMethod]
        public void GetHPIDetail_Success()
        {
            //Arrange
            const string url = baseRoute + "GetHPIDetail";
            var param = new NameValueCollection { { "contactID", _defaultContactID.ToString(CultureInfo.InvariantCulture) } };

            //Act
            var response = _communicationManager.Get<Response<PresentIllnessDetailModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one record must exists.");
        }

        /// <summary>
        /// The test method for GetHPIDetail failure
        /// </summary>
        [TestMethod]
        public void GetHPIDetail_Failure()
        {
            //Arrange
            const string url = baseRoute + "GetHPIDetail";
            var param = new NameValueCollection { { "contactID", "-1" } };

            //Act
            var response = _communicationManager.Get<Response<PresentIllnessDetailModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Data exists for invalid data.");
        }

        /// <summary>
        /// The test method for Addhpi detailsuccess
        /// </summary>
        [TestMethod]
        public void AddHPIDetail_Success()
        {
            //Arrange
            const string url = baseRoute + "AddHPIDetail_Success";
            var param = new NameValueCollection();
            var srModel = new PresentIllnessDetailModel
            {
                HPIDetailID = 57,
                HPIID = 1,
                Comment = "Some comment",
                Location = "SOme location",
                Quality = "Living with Family.",
                HPISeverityID = 1,
                Duration = " Some duration",
                Timing = "Some  timing",
                Context = "Some context",
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };

            //Act
            var response = _communicationManager.Post<PresentIllnessDetailModel, Response<PresentIllnessDetailModel>>(srModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 2, "HPI  could not be created.");
        }

        /// <summary>
        /// The test method for AddHPIDetail success
        /// </summary>
        [TestMethod]
        public void AddHPIDetail_Failure()
        {
            //Arrange
            const string url = baseRoute + "AddHPIDetail";
            var param = new NameValueCollection();
            var srModel = new PresentIllnessDetailModel
            {
                HPIDetailID = -57,
                HPIID = 1,
                Comment = "Some comment",
                Location = "SOme location",
                Quality = "Living with Family.",
                HPISeverityID = 1,
                Duration = " Some duration",
                Timing = "Some  timing",
                Context = "Some context",
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };

            //Act
            var response = _communicationManager.Post<PresentIllnessDetailModel, Response<PresentIllnessDetailModel>>(srModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected <= 2, "Presetn Illness created with invalid data.");
        }

        /// <summary>
        /// The test method for UpdateHPIDetail success
        /// </summary>
        [TestMethod]
        public void UpdateHPIDetail_Success()
        {
            //Arrange
            const string url = baseRoute + "UpdateHPIDetail";
            var param = new NameValueCollection();
            var srModel = new PresentIllnessDetailModel
            {
                HPIDetailID = 57,
                HPIID = 1,
                Comment = "Some reasonable comment",
                Location = "SOme different location",
                Quality = "Living with Family.",
                HPISeverityID = 1,
                Duration = " Some duration",
                Timing = "Some  timing",
                Context = "Some context",
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };

            //Act
            var response = _communicationManager.Put<PresentIllnessDetailModel, Response<PresentIllnessDetailModel>>(srModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 0, "Present Illness could not be updated.");
        }

        /// <summary>
        /// The test method for UpdateHPIDetail failure
        /// </summary>
        [TestMethod]
        public void UpdateHPIDetail_Failure()
        {
            //Arrange
            const string url = baseRoute + "UpdateHPIDetail";
            var param = new NameValueCollection();
            var srModel = new PresentIllnessDetailModel
            {

                HPIDetailID = -57,
                HPIID = 1,
                Comment = "Some reasonable comment",
                Location = "SOme different location",
                Quality = "SOme QUality Updated.",
                HPISeverityID = 1,
                Duration = " Some duration",
                Timing = "Some  timing",
                Context = "Some context",
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };

            //Act
            var response = _communicationManager.Put<PresentIllnessDetailModel, Response<PresentIllnessDetailModel>>(srModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 0, "HPI details updated for invalid record.");
        }

        /// <summary>
        /// The test method for DeleteHPIDetail success
        /// </summary>
        [TestMethod]
        public void DeleteHPIDetail_Success()
        {
            //Arrange
            const string apiUrl = baseRoute + "DeleteHPIDetail";
            var param = new NameValueCollection { { "Id", _defaultDeleteID.ToString(CultureInfo.InvariantCulture) } };

            //Act
            var response = _communicationManager.Delete<Response<PresentIllnessDetailModel>>(param, apiUrl);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 2, "HPI details could not be deleted.");
        }

        /// <summary>
        /// The test method for DeleteHPIDetail success
        /// </summary>
        [TestMethod]
        public void DeleteHPIDetail_Failure()
        {
            //Arrange
            const string apiUrl = baseRoute + "DeleteHPIDetail";
            var param = new NameValueCollection();
            param.Add("Id", "-1");

            //Act
            var response = _communicationManager.Delete<Response<PresentIllnessDetailModel>>(param, apiUrl);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected <= 2, "Social Relationship deleted for invalid record.");
        }


    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;
using Axis.Model.Clinical.PresentIllness;
using System.Globalization;

namespace Axis.DataEngine.Tests.Controllers.Clinical.PresentIllness
{
    [TestClass]
    public class PresentIllnessLiveTest
    {
        private CommunicationManager _communicationManager;
        private const string baseRoute = "presentIllness/";
        private long _defaultContactID = 1;
        private long _defaultDeleteContactID = 10003;

        [TestInitialize]
        public void Initialize()
        {
            _communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            _communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        /// <summary>
        /// The test method for GetHPI success
        /// </summary>
        [TestMethod]
        public void GetHPI_Success()
        {
            //Arrange
            const string url = baseRoute + "GetHPI";
            var param = new NameValueCollection { { "contactId", _defaultContactID.ToString(CultureInfo.InvariantCulture) } };

            //Act
            var response = _communicationManager.Get<Response<PresentIllnessModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one HPI must exists.");
        }

        /// <summary>
        /// The test method for GetHPI failure
        /// </summary>
        [TestMethod]
        public void GetHPI_Failure()
        {
            //Arrange
            const string url = baseRoute + "GetHPI";
            var param = new NameValueCollection { { "contactId", "-1" } };

            //Act
            var response = _communicationManager.Get<Response<PresentIllnessModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Data exists for invalid data.");
        }

        /// <summary>
        /// The test method for AddHPI success
        /// </summary>
        [TestMethod]
        public void AddHPI_Success()
        {
            //Arrange
            const string url = baseRoute + "AddHPI";
            var param = new NameValueCollection();
            var piModel = new PresentIllnessModel
            {
                HPIID = 0,
                ContactID = 1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };

            //Act
            var response = _communicationManager.Post<PresentIllnessModel, Response<PresentIllnessModel>>(piModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 2, "Present Illness could not be created.");
        }

        /// <summary>
        /// The test method for Addhpi success
        /// </summary>
        [TestMethod]
        public void AddHPI_Failure()
        {
            //Arrange
            const string url = baseRoute + "AddHPI";
            var param = new NameValueCollection();
            var piModel = new PresentIllnessModel
            {
                HPIID = -1,
                ContactID = -1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };

            //Act
            var response = _communicationManager.Post<PresentIllnessModel, Response<PresentIllnessModel>>(piModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected <= 2, "Present Illness created with invalid data.");
        }

        /// <summary>
        /// The test method for UpdateHPI success
        /// </summary>
        [TestMethod]
        public void UpdateHPI_Success()
        {
            //Arrange
            const string url = baseRoute + "UpdateHPI";
            var param = new NameValueCollection();
            var piModel = new PresentIllnessModel
            {
                HPIID = 10003,
                ContactID = 1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };

            //Act
            var response = _communicationManager.Put<PresentIllnessModel, Response<PresentIllnessModel>>(piModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 2, "Present Illness could not be updated.");
        }

        /// <summary>
        /// The test method for UpdateHPI failure
        /// </summary>
        [TestMethod]
        public void UpdateHPI_Failure()
        {
            //Arrange
            const string url = baseRoute + "UpdateHPI";
            var param = new NameValueCollection();
            var piModel = new PresentIllnessModel
            {
                HPIID = -1,
                ContactID = -1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.MinValue,
                ForceRollback = true
            };

            //Act
            var response = _communicationManager.Put<PresentIllnessModel, Response<PresentIllnessModel>>(piModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 0, "Present Illness updated for invalid record.");
        }

        /// <summary>
        /// The test method for Deletehpi success
        /// </summary>
        [TestMethod]
        public void DeleteHPI_Success()
        {
            //Arrange
            const string apiUrl = baseRoute + "DeleteHPI";
            var param = new NameValueCollection { { "ID", _defaultDeleteContactID.ToString(CultureInfo.InvariantCulture) } };

            //Act
            var response = _communicationManager.Delete<Response<PresentIllnessModel>>(param, apiUrl);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 2, "Present Illness could not be deleted.");
        }

        /// <summary>
        /// The test method for DeleteHPI success
        /// </summary>
        [TestMethod]
        public void DeleteHPI_Failure()
        {
            //Arrange
            const string apiUrl = baseRoute + "DeleteHPI";
            var param = new NameValueCollection();
            param.Add("Id", "-1");

            //Act
            var response = _communicationManager.Delete<Response<PresentIllnessModel>>(param, apiUrl);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected <= 2, "Present Illness deleted for invalid record.");
        }

    }
}

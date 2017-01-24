using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;
using Axis.Model.Clinical.SocialRelationship;
using System.Globalization;

namespace Axis.DataEngine.Tests.Controllers.Clinical.SocialRelationship
{
    [TestClass]
    public class SocialRelationshipLiveTest
    {
        private CommunicationManager _communicationManager;
        private const string baseRoute = "socialRelationship/";
        private long _defaultContactID = 1;
        private long _defaultDeleteContactID = 10003;

        [TestInitialize]
        public void Initialize()
        {
            _communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            _communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        /// <summary>
        /// The test method for GetSocialRelationshipsByContact success
        /// </summary>
        [TestMethod]
        public void GetSocialRelationshipsByContact_Success()
        {
            //Arrange
            const string url = baseRoute + "GetSocialRelationshipsByContact";
            var param = new NameValueCollection { { "contactId", _defaultContactID.ToString(CultureInfo.InvariantCulture) } };

            //Act
            var response = _communicationManager.Get<Response<SocialRelationshipModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Social Relationship must exists.");
        }

        /// <summary>
        /// The test method for GetSocialRelationshipsByContact failure
        /// </summary>
        [TestMethod]
        public void GetSocialRelationshipsByContact_Failure()
        {
            //Arrange
            const string url = baseRoute + "GetSocialRelationshipsByContact";
            var param = new NameValueCollection { { "contactId", "-1" } };

            //Act
            var response = _communicationManager.Get<Response<SocialRelationshipModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Data exists for invalid data.");
        }

        /// <summary>
        /// The test method for AddSocialRelationship success
        /// </summary>
        [TestMethod]
        public void AddSocialRelationship_Success()
        {
            //Arrange
            const string url = baseRoute + "AddSocialRelationship";
            var param = new NameValueCollection();
            var srModel = new SocialRelationshipModel
            {
                SocialRelationshipID = 0,
                ContactID = 1,
                EncounterID = null,
                ReviewedNoChanges = false,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };

            //Act
            var response = _communicationManager.Post<SocialRelationshipModel, Response<SocialRelationshipModel>>(srModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 2, "Social Relationship could not be created.");
        }

        /// <summary>
        /// The test method for AddSocialRelationship success
        /// </summary>
        [TestMethod]
        public void AddSocialRelationship_Failure()
        {
            //Arrange
            const string url = baseRoute + "AddSocialRelationship";
            var param = new NameValueCollection();
            var srModel = new SocialRelationshipModel
            {
                SocialRelationshipID = -1,
                ContactID = -1,
                EncounterID = null,
                ReviewedNoChanges = false,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };

            //Act
            var response = _communicationManager.Post<SocialRelationshipModel, Response<SocialRelationshipModel>>(srModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected <= 2, "Social Relationship created with invalid data.");
        }

        /// <summary>
        /// The test method for UpdateSocialRelationship success
        /// </summary>
        [TestMethod]
        public void UpdateSocialRelationship_Success()
        {
            //Arrange
            const string url = baseRoute + "UpdateSocialRelationship";
            var param = new NameValueCollection();
            var srModel = new SocialRelationshipModel
            {
                SocialRelationshipID = 10003,
                ContactID = 1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };

            //Act
            var response = _communicationManager.Put<SocialRelationshipModel, Response<SocialRelationshipModel>>(srModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 2, "Social Relationship could not be updated.");
        }

        /// <summary>
        /// The test method for UpdateSocialRelationship failure
        /// </summary>
        [TestMethod]
        public void UpdateSocialRelationship_Failure()
        {
            //Arrange
            const string url = baseRoute + "UpdateSocialRelationship";
            var param = new NameValueCollection();
            var srModel = new SocialRelationshipModel
            {
                SocialRelationshipID = -1,
                ContactID = -1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.MinValue,
                ForceRollback = true
            };

            //Act
            var response = _communicationManager.Put<SocialRelationshipModel, Response<SocialRelationshipModel>>(srModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 0, "Social Relationship updated for invalid record.");
        }

        /// <summary>
        /// The test method for DeleteSocialRelationship success
        /// </summary>
        [TestMethod]
        public void DeleteSocialRelationship_Success()
        {
            //Arrange
            const string apiUrl = baseRoute + "DeleteSocialRelationship";
            var param = new NameValueCollection { { "ID", _defaultDeleteContactID.ToString(CultureInfo.InvariantCulture) } };

            //Act
            var response = _communicationManager.Delete<Response<SocialRelationshipModel>>(param, apiUrl);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 2, "Social Relationship could not be deleted.");
        }

        /// <summary>
        /// The test method for DeleteSocialRelationship success
        /// </summary>
        [TestMethod]
        public void DeleteSocialRelationship_Failure()
        {
            //Arrange
            const string apiUrl = baseRoute + "DeleteSocialRelationship";
            var param = new NameValueCollection();
            param.Add("Id", "-1");

            //Act
            var response = _communicationManager.Delete<Response<SocialRelationshipModel>>(param, apiUrl);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected <= 2, "Social Relationship deleted for invalid record.");
        }

    }
}

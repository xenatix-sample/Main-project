using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;
using System.Globalization;
using Axis.Model.Clinical;

namespace Axis.DataEngine.Tests.Controllers.Clinical.SocialRelationship
{
    /// <summary>
    /// Test for Social Relationship
    /// </summary>
    [TestClass]
    public class SocialRelationshipHistoryLiveTest
    {
        private CommunicationManager _communicationManager;
        private const string baseRoute = "SocialRelationshipHistory/";
        //private long _defaultContactID = 1;
        //private long _defaultDeleteID = 10003;

        [TestInitialize]
        public void Initialize()
        {
            _communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            _communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        ///// <summary>
        ///// The test method for GetSocialRelationHistoryByContact success
        ///// </summary>
        //[TestMethod]
        //public void GetSocialRelationHistoryByContact_Success()
        //{
        //    //Arrange
        //    const string url = baseRoute + "GetSocialRelationHistoryByContact";
        //    var param = new NameValueCollection { { "contactID", _defaultContactID.ToString(CultureInfo.InvariantCulture) } };

        //    //Act
        //    var response = _communicationManager.Get<Response<SocialRelationshipHistoryModel>>(param, url);

        //    //Assert
        //    Assert.IsNotNull(response, "Response can not be null");
        //    Assert.IsNotNull(response.DataItems, "DataItems can not be null");
        //    Assert.IsTrue(response.DataItems.Count > 0, "Atleast one record must exists.");
        //}

        ///// <summary>
        ///// The test method for GetSocialRelationHistoryByContact failure
        ///// </summary>
        //[TestMethod]
        //public void GetSocialRelationHistoryByContact_Failure()
        //{
        //    //Arrange
        //    const string url = baseRoute + "GetSocialRelationHistoryByContact";
        //    var param = new NameValueCollection { { "contactID", "-1" } };

        //    //Act
        //    var response = _communicationManager.Get<Response<SocialRelationshipHistoryModel>>(param, url);

        //    //Assert
        //    Assert.IsNotNull(response, "Response can not be null");
        //    Assert.IsNotNull(response.DataItems, "DataItems can not be null");
        //    Assert.IsTrue(response.DataItems.Count == 0, "Data exists for invalid data.");
        //}

        ///// <summary>
        ///// The test method for AddSocialRelationHistory success
        ///// </summary>
        //[TestMethod]
        //public void AddSocialRelationHistory_Success()
        //{
        //    //Arrange
        //    const string url = baseRoute + "AddSocialRelationHistory";
        //    var param = new NameValueCollection();
        //    var srModel = new SocialRelationshipHistoryModel
        //    {
        //        SocialRelationshipDetailID = 1,
        //        SocialRelationShipID = 10003,
        //        FamilyRelationshipID = 1,
        //        ChildhoodHistory = "First Children",
        //        RelationShipHistory = "First RelationShip",
        //        FamilyHistory = "Enjoying with Family.",
        //        IsDeceased = false,
        //        IsInvolved = true,
        //        FirstName = "Peter",
        //        LastName = "Parker",
        //        RelationshipTypeID = 1,
        //        ReviewedNoChanges = false,
        //        ContactID = 1,
        //        EncounterID = null,
        //        TakenBy = 1,
        //        TakenTime = DateTime.Now,
        //        ForceRollback = true
        //    };

        //    //Act
        //    var response = _communicationManager.Post<SocialRelationshipHistoryModel, Response<SocialRelationshipHistoryModel>>(srModel, url);

        //    // Assert
        //    Assert.IsNotNull(response, "Response can not be null");
        //    Assert.IsTrue(response.RowAffected > 2, "Social Relationship could not be created.");
        //}

        ///// <summary>
        ///// The test method for AddSocialRelationHistory success
        ///// </summary>
        //[TestMethod]
        //public void AddSocialRelationHistory_Failure()
        //{
        //    //Arrange
        //    const string url = baseRoute + "AddSocialRelationHistory";
        //    var param = new NameValueCollection();
        //    var srModel = new SocialRelationshipHistoryModel
        //    {
        //        SocialRelationshipDetailID = -1,
        //        SocialRelationShipID = -1,
        //        FamilyRelationshipID = 1,
        //        ChildhoodHistory = "First Children",
        //        RelationShipHistory = "First RelationShip",
        //        FamilyHistory = "Enjoying with Family.",
        //        IsDeceased = false,
        //        IsInvolved = true,
        //        FirstName = "Peter",
        //        LastName = "Parker",
        //        RelationshipTypeID = 1,
        //        ReviewedNoChanges = false,
        //        ContactID = -1,
        //        EncounterID = null,
        //        TakenBy = 1,
        //        TakenTime = DateTime.Now,
        //        ForceRollback = true
        //    };

        //    //Act
        //    var response = _communicationManager.Post<SocialRelationshipHistoryModel, Response<SocialRelationshipHistoryModel>>(srModel, url);

        //    // Assert
        //    Assert.IsNotNull(response, "Response can not be null");
        //    Assert.IsTrue(response.RowAffected <= 2, "Social Relationship created with invalid data.");
        //}

        ///// <summary>
        ///// The test method for UpdateSocialRelationHistory success
        ///// </summary>
        //[TestMethod]
        //public void UpdateSocialRelationHistory_Success()
        //{
        //    //Arrange
        //    const string url = baseRoute + "UpdateSocialRelationHistory";
        //    var param = new NameValueCollection();
        //    var srModel = new SocialRelationshipHistoryModel
        //    {
        //        SocialRelationshipDetailID = 10003,
        //        SocialRelationShipID = 10003,
        //        FamilyRelationshipID = 1,
        //        ChildhoodHistory = "First Children",
        //        RelationShipHistory = "First RelationShip",
        //        FamilyHistory = "Enjoying with Family.",
        //        IsDeceased = false,
        //        IsInvolved = true,
        //        FirstName = "Peter",
        //        LastName = "Parker",
        //        RelationshipTypeID = 1,
        //        ReviewedNoChanges = false,
        //        ContactID = 1,
        //        EncounterID = null,
        //        TakenBy = 1,
        //        TakenTime = DateTime.Now,
        //        ForceRollback = true
        //    };

        //    //Act
        //    var response = _communicationManager.Put<SocialRelationshipHistoryModel, Response<SocialRelationshipHistoryModel>>(srModel, url);

        //    // Assert
        //    Assert.IsNotNull(response, "Response can not be null");
        //    Assert.IsTrue(response.RowAffected > 0, "Social Relationship could not be updated.");
        //}

        ///// <summary>
        ///// The test method for UpdateSocialRelationHistory failure
        ///// </summary>
        //[TestMethod]
        //public void UpdateSocialRelationHistory_Failure()
        //{
        //    //Arrange
        //    const string url = baseRoute + "UpdateSocialRelationHistory";
        //    var param = new NameValueCollection();
        //    var srModel = new SocialRelationshipHistoryModel
        //    {
        //        SocialRelationshipDetailID = -1,
        //        SocialRelationShipID = -1,
        //        FamilyRelationshipID = 1,
        //        ChildhoodHistory = "First Children",
        //        RelationShipHistory = "First RelationShip",
        //        FamilyHistory = "Enjoying with Family.",
        //        IsDeceased = false,
        //        IsInvolved = true,
        //        FirstName = "Peter",
        //        LastName = "Parker",
        //        RelationshipTypeID = 1,
        //        ReviewedNoChanges = false,
        //        ContactID = -1,
        //        EncounterID = null,
        //        TakenBy = 1,
        //        TakenTime = DateTime.Now,
        //        ForceRollback = true
        //    };

        //    //Act
        //    var response = _communicationManager.Put<SocialRelationshipHistoryModel, Response<SocialRelationshipHistoryModel>>(srModel, url);

        //    // Assert
        //    Assert.IsNotNull(response, "Response can not be null");
        //    Assert.IsTrue(response.RowAffected == 0, "Social Relationship details updated for invalid record.");
        //}

        ///// <summary>
        ///// The test method for DeleteSocialRelationHistory success
        ///// </summary>
        //[TestMethod]
        //public void DeleteSocialRelationHistory_Success()
        //{
        //    //Arrange
        //    const string apiUrl = baseRoute + "DeleteSocialRelationHistory";
        //    var param = new NameValueCollection { { "Id", _defaultDeleteID.ToString(CultureInfo.InvariantCulture) } };

        //    //Act
        //    var response = _communicationManager.Delete<Response<SocialRelationshipHistoryModel>>(param, apiUrl);

        //    // Assert
        //    Assert.IsNotNull(response, "Response can not be null");
        //    Assert.IsTrue(response.RowAffected > 2, "Social Relationship details could not be deleted.");
        //}

        ///// <summary>
        ///// The test method for DeleteSocialRelationHistory success
        ///// </summary>
        //[TestMethod]
        //public void DeleteSocialRelationHistory_Failure()
        //{
        //    //Arrange
        //    const string apiUrl = baseRoute + "DeleteSocialRelationHistory";
        //    var param = new NameValueCollection();
        //    param.Add("Id", "-1");

        //    //Act
        //    var response = _communicationManager.Delete<Response<SocialRelationshipHistoryModel>>(param, apiUrl);

        //    // Assert
        //    Assert.IsNotNull(response, "Response can not be null");
        //    Assert.IsTrue(response.RowAffected <= 2, "Social Relationship deleted for invalid record.");
        //}

        ///// <summary>
        ///// GetSocialRelationshipDetail success test case
        ///// </summary>
        //[TestMethod]
        //public void GetSocialRelationshipDetail_Success()
        //{
        //    //Arrange
        //    var socialRelationshipID = 10003;
        //    const string apiUrl = baseRoute + "GetSocialRelationshipDetail";
        //    var requestId = new NameValueCollection { { "socialRelationshipID", socialRelationshipID.ToString(CultureInfo.InvariantCulture) } };
        //    //Act
        //    var response = _communicationManager.Get<Response<SocialRelationshipHistoryModel>>(requestId, apiUrl);

        //    //Assert
        //    Assert.IsNotNull(response, "Response can not be null");
        //    Assert.IsNotNull(response.DataItems, "DataItems can not be null");
        //    Assert.IsTrue(response.DataItems.Count > 0, "Atleast one record should exists.");
        //}

        ///// <summary>
        ///// GetSocialRelationshipDetail success test case
        ///// </summary>
        //[TestMethod]
        //public void GetSocialRelationshipDetail_Failure()
        //{
        //    //Arrange
        //    var socialRelationshipID = -1;
        //    const string apiUrl = baseRoute + "GetSocialRelationshipDetail";
        //    var requestId = new NameValueCollection { { "socialRelationshipID", socialRelationshipID.ToString(CultureInfo.InvariantCulture) } };
        //    //Act
        //    var response = _communicationManager.Get<Response<SocialRelationshipHistoryModel>>(requestId, apiUrl);

        //    //Assert
        //    Assert.IsNotNull(response, "Response can not be null");
        //    Assert.IsNotNull(response.DataItems, "DataItems can not be null");
        //    Assert.IsTrue(response.DataItems.Count == 0, "Record exists for invalid data.");
        //}

    }
}

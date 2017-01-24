using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;
using Axis.Model.ECI;
using System.Globalization;

namespace Axis.RuleEngine.Tests.Controllers.ECI.IFSP
{
    [TestClass]
    public class IFSPLiveTest
    {
        private CommunicationManager communicationManager;
        private const string baseRoute = "ifsp/";
        private long _defaultContactID = 1;
        private long _defaultDeleteContactID = 11;

        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        /// <summary>
        /// The test method for GetIFSPList success
        /// </summary>
        [TestMethod]
        public void GetIFSPList_Success()
        {   
            //Arrange
            const string url = baseRoute + "GetIFSPList";
            var param = new NameValueCollection { { "contactId", _defaultContactID.ToString(CultureInfo.InvariantCulture) } };

            //Act
            var response = communicationManager.Get<Response<IFSPDetailModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one IFSP must exists.");
        }

        /// <summary>
        /// The test method for GetIFSPList failure
        /// </summary>
        [TestMethod]
        public void GetIFSPList_Failure()
        {
            //Arrange
            const string url = baseRoute + "GetIFSPList";
            var param = new NameValueCollection { { "contactId", "-1" } };

            //Act
            var response = communicationManager.Get<Response<IFSPDetailModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Data exists for invalid data.");
        }

        /// <summary>
        /// The test method for AddIFSP success
        /// </summary>
        [TestMethod]
        public void AddIFSP_Success()
        {
            //Arrange
            const string url = baseRoute + "AddIFSP";
            var param = new NameValueCollection();
            var ifspModel = new IFSPDetailModel
            {
                IFSPID = 1,
                ContactID = 1,
                IFSPTypeID = 1,
                IFSPMeetingDate = DateTime.Now,
                IFSPFamilySignedDate = DateTime.Now,
                MeetingDelayed = true,
                ReasonForDelayID = 1,
                Comments = "Success test case",
                AssessmentID = 1,
                ResponseID = 1,
                ForceRollback = true
            };

            //Act
            var response = communicationManager.Post<IFSPDetailModel, Response<IFSPDetailModel>>(ifspModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 0, "IFSP could not be created.");
        }

        /// <summary>
        /// The test method for AddIFSP success
        /// </summary>
        [TestMethod]
        public void AddIFSP_Failure()
        {
            //Arrange
            const string url = baseRoute + "AddIFSP";
            var param = new NameValueCollection();
            var ifspModel = new IFSPDetailModel
            {
                IFSPID = 1,
                ContactID = -1,
                IFSPTypeID = -1,
                IFSPMeetingDate = DateTime.MinValue,
                IFSPFamilySignedDate = DateTime.Now,
                MeetingDelayed = true,
                ReasonForDelayID = 1,
                Comments = "Failure test case",
                AssessmentID = 1,
                ResponseID = 1,
                ForceRollback = true
            };

            //Act
            var response = communicationManager.Post<IFSPDetailModel, Response<IFSPDetailModel>>(ifspModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 0, "IFSP created with invalid data.");
        }

        /// <summary>
        /// The test method for UpdateIFSP success
        /// </summary>
        [TestMethod]
        public void UpdateIFSP_Success()
        {
            //Arrange
            const string url = baseRoute + "UpdateIFSP";
            var param = new NameValueCollection();
            var ifspModel = new IFSPDetailModel
            {
                IFSPID = 12,
                ContactID = 1,
                IFSPTypeID = 1,
                IFSPMeetingDate = DateTime.Now,
                IFSPFamilySignedDate = DateTime.Now,
                MeetingDelayed = true,
                ReasonForDelayID = 1,
                Comments = "Success test case",
                AssessmentID = 1,
                ResponseID = 1,
                ForceRollback = true
            };

            //Act
            var response = communicationManager.Put<IFSPDetailModel, Response<IFSPDetailModel>>(ifspModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 0, "IFSP could not be updated.");
        }
        
        /// <summary>
        /// The test method for UpdateIFSP failure
        /// </summary>
        [TestMethod]
        public void UpdateIFSP_Failure() 
        {
            //Arrange
            const string url = baseRoute + "UpdateIFSP";
            var param = new NameValueCollection();
            var ifspModel = new IFSPDetailModel
            {
                IFSPID = -1,
                ContactID = -1,
                IFSPTypeID = 1,
                IFSPMeetingDate = DateTime.MinValue,
                IFSPFamilySignedDate = DateTime.Now,
                MeetingDelayed = true,
                ReasonForDelayID = 1,
                Comments = "Failure test case",
                AssessmentID = 1,
                ResponseID = 1,
                ForceRollback = true
            };

            //Act
            var response = communicationManager.Put<IFSPDetailModel, Response<IFSPDetailModel>>(ifspModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 0, "IFSP updated for invalid record.");
        }

        /// <summary>
        /// The test method for RemoveIFSP success
        /// </summary>
        [TestMethod]
        public void RemoveIFSP_Success()
        {
            //Arrange
            const string apiUrl = baseRoute + "RemoveIFSP";
            var param = new NameValueCollection { { "ifspID", _defaultDeleteContactID.ToString(CultureInfo.InvariantCulture) } };

            //Act
            var response = communicationManager.Delete<Response<bool>>(param, apiUrl);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 0, "IFSP could not be deleted.");
        }

        /// <summary>
        /// The test method for RemoveIFSP success
        /// </summary>
        [TestMethod]
        public void RemoveIFSP_Failure()
        {
            //Arrange
            const string apiUrl = baseRoute + "RemoveIFSP";
            var param = new NameValueCollection();
            param.Add("ifspID", "-1");

            //Act
            var response = communicationManager.Delete<Response<bool>>(param, apiUrl);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 0, "IFSP deleted for invalid record.");
        }

    }
}

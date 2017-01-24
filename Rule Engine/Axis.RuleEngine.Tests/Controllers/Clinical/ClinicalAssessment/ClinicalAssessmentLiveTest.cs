using Axis.Model.Common;
using Axis.Model.Clinical;
using Axis.Model.Clinical.Assessment;
using Axis.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;

namespace Axis.RuleEngine.Tests.Controllers.Clinical
{
    /// <summary>
    /// Test class for ClinicalAssessment
    /// </summary>
    [TestClass]
    public class ClinicalAssessmentLiveTest
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "ClinicalAssessment/";

        /// <summary>
        /// The ContactId
        /// </summary>
        private int contactID = 1;
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        /// <summary>
        /// Success test case for GetClinicalAssessments
        /// </summary>
        [TestMethod]
        public void GetClinicalAssessments_Success()
        {
            var clinicalAssessmentID = 6;

            // Arrange
            var url = baseRoute + "GetClinicalAssessments";
            var param = new NameValueCollection();
            param.Add("clinicalAssessmentID", clinicalAssessmentID.ToString());

            // Act
            var response = communicationManager.Get<Response<ClinicalAssessmentModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one clinical assesment must exists.");
        }


        /// <summary>
        /// Failed test case for GetClinicalAssessments
        /// </summary>
        [TestMethod]
        public void GetClinicalAssessments_Failed()
        {
            var clinicalAssessmentID = -1;

            // Arrange
            var url = baseRoute + "getClinicalAssessments";
            var param = new NameValueCollection();
            param.Add("clinicalAssessmentID", clinicalAssessmentID.ToString());

            // Act
            var response = communicationManager.Get<Response<ClinicalAssessmentModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Atleast one clinical assesment exists.");
        }

        /// <summary>
        /// Success test case for GetClinicalAssessmentsByContact
        /// </summary>
        [TestMethod]
        public void GetClinicalAssessmentsByContact_Success()
        {
           

            // Arrange
            var url = baseRoute + "getClinicalAssessmentsByContact";
            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());

            // Act
            var response = communicationManager.Get<Response<ClinicalAssessmentModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one clinical assesment must exists.");
        }

        /// <summary>
        /// Failed test case for GetClinicalAssessmentsByContact
        /// </summary>
        [TestMethod]
        public void GetClinicalAssessmentsByContact_Failed()
        {
            contactID = -1;

            // Arrange
            var url = baseRoute + "getClinicalAssessmentsByContact";
            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());

            // Act
            var response = communicationManager.Get<Response<ClinicalAssessmentModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Atleast one clinical assesment exists.");
        }


        /// <summary>
        /// Success test case for AddAssessment
        /// </summary>
        [TestMethod]
        public void AddAssessment_Success()
        {
            // Arrange
            var url = baseRoute + "addAssessment";

            //Arrange
            var assessment = new ClinicalAssessmentModel
            {                
                AssessmentID = 4,
                ContactID = 1,
                TakenTime = DateTime.UtcNow,
                TakenBy = 1,
                ResponseID = 1
            };

            // Act
            var response = communicationManager.Post<ClinicalAssessmentModel, Response<ClinicalAssessmentModel>>(assessment, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Add clinical assessment could not be created.");
        }

        /// <summary>
        /// Failed test case for AddAssessment
        /// </summary>
        [TestMethod]
        public void AddAssessment_Failed()
        {
            // Arrange
            var url = baseRoute + "addAssessment";

            //Arrange
            var assessment = new ClinicalAssessmentModel
            {
                AssessmentID = 4,
                ContactID = -1,
                TakenTime = DateTime.UtcNow,
                TakenBy = 1,
                ResponseID = -1
            };

            // Act
            var response = communicationManager.Post<ClinicalAssessmentModel, Response<ClinicalAssessmentModel>>(assessment, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Add clinical assessment expected to be failed");
        }

        /// <summary>
        /// Success test case for UpdateAssessment
        /// </summary>
        [TestMethod]
        public void UpdateAssessment_Success()
        {
            // Arrange
            var url = baseRoute + "updateAssessment";

            //Arrange
            var assessment = new ClinicalAssessmentModel
            {
                ClinicalAssessmentID=1,
                AssessmentID = 4,
                ContactID = 1,
                TakenTime = DateTime.UtcNow,
                TakenBy = 1,
                ResponseID = 1
            };

            // Act
            var response = communicationManager.Put<ClinicalAssessmentModel, Response<ClinicalAssessmentModel>>(assessment, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Update clinical assessment could not be Updated.");
        }

        /// <summary>
        /// Failed test case for UpdateAssessment
        /// </summary>
        [TestMethod]
        public void UpdateAssessment_Failed()
        {
            // Arrange
            var url = baseRoute + "updateAssessment";

            //Arrange
            var assessment = new ClinicalAssessmentModel
            {
                ClinicalAssessmentID = -1,
                AssessmentID = 4,
                ContactID = -1,
                TakenTime = DateTime.UtcNow,
                TakenBy = 1,
                ResponseID = 1
            };

            // Act
            var response = communicationManager.Put<ClinicalAssessmentModel, Response<ClinicalAssessmentModel>>(assessment, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Update clinical assessment could be Updated.");
        }

        //Success test case of DeleteAssessment
        [TestMethod]
        public void DeleteAssessment_Success()
        {
            const long clinicalAssessmentId = 2;
            //Arrange
            var url = baseRoute + "DeleteAssessment";
            var param = new NameValueCollection();
            param.Add("Id", clinicalAssessmentId.ToString(CultureInfo.InvariantCulture));

            //Act
            var response = communicationManager.Delete<Response<ClinicalAssessmentModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can not be null.");
            Assert.IsTrue(response.RowAffected > 2, "Clinical Assessment deleted for invalid record.");
        }

        //Failed test case of DeleteAssessment
        [TestMethod]
        public void DeleteAssessment_Failed()
        {
            const long clinicalAssessmentId = -1;
            //Arrange
            var url = baseRoute + "DeleteAssessment";
            var param = new NameValueCollection();
            param.Add("Id", clinicalAssessmentId.ToString(CultureInfo.InvariantCulture));

            //Act
            var response = communicationManager.Delete<Response<ClinicalAssessmentModel>>(param, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 2, "Clinical Assessment deleted for invalid record.");

        }

    }
}

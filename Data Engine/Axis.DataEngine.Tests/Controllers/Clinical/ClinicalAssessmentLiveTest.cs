using Axis.Model.Common;
using Axis.Model.Clinical.Assessment;
using Axis.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace Axis.DataEngine.Tests.Controllers.Clinical
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
        private const string baseRoute = "clinicalAssessment/";

        /// <summary>
        /// The clinicalAssessmentId
        /// </summary>
        private int clinicalAssessmentID = 1;

        /// <summary>
        /// The contactId
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

            // Arrange
            var url = baseRoute + "getClinicalAssessments";

            var param = new NameValueCollection();
            param.Add("clinicalAssessmentID", clinicalAssessmentID.ToString());

            // Act
            var response = communicationManager.Get<Response<ClinicalAssessmentModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one clinical assessment must exists.");
        }

        /// <summary>
        /// Failed test case for GetClinicalAssessments
        /// </summary>
        [TestMethod]
        public void GetClinicalAssessments_Failed()
        {

            // Arrange
            clinicalAssessmentID = -1;
            var url = baseRoute + "getClinicalAssessments";

            var param = new NameValueCollection();
            param.Add("clinicalAssessmentID", clinicalAssessmentID.ToString());

            // Act
            var response = communicationManager.Get<Response<ClinicalAssessmentModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
         
            Assert.IsTrue(response.DataItems.Count == 0, "Atleast one clinical assessment exists.");
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
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one clinical assessment must exists.");
        }

        /// <summary>
        /// Failed test case for GetClinicalAssessmentsByContact
        /// </summary>
        [TestMethod]
        public void GetClinicalAssessmentsByContact_Failed()
        {

            // Arrange
            contactID = -1;
            var url = baseRoute + "getClinicalAssessmentsByContact";
            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());

            // Act
            var response = communicationManager.Get<Response<ClinicalAssessmentModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Atleast one clinical assessment exists.");
        }


        /// <summary>
        /// Success test case for AddAssessment
        /// </summary>
        [TestMethod]
        public void AddAssessment_Success()
        {

            // Arrange
            var url = baseRoute + "addAssessment";
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
            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Clinical assessment could not be created.");
        }

        /// <summary>
        /// Failed test case for AddAssessment
        /// </summary>
        [TestMethod]
        public void AddAssessment_Failed()
        {

            // Arrange
            var url = baseRoute + "addAssessment";
            var assessment = new ClinicalAssessmentModel
            {
                AssessmentID = 4,
                ContactID = -1,
                TakenTime = DateTime.UtcNow,
                TakenBy = 1,
                ResponseID = 1
            };

            // Act
            var response = communicationManager.Post<ClinicalAssessmentModel, Response<ClinicalAssessmentModel>>(assessment, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "clinical assessment has been created for invalid contact.");
        }

        /// <summary>
        /// Success test case for UpdateAssessment
        /// </summary>
        [TestMethod]
        public void UpdateAssessment_Success()
        {

            // Arrange
            var url = baseRoute + "updateAssessment";
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
            Assert.IsTrue(response.ResultCode == 0, "Clinical assessment could not be updated.");
        }

        /// <summary>
        /// Failed test case for AddAssessment
        /// </summary>
        [TestMethod]
        public void UpdateAssessment_Failed()
        {

            // Arrange
            var url = baseRoute + "updateAssessment";
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
            Assert.IsTrue(response.ResultCode != 0, "Clinical assessment has been updated with invalid data.");
        }

        /// <summary>
        /// Success test case for DeleteAssessment
        /// </summary>
        [TestMethod]
        public void DeleteAssessment_Success()
        {

            // Arrange
            var url = baseRoute + "deleteAssessment";
            var param = new NameValueCollection();
            param.Add("Id", clinicalAssessmentID.ToString());
                    
            // Act
            var response = communicationManager.Delete<Response<ClinicalAssessmentModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.ResultCode == 0, "Clinical assessment could not be deleted.");
            Assert.IsTrue(response.RowAffected > 0, "Clinical assessment could not be deleted.");
        }

        /// <summary>
        /// Failed test case for DeleteAssessment
        /// </summary>
        [TestMethod]
        public void DeleteAssessment_Failed()
        {

            // Arrange
            var url = baseRoute + "deleteAssessment";
            var param = new NameValueCollection();
            param.Add("Id", "-1");

            // Act
            var response = communicationManager.Delete<Response<ClinicalAssessmentModel>>(param, url);

            //Assert            
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.RowAffected == 0, "Clinical assessment has been deleted which does not exist.");
        }
    }
}

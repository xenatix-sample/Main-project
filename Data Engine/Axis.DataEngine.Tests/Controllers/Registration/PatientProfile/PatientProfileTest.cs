using Axis.DataEngine.Helpers.Results;
using Axis.DataEngine.Plugins.Registration;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Axis.DataEngine.Tests.Controllers.Registration.PatientProfile
{
    /// <summary>
    /// Mock test method for Patient Profile
    /// </summary>
    [TestClass]
    public class PatientProfileTest
    {

        #region Variable

        /// <summary>
        /// The Patient Profile Data Provider
        /// </summary>
        private IPatientProfileDataProvider patientProfileDataProvider;

        /// <summary>
        /// The contact identifier
        /// </summary>
        private long contactId = 1;
       
        /// <summary>
        /// The Patient Profile Model
        /// </summary>
        private PatientProfileModel patientProfileModel = null;
        private PatientProfileController patientProfileController = null;

        /// <summary>
        /// The empty  Patient Profile Model
        /// </summary>
        //private PatientProfileModel emptyPatientProfileModel = null;
     
        #endregion

        #region Private Method


        /// <summary>
        ///Patient Profile Success
        /// </summary>
        private void  PatientProfile_Success()
        {
            Mock<IPatientProfileDataProvider> mock = new Mock<IPatientProfileDataProvider>();
            patientProfileDataProvider = mock.Object;

            var patientProfileModels = new List<PatientProfileModel>();
           patientProfileModel = new PatientProfileModel()
            {
                ContactID = 1,
                ClientTypeID = 1,
                FirstName = "FirstName",
                Middle = "MiddleName",
                LastName = "LastName",
                GenderID = 1,
                DOB = DateTime.Now,
                PreferredName = "PreferredName",
                
            };

           patientProfileModels.Add(patientProfileModel);
           var financialAssessment = new Response<PatientProfileModel>()
            {
                DataItems = patientProfileModels,
                RowAffected = 1
            };


           //Get PatientProfile
           Response<PatientProfileModel> patientProfileResponse = new Response<PatientProfileModel>();
           patientProfileResponse.DataItems = patientProfileModels.Where(contact => contact.ContactID == contactId).ToList();

            mock.Setup(r => r.GetPatientProfile(It.IsAny<long>()))
                .Returns(patientProfileResponse);
            
        }

        /// <summary>
        /// Patient Profile Failed
        /// </summary>
        private void PatientProfile_Failed()
        {
            Mock<IPatientProfileDataProvider> mock = new Mock<IPatientProfileDataProvider>();
            patientProfileDataProvider = mock.Object;

            var patientProfileModels = new List<PatientProfileModel>();
            patientProfileModel = new PatientProfileModel()
            {
                ContactID = 0,
                ClientTypeID = 1,
                FirstName = "FirstName",
                Middle = "MiddleName",
                LastName = "LastName",
                GenderID = 1,
                DOB = DateTime.Now,
                PreferredName = "PreferredName",

            };

            patientProfileModels.Add(patientProfileModel);
            var financialAssessment = new Response<PatientProfileModel>()
            {
                DataItems = null,
                RowAffected = 0
            };

            //Get PatientProfile
            Response<PatientProfileModel> patientProfileResponse = new Response<PatientProfileModel>();
            patientProfileResponse.DataItems = patientProfileModels.Where(contact => contact.ContactID == contactId).ToList();

            mock.Setup(r => r.GetPatientProfile(It.IsAny<long>()))
                .Returns(patientProfileResponse);
        }

        #endregion

        #region Public Method



        /// <summary>
        /// Gets the Patient Profile Success
        /// </summary>
        [TestMethod]
        public void GetPatientProfile_Success()
        {
            // Arrange
            PatientProfile_Success();
             
            patientProfileController = new PatientProfileController(patientProfileDataProvider);

            //Act
            var getPatientProfileResult = patientProfileController.GetPatientProfile(contactId);
            var response = getPatientProfileResult as HttpResult<Response<PatientProfileModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");            
        }

        /// <summary>
        /// Gets the Patient Profile Failed
        /// </summary>
        [TestMethod]
        public void GetFinancialAssessment_Failed()
        {
            // Arrange
            PatientProfile_Failed();
            patientProfileController = new PatientProfileController(patientProfileDataProvider);

            //Act
            var getPatientProfileResult = patientProfileController.GetPatientProfile(0);
            var response = getPatientProfileResult as HttpResult<Response<PatientProfileModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.DataItems.Count == 0);
        }       

        #endregion
    }
}
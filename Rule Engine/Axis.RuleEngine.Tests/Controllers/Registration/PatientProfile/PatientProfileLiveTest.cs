using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using Axis.Model.Registration;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;

namespace Axis.RuleEngine.Tests.Controllers.Registration.PatientProfile
{
    /// <summary>
    /// Live test method for Patient Profile
    /// </summary>
    [TestClass]
    public class PatientProfileLiveTest
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "PatientProfile/";

        /// <summary>
        /// The contact identifier
        /// </summary>
        private long contactId = 1;
        
       
        #endregion Class Variables

        #region Test Methods

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
        /// Gets the Patient Profile success.
        /// </summary>
        [TestMethod]
        public void GetPatientProfile_Success()
        {
            //Arrenge
            var url = baseRoute + "GetPatientProfile";
            var param = new NameValueCollection();
            param.Add("contactId", contactId.ToString());

            //Act
            var response = communicationManager.Get<Response<FinancialAssessmentModel>>(param, url);

            //Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Patient Profile record must exists.");
        }

        /// <summary>
        /// Gets the Patient Profile failed.
        /// </summary>
        [TestMethod]
        public void GetPatientProfile_Failed()
        {
            //Arrenge
            var url = baseRoute + "GetPatientProfile";
            var param = new NameValueCollection();
            param.Add("contactId", "0");

            //Act
            var response = communicationManager.Get<Response<FinancialAssessmentModel>>(param, url);

            //Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0);
        }

        #endregion
    }
}

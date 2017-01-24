using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using System.Configuration;

namespace Axis.DataEngine.Tests.Controllers
{
    [TestClass]
    public class AdditionalDemographyLiveTest
    {
        private CommunicationManager communicationManager;
        private const string baseRoute = "additionalDemographic/";        
        private long contactId = 1;

        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        [TestMethod]
        public void GetAdditionalDemographic_Success()
        {
            var url = baseRoute + "getAdditionalDemographic";

            var param = new NameValueCollection();
            param.Add("contactId", contactId.ToString());

            var response = communicationManager.Get<Response<AdditionalDemographicsModel>>(param, url);

            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one additional demography must exists.");
        }

        [TestMethod]
        public void AddAdditionalDemographic_Success()
        {
            var url = baseRoute + "addAdditionalDemographic";

            //Add Additional Demographic
            var addAdditionalDemographic = new AdditionalDemographicsModel
            {
                ContactID = 1,
                Name = "Joe Smith",
                MRN = 123456,
                EthnicityID = 2
            };

            var response = communicationManager.Post<AdditionalDemographicsModel, Response<AdditionalDemographicsModel>>(addAdditionalDemographic, url);

            Assert.IsTrue(response.RowAffected > 0, "Additional Demography could not be created.");
        }

        [TestMethod]
        public void UpdateAdditionalDemographic_Success()
        {
            var url = baseRoute + "updateAdditionalDemographic";

            //Update Additional Demographic
            var updateAdditionalDemographic = new AdditionalDemographicsModel
            {
                ContactID = 4,
                Name = "Joe Smith",
                MRN = 123456,
                EthnicityID = 3
            };

            var response = communicationManager.Post<AdditionalDemographicsModel, Response<AdditionalDemographicsModel>>(updateAdditionalDemographic, url);

            Assert.IsTrue(response.RowAffected > 0, "Additional Demography could not be updated.");
        }
    }
}
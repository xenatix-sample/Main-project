using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.Registration.ApiControllers;
using Axis.Plugins.Registration.Repository;
using System.Web.Mvc;
using Axis.Plugins.Registration.Model;
using Axis.Model.Common;
using System.Configuration;


namespace Axis.PresentationEngine.Tests.Controllers
{
    [TestClass]
    public class AdditionalDemographicTest
    {
        private long contactId = 1;
        private AdditionalDemographicController controller = null;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            controller = new AdditionalDemographicController(new AdditionalDemographicRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        [TestMethod]
        public void GetAdditionalDemographic_Success()
        {
            // Act
            var response = controller.GetAdditionalDemographic(contactId).Result;

            // Assert
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one additional demography must exists.");
        }

        [TestMethod]
        public void AddAdditionalDemographic_Success()
        {
            // Act
            var additionalDemographicsViewModel = new AdditionalDemographicsViewModel
            {
                ContactID = 1,
                EthnicityID = 1,
                ForceRollback = true
            };

            var response = controller.AddAdditionalDemographic(additionalDemographicsViewModel);

            // Assert
            Assert.IsTrue(response.RowAffected > 0, "Additional Demography could not be created.");
        }

        [TestMethod]
        public void UpdateAdditionalDemographic_Success()
        {
            // Act
            var additionalDemographicsViewModel = new AdditionalDemographicsViewModel 
            {
                ContactID = 1, 
                Name = "Test1", 
                MRN = 124586, 
                EthnicityID = 1, 
                ForceRollback = true 
            };

            var response = controller.UpdateAdditionalDemographic(additionalDemographicsViewModel);

            // Assert
            Assert.IsTrue(response.RowAffected > 0, "Additional Demography could not be updated.");
        }
    }
}

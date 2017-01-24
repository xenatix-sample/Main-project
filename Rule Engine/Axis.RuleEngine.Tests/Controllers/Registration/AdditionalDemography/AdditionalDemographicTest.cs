using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Axis.RuleEngine.Registration;
using System.Collections.Generic;
using Axis.Model.Registration;
using Axis.Model.Common;
using System.Linq;
using Axis.RuleEngine.Plugins.Registration;
using Axis.RuleEngine.Helpers.Results;

namespace Axis.RuleEngine.Tests.Controllers
{
    [TestClass]
    public class AdditionalDemographicTest
    {
        private IAdditionalDemographicRuleEngine additionalDemographicRuleEngine;
        private long contactId = 1;
        private AdditionalDemographicController additionalDemographicController = null;

        [TestInitialize]
        public void Initialize()
        {
            Mock<IAdditionalDemographicRuleEngine> mock = new Mock<IAdditionalDemographicRuleEngine>();
            additionalDemographicRuleEngine = mock.Object;

            additionalDemographicController = new AdditionalDemographicController(additionalDemographicRuleEngine);

            var additionalDemographics = new List<AdditionalDemographicsModel>();
            additionalDemographics.Add(new AdditionalDemographicsModel()
            {
                ContactID = 1,
                Name = "Joe Smith",
                MRN = 123456,
                EthnicityID = 1
            });
            additionalDemographics.Add(new AdditionalDemographicsModel()
            {
                ContactID = 2,
                Name = "Joe Smith",
                MRN = 123456,
                EthnicityID = 1
            });
            additionalDemographics.Add(new AdditionalDemographicsModel()
            {
                ContactID = 3,
                Name = "Joe Smith",
                MRN = 123456,
                EthnicityID = 1
            });

            var alladditionalDemography = new Response<AdditionalDemographicsModel>()
            {
                DataItems = additionalDemographics
            };

            //Get Additional Demographic
            Response<AdditionalDemographicsModel> additionalDemographicResponse = new Response<AdditionalDemographicsModel>();
            additionalDemographicResponse.DataItems = additionalDemographics.Where(x => x.ContactID == contactId).ToList();

            mock.Setup(r => r.GetAdditionalDemographic(It.IsAny<long>()))
                .Returns(additionalDemographicResponse);

            //Add AdditionalDemographic
            mock.Setup(r => r.AddAdditionalDemographic(It.IsAny<AdditionalDemographicsModel>()))
                .Callback((AdditionalDemographicsModel additionalDemographicsModel) => additionalDemographics.Add(additionalDemographicsModel))
                .Returns(alladditionalDemography);
           
            //Update AdditionalDemographic
            mock.Setup(r => r.UpdateAdditionalDemographic(It.IsAny<AdditionalDemographicsModel>()))
                .Callback((AdditionalDemographicsModel additionalDemographicsModel) => additionalDemographics.Add(additionalDemographicsModel))
                .Returns(alladditionalDemography);
       
        }

        [TestMethod]
        public void GetAdditionalDemographic_Success()
        {
            //Act
            var getadditionalDemographicResult = additionalDemographicController.GetAdditionalDemographic(contactId);
            var response = getadditionalDemographicResult as HttpResult<Response<AdditionalDemographicsModel>>;
            var additionalDemography = response.Value.DataItems;
            var count = additionalDemography.Count();

            //Assert
            Assert.IsNotNull(additionalDemography);
            Assert.IsTrue(additionalDemography != null, "Data items can't be null");
            Assert.IsTrue(count > 0, "Atleast one additional demography must exists.");
        }

        [TestMethod]
        public void AddAdditionalDemographic_Success()
        {
            //Act
            var addAdditionalDemographic = new AdditionalDemographicsModel 
            { 
                ContactID = 4, 
                Name = "Test4",
                MRN = 123456
            };
            var addAdditionalDemographicResult = additionalDemographicController.AddAdditionalDemographic(addAdditionalDemographic);
            var response = addAdditionalDemographicResult as HttpResult<Response<AdditionalDemographicsModel>>;
            var additionalDemography = response.Value.DataItems;
            var count = additionalDemography.Count();

            //Assert
            Assert.IsNotNull(additionalDemography);
            Assert.IsTrue(count > 0, "Additional Demography could not be created.");
        }

        [TestMethod]
        public void UpdateAdditionalDemographic_Success()
        {
            //Act
            var updateAdditionalDemographic = new AdditionalDemographicsModel 
            {
                ContactID = 4, 
                Name = "Test4Update",
                MRN = 123456
            };
            var updateAdditionalDemographicResult = additionalDemographicController.UpdateAdditionalDemographic(updateAdditionalDemographic);
            var response = updateAdditionalDemographicResult as HttpResult<Response<AdditionalDemographicsModel>>;
            var additionalDemography = response.Value.DataItems;
            var count = additionalDemography.Count();

            //Assert
            Assert.IsNotNull(additionalDemography);
            Assert.IsTrue(count > 0, "Additional Demography could not be updated.");
        }
    }
}

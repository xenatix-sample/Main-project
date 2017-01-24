using System;
using Axis.DataEngine.Helpers.Results;
using Axis.DataEngine.Plugins.Registration;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Axis.DataEngine.Tests.Controllers
{
    /// <summary>
    /// Moq unit test for additional demography
    /// </summary>
    [TestClass]
    public class AdditionalDemographicTest
    {
        /// <summary>
        /// The additional demographic data provider
        /// </summary>
        private IAdditionalDemographicDataProvider additionalDemographicDataProvider;

        /// <summary>
        /// The contact identifier
        /// </summary>
        private long contactId = 1;

        /// <summary>
        /// The additional demographic controller
        /// </summary>
        private AdditionalDemographicController additionalDemographicController = null;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            Mock<IAdditionalDemographicDataProvider> mock = new Mock<IAdditionalDemographicDataProvider>();
            additionalDemographicDataProvider = mock.Object;

            additionalDemographicController = new AdditionalDemographicController(additionalDemographicDataProvider);

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
                DataItems = additionalDemographics,
                RowAffected = 1
            };

            //Get AdditionalDemographic
            Response<AdditionalDemographicsModel> additionalDemographicResponse = new Response<AdditionalDemographicsModel>();
            additionalDemographicResponse.DataItems = additionalDemographics.Where(contact => contact.ContactID == contactId).ToList();
            additionalDemographicResponse.RowAffected = 1;


            mock.Setup(r => r.GetAdditionalDemographic(It.IsAny<long>()))
                .Returns(additionalDemographicResponse);

            //Add AdditionalDemographic
            mock.Setup(r => r.AddAdditionalDemographic(It.IsAny<AdditionalDemographicsModel>()))
                .Callback((AdditionalDemographicsModel additionalDemographicsModel) => additionalDemographics.Add(additionalDemographicsModel))
                .Returns(additionalDemographicResponse);

            //Update AdditionalDemographic
            mock.Setup(r => r.UpdateAdditionalDemographic(It.IsAny<AdditionalDemographicsModel>()))
                .Callback((AdditionalDemographicsModel additionalDemographicsModel) => additionalDemographics.Add(additionalDemographicsModel))
                .Returns(additionalDemographicResponse);

            //Delete
            mock.Setup(r => r.DeleteAdditionalDemographic(It.IsAny<long>(), DateTime.UtcNow))
                .Returns(additionalDemographicResponse);
        }

        /// <summary>
        /// Test the additional demographic for success.
        /// </summary>
        [TestMethod]
        public void GetAdditionalDemographic_Success()
        {
            //Act
            var getAdditionalDemographicResult = additionalDemographicController.GetAdditionalDemographic(contactId);
            var response = getAdditionalDemographicResult as HttpResult<Response<AdditionalDemographicsModel>>;
            var additionalDemography = response.Value.DataItems;

            //Assert
            Assert.IsNotNull(additionalDemography, "Data items can't be null");
            Assert.IsTrue(additionalDemography.Count() > 0, "Atleast one additional demography must exists.");
        }

        /// <summary>
        /// Adds the additional demographic for success.
        /// </summary>
        [TestMethod]
        public void AddAdditionalDemographic_Success()
        {
            //Act
            var addAdditionalDemographic = new AdditionalDemographicsModel
            {
                ContactID = 4,
                Name = "Test4",
                MRN = 123456,
                EthnicityID = 2
            };

            var addAdditionalDemographicResult = additionalDemographicController.AddAdditionalDemographic(addAdditionalDemographic);
            var response = addAdditionalDemographicResult as HttpResult<Response<AdditionalDemographicsModel>>;
            var additionalDemography = response.Value.DataItems;

            //Assert
            Assert.IsNotNull(additionalDemography);
            Assert.IsTrue(additionalDemography.Count() > 0, "Additional Demography could not be created.");
        }

        /// <summary>
        /// Updates the additional demographic for success.
        /// </summary>
        [TestMethod]
        public void UpdateAdditionalDemographic_Success()
        {
            //Act
            var updateAdditionalDemographic = new AdditionalDemographicsModel
            {
                ContactID = 4,
                Name = "Joe Smith",
                MRN = 123456,
                EthnicityID = 2
            };

            var updateAdditionalDemographicResult = additionalDemographicController.UpdateAdditionalDemographic(updateAdditionalDemographic);
            var response = updateAdditionalDemographicResult as HttpResult<Response<AdditionalDemographicsModel>>;
            var additionalDemography = response.Value.DataItems;
            var count = additionalDemography.Count();

            //Assert
            Assert.IsNotNull(additionalDemography);
            Assert.IsTrue(count > 0, "Additional Demography could not be updated.");
        }

        /// <summary>
        /// Deletes the additional demographic_ success.
        /// </summary>
        [TestMethod]
        public void DeleteAdditionalDemographic_Success()
        {
            //Act
            var updateAdditionalDemographicResult = additionalDemographicController.DeleteAdditionalDemographic(1, DateTime.UtcNow);
            var response = updateAdditionalDemographicResult as HttpResult<Response<AdditionalDemographicsModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.RowAffected > 0);
        }
    }
}
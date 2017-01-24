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

namespace Axis.DataEngine.Tests.Controllers
{
    /// <summary>
    /// Represent Registration Controller Test Class
    /// </summary>
    [TestClass]
    public class RegistrationControllerTest
    {
        /// <summary>
        /// The registration data provider
        /// </summary>
        private IRegistrationDataProvider registrationDataProvider;

        /// <summary>
        /// The contact identifier
        /// </summary>
        private long contactId = 4;

        /// <summary>
        /// The search criteria
        /// </summary>
        private string searchCriteria = "N";

        private string contactType = "P";

        /// <summary>
        /// The contact demographic model
        /// </summary>
        private ContactDemographicsModel contactDemographicModel = null;

        /// <summary>
        /// The empty contact demographic model
        /// </summary>
        private ContactDemographicsModel emptyContactDemographicModel = null;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
        }

        /// <summary>
        /// Mock_s the contact demographics_ success.
        /// </summary>
        public void Mock_ContactDemographics_Success()
        {
            Mock<IRegistrationDataProvider> mock = new Mock<IRegistrationDataProvider>();
            registrationDataProvider = mock.Object;

            var contactDemographics = new List<ContactDemographicsModel>();
            contactDemographicModel = new ContactDemographicsModel()
            {
                ContactID = 4,
                ContactTypeID = 1,
                ClientTypeID = 1,
                FirstName = "FirstName",
                Middle = "MiddleName",
                LastName = "LastName",
                SuffixID = 1,
                GenderID = 1,
                TitleID = 1,
                DOB = DateTime.Now,
                DOBStatusID = 1,
                SSN = "123451234",
                SSNStatusID = 1,
                DriverLicense = "765432",
                IsPregnant = false,
                PreferredName = "PreferredName",
                IsDeceased = true,
                DeceasedDate = DateTime.Now,
                CauseOfDeath = 1,
                ContactMethodID = 1,
                ReferralSourceID = 1,
                Addresses = new List<ContactAddressModel> { new ContactAddressModel() { AddressTypeID = 1, Line1 = "C40", City = "Noida", StateProvince = 1, County = 1,EffectiveDate=DateTime.Now,
                            ExpirationDate=DateTime.Now.AddDays(2) ,MailPermissionID=1} },
            };
            contactDemographics.Add(contactDemographicModel);
            var contactDemography = new Response<ContactDemographicsModel>()
            {
                DataItems = contactDemographics
            };

            //Get ContactDemographic
            Response<ContactDemographicsModel> contactDemographicResponse = new Response<ContactDemographicsModel>();
            contactDemographicResponse.DataItems = contactDemographics.Where(contact => contact.ContactID == contactId).ToList();

            mock.Setup(r => r.GetContactDemographics(It.IsAny<long>()))
                .Returns(contactDemographicResponse);

            //Add ContactDemographic
            mock.Setup(r => r.AddContactDemographics(It.IsAny<ContactDemographicsModel>()))
                .Callback((ContactDemographicsModel contactDemographicsModel) => contactDemographics.Add(contactDemographicsModel))
                .Returns(contactDemography);

            //Update ContactDemographic
            mock.Setup(r => r.UpdateContactDemographics(It.IsAny<ContactDemographicsModel>()))
                .Callback((ContactDemographicsModel contactDemographicsModel) => contactDemographics.Add(contactDemographicsModel))
                .Returns(contactDemography);

            //Get search client
            Response<ContactDemographicsModel> contactSearchResposne = new Response<ContactDemographicsModel>();
            contactSearchResposne.DataItems = contactDemographics.Where(x => x.FirstName.Contains(searchCriteria) || x.LastName.Contains(searchCriteria) ||
                                                                x.Middle.Contains(searchCriteria) || x.PreferredName.Contains(searchCriteria) ||
                                                                x.SSN.Contains(searchCriteria) || x.MPI.Contains(searchCriteria)).ToList();

            mock.Setup(r => r.GetClientSummary(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(contactDemographicResponse);
        }

        /// <summary>
        /// Mock_s the contact demographics_ failed.
        /// </summary>
        public void Mock_ContactDemographics_Failed()
        {
            Mock<IRegistrationDataProvider> mock = new Mock<IRegistrationDataProvider>();
            registrationDataProvider = mock.Object;

            var contactDemographics = new List<ContactDemographicsModel>();
            emptyContactDemographicModel = new ContactDemographicsModel()
            {
                ContactID = 8,
                ContactTypeID = 1,
                ClientTypeID = 1,
                FirstName = null,
                Middle = null,
                LastName = null,
                SuffixID = 1,
                GenderID = 1,
                TitleID = 1,
                DOB = DateTime.Now,
                DOBStatusID = 1,
                SSN = "123451234",
                SSNStatusID = 1,
                DriverLicense = "765432",
                IsPregnant = false,
                PreferredName = "PreferredName",
                DeceasedDate = DateTime.Now,
                ContactMethodID = 1,
                ReferralSourceID = 1,
                Addresses = new List<ContactAddressModel> { new ContactAddressModel() { AddressTypeID = 1, Line1 = "C40", City = "Noida", StateProvince = 1, County = 1,EffectiveDate=DateTime.Now,
                            ExpirationDate=DateTime.Now.AddDays(2),MailPermissionID=1 } },
            };
            contactDemographics.Add(emptyContactDemographicModel);
            var contactDemography = new Response<ContactDemographicsModel>()
            {
                DataItems = contactDemographics
            };

            //Get ContactDemographic
            Response<ContactDemographicsModel> contactDemographicResponse = new Response<ContactDemographicsModel>();
            contactDemographicResponse.DataItems = contactDemographics.Where(contact => contact.ContactID == contactId).ToList();

            mock.Setup(r => r.GetContactDemographics(It.IsAny<long>()))
                .Returns(contactDemographicResponse);

            //Add ContactDemographic
            mock.Setup(r => r.AddContactDemographics(It.IsAny<ContactDemographicsModel>()))
                .Callback((ContactDemographicsModel contactDemographicsModel) => contactDemographics.Add(contactDemographicsModel))
                .Returns(contactDemography);

            //Update ContactDemographic
            mock.Setup(r => r.UpdateContactDemographics(It.IsAny<ContactDemographicsModel>()))
                .Callback((ContactDemographicsModel contactDemographicsModel) => contactDemographics.Add(contactDemographicsModel))
                .Returns(contactDemography);
        }

        /// <summary>
        /// Test the Contact demographic for success.
        /// </summary>
        [TestMethod]
        public void GetContactDemographic_Success()
        {
            Mock_ContactDemographics_Success();
            // Arrange
            RegistrationController contactDemographicController = new RegistrationController(registrationDataProvider);

            //Act
            var getContactDemographicResult = contactDemographicController.GetContactDemographics(contactId);
            var response = getContactDemographicResult as HttpResult<Response<ContactDemographicsModel>>;
            var contactDemography = response.Value.DataItems;
            var count = contactDemography.Count();

            //Assert
            Assert.IsNotNull(contactDemography, "Data items can't be null");
            Assert.IsTrue(contactDemography.Count() > 0, "Atleast one Contact demography must exists.");
        }

        /// <summary>
        /// Test the Contact demographic for failed.
        /// </summary>
        [TestMethod]
        public void GetContactDemographic_Failed()
        {
            Mock_ContactDemographics_Failed();
            // Arrange
            RegistrationController contactDemographicController = new RegistrationController(registrationDataProvider);

            //Act
            var getContactDemographicResult = contactDemographicController.GetContactDemographics(0);
            var response = getContactDemographicResult as HttpResult<Response<ContactDemographicsModel>>;
            var contactDemography = response.Value.DataItems;

            //Assert
            Assert.IsNull(response.Value.DataItems[0].FirstName);
            Assert.IsNull(response.Value.DataItems[0].LastName);
        }

        /// <summary>
        /// Adds the Contact demographic for success.
        /// </summary>
        [TestMethod]
        public void AddContactDemographic_Success()
        {
            Mock_ContactDemographics_Success();
            // Arrange
            RegistrationController contactDemographicController = new RegistrationController(registrationDataProvider);

            //Act
            var saveContactDemographicResult = contactDemographicController.AddContactDemographics(contactDemographicModel);
            var response = saveContactDemographicResult as HttpResult<Response<ContactDemographicsModel>>;
            var contactDemography = response.Value.DataItems;
            var count = contactDemography.Count();

            //Assert
            Assert.IsNotNull(contactDemography);
            Assert.IsTrue(count > 0);
        }

        /// <summary>
        /// Adds the Contact demographic for failed.
        /// </summary>
        [TestMethod]
        public void AddContactDemographic_Failed()
        {
            Mock_ContactDemographics_Failed();
            // Arrange
            RegistrationController contactDemographicController = new RegistrationController(registrationDataProvider);

            //Act
            var saveContactDemographicResult = contactDemographicController.AddContactDemographics(emptyContactDemographicModel);
            var response = saveContactDemographicResult as HttpResult<Response<ContactDemographicsModel>>;
            var contactDemography = response.Value.DataItems;

            //Assert
            Assert.IsNull(response.Value.DataItems[0].FirstName);
            Assert.IsNull(response.Value.DataItems[0].LastName);
        }

        /// <summary>
        /// Updates the Contact demographic for success.
        /// </summary>
        [TestMethod]
        public void UpdateContactDemographic_Success()
        {
            Mock_ContactDemographics_Success();
            // Arrange
            RegistrationController contactDemographicController = new RegistrationController(registrationDataProvider);

            //Act
            var updateContactDemographicResult = contactDemographicController.UpdateContactDemographics(contactDemographicModel);
            var response = updateContactDemographicResult as HttpResult<Response<ContactDemographicsModel>>;
            var contactDemography = response.Value.DataItems;
            var count = contactDemography.Count();

            //Assert
            Assert.IsNotNull(contactDemography);
            Assert.IsTrue(count > 0);
        }

        /// <summary>
        /// Updates the Contact demographic for failed.
        /// </summary>
        [TestMethod]
        public void UpdateContactDemographic_Failed()
        {
            Mock_ContactDemographics_Failed();
            // Arrange
            RegistrationController contactDemographicController = new RegistrationController(registrationDataProvider);

            //Act
            var updateContactDemographicResult = contactDemographicController.UpdateContactDemographics(emptyContactDemographicModel);
            var response = updateContactDemographicResult as HttpResult<Response<ContactDemographicsModel>>;
            var contactDemography = response.Value.DataItems;

            //Assert
            Assert.IsNull(response.Value.DataItems[0].FirstName);
            Assert.IsNull(response.Value.DataItems[0].LastName);
        }

        #region Client Search

        /// <summary>
        /// Gets the client summary_ success.
        /// </summary>
        [TestMethod]
        public void GetClientSummary_Success()
        {
            Mock_ContactDemographics_Success();
            // Arrange
            RegistrationController contactDemographicController = new RegistrationController(registrationDataProvider);

            //Act
            var getClientResult = contactDemographicController.GetClientSummary(searchCriteria, contactType);
            var response = getClientResult as HttpResult<Response<ContactDemographicsModel>>;

            var clientList = response.Value.DataItems;
            //Assert
            Assert.IsNotNull(clientList);
            Assert.IsTrue(clientList.Count > 0);
        }


        /// <summary>
        /// Gets the client summary_ failed.
        /// </summary>
        [TestMethod]
        public void GetClientSummary_Failed()
        {
            Mock_ContactDemographics_Failed();
            // Arrange
            RegistrationController contactDemographicController = new RegistrationController(registrationDataProvider);

            //Act
            var getClientResult = contactDemographicController.GetClientSummary(searchCriteria, contactType);
            var response = getClientResult as HttpResult<Response<ContactDemographicsModel>>;

            //Assert
            Assert.IsNull(response.Value);

        }
        #endregion Client Search
    }
}

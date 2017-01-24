using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.RuleEngine.Registration;
using Moq;
using Axis.Model.Registration;
using Axis.Model.Address;
using Axis.Model.Common;
using System.Linq;
using Axis.RuleEngine.Plugins.Registration;
using Axis.RuleEngine.Service.Controllers;
using Axis.RuleEngine.Helpers.Results;

namespace Axis.RuleEngine.Tests.Controllers
{
    /// <summary>
    /// Represent Demographic mock test methods
    /// </summary>
    [TestClass]
    public class RegisterDemographicsTest
    {
        /// <summary>
        /// The registration rule engine
        /// </summary>
        private IRegistrationRuleEngine registrationRuleEngine;
        /// <summary>
        /// The contact identifier
        /// </summary>
        private long contactId = 8;
        /// <summary>
        /// The search criteria
        /// </summary>
        private string searchCriteria = "N";

        private string ContactType = "1";
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
        /// Contacts the demographic_ success.
        /// </summary>
        private void ContactDemographic_Success()
        {
            Mock<IRegistrationRuleEngine> mock = new Mock<IRegistrationRuleEngine>();
            registrationRuleEngine = mock.Object;

            var contactDemographics = new List<ContactDemographicsModel>();
            contactDemographicModel = new ContactDemographicsModel()
            {
                ContactID = 8,
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
                DeceasedDate = DateTime.Now,
                ContactMethodID = 1,
                ReferralSourceID = 1,
                Addresses = new List<ContactAddressModel> { new ContactAddressModel() { AddressTypeID = 1, Line1 = "C40", City = "Noida", StateProvince = 1, County = 1 ,EffectiveDate=DateTime.Now,
                            ExpirationDate=DateTime.Now.AddDays(2)} },
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
            mock.Setup(r => r.AddContactDemographics(It.IsAny<ContactDemographicsModel>())).
                 Callback((ContactDemographicsModel contactDemographicsModel) => contactDemographics.Add(contactDemographicsModel))
                 .Returns(contactDemography);

            //Update ContactDemographic
            mock.Setup(r => r.UpdateContactDemographics(It.IsAny<ContactDemographicsModel>()))
                .Callback((ContactDemographicsModel contactDemographicsModel) => contactDemographics.Add(contactDemographicsModel))
                .Returns(contactDemography);
            //Save ContactDemographics
            

            //Get search client
            Response<ContactDemographicsModel> contactSearchResponse = new Response<ContactDemographicsModel>();
            contactSearchResponse.DataItems = contactDemographics.Where(x => x.FirstName.Contains(searchCriteria) || x.LastName.Contains(searchCriteria) ||
                                                                x.Middle.Contains(searchCriteria) || x.PreferredName.Contains(searchCriteria) ||
                                                                x.SSN.Contains(searchCriteria) || x.MPI.Contains(searchCriteria)).ToList();

            mock.Setup(r => r.GetClientSummary(It.IsAny<string>(),It.IsAny<string>()))
                .Returns(contactSearchResponse);
        }

        /// <summary>
        /// Contacts the demographics_ failed.
        /// </summary>
        public void ContactDemographics_Failed()
        {
            Mock<IRegistrationRuleEngine> mock = new Mock<IRegistrationRuleEngine>();
            registrationRuleEngine = mock.Object;

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
                Addresses = new List<ContactAddressModel> { new ContactAddressModel() { AddressTypeID = 1, Line1 = "C40", City = "Noida", StateProvince = 1, County = 1, EffectiveDate=DateTime.Now,
                            ExpirationDate=DateTime.Now.AddDays(2) } },
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
        /// Gets the contact demographic_ success.
        /// </summary>
        [TestMethod]
        public void GetContactDemographic_Success()
        {
            // Arrange
            ContactDemographic_Success();
            RegistrationController contactDemographicController = new RegistrationController(registrationRuleEngine);

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
        /// Gets the contact demographic_ failed.
        /// </summary>
        [TestMethod]
        public void GetContactDemographic_Failed()
        {
            //Arrenge
            ContactDemographics_Failed();

            RegistrationController contactDemographicController = new RegistrationController(registrationRuleEngine);


            //Act
            var getContactDemographicResult = contactDemographicController.GetContactDemographics(0);
            var response = getContactDemographicResult as HttpResult<Response<ContactDemographicsModel>>;
            var contactDemography = response.Value.DataItems;
            var count = contactDemography.Count();

            //Arrenge
            Assert.IsNull(response.Value.DataItems[0].FirstName);
            Assert.IsNull(response.Value.DataItems[0].LastName);
        }

        /// <summary>
        /// Saves the contact demographic_ success.
        /// </summary>
        [TestMethod]
        public void SaveContactDemographic_Success()
        {
            // Arrange
            ContactDemographic_Success();
            RegistrationController contactDemographicController = new RegistrationController(registrationRuleEngine);

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
        /// Saves the contact demographic_ failed.
        /// </summary>
        [TestMethod]
        public void SaveContactDemographic_Failed()
        {
            // Arrange
            ContactDemographics_Failed();
            RegistrationController contactDemographicController = new RegistrationController(registrationRuleEngine);

            //Act
            var saveContactDemographicResult = contactDemographicController.AddContactDemographics(contactDemographicModel);
            var response = saveContactDemographicResult as HttpResult<Response<ContactDemographicsModel>>;
            var contactDemography = response.Value.DataItems;
            var count = contactDemography.Count();

            //Assert
            Assert.IsNull(contactDemography[0].FirstName);
            Assert.IsNull(contactDemography[0].LastName);
        }


        /// <summary>
        /// Updates the contact demographic_ success.
        /// </summary>
        [TestMethod]
        public void UpdateContactDemographic_Success()
        {
            // Arrange
            ContactDemographic_Success();
            RegistrationController contactDemographicController = new RegistrationController(registrationRuleEngine);

            //Act
            var updateContactDemographicsModel = new ContactDemographicsModel
            {
                ContactID = 8,
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
                DeceasedDate = DateTime.Now,
                ContactMethodID = 1,
                ReferralSourceID = 1,
            };
            var updateContactDemographicResult = contactDemographicController.UpdateContactDemographics(updateContactDemographicsModel);
            var response = updateContactDemographicResult as HttpResult<Response<ContactDemographicsModel>>;
            var contactDemography = response.Value.DataItems;
            var count = contactDemography.Count();

            //Assert
            Assert.IsNotNull(contactDemography);
            Assert.IsTrue(count > 0);
        }

        /// <summary>
        /// Updates the contact demographic_ failed.
        /// </summary>
        [TestMethod]
        public void UpdateContactDemographic_Failed()
        {
            // Arrange
            ContactDemographics_Failed();
            RegistrationController contactDemographicController = new RegistrationController(registrationRuleEngine);

            //Act
            var updateContactDemographicResult = contactDemographicController.UpdateContactDemographics(contactDemographicModel);
            var response = updateContactDemographicResult as HttpResult<Response<ContactDemographicsModel>>;
            var contactDemography = response.Value.DataItems;
            var count = contactDemography.Count();

            //Assert
            Assert.IsNull(contactDemography[0].FirstName);
            Assert.IsNull(contactDemography[0].LastName);
        }


        /// <summary>
        /// Gets the client summary_ success.
        /// </summary>
        [TestMethod]
        public void GetClientSummary_Success()
        {
            // Arrange
            ContactDemographic_Success();
            RegistrationController contactDemographicController = new RegistrationController(registrationRuleEngine);

            //Act
            var clientResult = contactDemographicController.GetClientSummary(searchCriteria,ContactType);
            var response = clientResult as HttpResult<Response<ContactDemographicsModel>>;
            var clientSearch = response.Value.DataItems;
    

            //Assert
            Assert.IsTrue(clientSearch != null, "Data items can't be null");
            Assert.IsTrue(clientSearch.Count() > 0, "Record not found");
        }
    }
}

using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Axis.DataEngine.Tests.Controllers
{
    /// <summary>
    /// Registration Controller Live Test for live testing of registration controller
    /// </summary>
    [TestClass]
    public class RegistrationControllerLiveTest
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "Registration/";

        /// <summary>
        /// The contact identifier
        /// </summary>
        private long contactId = 8;

        /// <summary>
        /// The search criteria
        /// </summary>
        private string searchCriteria = "N";

        private string contactType = "1";

        /// <summary>
        /// The request model
        /// </summary>
        private ContactDemographicsModel requestModel = null;

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

            requestModel = new ContactDemographicsModel
            {
                ContactID = 0,
                FirstName = "Demetrios",
                LastName = "Christopher",
                GenderID = 1,
                DOB = DateTime.Parse("07/01/1976"),
                DOBStatusID = 1,
                SSN = "123451234",
                SSNStatusID = 1,
                DriverLicense = "8765432",
                IsPregnant = false,
                PreferredName = "PreferredName",
                IsDeceased = true,
                DeceasedDate = DateTime.Now,
                ContactMethodID = 1,
                ReferralSourceID = 1,
            };
            requestModel.Addresses.Add(new ContactAddressModel
            {
                AddressTypeID = 1,
                Line1 = "400 E. Las Colinas",
                Line2 = "Suite 660",
                City = "Irving",
                StateProvince = 30,
                Zip = "75039",
                EffectiveDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(2)
            });
            requestModel.Emails.Add(new ContactEmailModel
            {
                Email = "demetrios.christopher@xenatix.com"
            });
            requestModel.Phones.Add(new ContactPhoneModel
            {
                Number = "214-676-6763"
            });
        }

        /// <summary>
        /// Gets the contact demographic_ success.
        /// </summary>
        [TestMethod]
        public void GetContactDemographic_Success()
        {
            //Arrenge
            var url = baseRoute + "getContactDemographics";
            var param = new NameValueCollection();
            param.Add("contactId", contactId.ToString());

            //Act
            var response = communicationManager.Get<Response<ContactDemographicsModel>>(param, url);

            //Assert
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one contact demography must exists.");
        }

        /// <summary>
        /// Gets the contact demographic_ failed.
        /// </summary>
        [TestMethod]
        public void GetContactDemographic_Failed()
        {
            //Arrenge
            var url = baseRoute + "getContactDemographics";
            var param = new NameValueCollection();
            param.Add("contactId", "0");

            //Act
            var response = communicationManager.Get<Response<ContactDemographicsModel>>(param, url);

            //Assert
            Assert.IsNull(response.DataItems[0].FirstName);
            Assert.IsNull(response.DataItems[0].LastName);
            Assert.IsNull(response.DataItems[0].GenderID);
        }

        /// <summary>
        /// Adds the contact demographic_ success.
        /// </summary>
        [TestMethod]
        public void AddContactDemographic_Success()
        {
            //Arrenge
            var url = baseRoute + "addContactDemographics";

            //Act
            var response = communicationManager.Post<ContactDemographicsModel, Response<ContactDemographicsModel>>(requestModel, url);

            //Assert
            Assert.IsTrue(response.RowAffected > 0, "Contact Demography could not be created.");
        }

        /// <summary>
        /// Adds the contact demographic_ failed.
        /// </summary>
        [TestMethod]
        public void AddContactDemographic_Failed()
        {
            //Arrenge
            var url = baseRoute + "addContactDemographics";

            //Act
            var requestModel = new ContactDemographicsModel
            {
                ContactID = 0,
                FirstName = null,
                LastName = null,
                GenderID = 0,
                DOB = null,
                DOBStatusID = 1,
                SSN = "123451234",
                SSNStatusID = 1,
                DriverLicense = "765432",
                IsPregnant = false,
                PreferredName = "PreferredName",
                DeceasedDate = DateTime.Now,
                ContactMethodID = 1,
                ReferralSourceID = 1
            };
            requestModel.Addresses.Add(new ContactAddressModel
            {
                AddressTypeID = 1,
                Line1 = "400 E. Las Colinas",
                Line2 = "Suite 660",
                City = "Irving",
                StateProvince = 30,
                Zip = "75039",
                EffectiveDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(2)
            });
            requestModel.Emails.Add(new ContactEmailModel
            {
                Email = "demetrios.christopher@xenatix.com"
            });
            requestModel.Phones.Add(new ContactPhoneModel
            {
                Number = "214-676-6763"
            });

            var response = communicationManager.Post<ContactDemographicsModel, Response<ContactDemographicsModel>>(requestModel, url);

            //Assert
            Assert.IsNull(response.DataItems);
        }

        /// <summary>
        /// Updates the contact demographic_ success.
        /// </summary>
        [TestMethod]
        public void UpdateContactDemographic_Success()
        {
            //Arrenge
            var url = baseRoute + "updateContactDemographics";
            var requestModel = new ContactDemographicsModel
            {
                ContactID = 8,
                FirstName = "Demetrios",
                LastName = "Christopher",
                GenderID = 1,
                DOB = DateTime.Parse("07/01/1976"),
                DOBStatusID = 1,
                SSN = "123451234",
                SSNStatusID = 1,
                DriverLicense = "765432",
                IsPregnant = false,
                PreferredName = "PreferredName",
                IsDeceased = false,
                DeceasedDate = DateTime.Now,
                ContactMethodID = 1,
                ReferralSourceID = 1,
            };
            requestModel.Addresses.Add(new ContactAddressModel
            {
                AddressTypeID = 1,
                Line1 = "400 E. Las Colinas",
                Line2 = "Suite 660",
                City = "Irving",
                StateProvince = 30,
                Zip = "75039",
                EffectiveDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(2)
            });
            requestModel.Emails.Add(new ContactEmailModel
            {
                Email = "demetrios.christopher@xenatix.com"
            });
            requestModel.Phones.Add(new ContactPhoneModel
            {
                Number = "214-676-6763"
            });
            //Act
            var response = communicationManager.Post<ContactDemographicsModel, Response<ContactDemographicsModel>>(requestModel, url);

            //Assert
            Assert.IsTrue(response.RowAffected > 0, "Contact Demography could not be updated.");
        }

        /// <summary>
        /// Updates the contact demographic_ failed.
        /// </summary>
        [TestMethod]
        public void UpdateContactDemographic_Failed()
        {
            //Arrenge
            var url = baseRoute + "updateContactDemographics";

            //Act
            var response = communicationManager.Post<ContactDemographicsModel, Response<ContactDemographicsModel>>(requestModel, url);

            //Assert
            Assert.IsNull(response.DataItems);
        }

        /// <summary>
        /// Verifies the duplicate contacts_ success.
        /// </summary>
        [TestMethod]
        public void VerifyDuplicateContacts_Success()
        {
            //Arrenge
            var url = baseRoute + "verifyDuplicateContacts";

            //Act
            var response = communicationManager.Post<ContactDemographicsModel, Response<ContactDemographicsModel>>(requestModel, url);

            //Assert
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one contact demography must exists.");
        }

        /// <summary>
        /// Verifies the duplicate contacts_ failed.
        /// </summary>
        [TestMethod]
        public void VerifyDuplicateContacts_Failed()
        {
            //Arrenge
            var url = baseRoute + "verifyDuplicateContacts";

            //Act
            var requestModel = new ContactDemographicsModel
            {
                ContactID = 0,
                FirstName = null,
                LastName = null,
                GenderID = 0,
                DOB = null,
                DOBStatusID = 1,
                SSN = "123451234",
                SSNStatusID = 1,
                DriverLicense = "765432",
                IsPregnant = false,
                PreferredName = "PreferredName",
                DeceasedDate = DateTime.Now,
                ContactMethodID = 1,
                ReferralSourceID = 1
            };

            var response = communicationManager.Post<ContactDemographicsModel, Response<ContactDemographicsModel>>(requestModel, url);

            //Assert
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Contact exists for invalid data.");
        }

        #endregion Test Methods

        #region Client Search

        /// <summary>
        /// Gets the client summary success.
        /// </summary>
        [TestMethod]
        public void GetClientSummary_Success()
        {
            //Arrenge
            var url = baseRoute + "GetClientSummary";

            //Act
            var param = new NameValueCollection();
            param.Add("SearchCriteria", searchCriteria);
            param.Add("ContactType", contactType);
            var response = communicationManager.Get<Response<ContactDemographicsModel>>(param, url);

            //Assert
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Failed to search contact details.");
        }

        /// <summary>
        /// Gets the client summary Failed.
        /// </summary>
        [TestMethod]
        public void GetClientSummary_Failed()
        {
            //Arrenge
            var url = baseRoute + "GetClientSummary";

            //Act
            var param = new NameValueCollection();
            param.Add("SearchCriteria", searchCriteria);
            param.Add("ContactType", "-1");
            var response = communicationManager.Get<Response<ContactDemographicsModel>>(param, url);

            //Assert
            Assert.IsTrue(response.DataItems.Count == 0, "Atleast one search contact exists.");
        }

        #endregion Client Search
    }
}

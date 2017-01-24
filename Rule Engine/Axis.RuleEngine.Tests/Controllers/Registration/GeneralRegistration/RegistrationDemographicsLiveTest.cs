using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Service.Controllers;
using Axis.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace Axis.RuleEngine.Tests.Controllers.Registration.ContactDemography
{
    /// <summary>
    /// Represent Demographic live data
    /// </summary>
    [TestClass]
    public class RegistrationDemographicsLiveTest
    {
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
        /// The request model
        /// </summary>
        private ContactDemographicsModel requestModel = null;

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
                DriverLicense = "765432",
                IsPregnant = false,
                PreferredName = "PreferredName",
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
                ExpirationDate = DateTime.Now.AddDays(2),
                MailPermissionID = 1
            });
            requestModel.Emails.Add(new ContactEmailModel
            {
                Email = "demetrios.christopher@xenatix.com"
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
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Contact demography must exists.");
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
            var url = baseRoute + "AddContactDemographics";

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
            var url = baseRoute + "AddContactDemographics";
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
                ExpirationDate = DateTime.Now.AddDays(2),
                MailPermissionID = 1
            });
            requestModel.Emails.Add(new ContactEmailModel
            {
                Email = "demetrios.christopher@xenatix.com"
            });

            //Act
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
            var updateContactDemographic = new ContactDemographicsModel
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
                Addresses = new List<ContactAddressModel> { new ContactAddressModel() { AddressTypeID = 1, Line1 = "C40", City = "Noida", StateProvince = 1, County = 1,EffectiveDate=DateTime.Now,
                            ExpirationDate=DateTime.Now.AddDays(2),MailPermissionID=1 } },
            };

            //Act
            var response = communicationManager.Post<ContactDemographicsModel, Response<ContactDemographicsModel>>(updateContactDemographic, url);

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
            Assert.IsTrue(response.RowAffected == 0, "Contact Demography could not be updated.");
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
    }
}
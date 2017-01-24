using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;
using Axis.Model.Registration;
using System.Collections.Generic;

namespace Axis.RuleEngine.Tests.Controllers.Registration.EmergencyContact
{
    [TestClass]
    public class EmergencyContactLiveTest
    {
        private CommunicationManager communicationManager;
        private const string baseRoute = "emergencyContact/";
        private long _defaultContactID = 3;
        private int _defaultContactTypeId = 3;
        private long _defaultDeleteContactID = 4;

        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        [TestMethod]
        public void GetEmergencyContacts_Success()
        {
            //Arrange
            var url = baseRoute + "getEmergencyContacts";
            var param = new NameValueCollection();
            param.Add("contactID", _defaultContactID.ToString());
            param.Add("contactTypeId", _defaultContactTypeId.ToString());

            //Act
            var response = communicationManager.Get<Response<EmergencyContactModel>>(param, url);

            //Assert
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one emergency contact must exists.");
        }

        [TestMethod]
        public void GetEmergencyContacts_Failure()
        {
            //Arrange
            var url = baseRoute + "getEmergencyContacts";
            var param = new NameValueCollection();
            param.Add("contactID", "-1");
            param.Add("contactTypeId", "-1");

            //Act
            var response = communicationManager.Get<Response<EmergencyContactModel>>(param, url);

            //Assert
            Assert.IsTrue(response.DataItems.Count == 0, "Atleast one emergency contact  exists.");
        }

        [TestMethod]
        public void AddEmergencyContact_Success()
        {
            //Arrange
            var url = baseRoute + "addEmergencyContact";

            //Add Emergency Contact
            var contactAddressModel = new List<ContactAddressModel>();
            contactAddressModel.Add(new ContactAddressModel { AddressID = 1, AddressTypeID = 2, Line1 = "Address Line1", Line2 = "AddressLine2", City = "Colorado", County = 1, StateProvince = 2, Zip = "zipCode" });

            var contactPhoneModel = new List<ContactPhoneModel>();
            contactPhoneModel.Add(new ContactPhoneModel { ContactPhoneID = 1, PhoneID = 1, PhoneTypeID = 2, PhonePermissionID = 3, Number = "9876458125" });

            var contactEmailModel = new List<ContactEmailModel>();
            contactEmailModel.Add(new ContactEmailModel { Email = "abc@rsystems.com" });

            var addEmergencyContact = new EmergencyContactModel
            {
                ParentContactID = 3,
                ContactID = 0,
                ContactTypeID = 3,
                FirstName = "firstName1",
                LastName = "lastName1",
                GenderID = 1,
                DOB = DateTime.Now,
                SuffixID = 2,
                ContactRelationshipID = 1,
                RelationshipTypeID = 2,
                Addresses = contactAddressModel,
                Phones = contactPhoneModel,
                Emails = contactEmailModel,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            //Act
            var response = communicationManager.Post<EmergencyContactModel, Response<EmergencyContactModel>>(addEmergencyContact, url);

            //Assert
            Assert.IsTrue(response.RowAffected > 0, "Emergency Contact could not be created.");
        }

        [TestMethod]
        public void AddEmergencyContact_Failure()
        {
            //Arrange
            var url = baseRoute + "addEmergencyContact";

            //Add Emergency Contact
            var contactAddressModel = new List<ContactAddressModel>();
            contactAddressModel.Add(new ContactAddressModel { AddressID = 1, AddressTypeID = 2, Line1 = "Address Line1", Line2 = "AddressLine2", City = "Colorado", County = 1, StateProvince = 2, Zip = "zipCode" });

            var contactPhoneModel = new List<ContactPhoneModel>();
            contactPhoneModel.Add(new ContactPhoneModel { ContactPhoneID = 1, PhoneID = 1, PhoneTypeID = 2, PhonePermissionID = 3, Number = "9876458125" });

            var contactEmailModel = new List<ContactEmailModel>();
            contactEmailModel.Add(new ContactEmailModel { Email = "abc@rsystems.com" });

            var addEmergencyContact = new EmergencyContactModel
            {
                ParentContactID = -1,
                ContactID = -1,
                ContactTypeID = -1,
                FirstName = "firstName1",
                LastName = "lastName1",
                GenderID = 1,
                DOB = DateTime.Now,
                SuffixID = 2,
                ContactRelationshipID = 1,
                RelationshipTypeID = 2,
                Addresses = contactAddressModel,
                Phones = contactPhoneModel,
                Emails = contactEmailModel,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            //Act
            var response = communicationManager.Post<EmergencyContactModel, Response<EmergencyContactModel>>(addEmergencyContact, url);

            //Assert
            Assert.IsTrue(response.ResultCode != 0);
            Assert.IsTrue(response.RowAffected == 0, "Emergency created.");
        }

        [TestMethod]
        public void UpdateEmergencyContact_Success()
        {
            //Arrange
            var url = baseRoute + "updateEmergencyContact";

            //Update Additional Demographic
            var updateEmergencyContact = new EmergencyContactModel
            {
                ParentContactID = 3,
                ContactID = 20,
                ContactTypeID = 3,
                FirstName = "firstName11",
                LastName = "lastName11",
                ContactRelationshipID = 3,
                RelationshipTypeID = 4,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            //Act
            var response = communicationManager.Put<EmergencyContactModel, Response<EmergencyContactModel>>(updateEmergencyContact, url);

            //Assert
            Assert.IsTrue(response.RowAffected > 0, "Emergency Contact could not be updated.");
        }

        [TestMethod]
        public void UpdateEmergencyContact_Failure()
        {
            //Arrange
            var url = baseRoute + "updateEmergencyContact";

            //Update Additional Demographic
            var updateEmergencyContact = new EmergencyContactModel
            {
                ParentContactID = 3,
                ContactID = -1,
                ContactTypeID = 3,
                FirstName = "firstName11",
                LastName = "lastName11",
                ContactRelationshipID = 3,
                RelationshipTypeID = 4,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            //Act
            var response = communicationManager.Put<EmergencyContactModel, Response<EmergencyContactModel>>(updateEmergencyContact, url);

            //Assert
            Assert.IsTrue(response.RowAffected == 0, "Emergency Contact updated.");
        }

        [TestMethod]
        public void DeleteEmergencyContact_Success()
        {
            //Arrange
            var url = baseRoute + "deleteEmergencyContact";
            var param = new NameValueCollection();
            param.Add("Id", _defaultDeleteContactID.ToString());

            //Act
            var response = communicationManager.Delete<Response<EmergencyContactModel>>(param, url);

            //Assert
            Assert.IsTrue(response.RowAffected > 0, "Emergency Contact could not be deleted.");
        }

        [TestMethod]
        public void DeleteEmergencyContact_Failure()
        {
            //Arrange
            var url = baseRoute + "deleteEmergencyContact";
            var param = new NameValueCollection();
            param.Add("Id", "-1");

            //Act
            var response = communicationManager.Delete<Response<EmergencyContactModel>>(param, url);

            //Assert
            Assert.IsTrue(response.RowAffected == 0, "Emergency Contact deleted.");
        }
    }
}

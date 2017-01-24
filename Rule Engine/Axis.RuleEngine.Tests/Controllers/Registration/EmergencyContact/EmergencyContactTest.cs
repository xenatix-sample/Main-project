using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.RuleEngine.Registration;
using Axis.RuleEngine.Plugins.Registration;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Axis.Model.Registration;
using Axis.Model.Common;
using Axis.RuleEngine.Helpers.Results;

namespace Axis.RuleEngine.Tests.Controllers.Registration.EmergencyContact
{
    [TestClass]
    public class EmergencyContactTest
    {
        private IEmergencyContactRuleEngine emergencyContactRuleEngine;

        private long _defaultContactId = 3;
        private int _defaultContactTypeId = 3;
        private long _defaultDeleteContactId = 1;

        private EmergencyContactController emergencyContactController = null;


        [TestInitialize]
        public void Initialize()
        {
        }
        public void Mock_EmergencyContact_Success()
        {
            Mock<IEmergencyContactRuleEngine> mock = new Mock<IEmergencyContactRuleEngine>();
            emergencyContactRuleEngine = mock.Object;

            emergencyContactController = new EmergencyContactController(emergencyContactRuleEngine);

            var emergencyContact = new List<EmergencyContactModel>();
            emergencyContact.Add(new EmergencyContactModel()
            {
                ParentContactID = 3,
                ContactID = 1,
                ContactTypeID = 3,
                FirstName = "firstName",
                LastName = "lastName",
                GenderID = 1,
                DOB = DateTime.Now,
                SuffixID = 2,
                ContactRelationshipID = 1,
                RelationshipTypeID = 2,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            });

            var allEmergencyContact = new Response<EmergencyContactModel>()
            {
                DataItems = emergencyContact
            };

            //Get EmergencyContacts
            Response<EmergencyContactModel> emergencyContactsResponse = new Response<EmergencyContactModel>();
            emergencyContactsResponse.DataItems = emergencyContact.Where(contact => contact.ParentContactID == _defaultContactId && contact.ContactTypeID == _defaultContactTypeId).ToList();

            mock.Setup(r => r.GetEmergencyContacts(It.IsAny<long>(), It.IsAny<int>()))
                .Returns(emergencyContactsResponse);

            //Add EmergencyContact
            mock.Setup(r => r.AddEmergencyContact(It.IsAny<EmergencyContactModel>()))
                .Callback((EmergencyContactModel emergencyContactModel) => emergencyContact.Add(emergencyContactModel))
                .Returns(allEmergencyContact);

            //Update EmergencyContact
            mock.Setup(r => r.UpdateEmergencyContact(It.IsAny<EmergencyContactModel>()))
                .Callback((EmergencyContactModel emergencyContactModel) => emergencyContact.Add(emergencyContactModel))
                .Returns(allEmergencyContact);

            //Delete EmergencyContact
            Response<EmergencyContactModel> deleteResponse = new Response<EmergencyContactModel>();
            deleteResponse.RowAffected = 1;
            deleteResponse.DataItems = emergencyContact;

            mock.Setup(r => r.DeleteEmergencyContact(It.IsAny<long>(), DateTime.UtcNow))
                .Callback((long id) => emergencyContact.Remove(emergencyContact.Find(deleteEmergencyContact => deleteEmergencyContact.ContactID == id)))
                .Returns(deleteResponse);
        }

        public void Mock_EmergencyContact_Failure()
        {
            Mock<IEmergencyContactRuleEngine> mock = new Mock<IEmergencyContactRuleEngine>();
            emergencyContactRuleEngine = mock.Object;

            emergencyContactController = new EmergencyContactController(emergencyContactRuleEngine);

            var emergencyContactFailure = new List<EmergencyContactModel>();
            emergencyContactFailure.Add(new EmergencyContactModel()
            {
                ParentContactID = -1,
                ContactID = -1,
                ContactTypeID = -1,
                FirstName = "firstName",
                LastName = "lastName",
                GenderID = 1,
                DOB = DateTime.Now,
                SuffixID = 2,
                ContactRelationshipID = 1,
                RelationshipTypeID = 2,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            });

            var allEmergencyContact = new Response<EmergencyContactModel>()
            {
                DataItems = emergencyContactFailure
            };

            //Get EmergencyContacts
            Response<EmergencyContactModel> emergencyContactsResponse = new Response<EmergencyContactModel>();
            emergencyContactsResponse.DataItems = emergencyContactFailure.Where(contact => contact.ParentContactID == _defaultContactId && contact.ContactTypeID == _defaultContactTypeId).ToList();

            mock.Setup(r => r.GetEmergencyContacts(It.IsAny<long>(), It.IsAny<int>()))
                .Returns(emergencyContactsResponse);

            //Add EmergencyContact
            mock.Setup(r => r.AddEmergencyContact(It.IsAny<EmergencyContactModel>()))
                .Callback((EmergencyContactModel emergencyContactModel) => emergencyContactFailure.Add(emergencyContactModel))
                .Returns(allEmergencyContact);

            //Update EmergencyContact
            mock.Setup(r => r.UpdateEmergencyContact(It.IsAny<EmergencyContactModel>()))
                .Callback((EmergencyContactModel emergencyContactModel) => emergencyContactFailure.Add(emergencyContactModel))
                .Returns(allEmergencyContact);

            //Delete EmergencyContact
            Response<EmergencyContactModel> deleteResponse = new Response<EmergencyContactModel>();
            deleteResponse.RowAffected = 1;
            deleteResponse.DataItems = emergencyContactFailure;

            mock.Setup(r => r.DeleteEmergencyContact(It.IsAny<long>(), DateTime.UtcNow))
                .Callback((long id) => emergencyContactFailure.Remove(emergencyContactFailure.Find(deleteEmergencyContact => deleteEmergencyContact.ContactID == id)))
                .Returns(deleteResponse);
        }

        [TestMethod]
        public void GetEmergencyContacts_Success()
        {
            //Act
            Mock_EmergencyContact_Success();
            var getEmergencyContactsResult = emergencyContactController.GetEmergencyContacts(_defaultContactId, _defaultContactTypeId);
            var response = getEmergencyContactsResult as HttpResult<Response<EmergencyContactModel>>;
            var emergencyContact = response.Value.DataItems;

            //Assert
            Assert.IsNotNull(emergencyContact, "Data items can't be null");
            Assert.IsTrue(emergencyContact.Count() > 0, "Atleast one emergency contact must exists.");
        }

        [TestMethod]
        public void GetEmergencyContacts_Failure()
        {
            //Act
            Mock_EmergencyContact_Failure();

            var getEmergencyContactsResult = emergencyContactController.GetEmergencyContacts(-1, -1);
            var response = getEmergencyContactsResult as HttpResult<Response<EmergencyContactModel>>;
            var emergencyContact = response.Value.DataItems;

            //Assert
            Assert.IsTrue(emergencyContact.Count() == 0, "Emergency contact already exists");
        }

        [TestMethod]
        public void AddEmergencyContact_Success()
        {
            //Act
            Mock_EmergencyContact_Success();

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

            var addEmergencyContactResult = emergencyContactController.AddEmergencyContact(addEmergencyContact);
            var response = addEmergencyContactResult as HttpResult<Response<EmergencyContactModel>>;
            var emergencyContact = response.Value;

            //Assert
            Assert.IsNotNull(emergencyContact);
            Assert.IsTrue(emergencyContact.RowAffected > 0, "Emergency contact could not be created.");
        }

        [TestMethod]
        public void AddEmergencyContact_Failure()
        {
            //Act
            Mock_EmergencyContact_Failure();

            var contactAddressModel = new List<ContactAddressModel>();
            contactAddressModel.Add(new ContactAddressModel { AddressID = 1, AddressTypeID = 2, Line1 = "Address Line1", Line2 = "AddressLine2", City = "Colorado", County = 1, StateProvince = 2, Zip = "zipCode" });

            var contactPhoneModel = new List<ContactPhoneModel>();
            contactPhoneModel.Add(new ContactPhoneModel { ContactPhoneID = 1, PhoneID = 1, PhoneTypeID = 2, PhonePermissionID = 3, Number = "9876458125" });

            var contactEmailModel = new List<ContactEmailModel>();
            contactEmailModel.Add(new ContactEmailModel { Email = "abc@rsystems.com" });

            var addEmergencyContactFailure = new EmergencyContactModel
            {
                ParentContactID = -1,
                ContactID = -1,
                ContactTypeID = -1,
                FirstName = null,
                LastName = null,
                GenderID = 1,
                DOB = DateTime.Now,
                SuffixID = 2,
                ContactRelationshipID = 0,
                RelationshipTypeID = 0,
                Addresses = contactAddressModel,
                Phones = contactPhoneModel,
                Emails = contactEmailModel,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            var addEmergencyContactResult = emergencyContactController.AddEmergencyContact(addEmergencyContactFailure);
            var response = addEmergencyContactResult as HttpResult<Response<EmergencyContactModel>>;
            var emergencyContact = response.Value;

            //Assert
            Assert.IsNotNull(emergencyContact);
            Assert.IsTrue(emergencyContact.RowAffected == 0, "Emergency contact created.");
        }

        [TestMethod]
        public void UpdateEmergencyContact_Success()
        {
            //Act
            Mock_EmergencyContact_Success();
            var updateEmergencyContact = new EmergencyContactModel
            {
                ParentContactID = 3,
                ContactID = 2,
                ContactTypeID = 3,
                FirstName = "firstName11",
                LastName = "lastName11",
                GenderID = 1,
                DOB = DateTime.Now,
                SuffixID = 2,
                ContactRelationshipID = 2,
                RelationshipTypeID = 4,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };
            var updateEmergencyContactResult = emergencyContactController.UpdateEmergencyContact(updateEmergencyContact);
            var response = updateEmergencyContactResult as HttpResult<Response<EmergencyContactModel>>;
            var emergencyContact = response.Value;

            //Assert
            Assert.IsNotNull(emergencyContact);
            Assert.IsTrue(emergencyContact.RowAffected > 0, "Emergency Contact could not be updated.");
        }

        [TestMethod]
        public void UpdateEmergencyContact_Failure()
        {
            //Act
            Mock_EmergencyContact_Failure();
            var updateEmergencyContactFailure = new EmergencyContactModel
            {
                ParentContactID = 3,
                ContactID = -1,
                ContactTypeID = 3,
                FirstName = "firstName11",
                LastName = "lastName11",
                GenderID = 1,
                DOB = DateTime.Now,
                SuffixID = 2,
                ContactRelationshipID = 2,
                RelationshipTypeID = 4,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };
            var updateEmergencyContactResult = emergencyContactController.UpdateEmergencyContact(updateEmergencyContactFailure);
            var response = updateEmergencyContactResult as HttpResult<Response<EmergencyContactModel>>;
            
            //Assert
            var emergencyContact = response.Value;

            //Assert
            Assert.IsNotNull(emergencyContact);
            Assert.IsTrue(emergencyContact.RowAffected == 0, "Emergency Contact updated.");
        }

        [TestMethod]
        public void DeleteEmergencyContact_Success()
        {
            //Act
            Mock_EmergencyContact_Success();
            var deleteEmergencyContactResult = emergencyContactController.DeleteEmergencyContact(_defaultDeleteContactId, DateTime.UtcNow);
            var response = deleteEmergencyContactResult as HttpResult<Response<EmergencyContactModel>>;
            var deleteEmergencyContact = response.Value;
            
            //Assert
            Assert.IsNotNull(deleteEmergencyContact);
            Assert.IsTrue(deleteEmergencyContact.RowAffected > 0, "Emergency Contact could not be deleted.");
        }

        [TestMethod]
        public void DeleteEmergencyContact_Failure()
        {
            //Act
            Mock_EmergencyContact_Failure();
            var deleteEmergencyContactResult = emergencyContactController.DeleteEmergencyContact(_defaultDeleteContactId, DateTime.UtcNow);
            var response = deleteEmergencyContactResult as HttpResult<Response<EmergencyContactModel>>;
            var deleteEmergencyContact = response.Value;

            //Assert
            Assert.IsNotNull(deleteEmergencyContact);
            Assert.IsTrue(deleteEmergencyContact.RowAffected == 0, "Emergency Contact deleted.");
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.RuleEngine.Registration;
using Axis.RuleEngine.Plugins.Registration;
using Moq;
using System.Linq;
using System.Collections.Generic;
using Axis.Model.Registration;
using Axis.Model.Common;
using Axis.RuleEngine.Helpers.Results;

namespace Axis.RuleEngine.Tests.Controllers.Registration.Collateral
{
    [TestClass]
    public class CollateralTest
    {
        private ICollateralRuleEngine collateralRuleEngine;

        private long _defaultContactId = 1;
        private int _defaultContactTypeId = 4;
        private long _defaultDeleteContactId = 1;
        private long _defaultParentDeleteContactId = 1;
        private CollateralController collateralController = null;

        [TestInitialize]
        public void Initialize()
        {
        }
        public void Mock_Collateral_Success()
        {
            Mock<ICollateralRuleEngine> mock = new Mock<ICollateralRuleEngine>();
            collateralRuleEngine = mock.Object;

            collateralController = new CollateralController(collateralRuleEngine);

            var collateral = new List<CollateralModel>();
            collateral.Add(new CollateralModel()
            {
                ParentContactID = 1,
                ContactID = 1,
                ContactTypeID = 4,
                DriverLicense = "driverLicense",
                DriverLicenseStateID = 10,
                ContactRelationshipID = 1,
                LivingWithClientStatus = true,
                ReceiveCorrespondenceID = 1,
                FirstName = "firstName",
                LastName = "lastName",
                GenderID = 1,
                DOB = DateTime.Now,
                SuffixID = 2,
                RelationshipTypeID = 2,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            });

            var allCollateral = new Response<CollateralModel>()
            {
                DataItems = collateral
            };

            //Get EmergencyContacts
            Response<CollateralModel> collateralResponse = new Response<CollateralModel>();
            collateralResponse.DataItems = collateral.Where(contact => contact.ParentContactID == _defaultContactId && contact.ContactTypeID == _defaultContactTypeId).ToList();

            mock.Setup(r => r.GetCollaterals(It.IsAny<long>(), It.IsAny<int>(),false))
                .Returns(collateralResponse);

            //Add EmergencyContact
            mock.Setup(r => r.AddCollateral(It.IsAny<CollateralModel>()))
                .Callback((CollateralModel collateralModel) => collateral.Add(collateralModel))
                .Returns(allCollateral);

            //Update EmergencyContact
            mock.Setup(r => r.UpdateCollateral(It.IsAny<CollateralModel>()))
                .Callback((CollateralModel collateralModel) => collateral.Add(collateralModel))
                .Returns(allCollateral);

            //Delete EmergencyContact
            Response<CollateralModel> deleteResponse = new Response<CollateralModel>();
            deleteResponse.RowAffected = 1;
            deleteResponse.DataItems = collateral;

            mock.Setup(r => r.DeleteCollateral(It.IsAny<long>(), It.IsAny<long>(), DateTime.UtcNow))
                .Callback((long id) => collateral.Remove(collateral.Find(collateralContact => collateralContact.ContactID == id)))
                .Returns(deleteResponse);
        }

        public void Mock_Collateral_Failure()
        {
            Mock<ICollateralRuleEngine> mock = new Mock<ICollateralRuleEngine>();
            collateralRuleEngine = mock.Object;

            collateralController = new CollateralController(collateralRuleEngine);

            var collateralFailure = new List<CollateralModel>();
            collateralFailure.Add(new CollateralModel()
            {
                ParentContactID = -1,
                ContactID = -1,
                ContactTypeID = -1,
                DriverLicense = "driverLicense",
                DriverLicenseStateID = 10,
                ContactRelationshipID = 1,
                LivingWithClientStatus = true,
                ReceiveCorrespondenceID = 1,
                FirstName = "firstName",
                LastName = "lastName",
                GenderID = 1,
                DOB = DateTime.Now,
                SuffixID = 2,
                RelationshipTypeID = 2,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            });

            var allCollateral = new Response<CollateralModel>()
            {
                DataItems = collateralFailure
            };

            //Get EmergencyContacts
            Response<CollateralModel> collateralResponse = new Response<CollateralModel>();
            collateralResponse.DataItems = collateralFailure.Where(contact => contact.ParentContactID == _defaultContactId && contact.ContactTypeID == _defaultContactTypeId).ToList();

            mock.Setup(r => r.GetCollaterals(It.IsAny<long>(), It.IsAny<int>(),false))
                .Returns(collateralResponse);

            //Add EmergencyContact
            mock.Setup(r => r.AddCollateral(It.IsAny<CollateralModel>()))
                .Callback((CollateralModel collateralModel) => collateralFailure.Add(collateralModel))
                .Returns(allCollateral);

            //Update EmergencyContact
            mock.Setup(r => r.UpdateCollateral(It.IsAny<CollateralModel>()))
                .Callback((CollateralModel collateralModel) => collateralFailure.Add(collateralModel))
                .Returns(allCollateral);

            //Delete EmergencyContact
            Response<CollateralModel> deleteResponse = new Response<CollateralModel>();
            deleteResponse.RowAffected = 1;
            deleteResponse.DataItems = collateralFailure;

            mock.Setup(r => r.DeleteCollateral(It.IsAny<long>(),It.IsAny<long>(), DateTime.UtcNow))
                .Callback((long id) => collateralFailure.Remove(collateralFailure.Find(collateralContact => collateralContact.ContactID == id)))
                .Returns(deleteResponse);
        }

        [TestMethod]
        public void GetCollaterals_Success()
        {
            //Act
            Mock_Collateral_Success();
            var getCollateralResult = collateralController.Getcollaterals(_defaultContactId, _defaultContactTypeId,false);
            var response = getCollateralResult as HttpResult<Response<CollateralModel>>;
            var collateral = response.Value.DataItems;

            //Assert
            Assert.IsNotNull(collateral, "Data items can't be null");
            Assert.IsTrue(collateral.Count() > 0, "Atleast one Collateral must exists.");
        }

        [TestMethod]
        public void GetCollaterals_Failure()
        {
            //Act
            Mock_Collateral_Failure();

            var getCollateralResult = collateralController.Getcollaterals(-1, -1,false);
            var response = getCollateralResult as HttpResult<Response<CollateralModel>>;
            var collateral = response.Value.DataItems;

            //Assert
            Assert.IsTrue(collateral.Count() == 0);
        }

        [TestMethod]
        public void AddCollateral_Success()
        {
            //Act
            Mock_Collateral_Success();

            var contactAddressModel = new List<ContactAddressModel>();
            contactAddressModel.Add(new ContactAddressModel { AddressID = 1, AddressTypeID = 2, Line1 = "Address Line1", Line2 = "AddressLine2", City = "Colorado", County = 1, StateProvince = 2, Zip = "zipCode" });

            var contactPhoneModel = new List<ContactPhoneModel>();
            contactPhoneModel.Add(new ContactPhoneModel { ContactPhoneID = 1, PhoneID = 1, PhoneTypeID = 2, PhonePermissionID = 3, Number = "9876458125" });

            var contactEmailModel = new List<ContactEmailModel>();
            contactEmailModel.Add(new ContactEmailModel { Email = "abc@rsystems.com" });

            var addCollateral = new CollateralModel
            {
                ParentContactID = 2,
                ContactID = 0,
                ContactTypeID = 4,
                DriverLicense = "driverLicense",
                DriverLicenseStateID = 8,
                ContactRelationshipID = 2,
                LivingWithClientStatus = false,
                ReceiveCorrespondenceID = 2,
                FirstName = "firstName11",
                LastName = "lastName11",
                GenderID = 1,
                DOB = DateTime.Now,
                SuffixID = 2,
                RelationshipTypeID = 2,
                Addresses = contactAddressModel,
                Phones = contactPhoneModel,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            var addCollateralResult = collateralController.Addcollateral(addCollateral);
            var response = addCollateralResult as HttpResult<Response<CollateralModel>>;
            var collateral = response.Value;

            //Assert
            Assert.IsNotNull(collateral);
            Assert.IsTrue(collateral.ResultCode == 0, "Collateral could not be created.");
        }

        [TestMethod]
        public void AddCollateral_Failure()
        {
            //Act
            Mock_Collateral_Failure();

            var contactAddressModel = new List<ContactAddressModel>();
            contactAddressModel.Add(new ContactAddressModel { AddressID = 1, AddressTypeID = 2, Line1 = "Address Line1", Line2 = "AddressLine2", City = "Colorado", County = 1, StateProvince = 2, Zip = "zipCode" });

            var contactPhoneModel = new List<ContactPhoneModel>();
            contactPhoneModel.Add(new ContactPhoneModel { ContactPhoneID = 1, PhoneID = 1, PhoneTypeID = 2, PhonePermissionID = 3, Number = "9876458125" });

            var contactEmailModel = new List<ContactEmailModel>();
            contactEmailModel.Add(new ContactEmailModel { Email = "abc@rsystems.com" });

            var addCollateralFailure = new CollateralModel
            {
                ParentContactID = -1,
                ContactID = -1,
                ContactTypeID = -1,
                FirstName = null,
                LastName = null,
                DriverLicense = "driverLicense",
                DriverLicenseStateID = 8,
                ContactRelationshipID = 2,
                LivingWithClientStatus = false,
                ReceiveCorrespondenceID = 2,
                GenderID = 1,
                DOB = DateTime.Now,
                SuffixID = 2,
                RelationshipTypeID = 2,
                Addresses = contactAddressModel,
                Phones = contactPhoneModel,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            var addCollateralResult = collateralController.Addcollateral(addCollateralFailure);
            var response = addCollateralResult as HttpResult<Response<CollateralModel>>;

            //Assert
            Assert.IsTrue(response.Value.ResultCode != 0, "Collateral created.");
        }

        [TestMethod]
        public void UpdateCollateral_Success()
        {
            //Act
            Mock_Collateral_Success();
            var updateCollateral = new CollateralModel
            {
                ParentContactID = 0,
                ContactID = 2,
                ContactTypeID = 4,
                FirstName = "firstName1",
                LastName = "lastName1",
                LivingWithClientStatus = false,
                ReceiveCorrespondenceID = 2,
                ContactRelationshipID = 2,
                RelationshipTypeID = 3,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };
            var updateCollateralResult = collateralController.Updatecollateral(updateCollateral);
            var response = updateCollateralResult as HttpResult<Response<CollateralModel>>;
            var collateral = response.Value;

            //Assert
            Assert.IsNotNull(collateral);
            Assert.IsTrue(collateral.ResultCode == 0, "Collateral could not be updated.");
        }

        [TestMethod]
        public void UpdateCollateral_Failure()
        {
            //Act
            Mock_Collateral_Failure();
            var updateCollateralFailure = new CollateralModel
            {
                ParentContactID = 0,
                ContactID = -1,
                ContactTypeID = 4,
                FirstName = "firstName1",
                LastName = "lastName1",
                LivingWithClientStatus = false,
                ReceiveCorrespondenceID = 2,
                ContactRelationshipID = 2,
                RelationshipTypeID = 3,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };
            var updateCollateralResult = collateralController.Updatecollateral(updateCollateralFailure);
            var response = updateCollateralResult as HttpResult<Response<CollateralModel>>;
            var collateral = response.Value;

            //Assert
            Assert.IsTrue(collateral.ResultCode != 0, "Collateral updated.");
        }

        [TestMethod]
        public void DeleteCollateral_Success()
        {
            //Act
            Mock_Collateral_Success();
            var deleteCollateralResult = collateralController.Deletecollateral(_defaultParentDeleteContactId,_defaultDeleteContactId, DateTime.UtcNow);
            var response = deleteCollateralResult as HttpResult<Response<CollateralModel>>;
            var deleteCollateral = response.Value;
            //Assert
            Assert.IsNotNull(deleteCollateral);
            Assert.IsTrue(deleteCollateral.ResultCode == 0, "Collateral could not be deleted.");
        }

        [TestMethod]
        public void DeleteEmergencyContact_Failure()
        {
            //Act
            Mock_Collateral_Failure();
            var deleteCollateralResult = collateralController.Deletecollateral(_defaultParentDeleteContactId,_defaultDeleteContactId, DateTime.UtcNow);
            var response = deleteCollateralResult as HttpResult<Response<CollateralModel>>;
            var deleteCollateral = response.Value;

            //Assert
            Assert.IsTrue(deleteCollateral.ResultCode != 0, "Collateral could be deleted.");
        }
    }
}

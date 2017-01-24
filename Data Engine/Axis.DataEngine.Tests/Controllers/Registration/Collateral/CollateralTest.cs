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

namespace Axis.DataEngine.Tests.Controllers.Registration.Collateral
{
    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class CollateralTest
    {
        /// <summary>
        /// The collateral data provider
        /// </summary>
        private ICollateralDataProvider collateralDataProvider;

        /// <summary>
        /// The default contact identifier
        /// </summary>
        private long defaultContactId = 1;

        /// <summary>
        /// The default contact type identifier
        /// </summary>
        private int defaultContactTypeId = 4;

        /// <summary>
        /// The default delete contact identifier
        /// </summary>
        private long defaultDeleteContactId = 1;

        /// <summary>
        /// The default delete contact identifier
        /// </summary>
        private long defaultDeleteParentContactId = 1;

        /// <summary>
        /// The collateral controller
        /// </summary>
        private CollateralController collateralController = null;

        /// <summary>
        /// The colleteral data for success
        /// </summary>
        private CollateralModel colleteralDataForSuccess = null;

        /// <summary>
        /// The colleteral data for failure
        /// </summary>
        private CollateralModel colleteralDataForFailure = null;

        /// <summary>
        /// The collateral response
        /// </summary>
        private Response<CollateralModel> collateralResponse = null;

        /// <summary>
        /// The collaterals
        /// </summary>
        private List<CollateralModel> collaterals = null;

        /// <summary>
        /// The mock
        /// </summary>
        private Mock<ICollateralDataProvider> mock = null;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            InitializeMock();

            colleteralDataForSuccess = new CollateralModel()
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
            };

            colleteralDataForFailure = new CollateralModel()
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
            };
        }

        /// <summary>
        /// Initializes the mock.
        /// </summary>
        private void InitializeMock()
        {
            mock = new Mock<ICollateralDataProvider>();
            collateralDataProvider = mock.Object;

            collateralController = new CollateralController(collateralDataProvider);
        }

        /// <summary>
        /// Prepares the response.
        /// </summary>
        /// <param name="collateralData">The collateral data.</param>
        private void PrepareResponse(CollateralModel collateralData)
        {
            collaterals = new List<CollateralModel>();
            collaterals.Add(collateralData);

            collateralResponse = new Response<CollateralModel>()
            {
                DataItems = collaterals
            };
        }

        /// <summary>
        /// Gets collaterals - success test case.
        /// </summary>
        [TestMethod]
        public void GetCollaterals_Success()
        {
            // Arrange
            PrepareResponse(colleteralDataForSuccess);

            Response<CollateralModel> collateralResponse = new Response<CollateralModel>();
            collateralResponse.DataItems = collaterals.Where(contact => contact.ParentContactID == defaultContactId && contact.ContactTypeID == defaultContactTypeId).ToList();

            mock.Setup(r => r.GetCollaterals(It.IsAny<long>(), It.IsAny<int>(),false))
                .Returns(collateralResponse);

            //Act
            var getCollateralResult = collateralController.GetCollaterals(defaultContactId, defaultContactTypeId,false);
            var response = getCollateralResult as HttpResult<Response<CollateralModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.DataItems.Count() > 0, "Atleast one Collateral must exists.");
        }

        /// <summary>
        /// Gets collaterals - failure test case.
        /// </summary>
        [TestMethod]
        public void GetCollaterals_Failure()
        {
            // Arrange
            PrepareResponse(colleteralDataForFailure);

            Response<CollateralModel> collateralResponse = new Response<CollateralModel>();
            collateralResponse.DataItems = collaterals.Where(contact => contact.ParentContactID == defaultContactId && contact.ContactTypeID == defaultContactTypeId).ToList();

            mock.Setup(r => r.GetCollaterals(It.IsAny<long>(), It.IsAny<int>(),false))
                .Returns(collateralResponse);

            //Act
            var getCollateralResult = collateralController.GetCollaterals(-1, -1,false);
            var response = getCollateralResult as HttpResult<Response<CollateralModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.DataItems.Count() == 0, "Collateral exists.");
        }

        /// <summary>
        /// Add collateral - success test case
        /// </summary>
        [TestMethod]
        public void AddCollateral_Success()
        {
            // Arrange
            PrepareResponse(colleteralDataForSuccess);

            mock.Setup(r => r.AddCollateral(It.IsAny<CollateralModel>()))
                .Callback((CollateralModel collateralModel) => collaterals.Add(collateralModel))
                .Returns(collateralResponse);

            //Act
            var contactAddressModel = new List<ContactAddressModel>();
            contactAddressModel.Add(new ContactAddressModel
            {
                AddressTypeID = 2,
                Line1 = "Address Line1",
                Line2 = "AddressLine2",
                City = "Colorado",
                County = 1,
                StateProvince = 2,
                Zip = "zipCode"
            });

            var contactPhoneModel = new List<ContactPhoneModel>();
            contactPhoneModel.Add(new ContactPhoneModel
            {
                PhoneTypeID = 2,
                PhonePermissionID = 3,
                Number = "9876458125"
            });

            var contactEmailModel = new List<ContactEmailModel>();
            contactEmailModel.Add(new ContactEmailModel
            {
                Email = "abc@rsystems.com"
            });

            var addCollateral = new CollateralModel
            {
                ParentContactID = 2,
                ContactID = 0,
                ContactTypeID = 4,
                DriverLicense = "driverLicense",
                DriverLicenseStateID = 8,
                ContactRelationshipID = 2,
                LivingWithClientStatus = false,
                ReceiveCorrespondenceID =2,
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

            var addCollateralResult = collateralController.AddCollateral(addCollateral);
            var response = addCollateralResult as HttpResult<Response<CollateralModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.Value.ResultCode == 0, "Collateral could not be created.");
            Assert.IsTrue(response.Value.RowAffected > 0, "Collateral could not be created.");
        }

        /// <summary>
        /// Add collateral - failure test case
        /// </summary>
        [TestMethod]
        public void AddCollateral_Failure()
        {
            // Arrange
            PrepareResponse(colleteralDataForFailure);

            mock.Setup(r => r.AddCollateral(It.IsAny<CollateralModel>()))
                .Callback((CollateralModel collateralModel) => collaterals.Add(collateralModel))
                .Returns(collateralResponse);

            //Act
            var contactAddressModel = new List<ContactAddressModel>();
            contactAddressModel.Add(new ContactAddressModel { AddressID = 1, AddressTypeID = 2, Line1 = "Address Line1", Line2 = "AddressLine2", City = "Colorado", County = 1, StateProvince = 2, Zip = "zipCode" });

            var contactPhoneModel = new List<ContactPhoneModel>();
            contactPhoneModel.Add(new ContactPhoneModel { ContactPhoneID = 1, PhoneID = 1, PhoneTypeID = 2, PhonePermissionID = 3, Number = "9876458125" });

            var contactEmailModel = new List<ContactEmailModel>();
            contactEmailModel.Add(new ContactEmailModel { Email = "abc@rsystems.com" });

            var addCollateralFailure = new CollateralModel
            {
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

            var addCollateralResult = collateralController.AddCollateral(addCollateralFailure);
            var response = addCollateralResult as HttpResult<Response<CollateralModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.Value.ResultCode != 0, "Collateral created.");
            Assert.IsTrue(response.Value.RowAffected == 0, "Collateral created.");
        }

        /// <summary>
        /// Update collateral - success test case.
        /// </summary>
        [TestMethod]
        public void UpdateCollateral_Success()
        {
            // Arrange
            PrepareResponse(colleteralDataForSuccess);

            mock.Setup(r => r.UpdateCollateral(It.IsAny<CollateralModel>()))
                .Callback((CollateralModel collateralModel) => collaterals.Add(collateralModel))
                .Returns(collateralResponse);

            //Act
            var updateCollateral = new CollateralModel
            {
                ParentContactID = 0,
                ContactID = 2,
                ContactTypeID = 4,
                FirstName = "firstName1",
                LastName = "lastName1",
                LivingWithClientStatus = false,
                ReceiveCorrespondenceID =2,
                ContactRelationshipID = 2,
                RelationshipTypeID = 3,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };
            var updateCollateralResult = collateralController.UpdateCollateral(updateCollateral);
            var response = updateCollateralResult as HttpResult<Response<CollateralModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.Value.ResultCode == 0, "Collateral could not be updated.");
            Assert.IsTrue(response.Value.RowAffected != 0, "Collateral could not be updated.");
        }

        /// <summary>
        /// Update collateral - failure test case.
        /// </summary>
        [TestMethod]
        public void UpdateCollateral_Failure()
        {
            // Arrange
            PrepareResponse(colleteralDataForFailure);

            mock.Setup(r => r.UpdateCollateral(It.IsAny<CollateralModel>()))
                .Callback((CollateralModel collateralModel) => collaterals.Add(collateralModel))
                .Returns(collateralResponse);

            //Act
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
            var updateCollateralResult = collateralController.UpdateCollateral(updateCollateralFailure);
            var response = updateCollateralResult as HttpResult<Response<CollateralModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.Value.ResultCode != 0, "Collateral updated.");
            Assert.IsTrue(response.Value.RowAffected == 0, "Collateral updated.");
        }

        /// <summary>
        /// Delete collateral - success test case.
        /// </summary>
        [TestMethod]
        public void DeleteCollateral_Success()
        {
            // Arrange
            PrepareResponse(colleteralDataForSuccess);

            collateralResponse.RowAffected = 1;

            mock.Setup(r => r.DeleteCollateral(It.IsAny<long>(), It.IsAny<long>(), DateTime.UtcNow))
                .Callback((long id) => collaterals.Remove(collaterals.Find(collateralContact => collateralContact.ContactID == id)))
                .Returns(collateralResponse);

            //Act
            var deleteCollateralResult = collateralController.DeleteCollateral(defaultDeleteParentContactId,defaultDeleteContactId, DateTime.UtcNow);
            var response = deleteCollateralResult as HttpResult<Response<CollateralModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.Value.ResultCode == 0, "Collateral could not be deleted.");
            Assert.IsTrue(response.Value.RowAffected > 0, "Collateral could not be deleted.");
        }

        /// <summary>
        /// Delete collateral - failure test case.
        /// </summary>
        [TestMethod]
        public void DeleteEmergencyContact_Failure()
        {
            // Arrange
            PrepareResponse(colleteralDataForSuccess);

            collateralResponse.RowAffected = 0;

            mock.Setup(r => r.DeleteCollateral(It.IsAny<long>(), It.IsAny<long>(), DateTime.UtcNow))
                .Callback((long id) => collaterals.Remove(collaterals.Find(collateralContact => collateralContact.ContactID == id)))
                .Returns(collateralResponse);

            //Act
            var deleteCollateralResult = collateralController.DeleteCollateral(defaultDeleteParentContactId,defaultDeleteContactId, DateTime.UtcNow);
            var response = deleteCollateralResult as HttpResult<Response<CollateralModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.Value.ResultCode != 0, "Collateral deleted.");
            Assert.IsTrue(response.Value.RowAffected == 0, "Collateral deleted.");
        }
    }
}
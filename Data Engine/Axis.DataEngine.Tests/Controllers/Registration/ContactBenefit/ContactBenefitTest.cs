using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.DataProvider.Registration;
using Axis.DataEngine.Plugins.Registration;
using Axis.Model.Registration;
using Axis.Model.Common;
using System.Collections.Generic;
using Moq;
using System.Linq;
using Axis.DataEngine.Helpers.Results;
using Axis.Model.Address;
using Axis.Model.Common.Lookups.PayorPlan;
using Axis.Model.Common.Lookups.PayorPlanGroup;

namespace Axis.DataEngine.Tests.Controllers.Registration.ContactBenefit
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class ContactBenefitTest
    {
        /// <summary>
        /// The contact benefit data provider
        /// </summary>
        private IContactBenefitDataProvider contactBenefitDataProvider;

        /// <summary>
        /// The contact identifier
        /// </summary>
        private long contactID = 1;
        /// <summary>
        /// The payor identifier
        /// </summary>
        private int payorID = 1;
        /// <summary>
        /// The plan identifier
        /// </summary>
        private int planID = 1;
        /// <summary>
        /// The contact benefit controller
        /// </summary>
        private ContactBenefitController contactBenefitController;
        /// <summary>
        /// The contact benefit model success
        /// </summary>
        private ContactBenefitModel contactBenefitModelSuccess;
        /// <summary>
        /// The contact benefit model failure
        /// </summary>
        private ContactBenefitModel contactBenefitModelFailure;
        /// <summary>
        /// The contact benefit response
        /// </summary>
        private Response<ContactBenefitModel> contactBenefitResponse;
        /// <summary>
        /// The contact benefit list
        /// </summary>
        private List<ContactBenefitModel> contactBenefitList;
        /// <summary>
        /// The mock
        /// </summary>
        private Mock<IContactBenefitDataProvider> mock = null;

        /// <summary>
        /// Initializes the mock.
        /// </summary>
        private void InitializeMock()
        {
            mock = new Mock<IContactBenefitDataProvider>();
            contactBenefitDataProvider = mock.Object;

            contactBenefitController = new ContactBenefitController(contactBenefitDataProvider);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            InitializeMock();

            contactBenefitModelSuccess = new ContactBenefitModel()
            {
                ContactID = 1,
                ContactPayorID = 1,
                GroupName = "A-Trinity HCS",
                PayorAddressID = 2,
                PayorGroupID = 2,
                PayorID = 117,
                PayorName = "A-Trinity HCS",
                PayorPlanID = 2,
                PlanName = "A-Trinity HCS",
                PolicyID = "3253"
            };

            contactBenefitModelFailure = new ContactBenefitModel()
            {
                ContactID = -1,
                ContactPayorID = 1,
                GroupName = "A-Trinity HCS",
                PayorAddressID = 2,
                PayorGroupID = 2,
                PayorID = 117,
                PayorName = "A-Trinity HCS",
                PayorPlanID = 2,
                PlanName = "A-Trinity HCS",
                PolicyID = "3253"
            };
        }

        /// <summary>
        /// Prepares the response.
        /// </summary>
        /// <param name="contactBenefitModel">The contact benefit model.</param>
        private void PrepareResponse(ContactBenefitModel contactBenefitModel)
        {
            contactBenefitList = new List<ContactBenefitModel>();
            contactBenefitList.Add(contactBenefitModel);

            contactBenefitResponse = new Response<ContactBenefitModel>()
            {
                DataItems = contactBenefitList
            };
        }

        /// <summary>
        /// Gets the contact benefits_ success.
        /// </summary>
        [TestMethod]
        public void GetContactBenefits_Success()
        {
            // Arrange
            PrepareResponse(contactBenefitModelSuccess);

            Response<ContactBenefitModel> ContactBenefitExpectedResponse = new Response<ContactBenefitModel>();
            ContactBenefitExpectedResponse.DataItems = contactBenefitList.Where(contact => contact.ContactID == contactID).ToList();

            mock.Setup(r => r.GetContactBenefits(It.IsAny<long>()))
                .Returns(ContactBenefitExpectedResponse);

            //Act
            var getContactBenefitResult = contactBenefitController.GetContactBenefits(contactID);
            var response = getContactBenefitResult as HttpResult<Response<ContactBenefitModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.DataItems.Count() > 0, "Atleast one Benefit must exists.");
        }

        /// <summary>
        /// Gets the contact benefits_ failure.
        /// </summary>
        [TestMethod]
        public void GetContactBenefits_Failure()
        {
            // Arrange
            PrepareResponse(contactBenefitModelFailure);

            Response<ContactBenefitModel> ContactBenefitExpectedResponse = new Response<ContactBenefitModel>();
            ContactBenefitExpectedResponse.DataItems = contactBenefitList.Where(contact => contact.ContactID == contactID).ToList();

            mock.Setup(r => r.GetContactBenefits(It.IsAny<long>()))
                .Returns(ContactBenefitExpectedResponse);

            //Act
            var getContactBenefitResult = contactBenefitController.GetContactBenefits(-1);
            var response = getContactBenefitResult as HttpResult<Response<ContactBenefitModel>>;

            //Assert
            Assert.IsNotNull(response, "Response is null");
            Assert.IsNotNull(response.Value.DataItems, "Data items is null");
            Assert.IsTrue(response.Value.DataItems.Count() == 0, "Contact Benefit returned data.");
        }

        /// <summary>
        /// Gets the payor plans and addresses_ success.
        /// </summary>
        [TestMethod]
        public void GetPayorPlans_Success()
        {
            // Arrange
           
            PayorPlan payorPlan= new PayorPlan(){ PayorPlanID=1, PlanID="1", PlanName= "Test Plan"};
            List<PayorPlan> payorPlansModelList = new List<PayorPlan> { payorPlan };

            Response<PayorPlan> payorPlansExpectedResponse = new Response<PayorPlan>();
            payorPlansExpectedResponse.DataItems = payorPlansModelList;

            mock.Setup(r => r.GetPayorPlans(It.IsAny<int>()))
                .Returns(payorPlansExpectedResponse);

            //Act
            var getpayorPlansAndAddresses = contactBenefitController.GetPayorPlans(payorID);
            var response = getpayorPlansAndAddresses as HttpResult<Response<PayorPlan>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.DataItems.Count() > 0, "Atleast one payor must exists.");
        }

        /// <summary>
        /// Gets the payor plans and addresses_ failure.
        /// </summary>
        [TestMethod]
        public void GetPayorPlans_Failure()
        {
            //Arrange
            List<PayorPlan> payorPlansModelList = new List<PayorPlan>();

            Response<PayorPlan> payorPlansExpectedResponse = new Response<PayorPlan>();
            payorPlansExpectedResponse.DataItems = payorPlansModelList;
            payorPlansExpectedResponse.ResultCode = -1;

            mock.Setup(r => r.GetPayorPlans(It.IsAny<int>()))
                .Returns(payorPlansExpectedResponse);

            //Act
            var getpayorPlans = contactBenefitController.GetPayorPlans(payorID);
            var response = getpayorPlans as HttpResult<Response<PayorPlan>>;

            //Assert
            Assert.IsNotNull(response, "Response is null");
            Assert.IsNotNull(response.Value.DataItems, "Data is null");
            Assert.IsTrue(response.Value.DataItems.Count() == 0, "Payor Plan and Address exists.");
        }

        /// <summary>
        /// Gets the groups for payor plan_ success.
        /// </summary>
        [TestMethod]
        public void GetGroupsAndAddressForPlan_Success()
        {
            AddressModel payorAddresses = new AddressModel() { AddressID = 1, AddressTypeID = 1 };
            PayorPlanGroup payorPlanGroup = new PayorPlanGroup() { GroupID = "1", GroupName = "Group1", PayorGroupID = 1, PayorPlanID = 2 };
            PlanGroupAndAddressesModel groupsAndAddressesModelSuccess = new PlanGroupAndAddressesModel()
            {
                PayorAddresses = new List<Model.Address.AddressModel> { payorAddresses },
                PlanGroups = new List<PayorPlanGroup> { payorPlanGroup }
            };

            List<PlanGroupAndAddressesModel> payorPlanGroupList = new List<PlanGroupAndAddressesModel> { groupsAndAddressesModelSuccess };

            Response<PlanGroupAndAddressesModel> payorPlanGroupExpectedResponse = new Response<PlanGroupAndAddressesModel>();
            payorPlanGroupExpectedResponse.DataItems = payorPlanGroupList;

            mock.Setup(r => r.GetGroupsAndAddressForPlan(It.IsAny<int>()))
                .Returns(payorPlanGroupExpectedResponse);

            //Act
            var getpayorPlans = contactBenefitController.GetGroupsAndAddressForPlan(planID);
            var response = getpayorPlans as HttpResult<Response<PayorPlanGroup>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.DataItems.Count() > 0, "Atleast one payor plan group must exists.");


        }

        /// <summary>
        /// Gets the groups for payor plan_ failure.
        /// </summary>
        [TestMethod]
        public void GetGroupsAndAddressForPlan_Failure()
        {
            //Arrange
            List<PlanGroupAndAddressesModel> payorPlanGroupList = new List<PlanGroupAndAddressesModel>();

            Response<PlanGroupAndAddressesModel> payorPlanGroupExpectedResponse = new Response<PlanGroupAndAddressesModel>();
            payorPlanGroupExpectedResponse.DataItems = payorPlanGroupList;

            mock.Setup(r => r.GetGroupsAndAddressForPlan(It.IsAny<int>()))
                .Returns(payorPlanGroupExpectedResponse);

            //Act
            var getpayorPlans = contactBenefitController.GetGroupsAndAddressForPlan(planID);
            var response = getpayorPlans as HttpResult<Response<PlanGroupAndAddressesModel>>;

            //Assert
            Assert.IsNotNull(response, "Response is null");
            Assert.IsNotNull(response.Value.DataItems, "Data is null");
            Assert.IsTrue(response.Value.DataItems.Count() == 0, "Payor Plan Group exists.");
        }

        /// <summary>
        /// Adds the contact benefit_ success.
        /// </summary>
        [TestMethod]
        public void AddContactBenefit_Success()
        {
            // Arrange
            PrepareResponse(contactBenefitModelSuccess);
            

            mock.Setup(r => r.AddContactBenefit(It.IsAny<ContactBenefitModel>()))
                .Callback((ContactBenefitModel contactBenefitModel) => contactBenefitList.Add(contactBenefitModel))
                .Returns(contactBenefitResponse);

            //Act
            var addContactBenefit = new ContactBenefitModel
            {
                ContactID = 2,
                ContactPayorID = 2,
                GroupName = "A-BS",
                PayorAddressID = 2,
                PayorGroupID = 4,
                PayorID = 117,
                PayorName = "A-BS",
                PayorPlanID = 4,
                PlanName = "A-BS",
                PolicyID = "1111",
                Copay = 20,
                Deductible =90,
                AddressID = 2,
                AddressTypeID =1,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            var addContactBenefitResult = contactBenefitController.AddContactBenefit(addContactBenefit);
            var response = addContactBenefitResult as HttpResult<Response<ContactBenefitModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.Value.ResultCode == 0, "Contact Benefit could not be created.");
           
        }

        /// <summary>
        /// Adds the contact benefit_ failure.
        /// </summary>
        [TestMethod]
        public void AddContactBenefit_Failure()
        {
            // Arrange
            PrepareResponse(contactBenefitModelSuccess);
            contactBenefitResponse.ResultCode = -1;
            mock.Setup(r => r.AddContactBenefit(It.IsAny<ContactBenefitModel>()))
                .Callback((ContactBenefitModel contactBenefitModel) => contactBenefitList.Add(contactBenefitModel))
                .Returns(contactBenefitResponse);

            //Act
            var addContactBenefit = new ContactBenefitModel
            {
                ContactID = -1,
                ContactPayorID = -22,
                GroupName = "A-BS",
                PayorAddressID = 2,
                PayorGroupID = 4,
                PayorID = 117,
                PayorName = "A-BS",
                PayorPlanID = 4,
                PlanName = "A-BS",
                PolicyID = "1111",
                Copay = 20,
                Deductible = 90,
                AddressID = 2,
                AddressTypeID = 1,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            var addContactBenefitResult = contactBenefitController.AddContactBenefit(addContactBenefit);
            var response = addContactBenefitResult as HttpResult<Response<ContactBenefitModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.Value.ResultCode != 0, "Contact Benefit created.");
            Assert.IsTrue(response.Value.RowAffected == 0, "Contact Benefit created.");
        }

        /// <summary>
        /// Updates the contact benefit_ success.
        /// </summary>
        [TestMethod]
        public void UpdateContactBenefit_Success()
        {
            // Arrange
            var updateContactBenefit = new ContactBenefitModel
            {
                ContactID = 1,
                ContactPayorID = 1,
                GroupName = "A-Trinity HCS1",
                PayorAddressID = 2,
                PayorGroupID = 2,
                PayorID = 117,
                PayorName = "A-Trinity HCS1",
                PayorPlanID = 2,
                PlanName = "A-Trinity HCS1",
                PolicyID = "3253"
            };

            PrepareResponse(contactBenefitModelSuccess);
            contactBenefitResponse.RowAffected = 1;
            mock.Setup(r => r.UpdateContactBenefit(It.IsAny<ContactBenefitModel>()))
                .Callback((ContactBenefitModel contactBenefitModel) => contactBenefitList.Add(updateContactBenefit))
                .Returns(contactBenefitResponse);

            //Act
            var updateContactBenefitResult = contactBenefitController.UpdateContactBenefit(updateContactBenefit);
            var response = updateContactBenefitResult as HttpResult<Response<ContactBenefitModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.Value.ResultCode == 0, "Contact Benefit can not be updated.");
            Assert.IsTrue(response.Value.RowAffected > 0, "Contact Benefit can not be updated.");
        }

        /// <summary>
        /// Updates the contact benefit_ failure.
        /// </summary>
        [TestMethod]
        public void UpdateContactBenefit_Failure()
        {
            // Arrange
            var updateContactBenefit = new ContactBenefitModel
            {
                ContactID = -1,
                ContactPayorID = -1,
                GroupName = "A-Trinity HCS1",
                PayorAddressID = 2,
                PayorGroupID = 2,
                PayorID = 117,
                PayorName = "A-Trinity HCS1",
                PayorPlanID = 2,
                PlanName = "A-Trinity HCS1",
                PolicyID = "3253"
            };

            PrepareResponse(contactBenefitModelSuccess);
            mock.Setup(r => r.UpdateContactBenefit(It.IsAny<ContactBenefitModel>()))
                .Callback((ContactBenefitModel contactBenefitModel) => contactBenefitList.Add(updateContactBenefit))
                .Returns(contactBenefitResponse);

            //Act
            var updateContactBenefitResult = contactBenefitController.UpdateContactBenefit(updateContactBenefit);
            var response = updateContactBenefitResult as HttpResult<Response<ContactBenefitModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.Value.ResultCode == 0, "Contact Benefit updated.");
            Assert.IsTrue(response.Value.RowAffected == 0, "Contact Benefit updated.");
        }

        /// <summary>
        /// Deletes the contact benefit_ success.
        /// </summary>
        [TestMethod]
        public void DeleteContactBenefit_Success()
        {
            // Arrange
            PrepareResponse(contactBenefitModelSuccess);

            contactBenefitResponse.RowAffected = 1;

            mock.Setup(r => r.DeleteContactBenefit(It.IsAny<long>(), DateTime.UtcNow))
                .Callback((long id) => contactBenefitList.Remove(contactBenefitList.Find(contactBenefit => contactBenefit.ContactPayorID == 1)))
                .Returns(contactBenefitResponse);

            //Act
            var deleteContactBenefitResult = contactBenefitController.DeleteContactBenefit(1, DateTime.UtcNow);
            var response = deleteContactBenefitResult as HttpResult<Response<ContactBenefitModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.Value.ResultCode == 0, "Contact Benefit could not be deleted.");
            Assert.IsTrue(response.Value.RowAffected > 0, "Contact Benefit could not be deleted.");
        }

        /// <summary>
        /// Deletes the contact benefit_ failure.
        /// </summary>
        [TestMethod]
        public void DeleteContactBenefit_Failure()
        {
            // Arrange
            PrepareResponse(contactBenefitModelFailure);

            mock.Setup(r => r.DeleteContactBenefit(It.IsAny<long>(), DateTime.UtcNow))
                .Callback((long id) => contactBenefitList.Remove(contactBenefitList.Find(contactBenefit => contactBenefit.ContactPayorID == 1)))
                .Returns(contactBenefitResponse);

            //Act
            var deleteContactBenefitResult = contactBenefitController.DeleteContactBenefit(1, DateTime.UtcNow);
            var response = deleteContactBenefitResult as HttpResult<Response<ContactBenefitModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.Value.ResultCode == 0, "Contact Benefit deleted.");
            Assert.IsTrue(response.Value.RowAffected == 0, "Contact Benefit deleted.");
        }

       
    }
}

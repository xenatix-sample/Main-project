using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlanGroup;
using MvcControllers = Axis.Plugins.Registration.Controllers;
using Axis.Plugins.Registration.ApiControllers;
using Axis.Plugins.Registration.Model;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Tests.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class ContactBenefitControllerTest
    {
        #region Class Variables

        /// <summary>
        /// The controller
        /// </summary>
        private ContactBenefitController controller;

        //private MvcControllers.ContactBenefitController mvController;

        /// <summary>
        /// The token
        /// </summary>
        private readonly string token = ConfigurationManager.AppSettings["UnitTestToken"];

        /// <summary>
        /// The contact benefit view model
        /// </summary>
        private ContactBenefitViewModel contactBenefitViewModel;

        #endregion Class Variables

        #region Test Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            controller = new ContactBenefitController(new ContactBenefitRepository(token), new RegistrationRepository(token));
        }

        #region Action Results

        /// <summary>
        /// Index_s the success.
        /// </summary>
        [TestMethod]
        public void Index_Success()
        {
            //var result = mvController.Index();

            //Assert.IsNotNull(result);
        }

        /// <summary>
        /// Indexes the instance_ success.
        /// </summary>
        [TestMethod]
        public void IndexInstance_Success()
        {
            //var result = mvController.Index();

            //Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        #endregion Action Results

        #endregion Test Methods

        #region Json Results

        #region Private Methods

        /// <summary>
        /// Gets the contact benefits.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private Response<ContactBenefitViewModel> GetContactBenefits(long id)
        {
            return controller.GetContactBenefits(id).Result;
        }

        /// <summary>
        /// Adds the contact benefit.
        /// </summary>
        /// <returns></returns>
        private Response<ContactBenefitViewModel> AddContactBenefit()
        {
            return controller.AddContactBenefit(contactBenefitViewModel);
        }

        #endregion Private Methods

        #region Public Method

        /// <summary>
        /// Gets the contact benefits_ success.
        /// </summary>
        [TestMethod]
        public void GetContactBenefits_Success()
        {
            //Act
            var modelResponseData = GetContactBenefits(1);

            //Assert
            Assert.IsNotNull(modelResponseData);
            Assert.IsNotNull(modelResponseData.DataItems);
            Assert.IsTrue(modelResponseData.DataItems.Count > 0);
        }

        /// <summary>
        /// Gets the contact benefits_ failed.
        /// </summary>
        [TestMethod]
        public void GetContactBenefits_Failed()
        {
            //Act
            var modelResponseData = GetContactBenefits(0);
            var response = modelResponseData;
            //Assert
            Assert.IsTrue(response.DataItems.Count == 0);
        }

        /// <summary>
        /// Adds the contact benefit_ success.
        /// </summary>
        [TestMethod]
        public void AddContactBenefit_Success()
        {
            //Arrange
            contactBenefitViewModel = new ContactBenefitViewModel
            {
                ContactID = 1,
                ContactPayorID = 0,
                PayorID = 7,
                PayorPlanID = 159,
                PolicyHolderFirstName  = "PHFirstName",
                PolicyHolderMiddleName = "PHMiddleName",
                PolicyHolderLastName = "PHLastName",
                PolicyHolderSuffixID = 1,
                ForceRollback = true 
            };

            //Act
            var response = AddContactBenefit();

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.ResultCode == 0);
        }

        /// <summary>
        /// Adds the contact benefit_ failed.
        /// </summary>
        [TestMethod]
        public void AddContactBenefit_Failed()
        {
            //Arrange
            contactBenefitViewModel = new ContactBenefitViewModel
            {
                ContactID = 0,
                ContactPayorID = 0,
                PayorID = 7,
                PayorPlanID = 200,
                PolicyHolderFirstName = "PHFirstName",
                PolicyHolderMiddleName = "PHMiddleName",
                PolicyHolderLastName = "PHLastName",
                PolicyHolderSuffixID = 99,
                ForceRollback = true
            };

            //Act
            var response = AddContactBenefit();

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.ResultCode != 0);
        }

        /// <summary>
        /// Updates the contact benefit_ success.
        /// </summary>
        [TestMethod]
        public void UpdateContactBenefit_Success()
        {
            //Arrange
            contactBenefitViewModel = new ContactBenefitViewModel
            {
                ContactID = 1,
                ContactPayorID = 1,
                ContactPayorRank = 2,
                PolicyHolderFirstName = "PHFirstName",
                PolicyHolderMiddleName = "PHMiddleName",
                PolicyHolderLastName = "PHLastName",
                PolicyHolderSuffixID = 1,
                ForceRollback = true
            };

            //Act
            var modelResponse = controller.UpdateContactBenefit(contactBenefitViewModel);

            //Assert
            Assert.IsNotNull(modelResponse);
            Assert.IsTrue(modelResponse.ResultCode == 0);
        }

        /// <summary>
        /// Updates the contact benefit_ failed.
        /// </summary>
        [TestMethod]
        public void UpdateContactBenefit_Failed()
        {
            //Arrange
            contactBenefitViewModel = new ContactBenefitViewModel
            {
                ContactID = 0,
                ContactPayorID = 1,
                ContactPayorRank = 2,
                PolicyHolderFirstName = "PHFirstName",
                PolicyHolderMiddleName = "PHMiddleName",
                PolicyHolderLastName = "PHLastName",
                PolicyHolderSuffixID = 99,
                ForceRollback = true
            };

            //Act
            var modelResponse = controller.UpdateContactBenefit(contactBenefitViewModel);

            //Assert
            Assert.IsNotNull(modelResponse);
            Assert.IsTrue(modelResponse.ResultCode != 0);
        }

        ///// <summary>
        ///// Deletes the contact benefit_ success.
        ///// </summary>
        //[TestMethod]
        //public void DeleteContactBenefit_Success()
        //{
        //    //Arrange
        //    const int contactBenefitId = 1;
            
        //    //Act
        //    var result = controller.DeleteContactBenefit(contactBenefitId);
        //    var data = result.Data;
        //    var modelResponse = data as Response<ContactBenefitViewModel>;

        //    //Assert
        //    Assert.IsNotNull(modelResponse);
        //    Assert.IsTrue(modelResponse.ResultCode == 0);
        //}

        ///// <summary>
        ///// Deletes the contact benefit_ failed.
        ///// </summary>
        //[TestMethod]
        //public void DeleteContactBenefit_Failed()
        //{
        //    //Arrange
        //    const int contactBenefitId = 0;

        //    //Act
        //    var result = controller.DeleteContactBenefit(contactBenefitId);
        //    var data = result.Data;
        //    var modelResponse = data as Response<ContactBenefitViewModel>;

        //    //Assert
        //    Assert.IsNotNull(modelResponse);
        //    Assert.IsTrue(modelResponse.ResultCode != 0);
        //}

        /// <summary>
        /// Gets the payor plans_ success.
        /// </summary>
        [TestMethod]
        public void GetPayorPlans_Success()
        {
            //Arrange
            const int payorId = 1;

            //Act
            var modelResponse = controller.GetPayorPlans(payorId);

            //Assert
            Assert.IsNotNull(modelResponse);
            Assert.IsTrue(modelResponse.DataItems.Count > 0);
        }

        /// <summary>
        /// Gets the payor plans_ failed.
        /// </summary>
        [TestMethod]
        public void GetPayorPlans_Failed()
        {
            //Arrange
            const int payorId = 0;

            //Act
            var modelResponse = controller.GetPayorPlans(payorId);

            //Assert
            Assert.IsNotNull(modelResponse);
            Assert.IsTrue(modelResponse.DataItems.Count == 0);
        }

        /// <summary>
        /// Gets the groups and addresses for plan_ failed.
        /// </summary>
        [TestMethod]
        public void GetGroupsAndAddressesForPlan_Failed()
        {
            //Arrange
            const int planId = 0;

            //Act
            var modelResponse = controller.GetGroupsAndAddressesForPlan(planId);
            //var groupsAndAddressesViewModel = modelResponse.DataItems[0];
            
            //Assert
            Assert.IsNotNull(modelResponse, "Response can't be null");
            Assert.IsNotNull(modelResponse.DataItems, "Data items can't be null");
            Assert.IsTrue(modelResponse.DataItems.Count == 0, "Group exists for invalid PlanName/PlanId.");
            Assert.IsNotNull(modelResponse.DataItems[0].PayorAddresses, "Address does not exist for payor.");
            Assert.IsTrue(modelResponse.DataItems[0].PayorAddresses.Count == 0, "PayorAddresses exists for invalid data.");
            Assert.IsNotNull(modelResponse.DataItems[0].PlanGroups, "Plan groups does not exist for plan.");
            Assert.IsTrue(modelResponse.DataItems[0].PlanGroups.Count == 0, "PlanGroups exists for invalid data.");
        }

        /// <summary>
        /// Gets the groups and addresses for plan_ success.
        /// </summary>
        [TestMethod]
        public void GetGroupsAndAddressesForPlan_Success()
        {
            //Arrange
            const int planId = 1;

            //Act
            var modelResponse = controller.GetGroupsAndAddressesForPlan(planId);

            //Assert
            Assert.IsNotNull(modelResponse);
            Assert.IsTrue(modelResponse.DataItems.Count > 0);
        }

        #endregion Public Method

        #endregion Json Results
    }
}
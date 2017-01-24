using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlan;
using Axis.Model.Common.Lookups.PayorPlanGroup;
using Axis.Model.Registration;
using Axis.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using System.Configuration;

namespace Axis.RuleEngine.Tests.Controllers.Registration.ContactBenefit
{
    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class ContactBenefitLiveTest
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "ContactBenefit/";

        /// <summary>
        /// The contact identifier
        /// </summary>
        private long contactID = 1;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        /// <summary>
        /// Gets the contact benefits_ success.
        /// </summary>
        [TestMethod]
        public void GetContactBenefits_Success()
        {
            // Arrange
            var url = baseRoute + "GetContactBenefits";
            var param = new NameValueCollection();
            param.Add("ContactId", contactID.ToString());

            //Act
            var response = communicationManager.Get<Response<ContactBenefitModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.DataItems, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Benefit must exist.");
        }

        /// <summary>
        /// Gets the contact benefits_ failure.
        /// </summary>
        [TestMethod]
        public void GetContactBenefits_Failure()
        {
            //Arrange
            var url = baseRoute + "GetContactBenefits";
            var param = new NameValueCollection();
            param.Add("contactID", "-1");

            //Act
            var response = communicationManager.Get<Response<ContactBenefitModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.DataItems, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Test failed as data exist for invalid contact.");
        }

        /// <summary>
        /// Gets the payor plans and addresses_ success.
        /// </summary>
        [TestMethod]
        public void GetPayorPlans_Success()
        {
            // Arrange
            var url = baseRoute + "GetPayorPlans";
            var param = new NameValueCollection();
            int payorId = 1;
            param.Add("payorId", payorId.ToString());

            //Act
            var response = communicationManager.Get<Response<PayorPlan>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.DataItems, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one plan or address must exist for payor.");
        }

        /// <summary>
        /// Gets the payor plans and addresses_ failure.
        /// </summary>
        [TestMethod]
        public void GetPayorPlans_Failure()
        {
            // Arrange
            var url = baseRoute + "GetPayorPlans";
            var param = new NameValueCollection();
            int payorId = -1;
            param.Add("payorId", payorId.ToString());

            //Act
            var response = communicationManager.Get<Response<PayorPlan>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.DataItems, "Response must contain data items");
            Assert.IsTrue(response.DataItems.Count > 0, "Response must contain data items");
        }

        /// <summary>
        /// Gets the groups for payor plan_ success.
        /// </summary>
        [TestMethod]
        public void GetGroupsAndAddressesForPlan_Success()
        {
            // Arrange
            var url = baseRoute + "GetGroupsAndAddressesForPlan";
            var param = new NameValueCollection();
            int planId = 23;
            param.Add("planId", planId.ToString());

            //Act
            var response = communicationManager.Get<Response<PlanGroupAndAddressesModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.DataItems, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one payor group must exists.");
        }

        /// <summary>
        /// Gets the groups for payor plan_ failure.
        /// </summary>
        [TestMethod]
        public void GetGroupsAndAddressesForPlan_Failure()
        {
            // Arrange
            var url = baseRoute + "GetGroupsAndAddressesForPlan";
            var param = new NameValueCollection();
            int planId = -1;
            param.Add("planId", planId.ToString());

            //Act
            var response = communicationManager.Get<Response<PlanGroupAndAddressesModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.DataItems, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Group exists for invalid PlanName/PlanId.");
            Assert.IsNotNull(response.DataItems[0].PayorAddresses, "Address does not exist for payor.");
            Assert.IsTrue(response.DataItems[0].PayorAddresses.Count == 0, "PayorAddresses exists for invalid data.");
            Assert.IsNotNull(response.DataItems[0].PlanGroups, "Plan groups does not exist for plan.");
            Assert.IsTrue(response.DataItems[0].PlanGroups.Count == 0, "PlanGroups exists for invalid data.");
        }

        /// <summary>
        /// Adds the contact benefit_ success.
        /// </summary>
        [TestMethod]
        public void AddContactBenefit_Success()
        {
            //Arrange
            var url = baseRoute + "AddContactBenefit";
            var addContactBenefit = new ContactBenefitModel
            {
                ContactID = 1,
                //ContactPayorID = 0,
                GroupName = "A-Trinity HCS1",
                PayorAddressID = 2,
                PayorGroupID = 2,
                PayorID = 11,
                PayorName = "A-Trinity HCS1",
                PayorPlanID = 2,
                PlanName = "A-Trinity HCS1",
                PolicyID = "32544",
                ForceRollback = true
            };

            //Act
            var response = communicationManager.Post<ContactBenefitModel, Response<ContactBenefitModel>>(addContactBenefit, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.ResultCode == 0, "Contact benefit could not be created.");
        }

        /// <summary>
        /// Adds the contact benefit_ failure.
        /// </summary>
        [TestMethod]
        public void AddContactBenefit_Failure()
        {
            //Arrange
            var url = baseRoute + "AddContactBenefit";
            var addContactBenefit = new ContactBenefitModel
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
                PolicyID = "3253",
                ForceRollback = true
            };

            //Act
            var response = communicationManager.Post<ContactBenefitModel, Response<ContactBenefitModel>>(addContactBenefit, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.ResultCode != 0, "Contact Benefit created with invalid data.");
        }

        /// <summary>
        /// Updates the contact benefit_ success.
        /// </summary>
        [TestMethod]
        public void UpdateContactBenefit_Success()
        {
            //Arrange
            var url = baseRoute + "UpdateContactBenefit";
            var updateContactBenefit = new ContactBenefitModel
            {
                ContactID = 1,
                ContactPayorID = 1,
                GroupName = "A-Trinity HCS1",
                PayorAddressID = 2,
                PayorGroupID = 10,
                PayorID = 109,
                PayorName = "A-Trinity HCS1",
                PayorPlanID = 2,
                PlanName = "A-Trinity HCS1",
                PolicyID = "3253",
                ForceRollback = true
            };

            //Act
            var response = communicationManager.Put<ContactBenefitModel, Response<ContactBenefitModel>>(updateContactBenefit, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.ResultCode == 0, "Contact benefit could not be updated.");
        }

        /// <summary>
        /// Updates the contact benefit_ failure.
        /// </summary>
        [TestMethod]
        public void UpdateContactBenefit_Failure()
        {
            //Arrange
            var url = baseRoute + "UpdateContactBenefit";
            var updateContactBenefit = new ContactBenefitModel
            {
                ContactID = 1,
                ContactPayorID = 1,
                GroupName = "A-Trinity HCS1",
                PayorAddressID = 600,
                PayorGroupID = 0,
                PayorID = 7896,
                PayorName = "A-B",
                PayorPlanID = 0,
                PlanName = "A-B",
                PolicyID = "3253",
                EffectiveDate = System.DateTime.MinValue,
                ForceRollback = true
            };

            //Act
            var response = communicationManager.Put<ContactBenefitModel, Response<ContactBenefitModel>>(updateContactBenefit, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.ResultCode != 0, "Contact benefit got updated for invalid data.");
            Assert.IsNull(response.DataItems, "Contact benefit got updated.");
        }

        /// <summary>
        /// Deletes the contact benefit_ success.
        /// </summary>
        [TestMethod]
        public void DeleteContactBenefit_Success()
        {
            //Arrange
            var url = baseRoute;
            var param = new NameValueCollection();
            int id = 15;
            param.Add("id", id.ToString());

            //Act
            var response = communicationManager.Delete<Response<ContactBenefitModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.ResultCode == 0, "Contact Benefit could not be deleted.");
        }

        /// <summary>
        /// Deletes the contact benefit_ failure.
        /// </summary>
        //[TestMethod]
        //public void DeleteContactBenefit_Failure()
        //{
        //    //Arrange
        //    var url = baseRoute;
        //    var param = new NameValueCollection();
        //    int id = -1;
        //    param.Add("id", id.ToString());

        //    //Act
        //    var response = communicationManager.Delete<Response<CollateralModel>>(param, url);

        //    //Assert
        //    Assert.IsNotNull(response, "Response can't be null.");
        //    Assert.IsTrue(response.ResultCode == 0, "Contact Benefit deleted for invalid data.");
        //    Assert.IsTrue(response.RowAffected == 0, "Contact Benefit deleted.");
        //}
    }
}
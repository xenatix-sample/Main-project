using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using Axis.Model.Registration;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;
using System.Collections.Generic;

namespace Axis.RuleEngine.Tests.Controllers.Registration.FinancialSummary
{
    [TestClass]
    public class FinancialSummaryLiveTest
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "FinancialSummary/";

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

        [TestMethod]
        public void GetFinancialSummary_Success()
        {
            // Arrange
            var url = baseRoute + "GetFinancialSummary";

            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());

            // Act
            var response = communicationManager.Get<Response<FinancialSummaryModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one financial summary must exist.");
        }

        [TestMethod]
        public void GetFinancialSummary_Failed()
        {
            // Arrange
            var url = baseRoute + "GetFinancialSummary";

            var param = new NameValueCollection();
            param.Add("contactID", "-1");

            // Act
            var response = communicationManager.Get<Response<FinancialSummaryModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Financial Summary should not exist for this test case.");
        }

        [TestMethod]
        public void AddFinancialSummary_Success()
        {
            // Arrange
            var url = baseRoute + "AddFinancialSummary";

            var financialSummary = new FinancialSummaryModel
            {
                ContactID = 1,
                EffectiveDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow,
                OrganizationID = 1,
                DateSigned = DateTime.UtcNow,
                SignatureStatusID = 1,
                UserID = 1,
                UserPhoneID = 4,
                CredentialID = 1,
                EmploymentStatusID = 1,
                LookingForWork = true,
                FinancialAssessment =
                     new FinancialAssessmentModel()
                     {
                         TotalIncome = 10,
                         TotalExpenses = 08,
                         TotalExtraOrdinaryExpenses = 06,
                         TotalOther = 10
                     },
                FinancialAssessmentDetails =
                   new List<FinancialAssessmentDetailsModel>
                    {
                        new FinancialAssessmentDetailsModel()
                        {
                            CategoryTypeID = 1,
                            Amount = 20,
                            FinanceFrequencyID = 1,
                            CategoryID = 1
                        }
                    },
                SelfPay =
                  new FinancialSummarySelfPayModel()
                  {
                      ISChildInConservatorship = true,
                      IsEnrolledInPublicBenefits = false,
                  },
                Payors =
                 new List<BenefitModel>
                    {
                        new BenefitModel()
                        {
                            PayorName = "AARP",
                            PolicyHolderID = 20,
                            RelationshipTypeID = 1,
                            EffectiveDate = DateTime.UtcNow,
                            ExpirationDate = DateTime.UtcNow,
                            PlanID = "Plan 1",
                            PlanName = "TEAMCARE-(ILLINOI)",
                            GroupID = "Group 1",
                            GroupName = "TEAMCARE-(ILLINOI)"
                        }
                    },
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Post<FinancialSummaryModel, Response<FinancialSummaryModel>>(financialSummary, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Financial Summary could not be created.");
        }

        [TestMethod]
        public void AddFinancialSummary_Failed()
        {
            // Arrange
            var url = baseRoute + "AddFinancialSummary";

            var financialSummary = new FinancialSummaryModel
            {
                ContactID = -1,
                EffectiveDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow,
                OrganizationID = 1,
                DateSigned = DateTime.UtcNow,
                SignatureStatusID = 1,
                UserID = 1,
                UserPhoneID = 4,
                CredentialID = 1,
                EmploymentStatusID = 1,
                LookingForWork = true,
                FinancialAssessment =
                     new FinancialAssessmentModel()
                     {
                         TotalIncome = 10,
                         TotalExpenses = 08,
                         TotalExtraOrdinaryExpenses = 06,
                         TotalOther = 10
                     },
                FinancialAssessmentDetails =
                   new List<FinancialAssessmentDetailsModel>
                    {
                        new FinancialAssessmentDetailsModel()
                        {
                            CategoryTypeID = 1,
                            Amount = 20,
                            FinanceFrequencyID = 1,
                            CategoryID = 1
                        }
                    },
                SelfPay =
                  new FinancialSummarySelfPayModel()
                  {
                      ISChildInConservatorship = true,
                      IsEnrolledInPublicBenefits = false,
                  },
                Payors =
                 new List<BenefitModel>
                    {
                        new BenefitModel()
                        {
                            PayorName = "AARP",
                            PolicyHolderID = 20,
                            RelationshipTypeID = 1,
                            EffectiveDate = DateTime.UtcNow,
                            ExpirationDate = DateTime.UtcNow,
                            PlanID = "Plan 1",
                            PlanName = "TEAMCARE-(ILLINOI)",
                            GroupID = "Group 1",
                            GroupName = "TEAMCARE-(ILLINOI)"
                        }
                    },
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Post<FinancialSummaryModel, Response<FinancialSummaryModel>>(financialSummary, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Add financial summary expected to be failed.");
        }

        [TestMethod]
        public void UpdateFinancialSummary_Success()
        {
            // Arrange
            var url = baseRoute + "UpdateFinancialSummary";

            var financialSummary = new FinancialSummaryModel
            {
                FinancialSummaryID = 1,
                ContactID = 1,
                EffectiveDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow,
                OrganizationID = 1,
                DateSigned = DateTime.UtcNow,
                SignatureStatusID = 1,
                UserID = 1,
                UserPhoneID = 4,
                CredentialID = 1,
                EmploymentStatusID = 1,
                LookingForWork = true,
                FinancialAssessment =
                     new FinancialAssessmentModel()
                     {
                         TotalIncome = 10,
                         TotalExpenses = 08,
                         TotalExtraOrdinaryExpenses = 06,
                         TotalOther = 10
                     },
                FinancialAssessmentDetails =
                   new List<FinancialAssessmentDetailsModel>
                    {
                        new FinancialAssessmentDetailsModel()
                        {
                            CategoryTypeID = 1,
                            Amount = 20,
                            FinanceFrequencyID = 1,
                            CategoryID = 1
                        }
                    },
                SelfPay =
                  new FinancialSummarySelfPayModel()
                  {
                      ISChildInConservatorship = true,
                      IsEnrolledInPublicBenefits = false,
                  },
                Payors =
                 new List<BenefitModel>
                    {
                        new BenefitModel()
                        {
                            PayorName = "AARP",
                            PolicyHolderID = 20,
                            RelationshipTypeID = 1,
                            EffectiveDate = DateTime.UtcNow,
                            ExpirationDate = DateTime.UtcNow,
                            PlanID = "Plan 1",
                            PlanName = "TEAMCARE-(ILLINOI)",
                            GroupID = "Group 1",
                            GroupName = "TEAMCARE-(ILLINOI)"
                        }
                    },
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Put<FinancialSummaryModel, Response<FinancialSummaryModel>>(financialSummary, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Financial Summary could not be updated.");
        }

        [TestMethod]
        public void UpdateFinancialSummary_Failed()
        {
            // Arrange
            var url = baseRoute + "UpdateFinancialSummary";

            var financialSummary = new FinancialSummaryModel
            {
                FinancialSummaryID = -1,
                ContactID = -1,
                EffectiveDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow,
                OrganizationID = 1,
                DateSigned = DateTime.UtcNow,
                SignatureStatusID = 1,
                UserID = 1,
                UserPhoneID = 4,
                CredentialID = 1,
                EmploymentStatusID = 1,
                LookingForWork = true,
                FinancialAssessment =
                     new FinancialAssessmentModel()
                     {
                         TotalIncome = 10,
                         TotalExpenses = 08,
                         TotalExtraOrdinaryExpenses = 06,
                         TotalOther = 10
                     },
                FinancialAssessmentDetails =
                   new List<FinancialAssessmentDetailsModel>
                    {
                        new FinancialAssessmentDetailsModel()
                        {
                            CategoryTypeID = 1,
                            Amount = 20,
                            FinanceFrequencyID = 1,
                            CategoryID = 1
                        }
                    },
                SelfPay =
                  new FinancialSummarySelfPayModel()
                  {
                      ISChildInConservatorship = true,
                      IsEnrolledInPublicBenefits = false,
                  },
                Payors =
                 new List<BenefitModel>
                    {
                        new BenefitModel()
                        {
                            PayorName = "AARP",
                            PolicyHolderID = 20,
                            RelationshipTypeID = 1,
                            EffectiveDate = DateTime.UtcNow,
                            ExpirationDate = DateTime.UtcNow,
                            PlanID = "Plan 1",
                            PlanName = "TEAMCARE-(ILLINOI)",
                            GroupID = "Group 1",
                            GroupName = "TEAMCARE-(ILLINOI)"
                        }
                    },
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Put<FinancialSummaryModel, Response<FinancialSummaryModel>>(financialSummary, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected == 0, "Update financial summary expected to be failed.");
        }
    }
}

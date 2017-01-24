using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.Common;
using Axis.Model.Address;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlan;
using Axis.Model.Common.Lookups.PayorPlanGroup;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Registration
{
    /// <summary>
    ///
    /// </summary>
    public class ContactBenefitDataProvider : IContactBenefitDataProvider
    {
        #region initializations

        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork = null;

        /// <summary>
        /// The payor data provider
        /// </summary>
        private IPayorDataProvider payorDataProvider;

        /// <summary>
        /// The policy holder data provider
        /// </summary>
        private IPolicyHolderDataProvider policyHolderDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactBenefitDataProvider" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="payorDataProvider">The payor data provider.</param>
        /// <param name="policyHolderDataProvider">The policy holder data provider.</param>
        public ContactBenefitDataProvider(IUnitOfWork unitOfWork,
            IPayorDataProvider payorDataProvider,
            IPolicyHolderDataProvider policyHolderDataProvider)
        {
            this.unitOfWork = unitOfWork;
            this.policyHolderDataProvider = policyHolderDataProvider;
            this.payorDataProvider = payorDataProvider;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the contact benefits.
        /// </summary>
        /// <param name="ContactId">The contact identifier.</param>
        /// <param name="contactPayorId">The contact payor identifier.</param>
        /// <returns></returns>
        public Response<ContactBenefitModel> GetContactBenefits(long ContactId)
        {
            var contactBenefitRepository = unitOfWork.GetRepository<ContactBenefitModel>(SchemaName.Registration);
            List<SqlParameter> procParams = new List<SqlParameter>() { new SqlParameter("ContactID", ContactId) };
            var contactBenefit = contactBenefitRepository.ExecuteStoredProc("usp_GetContactBenefits", procParams);
            return contactBenefit;
        }



        /// <summary>
        /// Gets the payor details.
        /// </summary>
        /// <param name="searchText">The search field identifier.</param>
        /// <returns></returns>
        public Response<PayorDetailModel> GetPayors(string searchText)
        {
            var contactBenefitRepository = unitOfWork.GetRepository<PayorDetailModel>(SchemaName.Reference);
            List<SqlParameter> procParams = new List<SqlParameter>() { new SqlParameter("PayorSch", searchText) };
            var payorDetails = contactBenefitRepository.ExecuteStoredProc("usp_GetPayorSearch", procParams);
            return payorDetails;
        }

        /// <summary>
        /// Gets the payor plans.
        /// </summary>
        /// <param name="PayorId">The payor identifier.</param>
        /// <returns></returns>
        public Response<PayorPlan> GetPayorPlans(int PayorId)
        {

            // Get Payor Plans
            var benefitPlanRepository = unitOfWork.GetRepository<PayorPlan>(SchemaName.Reference);
            SqlParameter payorIdParam = new SqlParameter("PayorId", PayorId);
            List<SqlParameter> procParams = new List<SqlParameter>() { payorIdParam };
            return benefitPlanRepository.ExecuteStoredProc("usp_GetPlansForPayor", procParams);

        }

        /// <summary>
        /// Gets the groups and address for plan.
        /// </summary>
        /// <param name="PlanID">The plan identifier.</param>
        /// <returns></returns>
        public Response<PlanGroupAndAddressesModel> GetGroupsAndAddressForPlan(int PlanID)
        {

            var model = new Response<PlanGroupAndAddressesModel>();

            //Get Plan Groups
            var benefitRepository = unitOfWork.GetRepository<PayorPlanGroup>(SchemaName.Reference);
            SqlParameter planIdParam = new SqlParameter("PlanID", PlanID);
            List<SqlParameter> procParams = new List<SqlParameter>() { planIdParam };
            var planGroups = benefitRepository.ExecuteStoredProc("usp_GetGroupsForPayorPlan", procParams);



            // Get Payor Address


            var benefitAddressRepository = unitOfWork.GetRepository<AddressModel>(SchemaName.Reference);
            procParams = new List<SqlParameter>() { new SqlParameter("PayorPlanId", PlanID) };
            var payorAddresses = benefitAddressRepository.ExecuteStoredProc("usp_GetAddressesForPayor", procParams);

            if (planGroups.ResultCode != 0)
            {
                model.ResultCode = planGroups.ResultCode;
                model.ResultMessage = planGroups.ResultMessage;
            }
            else
            {
                model.ResultCode = payorAddresses.ResultCode;
                model.ResultMessage = payorAddresses.ResultMessage;
            }

            model.DataItems = new List<PlanGroupAndAddressesModel>{
                new PlanGroupAndAddressesModel
                {
                    PayorAddresses = payorAddresses.DataItems,
                    PlanGroups = planGroups.DataItems
                }
            };

            return model;

        }

        /// <summary>
        /// Adds the contact benefit.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ContactBenefitModel> AddContactBenefit(ContactBenefitModel contact)
        {
            var contactBenefitParameters = BuildContactBenefitSpParams(contact);
            var contactBenefitRepository = unitOfWork.GetRepository<ContactBenefitModel>(SchemaName.Registration);
            return contactBenefitRepository.ExecuteNQStoredProc("usp_AddContactBenefit", contactBenefitParameters);
        }

        /// <summary>
        /// Updates the contact benefit.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ContactBenefitModel> UpdateContactBenefit(ContactBenefitModel contact)
        {
            var contactBenefitParameters = BuildContactBenefitSpParams(contact);
            var contactBenefitRepository = unitOfWork.GetRepository<ContactBenefitModel>(SchemaName.Registration);
            return contactBenefitRepository.ExecuteNQStoredProc("usp_UpdateContactBenefit", contactBenefitParameters);
        }

        /// <summary>
        /// Deletes the contact benefit.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ContactBenefitModel> DeleteContactBenefit(long id, DateTime modifiedOn)
        {
            List<SqlParameter> procsParameters = new List<SqlParameter> { new SqlParameter("ContactPayorId", id), new SqlParameter("ModifiedOn", modifiedOn) };
            var contactBenefitRepository = unitOfWork.GetRepository<ContactBenefitModel>(SchemaName.Registration);
            Response<ContactBenefitModel> spResults = new Response<ContactBenefitModel>();
            spResults = contactBenefitRepository.ExecuteNQStoredProc("usp_DeleteContactBenefit", procsParameters);
            return spResults;
        }

        #endregion exposed functionality

        #region Helpers

        /// <summary>
        /// Builds the contact benefit sp parameters.
        /// </summary>
        /// <param name="contactBenefit">The contact benefit.</param>
        /// <returns></returns>
        private List<SqlParameter> BuildContactBenefitSpParams(ContactBenefitModel contactBenefit)
        {
            var spParameters = new List<SqlParameter>
            {
                new SqlParameter("ContactPayorID", (object) contactBenefit.ContactPayorID ?? DBNull.Value),
                new SqlParameter("ContactID", (object) contactBenefit.ContactID ?? DBNull.Value),
                new SqlParameter("PolicyHolderID", (object) contactBenefit.PolicyHolderID ?? DBNull.Value),
                new SqlParameter("PolicyHolderName", (object) contactBenefit.PolicyHolderName ?? DBNull.Value),

                new SqlParameter("PolicyHolderFirstName", (object) contactBenefit.PolicyHolderFirstName ?? DBNull.Value),
                new SqlParameter("PolicyHolderMiddleName", (object) contactBenefit.PolicyHolderMiddleName ?? DBNull.Value),
                new SqlParameter("PolicyHolderLastName", (object) contactBenefit.PolicyHolderLastName ?? DBNull.Value),
                new SqlParameter("PolicyHolderEmployer", (object) contactBenefit.PolicyHolderEmployer ?? DBNull.Value),
                new SqlParameter("PolicyHolderSuffixID", (object) contactBenefit.PolicyHolderSuffixID ?? DBNull.Value),

                new SqlParameter("BillingFirstName", (object) contactBenefit.BillingFirstName ?? DBNull.Value),
                new SqlParameter("BillingMiddleName", (object) contactBenefit.BillingMiddleName ?? DBNull.Value),
                new SqlParameter("BillingLastName", (object) contactBenefit.BillingLastName ?? DBNull.Value),
                new SqlParameter("BillingSuffixID", (object) contactBenefit.BillingSuffixID ?? DBNull.Value),
                new SqlParameter("AdditionalInformation", (object) contactBenefit.AdditionalInformation ?? DBNull.Value),


                new SqlParameter("PolicyID", (object) contactBenefit.PolicyID ?? DBNull.Value),
                new SqlParameter("Deductible", (object) contactBenefit.Deductible ?? DBNull.Value),
                new SqlParameter("CoPay", (object) contactBenefit.Copay ?? DBNull.Value),
                new SqlParameter("CoInsurance", (object) contactBenefit.CoInsurance ?? DBNull.Value),
                new SqlParameter("EffectiveDate", (object) contactBenefit.EffectiveDate ?? DBNull.Value),
                new SqlParameter("ExpirationDate", (object) contactBenefit.ExpirationDate ?? DBNull.Value),
                 new SqlParameter("PayorExpirationReasonID", (object) contactBenefit.PayorExpirationReasonID ?? DBNull.Value),
                new SqlParameter("ExpirationReason", (object) contactBenefit.ExpirationReason ?? DBNull.Value),
                new SqlParameter("AddRetroDate", (object) contactBenefit.AddRetroDate ?? DBNull.Value),
                new SqlParameter("ContactPayorRank", (object) contactBenefit.ContactPayorRank ?? DBNull.Value),
               
                new SqlParameter("PayorID", (object) contactBenefit.PayorID ?? DBNull.Value),
                new SqlParameter("PlanID", (object) contactBenefit.PlanID ?? DBNull.Value),
                new SqlParameter("GroupID", (object) contactBenefit.GroupID ?? DBNull.Value),
                
                new SqlParameter("PayorPlanID", (object) contactBenefit.PayorPlanID ?? DBNull.Value),
                new SqlParameter("PayorGroupID", (object) contactBenefit.PayorGroupID ?? DBNull.Value),
                new SqlParameter("PayorAddressID", (object) contactBenefit.PayorAddressID ?? DBNull.Value),
                new SqlParameter("HasPolicyHolderSameCardName", contactBenefit.HasPolicyHolderSameCardName),
                

                new SqlParameter("ModifiedOn", contactBenefit.ModifiedOn ?? DateTime.Now)
            };

            return spParameters;
        }

        #endregion Helpers
    }
}

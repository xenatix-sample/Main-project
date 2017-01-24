using System;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlanGroup;
using Axis.Model.Registration;
using Axis.Model.Common.Lookups.PayorPlan;
using Axis.Model.Address;

namespace Axis.DataProvider.Registration
{
    /// <summary>
    ///
    /// </summary>
    public interface IContactBenefitDataProvider
    {
        /// <summary>
        /// Gets the contact benefits.
        /// </summary>
        /// <param name="ContactId">The contact identifier.</param>
        /// <returns></returns>
        Response<ContactBenefitModel> GetContactBenefits(long ContactId);

        /// <summary>
        /// Gets the payor details.
        /// </summary>
        /// <param name="searchText">The search field identifier.</param>
        /// <returns></returns>
        Response<PayorDetailModel> GetPayors(string searchText);

        /// <summary>
        /// Gets the payor plans.
        /// </summary>
        /// <param name="payorId">The payor identifier.</param>
        /// <returns></returns>
        Response<PayorPlan> GetPayorPlans(int payorId);
        
        
        /// <summary>
        /// Gets the groups and addresses for plan.
        /// </summary>
        /// <param name="planId">The plan identifier.</param>
        /// <returns></returns>
        Response<PlanGroupAndAddressesModel> GetGroupsAndAddressForPlan(int planId);
        
        /// <summary>
        /// Adds the contact benefit.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        Response<ContactBenefitModel> AddContactBenefit(ContactBenefitModel contact);
        /// <summary>
        /// Updates the contact benefit.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        Response<ContactBenefitModel> UpdateContactBenefit(ContactBenefitModel contact);
        /// <summary>
        /// Deletes the contact benefit.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ContactBenefitModel> DeleteContactBenefit(long id, DateTime modifiedOn);
    }
}
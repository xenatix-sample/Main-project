using System;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlanGroup;
using Axis.Model.Registration;
using Axis.Model.Common.Lookups.PayorPlan;

namespace Axis.Service.Registration
{
    /// <summary>
    ///
    /// </summary>
    public interface IContactBenefitService
    {
        /// <summary>
        /// Gets the contact benefits.
        /// </summary>
        /// <param name="ContactId">The contact identifier.</param>
        /// <returns></returns>
        Response<ContactBenefitModel> GetContactBenefits(long ContactId);



        /// <summary>
        /// Gets the contact benefits.
        /// </summary>
        /// <param name="searchText">The search field identifier.</param>
        /// <returns></returns>
        Response<PayorDetailModel> GetPayors(string searchText);


        /// <summary>
        /// Gets the payor group plan.
        /// </summary>
        /// <param name="payorID">The payor identifier.</param>
        /// <returns></returns>
        Response<PayorPlan> GetPayorPlans(int payorID);

        /// <summary>
        /// Gets the groups for payor plan.
        /// </summary>
        /// <param name="planID">The plan identifier.</param>
        /// <returns></returns>
        Response<PlanGroupAndAddressesModel> GetGroupsAndAddressForPlan(int planID);

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
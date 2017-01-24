using System;
using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.Plugins.Registration.Model;
using Axis.Plugins.Registration.Models;
using Axis.Model.Common.Lookups.PayorPlanGroup;
using Axis.Model.Common.Lookups.PayorPlan;

namespace Axis.Plugins.Registration.Repository
{
    /// <summary>
    ///
    /// </summary>
    public interface IContactBenefitRepository
    {
        /// <summary>
        /// Gets the contact benefits.
        /// </summary>
        /// <param name="ContactId">The contact identifier.</param>
        /// <returns></returns>
        Response<ContactBenefitViewModel> GetContactBenefits(long ContactId);
        Task<Response<ContactBenefitViewModel>> GetContactBenefitsAsync(long ContactId);

        /// <summary>
        /// Gets the payor details.
        /// </summary>
        /// <param name="Entered Data">The search field identifier.</param>
        /// <returns></returns>
        Response<PayorDetailViewModel> GetPayors(string searchText);


        /// <summary>
        /// Adds the contact benefit.
        /// </summary>
        /// <param name="contactBenefit">The contact benefit.</param>
        /// <returns></returns>
        /// 
        Response<ContactBenefitViewModel> AddContactBenefit(ContactBenefitViewModel contactBenefit);

        /// <summary>
        /// Updates the contact benefit.
        /// </summary>
        /// <param name="contactBenefit">The contact benefit.</param>
        /// <returns></returns>
        Response<ContactBenefitViewModel> UpdateContactBenefit(ContactBenefitViewModel contactBenefit);

        /// <summary>
        /// Deletes the contact benefit.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Response<ContactBenefitViewModel> DeleteContactBenefit(long id, DateTime modifiedOn);

        /// <summary>
        /// Gets the payor group plan.
        /// </summary>
        /// <param name="payorId">The payor identifier.</param>
        /// <returns></returns>
        Response<PayorPlan> GetPayorPlans(int payorId);

        /// <summary>
        /// Gets the groups and addresses for plan.
        /// </summary>
        /// <param name="planId">The plan identifier.</param>
        /// <returns></returns>
        Response<PlanGroupAndAddressModelViewModel> GetGroupsAndAddressesForPlan(int planId);
    }
}
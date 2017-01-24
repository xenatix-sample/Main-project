using System;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlanGroup;
using Axis.Model.Registration;
using Axis.Service.Registration;
using Axis.Model.Common.Lookups.PayorPlan;

namespace Axis.RuleEngine.Registration
{
    /// <summary>
    ///
    /// </summary>
    public class ContactBenefitRuleEngine : IContactBenefitRuleEngine
    {
        /// <summary>
        /// The contact benefit service
        /// </summary>
        private readonly IContactBenefitService contactBenefitService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactBenefitRuleEngine" /> class.
        /// </summary>
        /// <param name="contactBenefitService">The contact benefit service.</param>
        public ContactBenefitRuleEngine(IContactBenefitService contactBenefitService)
        {
            this.contactBenefitService = contactBenefitService;
        }

        /// <summary>
        /// Gets the contact benefits.
        /// </summary>
        /// <param name="ContactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactBenefitModel> GetContactBenefits(long ContactId)
        {
            return contactBenefitService.GetContactBenefits(ContactId);
        }

        /// <summary>
        /// Gets the payor details.
        /// </summary>
        /// <param name="Entered Data">The search field identifier.</param>
        /// <returns></returns>
        public Response<PayorDetailModel> GetPayors(string searchText)
        {
            return contactBenefitService.GetPayors(searchText);

        }

        /// <summary>
        /// Gets the payor group plan.
        /// </summary>
        /// <param name="payorID">The payor identifier.</param>
        /// <returns></returns>
        public Response<PayorPlan> GetPayorPlans(int payorID)
        {
            return contactBenefitService.GetPayorPlans(payorID);
        }


        /// <summary>
        /// Gets the groups and address for plan.
        /// </summary>
        /// <param name="planID">The plan identifier.</param>
        /// <returns></returns>
        public Response<PlanGroupAndAddressesModel> GetGroupsAndAddressForPlan(int planID)
        {
            return contactBenefitService.GetGroupsAndAddressForPlan(planID);
        }

        /// <summary>
        /// Adds the contact benefit.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ContactBenefitModel> AddContactBenefit(ContactBenefitModel contact)
        {
            //TODO: Remove this hack!!
            if (contact.AddressTypeID == 0)
                contact.AddressTypeID = 1;
            return contactBenefitService.AddContactBenefit(contact);
        }

        /// <summary>
        /// Updates the contact benefit.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ContactBenefitModel> UpdateContactBenefit(ContactBenefitModel contact)
        {
            return contactBenefitService.UpdateContactBenefit(contact);
        }

        /// <summary>
        /// Deletes the contact benefit.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ContactBenefitModel> DeleteContactBenefit(long id, DateTime modifiedOn)
        {
            return contactBenefitService.DeleteContactBenefit(id, modifiedOn);
        }
    }
}
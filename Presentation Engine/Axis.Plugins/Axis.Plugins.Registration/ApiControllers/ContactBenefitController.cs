using System;
using System.Linq;
using Axis.Plugins.Registration.Model;
using Axis.Plugins.Registration.Repository;
using System.Threading.Tasks;
using System.Web.Http;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlanGroup;
using Axis.Model.Common.Lookups.PayorPlan;
using Axis.Plugins.Registration.Models;

namespace Axis.Plugins.Registration.ApiControllers
{
    /// <summary>
    ///
    /// </summary>
    public class ContactBenefitController : BaseApiController
    {
        /// <summary>
        /// The contact benefit repository
        /// </summary>
        private readonly IContactBenefitRepository _contactBenefitRepository;
        private readonly IRegistrationRepository _registrationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactBenefitController" /> class.
        /// </summary>
        /// <param name="contactBenefitRepository">The contact benefit repository.</param>
        public ContactBenefitController(IContactBenefitRepository contactBenefitRepository, IRegistrationRepository registrationRepository)
        {
            this._contactBenefitRepository = contactBenefitRepository;
            this._registrationRepository = registrationRepository;
        }

        /// <summary>
        /// Gets the contact benefits.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ContactBenefitViewModel>> GetContactBenefits(long contactId)
        {
            var contact = (await _registrationRepository.GetContactDemographics(contactId)).DataItems.FirstOrDefault();
            string contactFullName = ((contact != null) && (contact.ContactID == contactId))
                ? string.Format("{0} {1}", contact.FirstName, contact.LastName)
                : String.Empty;
            var response = await _contactBenefitRepository.GetContactBenefitsAsync(contactId);
            response.AdditionalResult = contactFullName;
            return response;
        }

        /// <summary>
        /// GetPayors
        /// </summary>
        /// <param name="searchText">searchText</param>
        /// <returns></returns>
        [HttpGet]
        public Response<PayorDetailViewModel> GetPayors(string searchText)
        {
            if (string.IsNullOrEmpty(searchText) || string.IsNullOrWhiteSpace(searchText))
                searchText = string.Empty;
            return _contactBenefitRepository.GetPayors(searchText);
        }

        /// <summary>
        /// Adds the contact benefit.
        /// </summary>
        /// <param name="contactBenefit">The contact benefit.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ContactBenefitViewModel> AddContactBenefit(ContactBenefitViewModel contactBenefit)
        {
            return _contactBenefitRepository.AddContactBenefit(contactBenefit);
        }

        /// <summary>
        /// Updates the contact benefit.
        /// </summary>
        /// <param name="contactBenefit">The contact benefit.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<ContactBenefitViewModel> UpdateContactBenefit(ContactBenefitViewModel contactBenefit)
        {
            return _contactBenefitRepository.UpdateContactBenefit(contactBenefit);
        }

        /// <summary>
        /// Deletes the contact benefit.
        /// </summary>
        /// <param name="contactBenefitId">The contact benefit identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<ContactBenefitViewModel> DeleteContactBenefit(long contactBenefitId, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _contactBenefitRepository.DeleteContactBenefit(contactBenefitId, modifiedOn);
        }

        /// <summary>
        /// Gets the payor group plan.
        /// </summary>
        /// <param name="payorId">The payor identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<PayorPlan> GetPayorPlans(int payorId)
        {
            return _contactBenefitRepository.GetPayorPlans(payorId);
        }

        [HttpGet]
        public Response<PlanGroupAndAddressModelViewModel> GetGroupsAndAddressesForPlan(int planId)
        {
            return _contactBenefitRepository.GetGroupsAndAddressesForPlan(planId);
        }
        
    }
}
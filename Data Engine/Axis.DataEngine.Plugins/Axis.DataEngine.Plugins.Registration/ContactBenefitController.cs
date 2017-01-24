using System;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlanGroup;
using Axis.Model.Registration;
using System.Web.Http;
using System.Web.Http.Cors;
using Axis.Model.Common.Lookups.PayorPlan;

namespace Axis.DataEngine.Plugins.Registration
{
    /// <summary>
    ///
    /// </summary>
    public class ContactBenefitController : BaseApiController
    {
        /// <summary>
        /// The contact benefit data provider
        /// </summary>
        private readonly IContactBenefitDataProvider contactBenefitDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactBenefitController"/> class.
        /// </summary>
        /// <param name="contactBenefitDataProvider">The contact benefit data provider.</param>
        public ContactBenefitController(IContactBenefitDataProvider contactBenefitDataProvider)
        {
            this.contactBenefitDataProvider = contactBenefitDataProvider;
        }

        /// <summary>
        /// Gets the contact benefits.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetContactBenefits(long contactId)
        {
            return new HttpResult<Response<ContactBenefitModel>>(contactBenefitDataProvider.GetContactBenefits(contactId), Request);
        }


        /// <summary>
        /// Gets the payor details.
        /// </summary>
        /// <param name="seachText">The search field identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPayors(string searchText)
        {
            return new HttpResult<Response<PayorDetailModel>>(contactBenefitDataProvider.GetPayors(searchText), Request);
        }

        /// <summary>
        /// Gets the payor group plan.
        /// </summary>
        /// <param name="payorID">The payor identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPayorPlans(int payorID)
        {
            return new HttpResult<Response<PayorPlan>>(contactBenefitDataProvider.GetPayorPlans(payorID), Request);
        }


        /// <summary>
        /// Gets the groups and addresses for plan.
        /// </summary>
        /// <param name="planID">The plan identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult GetGroupsAndAddressForPlan(int planID)
        {
            return new HttpResult<Response<PlanGroupAndAddressesModel>>(contactBenefitDataProvider.GetGroupsAndAddressForPlan(planID), Request);
        }

        /// <summary>
        /// Adds the contact benefit.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddContactBenefit(ContactBenefitModel contact)
        {
            return new HttpResult<Response<ContactBenefitModel>>(contactBenefitDataProvider.AddContactBenefit(contact), Request);
        }

        /// <summary>
        /// Updates the contact benefit.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateContactBenefit(ContactBenefitModel contact)
        {
            return new HttpResult<Response<ContactBenefitModel>>(contactBenefitDataProvider.UpdateContactBenefit(contact), Request);
        }

        /// <summary>
        /// Deletes the contact benefit.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteContactBenefit(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<ContactBenefitModel>>(contactBenefitDataProvider.DeleteContactBenefit(id, modifiedOn), Request);
        }
    }
}
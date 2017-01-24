using System;
using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlanGroup;
using Axis.Model.Registration;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Model.Common.Lookups.PayorPlan;

namespace Axis.Service.Registration
{
    /// <summary>
    ///
    /// </summary>
    public class ContactBenefitService : IContactBenefitService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "contactBenefit/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactBenefitService" /> class.
        /// </summary>
        public ContactBenefitService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Gets the contact benefits.
        /// </summary>
        /// <param name="ContactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactBenefitModel> GetContactBenefits(long ContactId)
        {
            const string apiUrl = BaseRoute + "GetContactBenefits";
            var requestXMLValueNvc = new NameValueCollection { { "ContactId", ContactId.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ContactBenefitModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Gets the payor details.
        /// </summary>
        /// <param name="searchText">The search field identifier.</param>
        /// <returns></returns>
        public Response<PayorDetailModel> GetPayors(string searchText)
        {
            const string apiUrl = BaseRoute + "GetPayors";
            var requestXMLValueNvc = new NameValueCollection { { "searchText", searchText } };
            return communicationManager.Get<Response<PayorDetailModel>>(requestXMLValueNvc, apiUrl);

        }

        /// <summary>
        /// Gets the payor group plan.
        /// </summary>
        /// <param name="payorID">The payor identifier.</param>
        /// <returns></returns>
        public Response<PayorPlan> GetPayorPlans(int payorID)
        {
            const string apiUrl = BaseRoute + "GetPayorPlans";
            var requestXMLValueNvc = new NameValueCollection { { "payorID", payorID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<PayorPlan>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Gets the groups for payor plan.
        /// </summary>
        /// <param name="planID">The plan identifier.</param>
        /// <returns></returns>
        public Response<PlanGroupAndAddressesModel> GetGroupsAndAddressForPlan(int planID)
        {
            const string apiUrl = BaseRoute + "GetGroupsAndAddressForPlan";
            var requestXMLValueNvc = new NameValueCollection { { "planID", planID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<PlanGroupAndAddressesModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Adds the contact benefit.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ContactBenefitModel> AddContactBenefit(ContactBenefitModel contact)
        {
            const string apiUrl = BaseRoute + "AddContactBenefit";
            return communicationManager.Post<ContactBenefitModel, Response<ContactBenefitModel>>(contact, apiUrl);
        }

        /// <summary>
        /// Updates the contact benefit.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ContactBenefitModel> UpdateContactBenefit(ContactBenefitModel contact)
        {
            const string apiUrl = BaseRoute + "UpdateContactBenefit";
            return communicationManager.Put<ContactBenefitModel, Response<ContactBenefitModel>>(contact, apiUrl);
        }

        /// <summary>
        /// Deletes the contact benefit.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ContactBenefitModel> DeleteContactBenefit(long id, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteContactBenefit";
            var requestXMLValueNvc = new NameValueCollection
            {
                { "id", id.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return communicationManager.Delete<Response<ContactBenefitModel>>(requestXMLValueNvc, apiUrl);
        }

       
    }
}
using System;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlanGroup;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Translator;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;

using Axis.Model.Common.Lookups.PayorPlan;

namespace Axis.Plugins.Registration.Repository
{
    /// <summary>
    ///
    /// </summary>
    public class ContactBenefitRepository : IContactBenefitRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "ContactBenefit/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactBenefitRepository" /> class.
        /// </summary>
        public ContactBenefitRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactBenefitRepository" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ContactBenefitRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the contact benefits.
        /// </summary>
        /// <param name="ContactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactBenefitViewModel> GetContactBenefits(long ContactId)
        {
            return GetContactBenefitsAsync(ContactId).Result;
        }

        /// <summary>
        /// Gets the contact benefits.
        /// </summary>
        /// <param name="ContactId">The contact identifier.</param>
        /// <returns></returns>
        public async Task<Response<ContactBenefitViewModel>> GetContactBenefitsAsync(long ContactId)
        {
            string apiUrl = baseRoute + "GetContactBenefits";
            var parameters = new NameValueCollection { { "ContactId", ContactId.ToString(CultureInfo.InvariantCulture) } };
            var response = await communicationManager.GetAsync<Response<ContactBenefitModel>>(parameters, apiUrl);
            return response.ToViewModel();
        }

        public Response<PayorDetailViewModel> GetPayors(string searchText)
        {
            string apiUrl =  baseRoute + "GetPayors";
            var parameters = new NameValueCollection { { "searchText", searchText } };
            var response = communicationManager.Get<Response<PayorDetailModel>>(parameters, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Adds the contact benefit.
        /// </summary>
        /// <param name="contactBenefit">The contact benefit.</param>
        /// <returns></returns>
        public Response<ContactBenefitViewModel> AddContactBenefit(ContactBenefitViewModel contactBenefit)
        {
            string apiUrl = baseRoute + "AddContactBenefit";
            var response = communicationManager.Post<ContactBenefitModel, Response<ContactBenefitModel>>(contactBenefit.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the contact benefit.
        /// </summary>
        /// <param name="contactBenefit">The contact benefit.</param>
        /// <returns></returns>
        public Response<ContactBenefitViewModel> UpdateContactBenefit(ContactBenefitViewModel contactBenefit)
        {
            string apiUrl = baseRoute + "UpdateContactBenefit";
            var response = communicationManager.Put<ContactBenefitModel, Response<ContactBenefitModel>>(contactBenefit.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the contact benefit.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Response<ContactBenefitViewModel> DeleteContactBenefit(long id, DateTime modifiedOn)
        {
            string apiUrl = baseRoute;
            var param = new NameValueCollection
            {
                {"id", id.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return communicationManager.Delete<Response<ContactBenefitViewModel>>(param, apiUrl);
        }

        /// <summary>
        /// Gets the payor group plan.
        /// </summary>
        /// <param name="payorId">The payor identifier.</param>
        /// <returns></returns>
        public Response<PayorPlan> GetPayorPlans(int payorId)
        {
            string apiUrl = baseRoute + "GetPayorPlans";
            var parameters = new NameValueCollection  {
                                                          { "payorId", payorId.ToString(CultureInfo.InvariantCulture) }
                                                      };
            return communicationManager.Get<Response<PayorPlan>>(parameters, apiUrl);
            
        }


        public Response<PlanGroupAndAddressModelViewModel> GetGroupsAndAddressesForPlan(int planId)
        {
            string apiUrl = baseRoute + "GetGroupsAndAddressForPlan";
            var parameters = new NameValueCollection  {
                                                          { "planId", planId.ToString(CultureInfo.InvariantCulture) }
                                                      };
            var response = communicationManager.Get<Response<PlanGroupAndAddressesModel>>(parameters, apiUrl);
            return response.ToViewModel();
        }

    }
}
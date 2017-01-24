using System;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlanGroup;
using Axis.Model.Registration;

using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;

using Axis.Model.Common.Lookups.PayorPlan;
using Axis.PresentationEngine.Areas.BusinessAdmin.Models;
using Axis.Model.BusinessAdmin;
using Axis.PresentationEngine.Areas.BusinessAdmin.Translator;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.HealthRecords;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Respository.PayorPlans
{
    /// <summary>
    /// Payor Plans Repository
    /// </summary>
    /// <seealso cref="Axis.PresentationEngine.Areas.BusinessAdmin.Respository.PayorPlans.IPayorPlansRepository" />
    public class PayorPlansRepository : IPayorPlansRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "PayorPlans/";

        /// <summary>
        /// Initializes a new instance of the <see cref="PayorPlansRepository"/> class.
        /// </summary>
        public PayorPlansRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PayorPlansRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public PayorPlansRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the payor plans.
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

        /// <summary>
        /// Gets the payor plan by identifier.
        /// </summary>
        /// <param name="payorPlanId">The payor plan identifier.</param>
        /// <returns></returns>
        public Response<PayorPlan> GetPayorPlanByID(int payorPlanId)
        {
            string apiUrl = baseRoute + "GetPayorPlanByID";
            var parameters = new NameValueCollection  {
                                                          { "payorPlanId", payorPlanId.ToString(CultureInfo.InvariantCulture) }
                                                      };
            return communicationManager.Get<Response<PayorPlan>>(parameters, apiUrl);

        }

        /// <summary>
        /// Adds the payor plan.
        /// </summary>
        /// <param name="payorPlanDetails">The payor plan details.</param>
        /// <returns></returns>
        public Response<PayorPlan> AddPayorPlan(PayorPlan payorPlanDetails)
        {
            string apiUrl = baseRoute + "AddPayorPlan";
            var response = communicationManager.Post<PayorPlan, Response<PayorPlan>>(payorPlanDetails, apiUrl);
            return response;
        }


        /// <summary>
        /// Updates the payor plan.
        /// </summary>
        /// <param name="payorPlanDetails">The payor plan details.</param>
        /// <returns></returns>
        public Response<PayorPlan> UpdatePayorPlan(PayorPlan payorPlanDetails)
        {
            string apiUrl = baseRoute + "UpdatePayorPlan";
            var response = communicationManager.Put<PayorPlan, Response<PayorPlan>>(payorPlanDetails, apiUrl);
            return response;
        }



    }
}
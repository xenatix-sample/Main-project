using System;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Configuration;
using Axis.Security;
using Axis.Service.BusinessAdmin.Payors;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Model.Common.Lookups.PayorPlan;

namespace Axis.Service.BusinessAdmin.PayorPlans
{
    public class PayorPlansService : IPayorPlansService
    {
        #region Class Variables        
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "PayorPlans/";

        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="PayorPlansService"/> class.
        /// </summary>
        public PayorPlansService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the payor plans.
        /// </summary>
        /// <param name="payorID">The payor identifier.</param>
        /// <returns></returns>
        public Response<PayorPlan> GetPayorPlans(int payorID)
        {
            const string apiUrl = BaseRoute + "GetPayorPlans";
            var requestXMLValueNvc = new NameValueCollection { { "payorID", payorID.ToString() } };
            return communicationManager.Get<Response<PayorPlan>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Gets the payor plan by identifier.
        /// </summary>
        /// <param name="payorPlanId">The payor plan identifier.</param>
        /// <returns></returns>
        public Response<PayorPlan> GetPayorPlanByID(int payorPlanId)
        {
            const string apiUrl = BaseRoute + "GetPayorPlanByID";
            var requestXMLValueNvc = new NameValueCollection { { "payorPlanId", payorPlanId.ToString() } };
            return communicationManager.Get<Response<PayorPlan>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Adds the payor plan.
        /// </summary>
        /// <param name="payorPlanDetails">The payor plan details.</param>
        /// <returns></returns>
        public Response<PayorPlan> AddPayorPlan(PayorPlan payorPlanDetails)
        {
            const string apiUrl = BaseRoute + "AddPayorPlan";
            return communicationManager.Post<PayorPlan, Response<PayorPlan>>(payorPlanDetails, apiUrl);
        }

        /// <summary>
        /// Updates the payor plan.
        /// </summary>
        /// <param name="payorPlanDetails">The payor plan details.</param>
        /// <returns></returns>
        public Response<PayorPlan> UpdatePayorPlan(PayorPlan payorPlanDetails)
        {
            const string apiUrl = BaseRoute + "UpdatePayorPlan";
            return communicationManager.Put<PayorPlan, Response<PayorPlan>>(payorPlanDetails, apiUrl);
        }

        #endregion
    }
}

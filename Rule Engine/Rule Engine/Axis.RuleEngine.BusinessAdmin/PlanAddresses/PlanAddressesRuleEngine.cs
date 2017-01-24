
using System;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Service.BusinessAdmin.ClientMerge;
using Axis.Service.BusinessAdmin.Payors;
using Axis.RuleEngine.BusinessAdmin.PayorPlans;
using Axis.Model.Common.Lookups.PayorPlan;
using Axis.Service.BusinessAdmin.PlanAddresses;

namespace Axis.RuleEngine.BusinessAdmin.PlanAddresses
{
    public class PlanAddressesRuleEngine : IPlanAddressesRuleEngine
    {
        #region Class Variables

        private readonly IPlanAddressesService _planAddressesService;

        #endregion

        #region Constructors

        public PlanAddressesRuleEngine(IPlanAddressesService planAddressesService)
        {
            _planAddressesService = planAddressesService;
        }

        #endregion

        /// <summary>
        /// Gets the payor group plan.
        /// </summary>
        /// <param name="payorPlanID">The payor plan identifier.</param>
        /// <returns></returns>
        public Response<PlanAddressesModel> GetPlanAddresses(int payorPlanID)
        {
            return _planAddressesService.GetPlanAddresses(payorPlanID);
        }

        /// <summary>
        /// Gets the payor group plan.
        /// </summary>
        /// <param name="payorAddressID">The plan address identifier.</param>
        /// <returns></returns>
        public Response<PlanAddressesModel> GetPlanAddress(int payorAddressID)
        {
            return _planAddressesService.GetPlanAddress(payorAddressID);
        }

        /// <summary>
        /// Gets the payor group plan.
        /// </summary>
        /// <param name="payorAddressID">The plan address identifier.</param>
        /// <returns></returns>
        public Response<PlanAddressesModel> AddPlanAddress(PlanAddressesModel payorDetails)
        {
            return _planAddressesService.AddPlanAddress(payorDetails);
        }

        /// <summary>
        /// Gets the payor group plan.
        /// </summary>
        /// <param name="payorAddressID">The plan address identifier.</param>
        /// <returns></returns>
        public Response<PlanAddressesModel> UpdatePlanAddress(PlanAddressesModel payorDetails)
        {
            return _planAddressesService.UpdatePlanAddress(payorDetails);
        }






    }
}

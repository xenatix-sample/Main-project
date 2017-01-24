
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlan;

namespace Axis.RuleEngine.BusinessAdmin.PlanAddresses
{
    public interface IPlanAddressesRuleEngine
    {

        /// <summary>
        /// Gets the payor group plan.
        /// </summary>
        /// <param name="payorPlanID">The payor plan identifier.</param>
        /// <returns></returns>
        Response<PlanAddressesModel> GetPlanAddresses(int payorPlanID);

        /// <summary>
        /// Gets the payor group plan.
        /// </summary>
        /// <param name="payorAddressID">The plan address identifier.</param>
        /// <returns></returns>
        Response<PlanAddressesModel> GetPlanAddress(int payorAddressID);

        /// <summary>
        /// Adds the payor plan.
        /// </summary>
        /// <param name="payorAddressID">The plan address identifier.</param>
        /// <returns></returns>
        Response<PlanAddressesModel> AddPlanAddress(PlanAddressesModel payorDetails);

        /// <summary>
        /// Updates the payor plan.
        /// </summary>
        /// <param name="payorAddressID">The plan address identifier.</param>
        /// <returns></returns>
        Response<PlanAddressesModel> UpdatePlanAddress(PlanAddressesModel payorDetails);





    }


}

using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlan;
using Axis.PresentationEngine.Areas.BusinessAdmin.Models;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Respository.PlanAddresses
{
    public interface IPlanAddressesRepository
    {


        /// <summary>
        /// Gets the plan addresses.
        /// </summary>
        /// <param name="payorPlanID">The payor plan identifier.</param>
        /// <returns></returns>
        Response<PlanAddressesModel> GetPlanAddresses(int payorPlanID);


        /// <summary>
        /// Gets the plan address.
        /// </summary>
        /// <param name="payorAddressID">The payor address identifier.</param>
        /// <returns></returns>
        Response<PlanAddressesModel> GetPlanAddress(int payorAddressID);

        /// <summary>
        /// Adds the plan address.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        Response<PlanAddressesModel> AddPlanAddress(PlanAddressesModel payorDetails);


        /// <summary>
        /// Updates the plan address.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        Response<PlanAddressesModel> UpdatePlanAddress(PlanAddressesModel payorDetails);


    }
}

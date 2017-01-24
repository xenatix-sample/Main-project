using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.BusinessAdmin.Models;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.ClientMerge;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.PlanAddresses;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Http;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.ApiControllers
{
    public class PlanAddressesController : BaseApiController
    {

        #region Class Variables
        private readonly IPlanAddressesRepository _planAddressesRepository;

        #endregion

        #region Constructors
        public PlanAddressesController(IPlanAddressesRepository planAddressesRepository)
        {
            _planAddressesRepository = planAddressesRepository;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the plan addresses.
        /// </summary>
        /// <param name="payorPlanID">The payor plan identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<PlanAddressesModel> GetPlanAddresses(int payorPlanID)
        {
            return _planAddressesRepository.GetPlanAddresses(payorPlanID);
        }

        /// <summary>
        /// Gets the plan address.
        /// </summary>
        /// <param name="payorAddressID">The payor address identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<PlanAddressesModel> GetPlanAddress(int payorAddressID)
        {
            return _planAddressesRepository.GetPlanAddress(payorAddressID);
        }

        /// <summary>
        /// Adds the plan address.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<PlanAddressesModel> AddPlanAddress(PlanAddressesModel payorDetails)
        {
            return _planAddressesRepository.AddPlanAddress(payorDetails);
        }

        /// <summary>
        /// Updates the plan address.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<PlanAddressesModel> UpdatePlanAddress(PlanAddressesModel payorDetails)
        {
            return _planAddressesRepository.UpdatePlanAddress(payorDetails);
        }

        #endregion


    }
}
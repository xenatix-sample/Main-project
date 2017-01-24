using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.BusinessAdmin.PlanAddresses;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    /// <summary>
    /// Plan Addresses Controller
    /// </summary>
    /// <seealso cref="Axis.DataEngine.Helpers.Controllers.BaseApiController" />
    public class PlanAddressesController : BaseApiController
    {
        #region Class Variables        
        /// <summary>
        /// The plan addresses data provider
        /// </summary>
        readonly IPlanAddressesDataProvider _planAddressesDataProvider = null;

        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="PlanAddressesController"/> class.
        /// </summary>
        /// <param name="planAddressesDataProvider">The plan addresses data provider.</param>
        public PlanAddressesController(IPlanAddressesDataProvider planAddressesDataProvider)
        {
            _planAddressesDataProvider = planAddressesDataProvider;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the plan addresses.
        /// </summary>
        /// <param name="payorPlanID">The payor plan identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPlanAddresses(int payorPlanID)
        {
            return new HttpResult<Response<PlanAddressesModel>>(_planAddressesDataProvider.GetPlanAddresses(payorPlanID), Request);
        }

        /// <summary>
        /// Gets the plan address.
        /// </summary>
        /// <param name="payorAddressID">The payor address identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPlanAddress(int payorAddressID)
        {
            return new HttpResult<Response<PlanAddressesModel>>(_planAddressesDataProvider.GetPlanAddress(payorAddressID), Request);
        }

        /// <summary>
        /// Adds the plan address.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddPlanAddress(PlanAddressesModel payorDetails)
        {
            return new HttpResult<Response<PlanAddressesModel>>(_planAddressesDataProvider.AddPlanAddress(payorDetails), Request);
        }


        /// <summary>
        /// Updates the plan address.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdatePlanAddress(PlanAddressesModel payorDetails)
        {
            return new HttpResult<Response<PlanAddressesModel>>(_planAddressesDataProvider.UpdatePlanAddress(payorDetails), Request);
        }

        #endregion
    }
}
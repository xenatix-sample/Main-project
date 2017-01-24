using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.BusinessAdmin.Payors;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    /// <summary>
    /// Payors Controller
    /// </summary>
    /// <seealso cref="Axis.DataEngine.Helpers.Controllers.BaseApiController" />
    public class PayorsController : BaseApiController
    {
        #region Class Variables        
        /// <summary>
        /// The payors data provider
        /// </summary>
        readonly IPayorsDataProvider _payorsDataProvider = null;

        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="PayorsController"/> class.
        /// </summary>
        /// <param name="payorsDataProvider">The payors data provider.</param>
        public PayorsController(IPayorsDataProvider payorsDataProvider)
        {
            _payorsDataProvider = payorsDataProvider;
        }

        #endregion

        #region Public Methods 

        /// <summary>
        /// Gets payors.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPayors(string searchText)
        {
            return new HttpResult<Response<PayorsModel>>(_payorsDataProvider.GetPayors(searchText), Request);
        }

       
        /// <summary>
        /// Adds the payor.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddPayor(PayorsModel payorDetails)
        {
            return new HttpResult<Response<PayorsModel>>(_payorsDataProvider.AddPayor(payorDetails), Request);
        }


        /// <summary>
        /// Updates the payor.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdatePayor(PayorsModel payorDetails)
        {
            return new HttpResult<Response<PayorsModel>>(_payorsDataProvider.UpdatePayor(payorDetails), Request);
        }

        /// <summary>
        /// Gets the payor by identifier.
        /// </summary>
        /// <param name="payorID">The payor identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPayorByID(int payorID)
        {
            return new HttpResult<Response<PayorsModel>>(_payorsDataProvider.GetPayorByID(payorID), Request);
        }



        #endregion
    }
}
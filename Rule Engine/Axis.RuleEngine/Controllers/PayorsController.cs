using Axis.Helpers.Validation;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.RuleEngine.BusinessAdmin.Payors;
using Axis.RuleEngine.Helpers.Results;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    /// <summary>
    /// Payors Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class PayorsController : ApiController
    {
        #region Class Variables

        /// <summary>
        /// The payors rule engine
        /// </summary>
        private readonly IPayorsRuleEngine _payorsRuleEngine = null;

        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="PayorsController"/> class.
        /// </summary>
        /// <param name="payorsRuleEngine">The payors rule engine.</param>
        public PayorsController(IPayorsRuleEngine payorsRuleEngine)
        {
            _payorsRuleEngine = payorsRuleEngine;
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
            return new HttpResult<Response<PayorsModel>>(_payorsRuleEngine.GetPayors(searchText), Request);
        }


        /// <summary>
        /// Adds the payor.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddPayor(PayorsModel payorDetails)
        {
            return new HttpResult<Response<PayorsModel>>(_payorsRuleEngine.AddPayor(payorDetails), Request);
        }

        /// <summary>
        /// Updates the payor.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdatePayor(PayorsModel payorDetails)
        {
            return new HttpResult<Response<PayorsModel>>(_payorsRuleEngine.UpdatePayor(payorDetails), Request);
        }


        /// <summary>
        /// Gets the payor by identifier.
        /// </summary>
        /// <param name="payorId">The payor identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPayorByID(int payorId)
        {
            return new HttpResult<Response<PayorsModel>>(_payorsRuleEngine.GetPayorByID(payorId), Request);
        }

        #endregion
    }
}
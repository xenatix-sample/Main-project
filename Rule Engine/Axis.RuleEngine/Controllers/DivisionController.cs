using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.RuleEngine.BusinessAdmin.Division;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    public class DivisionController : BaseApiController
    {
        #region Class Variables

        private readonly IDivisionRuleEngine _divisionRuleEngine = null;

        #endregion Class Variables

        #region Constructors

        public DivisionController(IDivisionRuleEngine divisionRuleEngine)
        {
            _divisionRuleEngine = divisionRuleEngine;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the divisions.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetDivisions()
        {
            return new HttpResult<Response<OrganizationModel>>(_divisionRuleEngine.GetDivisions(), Request);
        }

        /// <summary>
        /// Gets the division by identifier.
        /// </summary>
        /// <param name="divisionID">The division identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetDivisionByID(long divisionID)
        {
            return new HttpResult<Response<DivisionDetailsModel>>(_divisionRuleEngine.GetDivisionByID(divisionID), Request);
        }

        /// <summary>
        /// Saves the division.
        /// </summary>
        /// <param name="division">The division.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveDivision(DivisionDetailsModel division)
        {
            return new HttpResult<Response<DivisionDetailsModel>>(_divisionRuleEngine.SaveDivision(division), Request);
        }

        #endregion Public Methods
    }
}
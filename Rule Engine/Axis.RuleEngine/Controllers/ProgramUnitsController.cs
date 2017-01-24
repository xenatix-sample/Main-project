using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.RuleEngine.BusinessAdmin.ProgramUnit;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    public class ProgramUnitsController : BaseApiController
    {
        #region Class Variables

        /// <summary>
        /// The program units rule engine
        /// </summary>
        private readonly IProgramUnitsRuleEngine _programUnitsRuleEngine = null;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramUnitsController"/> class.
        /// </summary>
        /// <param name="programUnitsRuleEngine">The program units rule engine.</param>
        public ProgramUnitsController(IProgramUnitsRuleEngine programUnitsRuleEngine)
        {
            _programUnitsRuleEngine = programUnitsRuleEngine;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the program units.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetProgramUnits()
        {
            return new HttpResult<Response<OrganizationModel>>(_programUnitsRuleEngine.GetProgramUnits(), Request);
        }

        /// <summary>
        /// Gets the program unit by identifier.
        /// </summary>
        /// <param name="programUnitID">The program unit identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetProgramUnitByID(long programUnitID)
        {
            return new HttpResult<Response<ProgramUnitDetailsModel>>(_programUnitsRuleEngine.GetProgramUnitByID(programUnitID), Request);
        }

        /// <summary>
        /// Saves the program unit.
        /// </summary>
        /// <param name="programUnit">The program unit.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveProgramUnit(ProgramUnitDetailsModel programUnit)
        {
            return new HttpResult<Response<ProgramUnitDetailsModel>>(_programUnitsRuleEngine.SaveProgramUnit(programUnit), Request);
        }

        #endregion Public Methods
    }
}
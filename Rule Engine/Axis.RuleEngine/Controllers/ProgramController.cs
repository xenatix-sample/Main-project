using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.RuleEngine.BusinessAdmin.Program;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    public class ProgramController : BaseApiController
    {
        #region Class Variables

        private readonly IProgramRuleEngine _programRuleEngine = null;

        #endregion Class Variables

        #region Constructors

        public ProgramController(IProgramRuleEngine programRuleEngine)
        {
            _programRuleEngine = programRuleEngine;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the programs.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPrograms()
        {
            return new HttpResult<Response<OrganizationModel>>(_programRuleEngine.GetPrograms(), Request);
        }

        /// <summary>
        /// Gets the program by identifier.
        /// </summary>
        /// <param name="programID">The program identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetProgramByID(long programID)
        {
            return new HttpResult<Response<ProgramDetailsModel>>(_programRuleEngine.GetProgramByID(programID), Request);
        }

        /// <summary>
        /// Saves the program.
        /// </summary>
        /// <param name="program">The program.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveProgram(ProgramDetailsModel program)
        {
            return new HttpResult<Response<ProgramDetailsModel>>(_programRuleEngine.SaveProgram(program), Request);
        }

        #endregion Public Methods
    }
}
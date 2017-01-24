using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.BusinessAdmin.Program;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    public class ProgramController : BaseApiController
    {
        #region Class Variables

        private readonly IProgramsDataProvider _programDataProvider = null;

        #endregion Class Variables

        #region Constructors

        public ProgramController(IProgramsDataProvider programDataProvider)
        {
            _programDataProvider = programDataProvider;
        }

        #endregion Constructors

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetPrograms()
        {
            return new HttpResult<Response<OrganizationModel>>(_programDataProvider.GetPrograms(), Request);
        }

        /// <summary>
        /// Gets the program by identifier.
        /// </summary>
        /// <param name="programID">The program identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetProgramByID(long programID)
        {
            return new HttpResult<Response<ProgramDetailsModel>>(_programDataProvider.GetProgramByID(programID), Request);
        }

        /// <summary>
        /// Saves the program.
        /// </summary>
        /// <param name="program">The program.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveProgram(ProgramDetailsModel program)
        {
            return new HttpResult<Response<ProgramDetailsModel>>(_programDataProvider.SaveProgram(program), Request);
        }

        #endregion Public Methods
    }
}
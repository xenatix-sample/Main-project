using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.BusinessAdmin.ProgramUnit;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    public class ProgramUnitsController : BaseApiController
    {
        #region Class Variables

        /// <summary>
        /// The payors data provider
        /// </summary>
        private readonly IProgramUnitsDataProvider _programUnitsDataProvider = null;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PayorsController"/> class.
        /// </summary>
        /// <param name="payorsDataProvider">The payors data provider.</param>
        public ProgramUnitsController(IProgramUnitsDataProvider programUnitsDataProvider)
        {
            _programUnitsDataProvider = programUnitsDataProvider;
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
            return new HttpResult<Response<OrganizationModel>>(_programUnitsDataProvider.GetProgramUnits(), Request);
        }

        /// <summary>
        /// Gets the program unit by identifier.
        /// </summary>
        /// <param name="programUnitID">The program unit identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetProgramUnitByID(long programUnitID)
        {
            return new HttpResult<Response<ProgramUnitDetailsModel>>(_programUnitsDataProvider.GetProgramUnitByID(programUnitID), Request);
        }

        /// <summary>
        /// Saves the program unit.
        /// </summary>
        /// <param name="programUnit">The program unit.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveProgramUnit(ProgramUnitDetailsModel programUnit)
        {
            return new HttpResult<Response<ProgramUnitDetailsModel>>(_programUnitsDataProvider.SaveProgramUnit(programUnit), Request);
        }

        #endregion Public Methods
    }
}
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Admin.DivisionProgram;
using Axis.Model.Admin;
using Axis.Model.Common;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    public class DivisionProgramController : BaseApiController
    {
        #region Class Variables

        readonly IDivisionProgramDataProvider _divisionProgramDataProvider = null;

        #endregion

        #region Constructors

        public DivisionProgramController(IDivisionProgramDataProvider divisionProgramDataProvider)
        {
            _divisionProgramDataProvider = divisionProgramDataProvider;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetDivisionPrograms(int userID)
        {
            return new HttpResult<Response<DivisionProgramModel>>(_divisionProgramDataProvider.GetDivisionPrograms(userID), Request);
        }

        [HttpPost]
        public IHttpActionResult SaveDivisionProgram(DivisionProgramModel divisionProgram)
        {
            return new HttpResult<Response<DivisionProgramModel>>(_divisionProgramDataProvider.SaveDivisionProgram(divisionProgram), Request);
        }

        #endregion
    }
}
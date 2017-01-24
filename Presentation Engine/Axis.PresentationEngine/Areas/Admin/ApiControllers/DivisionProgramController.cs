using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Admin.Models;
using Axis.PresentationEngine.Areas.Admin.Respository.DivisionProgram;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.Admin.ApiControllers
{
    public class DivisionProgramController : BaseApiController
    {
        #region Class Variables

        private readonly IDivisionProgramRepository _divisionProgramRepository;

        #endregion

        #region Constructors

        public DivisionProgramController(IDivisionProgramRepository divisionProgramRepository)
        {
            _divisionProgramRepository = divisionProgramRepository;
        }

        #endregion

        #region Public Methods
        public Response<DivisionProgramViewModel> GetDivisionPrograms(int userID, bool isMyProfile)
        {
            return _divisionProgramRepository.GetDivisionPrograms(userID, isMyProfile);
        }

        public Response<DivisionProgramViewModel> SaveDivisionProgram(DivisionProgramViewModel divisionProgram, bool isMyProfile)
        {
            var result = _divisionProgramRepository.SaveDivisionProgram(divisionProgram, isMyProfile);
            ClearCache(result);
            return result;
        }

        #endregion
    }
}

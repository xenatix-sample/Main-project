using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.ProgramUnit;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.ApiControllers
{
    public class ProgramUnitsController : BaseApiController
    {
        #region Class Variables

        /// <summary>
        /// The program units repository
        /// </summary>
        private readonly IProgramUnitsRepository _programUnitsRepository;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramUnitsController"/> class.
        /// </summary>
        /// <param name="programUnitsRepository">The program units repository.</param>
        public ProgramUnitsController(IProgramUnitsRepository programUnitsRepository)
        {
            _programUnitsRepository = programUnitsRepository;
        }

        /// <summary>
        /// Gets the program units.
        /// </summary>
        /// <returns></returns>
        public Response<OrganizationModel> GetProgramUnits()
        {
            return _programUnitsRepository.GetProgramUnits();
        }

        /// <summary>
        /// Gets the program unit by identifier.
        /// </summary>
        /// <param name="programUnitID">The program unit identifier.</param>
        /// <returns></returns>
        public Response<ProgramUnitDetailsModel> GetProgramUnitByID(long programUnitID)
        {
            return _programUnitsRepository.GetProgramUnitByID(programUnitID);
        }

        /// <summary>
        /// Save the program unit.
        /// </summary>
        /// <param name="programUnit">The program unit.</param>
        /// <returns></returns>
        public Response<ProgramUnitDetailsModel> SaveProgramUnit(ProgramUnitDetailsModel programUnit)
        {
            return _programUnitsRepository.SaveProgramUnit(programUnit);
        }

        #endregion Constructors
    }
}
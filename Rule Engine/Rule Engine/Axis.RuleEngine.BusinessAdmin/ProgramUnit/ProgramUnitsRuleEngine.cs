using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Service.BusinessAdmin.ProgramUnit;

namespace Axis.RuleEngine.BusinessAdmin.ProgramUnit
{
    public class ProgramUnitsRuleEngine : IProgramUnitsRuleEngine
    {
        #region Class Variables

        /// <summary>
        /// The program units service
        /// </summary>
        private readonly IProgramUnitsService _programUnitsService;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramUnitsRuleEngine"/> class.
        /// </summary>
        /// <param name="programUnitsService">The program units service.</param>
        public ProgramUnitsRuleEngine(IProgramUnitsService programUnitsService)
        {
            _programUnitsService = programUnitsService;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the program units.
        /// </summary>
        /// <returns></returns>
        public Response<OrganizationModel> GetProgramUnits()
        {
            return _programUnitsService.GetProgramUnits();
        }

        /// <summary>
        /// Gets the program unit by identifier.
        /// </summary>
        /// <param name="programUnitID">The program unit identifier.</param>
        /// <returns></returns>
        public Response<ProgramUnitDetailsModel> GetProgramUnitByID(long programUnitID)
        {
            return _programUnitsService.GetProgramUnitByID(programUnitID);
        }

        /// <summary>
        /// Save the program unit.
        /// </summary>
        /// <param name="programUnit">The program unit.</param>
        /// <returns></returns>
        public Response<ProgramUnitDetailsModel> SaveProgramUnit(ProgramUnitDetailsModel programUnit)
        {
            return _programUnitsService.SaveProgramUnit(programUnit);
        }

        #endregion Public Methods
    }
}
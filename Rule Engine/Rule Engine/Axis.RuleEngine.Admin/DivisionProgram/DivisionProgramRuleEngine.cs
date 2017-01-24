using System;
using Axis.Model.Admin;
using Axis.Model.Common;
using Axis.Service.Admin.DivisionProgram;

namespace Axis.RuleEngine.Admin.DivisionProgram
{
    public class DivisionProgramRuleEngine : IDivisionProgramRuleEngine
    {
        #region Class Variables

        private readonly IDivisionProgramService _divisionProgramService;

        #endregion

        #region Constructors

        public DivisionProgramRuleEngine(IDivisionProgramService divisionProgramService)
        {
            _divisionProgramService = divisionProgramService;
        }

        #endregion

        #region Public Methods
        public Response<DivisionProgramModel> GetDivisionPrograms(int userID)
        {
            return _divisionProgramService.GetDivisionPrograms(userID);
        }

        public Response<DivisionProgramModel> SaveDivisionProgram(DivisionProgramModel divisionProgram)
        {
            return _divisionProgramService.SaveDivisionProgram(divisionProgram);
        }
        #endregion
    }
}

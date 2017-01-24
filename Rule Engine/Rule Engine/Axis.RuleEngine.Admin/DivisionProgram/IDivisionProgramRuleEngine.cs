using Axis.Model.Admin;
using Axis.Model.Common;
using System;


namespace Axis.RuleEngine.Admin.DivisionProgram
{
    public interface IDivisionProgramRuleEngine
    {
        Response<DivisionProgramModel> GetDivisionPrograms(int userID);
        Response<DivisionProgramModel> SaveDivisionProgram(DivisionProgramModel divisionProgram);
    }
}

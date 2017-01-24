

using Axis.Model.Admin;
using Axis.Model.Common;

namespace Axis.DataProvider.Admin.DivisionProgram
{
    public interface IDivisionProgramDataProvider
    {
        Response<DivisionProgramModel> GetDivisionPrograms(int userID);
        Response<DivisionProgramModel> SaveDivisionProgram(DivisionProgramModel divisionProgram);
    }
}

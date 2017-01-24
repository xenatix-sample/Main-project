
using Axis.Model.Admin;
using Axis.Model.Common;

namespace Axis.Service.Admin.DivisionProgram
{
    public interface IDivisionProgramService
    {
        Response<DivisionProgramModel> GetDivisionPrograms(int userID);

        Response<DivisionProgramModel> SaveDivisionProgram(DivisionProgramModel divisionProgram);
    }
}

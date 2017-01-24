using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Admin.Models;

namespace Axis.PresentationEngine.Areas.Admin.Respository.DivisionProgram
{
    public interface IDivisionProgramRepository
    {
        Response<DivisionProgramViewModel> GetDivisionPrograms(int userID, bool isMyProfile);
        Response<DivisionProgramViewModel> SaveDivisionProgram(DivisionProgramViewModel divisionProgram, bool isMyProfile);
        Task<Response<DivisionProgramViewModel>> GetDivisionProgramsAsync(int userID, bool isMyProfile);
    }
}

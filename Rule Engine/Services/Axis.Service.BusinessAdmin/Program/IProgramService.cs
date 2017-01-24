using Axis.Model.BusinessAdmin;
using Axis.Model.Common;

namespace Axis.Service.BusinessAdmin.Program
{
    /// <summary>
    ///
    /// </summary>
    public interface IProgramService
    {
        /// <summary>
        /// Gets the programs.
        /// </summary>
        /// <returns></returns>
        Response<OrganizationModel> GetPrograms();

        /// <summary>
        /// Gets the program by identifier.
        /// </summary>
        /// <param name="programID">The program identifier.</param>
        /// <returns></returns>
        Response<ProgramDetailsModel> GetProgramByID(long programID);

        /// <summary>
        /// Saves the program.
        /// </summary>
        /// <param name="program">The program.</param>
        /// <returns></returns>
        Response<ProgramDetailsModel> SaveProgram(ProgramDetailsModel program);
    }
}
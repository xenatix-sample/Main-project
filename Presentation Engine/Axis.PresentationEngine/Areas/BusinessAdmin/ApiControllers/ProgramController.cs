using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.Division;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.Program;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.ApiControllers
{
    public class ProgramController : BaseApiController
    {
        #region Class Variables

        private readonly IProgramRepository _programRepository;

        #endregion Class Variables

        #region Constructors

        public ProgramController(IProgramRepository programRepository)
        {
            _programRepository = programRepository;
        }

        /// <summary>
        /// Gets the divisions.
        /// </summary>
        /// <returns></returns>
        public Response<OrganizationModel> GetPrograms()
        {
            return _programRepository.GetPrograms();
        }

        /// <summary>
        /// Gets the division by identifier.
        /// </summary>
        /// <param name="programID">The division identifier.</param>
        /// <returns></returns>
        public Response<ProgramDetailsModel> GetProgramByID(long programID)
        {
            return _programRepository.GetProgramByID(programID);
        }

        /// <summary>
        /// Saves the division.
        /// </summary>
        /// <param name="program">The division.</param>
        /// <returns></returns>
        public Response<ProgramDetailsModel> SaveProgram(ProgramDetailsModel program)
        {
            return _programRepository.SaveProgram(program);
        }

        #endregion Constructors
    }
}
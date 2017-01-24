using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Service.BusinessAdmin.Program;

namespace Axis.RuleEngine.BusinessAdmin.Program
{
    public class ProgramRuleEngine : IProgramRuleEngine
    {
        #region Class Variables

        
        private readonly IProgramService _programService;

        #endregion Class Variables

        #region Constructors

        public ProgramRuleEngine(IProgramService programService)
        {
            _programService = programService;
        }

        #endregion Constructors

        #region Public Methods

        
        public Response<OrganizationModel> GetPrograms()
        {
            return _programService.GetPrograms();
        }

        
        public Response<ProgramDetailsModel> GetProgramByID(long programID)
        {
            return _programService.GetProgramByID(programID);
        }


        
        public Response<ProgramDetailsModel> SaveProgram(ProgramDetailsModel program)
        {
            return _programService.SaveProgram(program);
        }

        #endregion Public Methods
    }
}
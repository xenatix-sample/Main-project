using Axis.Configuration;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Security;
using System.Collections.Specialized;

namespace Axis.Service.BusinessAdmin.Program
{
    public class ProgramService : IProgramService
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "Program/";

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramService"/> class.
        /// </summary>
        public ProgramService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the programs.
        /// </summary>
        /// <returns></returns>
        public Response<OrganizationModel> GetPrograms()
        {
            const string apiUrl = BaseRoute + "GetPrograms";
            return communicationManager.Get<Response<OrganizationModel>>(apiUrl);
        }

        /// <summary>
        /// Gets the program by identifier.
        /// </summary>
        /// <param name="programID">The program identifier.</param>
        /// <returns></returns>
        public Response<ProgramDetailsModel> GetProgramByID(long programID)
        {
            const string apiUrl = BaseRoute + "GetProgramByID";
            var param = new NameValueCollection { { "programID", programID.ToString() } };
            return communicationManager.Get<Response<ProgramDetailsModel>>(param, apiUrl);
        }

        /// <summary>
        /// Saves the program.
        /// </summary>
        /// <param name="program">The program.</param>
        /// <returns></returns>
        public Response<ProgramDetailsModel> SaveProgram(ProgramDetailsModel program)
        {
            const string apiUrl = BaseRoute + "SaveProgram";
            return communicationManager.Post<ProgramDetailsModel, Response<ProgramDetailsModel>>(program, apiUrl);
        }

        #endregion Public Methods
    }
}
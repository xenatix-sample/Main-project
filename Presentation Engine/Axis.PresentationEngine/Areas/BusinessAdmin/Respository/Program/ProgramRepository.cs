using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Service;
using System.Collections.Specialized;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Respository.Program
{
    public class ProgramRepository : IProgramRepository
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
        /// Initializes a new instance of the <see cref="CompanyRepository"/> class.
        /// </summary>
        public ProgramRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ProgramRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
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
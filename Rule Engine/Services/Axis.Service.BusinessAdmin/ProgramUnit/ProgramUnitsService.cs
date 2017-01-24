using Axis.Configuration;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Security;
using System.Collections.Specialized;

namespace Axis.Service.BusinessAdmin.ProgramUnit
{
    public class ProgramUnitsService : IProgramUnitsService
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "ProgramUnits/";

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramUnitsService"/> class.
        /// </summary>
        public ProgramUnitsService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the program units.
        /// </summary>
        /// <returns></returns>
        public Response<OrganizationModel> GetProgramUnits()
        {
            const string apiUrl = BaseRoute + "GetProgramUnits";
            return communicationManager.Get<Response<OrganizationModel>>(apiUrl);
        }

        /// <summary>
        /// Gets the program unit by identifier.
        /// </summary>
        /// <param name="programUnitID">The program unit identifier.</param>
        /// <returns></returns>
        public Response<ProgramUnitDetailsModel> GetProgramUnitByID(long programUnitID)
        {
            const string apiUrl = BaseRoute + "GetProgramUnitByID";
            var requestXMLValueNvc = new NameValueCollection { { "programUnitID", programUnitID.ToString() } };
            return communicationManager.Get<Response<ProgramUnitDetailsModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Save the program unit.
        /// </summary>
        /// <param name="programUnit">The program unit.</param>
        /// <returns></returns>
        public Response<ProgramUnitDetailsModel> SaveProgramUnit(ProgramUnitDetailsModel programUnit)
        {
            const string apiUrl = BaseRoute + "SaveProgramUnit";
            return communicationManager.Post<ProgramUnitDetailsModel, Response<ProgramUnitDetailsModel>>(programUnit, apiUrl);
        }

        #endregion Public Methods
    }
}
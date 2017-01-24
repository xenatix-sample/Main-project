using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Service;
using System.Collections.Specialized;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Respository.ProgramUnit
{
    public class ProgramUnitsRepository : IProgramUnitsRepository
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

        public ProgramUnitsRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanAddressesRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ProgramUnitsRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
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
            var param = new NameValueCollection { { "programUnitID", programUnitID.ToString() } };
            return communicationManager.Get<Response<ProgramUnitDetailsModel>>(param, apiUrl);
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
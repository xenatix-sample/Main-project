using Axis.Configuration;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Security;
using System.Collections.Specialized;

namespace Axis.Service.BusinessAdmin.Division
{
    public class DivisionService : IDivisionService
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "Division/";

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DivisionService"/> class.
        /// </summary>
        public DivisionService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the divisions.
        /// </summary>
        /// <returns></returns>
        public Response<OrganizationModel> GetDivisions()
        {
            const string apiUrl = BaseRoute + "GetDivisions";
            return communicationManager.Get<Response<OrganizationModel>>(apiUrl);
        }

        /// <summary>
        /// Gets the division by identifier.
        /// </summary>
        /// <param name="divisionID">The division identifier.</param>
        /// <returns></returns>
        public Response<DivisionDetailsModel> GetDivisionByID(long divisionID)
        {
            const string apiUrl = BaseRoute + "GetDivisionByID";
            var param = new NameValueCollection { { "divisionID", divisionID.ToString() } };
            return communicationManager.Get<Response<DivisionDetailsModel>>(param, apiUrl);
        }

        /// <summary>
        /// Saves the division.
        /// </summary>
        /// <param name="division">The division.</param>
        /// <returns></returns>
        public Response<DivisionDetailsModel> SaveDivision(DivisionDetailsModel division)
        {
            const string apiUrl = BaseRoute + "SaveDivision";
            return communicationManager.Post<DivisionDetailsModel, Response<DivisionDetailsModel>>(division, apiUrl);
        }

        #endregion Public Methods
    }
}
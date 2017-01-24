using Axis.Configuration;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Security;
using System.Collections.Specialized;

namespace Axis.Service.BusinessAdmin.OrganizationStructure
{
    public class OrganizationStructureService : IOrganizationStructureService
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "OrganizationStructure/";

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationStructureService"/> class.
        /// </summary>
        public OrganizationStructureService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the organization structure by identifier.
        /// </summary>
        /// <param name="detailID">The detail identifier.</param>
        /// <returns></returns>
        public Response<OrganizationDetailsModel> GetOrganizationStructureByID(long detailID)
        {
            const string apiUrl = BaseRoute + "GetOrganizationStructureByID";
            var requestParams = new NameValueCollection { { "detailID", detailID.ToString() } };
            return communicationManager.Get<Response<OrganizationDetailsModel>>(requestParams, apiUrl);
        }

        /// <summary>
        /// Gets the service organization details by identifier.
        /// </summary>
        /// <param name="detailID">The detail identifier.</param>
        /// <returns></returns>
        public Response<ServiceOrganizationModel> GetServiceOrganizationDetailsByID(long detailID)
        {
            const string apiUrl = BaseRoute + "GetServiceOrganizationDetailsByID";
            var requestParams = new NameValueCollection { { "detailID", detailID.ToString() } };
            return communicationManager.Get<Response<ServiceOrganizationModel>>(requestParams, apiUrl);
        }

        #endregion Public Methods
    }
}
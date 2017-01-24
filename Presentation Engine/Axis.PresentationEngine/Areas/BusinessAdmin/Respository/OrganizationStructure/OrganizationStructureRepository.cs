using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Service;
using System.Collections.Specialized;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Respository.OrganizationStructure
{
    public class OrganizationStructureRepository : IOrganizationStructureRepository
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

        public OrganizationStructureRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public OrganizationStructureRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
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
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.BusinessAdmin.OrganizationStructure;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    public class OrganizationStructureController : BaseApiController
    {
        #region Class Variables

        /// <summary>
        /// The organization structure data provider
        /// </summary>
        private readonly IOrganizationStructureDataProvider _organizationStructureDataProvider = null;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationStructureController"/> class.
        /// </summary>
        /// <param name="organizationStructureDataProvider">The organization structure data provider.</param>
        public OrganizationStructureController(IOrganizationStructureDataProvider organizationStructureDataProvider)
        {
            _organizationStructureDataProvider = organizationStructureDataProvider;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the organization structure by identifier.
        /// </summary>
        /// <param name="detailID">The detail identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetOrganizationStructureByID(long detailID)
        {
            return new HttpResult<Response<OrganizationDetailsModel>>(_organizationStructureDataProvider.GetOrganizationStructureByID(detailID), Request);
        }

        /// <summary>
        /// Gets the service organization details by identifier.
        /// </summary>
        /// <param name="detailID">The detail identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetServiceOrganizationDetailsByID(long detailID)
        {
            return new HttpResult<Response<ServiceOrganizationModel>>(_organizationStructureDataProvider.GetServicesOrganizationModuleComponentDetailsByDetailID(detailID), Request);
        }

        #endregion Public Methods
    }
}
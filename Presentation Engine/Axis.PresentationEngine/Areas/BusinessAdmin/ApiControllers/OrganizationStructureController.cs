using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.OrganizationStructure;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.ApiControllers
{
    public class OrganizationStructureController : BaseApiController
    {
        #region Class Variables

        /// <summary>
        /// The organization structure repository
        /// </summary>
        private readonly IOrganizationStructureRepository _organizationStructureRepository;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationStructureController"/> class.
        /// </summary>
        /// <param name="organizationStructureRepository">The organization structure repository.</param>
        public OrganizationStructureController(IOrganizationStructureRepository organizationStructureRepository)
        {
            _organizationStructureRepository = organizationStructureRepository;
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
            return _organizationStructureRepository.GetOrganizationStructureByID(detailID);
        }

        /// <summary>
        /// Gets the service organization details by identifier.
        /// </summary>
        /// <param name="detailID">The detail identifier.</param>
        /// <returns></returns>
        public Response<ServiceOrganizationModel> GetServiceOrganizationDetailsByID(long detailID)
        {
            return _organizationStructureRepository.GetServiceOrganizationDetailsByID(detailID);
        }

        #endregion Public Methods
    }
}
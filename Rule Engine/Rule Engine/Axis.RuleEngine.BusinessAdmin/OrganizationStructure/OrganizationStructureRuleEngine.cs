using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Service.BusinessAdmin.OrganizationStructure;

namespace Axis.RuleEngine.BusinessAdmin.OrganizationStructure
{
    /// <summary>
    ///
    /// </summary>
    public class OrganizationStructureRuleEngine : IOrganizationStructureRuleEngine
    {
        #region Class Variables

        /// <summary>
        /// The program units service
        /// </summary>
        private readonly IOrganizationStructureService _organizationStructureService;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationStructureRuleEngine"/> class.
        /// </summary>
        /// <param name="organizationStructureService">The organization structure service.</param>
        public OrganizationStructureRuleEngine(IOrganizationStructureService organizationStructureService)
        {
            _organizationStructureService = organizationStructureService;
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
            return _organizationStructureService.GetOrganizationStructureByID(detailID);
        }

        /// <summary>
        /// Gets the service organization details by identifier.
        /// </summary>
        /// <param name="detailID">The detail identifier.</param>
        /// <returns></returns>
        public Response<ServiceOrganizationModel> GetServiceOrganizationDetailsByID(long detailID)
        {
            return _organizationStructureService.GetServiceOrganizationDetailsByID(detailID);
        }
        #endregion Public Methods
    }
}
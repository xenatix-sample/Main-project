using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.RuleEngine.BusinessAdmin.OrganizationStructure;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    public class OrganizationStructureController : BaseApiController
    {
        #region Class Variables

        /// <summary>
        /// The organization structure rule engine
        /// </summary>
        private readonly IOrganizationStructureRuleEngine _organizationStructureRuleEngine = null;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationStructureController"/> class.
        /// </summary>
        /// <param name="organizationStructureRuleEngine">The organization structure rule engine.</param>
        public OrganizationStructureController(IOrganizationStructureRuleEngine organizationStructureRuleEngine)
        {
            _organizationStructureRuleEngine = organizationStructureRuleEngine;
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
            return new HttpResult<Response<OrganizationDetailsModel>>(_organizationStructureRuleEngine.GetOrganizationStructureByID(detailID), Request);
        }

        /// <summary>
        /// Gets the service organization details by identifier.
        /// </summary>
        /// <param name="detailID">The detail identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetServiceOrganizationDetailsByID(long detailID)
        {
            return new HttpResult<Response<ServiceOrganizationModel>>(_organizationStructureRuleEngine.GetServiceOrganizationDetailsByID(detailID), Request);
        }

        #endregion Public Methods
    }
}
using System;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.RuleEngine.ECI;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.ECI
{
 
    public class IFSPController : BaseApiController
    {
        #region Class Variables

        readonly IIFSPRuleEngine _ifspRuleEngine;

        #endregion

        #region Constructors

        public IFSPController(IIFSPRuleEngine ifspRuleEngine)
        {
            _ifspRuleEngine = ifspRuleEngine;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets IFSP List
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [Authorization(PermissionKey = ECIPermissionKey.ECI_IFSP_IFSP, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetIFSPList(long contactId)
        {
            return new HttpResult<Response<IFSPDetailModel>>(_ifspRuleEngine.GetIFSPList(contactId), Request);
        }

        [Authorization(PermissionKey = ECIPermissionKey.ECI_IFSP_IFSP, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetIFSP(long ifspID)
        {
            return new HttpResult<Response<IFSPDetailModel>>(_ifspRuleEngine.GetIFSP(ifspID), Request);
        }

        /// <summary>
        /// Adds IFSP
        /// </summary>
        /// <param name="ifspDetail"></param>
        /// <returns></returns>
        [Authorization(PermissionKey = ECIPermissionKey.ECI_IFSP_IFSP, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddIFSP(IFSPDetailModel ifspDetail)
        {
            return new HttpResult<Response<IFSPDetailModel>>(_ifspRuleEngine.AddIFSP(ifspDetail), Request);
        }

        /// <summary>
        /// Updates IFSP
        /// </summary>
        /// <param name="ifspDetail"></param>
        /// <returns></returns>
        [Authorization(PermissionKey = ECIPermissionKey.ECI_IFSP_IFSP, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateIFSP(IFSPDetailModel ifspDetail)
        {
            return new HttpResult<Response<IFSPDetailModel>>(_ifspRuleEngine.UpdateIFSP(ifspDetail), Request);
        }

        [Authorization(PermissionKey = ECIPermissionKey.ECI_IFSP_IFSP, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult RemoveIFSP(long ifspID, DateTime modifiedOn)
        {
            return new HttpResult<Response<bool>>(_ifspRuleEngine.RemoveIFSP(ifspID, modifiedOn), Request);
        }

        /// <summary>
        /// Load the tema member data for the IFSP
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [Authorization(PermissionKey = ECIPermissionKey.ECI_IFSP_IFSP, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetIFSPMembers(long contactId)
        {
            return new HttpResult<Response<IFSPTeamMemberModel>>(_ifspRuleEngine.GetIFSPMembers(contactId), Request);
        }
        [Authorization(PermissionKey = ECIPermissionKey.ECI_IFSP_IFSP, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetIFSPParentGuardians(long contactId)
        {
            return new HttpResult<Response<IFSPParentGuardianModel>>(_ifspRuleEngine.GetIFSPParentGuardians(contactId), Request);
        }

        #endregion 
    }
}
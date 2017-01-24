using System;
using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.ECI.IFSP;
using Axis.Model.Common;
using Axis.Model.ECI;

namespace Axis.DataEngine.Plugins.ECI
{
    public class IFSPController : BaseApiController
    {
        #region Class Variables

        readonly IIFSPDataProvider _ifspDataProvider;

        #endregion

        #region Constructors

        public IFSPController(IIFSPDataProvider ifspDataProvider)
        {
            _ifspDataProvider = ifspDataProvider;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetIFSP(long ifspID)
        {
            return new HttpResult<Response<IFSPDetailModel>>(_ifspDataProvider.GetIFSP(ifspID), Request);
        }

        /// <summary>
        /// Gets the list of IFSP
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetIFSPList(long contactId)
        {
            return new HttpResult<Response<IFSPDetailModel>>(_ifspDataProvider.GetIFSPList(contactId), Request);
        }

        /// <summary>
        /// Adds new IFSP
        /// </summary>
        /// <param name="ifspDetail"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddIFSP(IFSPDetailModel ifspDetail)
        {
            return new HttpResult<Response<IFSPDetailModel>>(_ifspDataProvider.AddIFSP(ifspDetail), Request);
        }

        /// <summary>
        /// Updates IFSP
        /// </summary>
        /// <param name="ifspDetail"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateIFSP(IFSPDetailModel ifspDetail)
        {
            return new HttpResult<Response<IFSPDetailModel>>(_ifspDataProvider.UpdateIFSP(ifspDetail), Request);
        }

        [HttpDelete]
        public IHttpActionResult RemoveIFSP(long ifspID, DateTime modifiedOn)
        {
            return new HttpResult<Response<bool>>(_ifspDataProvider.RemoveIFSP(ifspID, modifiedOn), Request);
        }

        /// <summary>
        /// Load the tema member data for the IFSP
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetIFSPMembers(long contactId)
        {
            return new HttpResult<Response<IFSPTeamMemberModel>>(_ifspDataProvider.GetIFSPMembers(contactId), Request);
        }

        [HttpGet]
        public IHttpActionResult GetIFSPParentGuardians(long contactId)
        {
            return new HttpResult<Response<IFSPParentGuardianModel>>(_ifspDataProvider.GetIFSPParentGuardians(contactId), Request);
        }

        #endregion 
    }
}
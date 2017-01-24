using System;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Service.ECI;

namespace Axis.RuleEngine.ECI
{
    public class IFSPRuleEngine : IIFSPRuleEngine
    {
        #region Class Variables

        private readonly IIFSPService _ifspService;

        #endregion

        #region Constructors

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="ifspService"></param>
        public IFSPRuleEngine(IIFSPService ifspService)
        {
            _ifspService = ifspService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets IFSP List
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public Response<IFSPDetailModel> GetIFSPList(long contactId)
        {
            return _ifspService.GetIFSPList(contactId);
        }

        public Response<IFSPDetailModel> GetIFSP(long ifspID)
        {
            return _ifspService.GetIFSP(ifspID);
        }

        /// <summary>
        /// Adds IFSP
        /// </summary>
        /// <param name="ifspDetail"></param>
        /// <returns></returns>
        public Response<IFSPDetailModel> AddIFSP(IFSPDetailModel ifspDetail)
        {
            return _ifspService.AddIFSP(ifspDetail);
        }

        /// <summary>
        /// Updates IFSP
        /// </summary>
        /// <param name="ifspDetail"></param>
        /// <returns></returns>
        public Response<IFSPDetailModel> UpdateIFSP(IFSPDetailModel ifspDetail)
        {
            return _ifspService.UpdateIFSP(ifspDetail);
        }

        public Response<bool> RemoveIFSP(long ifspID, DateTime modifiedOn)
        {
            return _ifspService.RemoveIFSP(ifspID, modifiedOn);
        }

        /// <summary>
        /// Get IFSP member list
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public Response<IFSPTeamMemberModel> GetIFSPMembers(long contactId)
        {
            return _ifspService.GetIFSPMembers(contactId);
        }

        public Response<IFSPParentGuardianModel> GetIFSPParentGuardians(long contactId)
        {
            return _ifspService.GetIFSPParentGuardians(contactId);
        }

        #endregion
    }
}
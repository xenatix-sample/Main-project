using System;
using Axis.Model.Common;
using Axis.Model.ECI;

namespace Axis.RuleEngine.ECI
{
    public interface IIFSPRuleEngine
    {
        /// <summary>
        /// Gets the list of IFSP
        /// </summary>
        /// <returns></returns>
        Response<IFSPDetailModel> GetIFSPList(long contactId);

        Response<IFSPDetailModel> GetIFSP(long ifspID);

        /// <summary>
        /// Adds IFSP
        /// </summary>
        /// <param name="ifspDetail"></param>
        /// <returns></returns>
        Response<IFSPDetailModel> AddIFSP(IFSPDetailModel ifspDetail);

        /// <summary>
        /// Adds IFSP
        /// </summary>
        /// <param name="ifspDetail"></param>
        /// <returns></returns>
        Response<IFSPDetailModel> UpdateIFSP(IFSPDetailModel ifspDetail);

        Response<bool> RemoveIFSP(long ifspID, DateTime modifiedOn);

        /// <summary>
        /// Load the tema member data for the IFSP
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        Response<IFSPTeamMemberModel> GetIFSPMembers(long contactId);

        Response<IFSPParentGuardianModel> GetIFSPParentGuardians(long contactId);
    }
}
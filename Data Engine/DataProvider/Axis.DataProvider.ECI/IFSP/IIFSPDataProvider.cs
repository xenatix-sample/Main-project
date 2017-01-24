using System;
using Axis.Model.Common;
using Axis.Model.ECI;

namespace Axis.DataProvider.ECI.IFSP
{
    public interface IIFSPDataProvider
    {
        Response<IFSPDetailModel> GetIFSP(long ifspID);

        /// <summary>
        /// Gets the list of IFSP
        /// </summary>
        /// <returns></returns>
        Response<IFSPDetailModel> GetIFSPList(long contactId);

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

        /// <summary>
        /// Load the tema member data for the IFSP
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        Response<IFSPTeamMemberModel> GetIFSPMembers(long contactId);

        Response<IFSPParentGuardianModel> GetIFSPParentGuardians(long contactId);
        Response<bool> RemoveIFSP(long screeningID, DateTime modifiedOn);

    }
}
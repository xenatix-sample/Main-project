using System;
using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Plugins.ECI.Model;

namespace Axis.Plugins.ECI
{
    public interface IIFSPRepository
    {

        Response<IFSPDetailModel> GetIFSP(long ifspID);

        /// <summary>
        /// Gets the list of IFSP
        /// </summary>
        /// <returns></returns>
        Response<IFSPDetailModel> GetIFSPList(long contactId);

        /// <summary>
        /// Gets list of IFSP asynchrnously
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        Task<Response<IFSPDetailModel>> GetIFSPListAsync(long contactId);

        /// <summary>
        /// Adds IFSP
        /// </summary>
        /// <param name="ifspDetail"></param>
        /// <returns></returns>
        Response<IFSPDetailViewModel> AddIFSP(IFSPDetailViewModel ifspDetail);

        /// <summary>
        /// Adds IFSP
        /// </summary>
        /// <param name="ifspDetail"></param>
        /// <returns></returns>
        Response<IFSPDetailViewModel> UpdateIFSP(IFSPDetailViewModel ifspDetail);

        Response<bool> RemoveIFSP(long ifspID, DateTime modifiedOn);

        /// <summary>
        /// Gets IFSP members list
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        Response<IFSPTeamMemberModel> GetIFSPMembers(long contactId);

        Response<IFSPParentGuardianModel> GetIFSPParentGuardians(long contactId);
    }
}
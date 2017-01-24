using System;
using System.Threading.Tasks;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Plugins.ECI.Model;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.ECI.ApiControllers
{
    public class IFSPController : BaseApiController
    {
        private readonly IIFSPRepository _ifspRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ifspRepository"></param>
        public IFSPController(IIFSPRepository ifspRepository)
        {
            _ifspRepository = ifspRepository;
        }

        [HttpGet]
        public Response<Axis.Model.ECI.IFSPDetailModel> GetIFSP(long ifspID)
        {
            return _ifspRepository.GetIFSP(ifspID);
        }

        /// <summary>
        /// Gets the list of IFSP
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<Axis.Model.ECI.IFSPDetailModel>> GetIFSPList(long contactId)
        {
            var result = await _ifspRepository.GetIFSPListAsync(contactId);
            return result;
        }

        [HttpPost]
        public Response<IFSPDetailViewModel> AddIFSP(IFSPDetailViewModel ifsp)
        {
            return _ifspRepository.AddIFSP(ifsp);
        }

        [HttpPut]
        public Response<IFSPDetailViewModel> UpdateIFSP(IFSPDetailViewModel ifsp)
        {
            return _ifspRepository.UpdateIFSP(ifsp);
        }

        [HttpDelete]
        public Response<bool> RemoveIFSP(long ifspID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _ifspRepository.RemoveIFSP(ifspID, modifiedOn);
        }

        /// <summary>
        /// Load the tema member data for the IFSP
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [HttpGet]
        public Response<Axis.Model.ECI.IFSPTeamMemberModel> GetIFSPMembers(long contactId)
        {
            return _ifspRepository.GetIFSPMembers(contactId);
        }

        [HttpGet]
        public Response<Axis.Model.ECI.IFSPParentGuardianModel> GetIFSPParentGuardians(long contactId)
        {
            return _ifspRepository.GetIFSPParentGuardians(contactId);
        }
    }
}
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Plugins.ECI.Model;
using Axis.Plugins.ECI.Translator;
using Axis.Service;


namespace Axis.Plugins.ECI
{
    /// <summary>
    /// Repository for IFSP
    /// </summary>
    public class IFSPRepository : IIFSPRepository
    {
        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "ifsp/";

        /// <summary>
        /// Constructor
        /// </summary>
        public IFSPRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public IFSPRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the list of IFSP
        /// </summary>
        /// <returns></returns>
     
        public Response<IFSPDetailModel> GetIFSPList(long contactId)
        {
            return GetIFSPListAsync(contactId).Result;
        }
 
        public Response<IFSPDetailModel> GetIFSP(long ifspID)
        {
            string apiUrl = BaseRoute + "GetIFSP";
            var param = new NameValueCollection { { "ifspID", ifspID.ToString(CultureInfo.InvariantCulture) } };
            var response = _communicationManager.Get<Response<IFSPDetailModel>>(param, apiUrl);
            return response;
        }

        /// <summary>
        /// Gets the list of IFSP
        /// </summary>
        /// <returns></returns>
    
        public async Task<Response<IFSPDetailModel>> GetIFSPListAsync(long contactId)
        {
            const string apiUrl = BaseRoute + "GetIFSPList";
            var param = new NameValueCollection {{"contactId", contactId.ToString(CultureInfo.InvariantCulture)}};
            return await _communicationManager.GetAsync<Response<IFSPDetailModel>>(param, apiUrl);
        }

        /// <summary>
        /// Adds IFSP
        /// </summary>
        /// <param name="ifspDetail"></param>
        /// <returns></returns>
    
        public Response<IFSPDetailViewModel> AddIFSP(IFSPDetailViewModel ifspDetail)
        {
            const string apiUrl = BaseRoute + "AddIFSP";
            var response = _communicationManager.Post<IFSPDetailModel, Response<IFSPDetailModel>>(ifspDetail.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates IFSP
        /// </summary>
        /// <param name="ifspDetail"></param>
        /// <returns></returns>
     
        public Response<IFSPDetailViewModel> UpdateIFSP(IFSPDetailViewModel ifspDetail)
        {
            const string apiUrl = BaseRoute + "UpdateIFSP";
            var response = _communicationManager.Put<IFSPDetailModel, Response<IFSPDetailModel>>(ifspDetail.ToModel(), apiUrl);
            return response.ToViewModel();
        }

  
        public Response<bool> RemoveIFSP(long ifspID, DateTime modifiedOn)
        {
            string apiUrl = BaseRoute + "RemoveIFSP";
            var param = new NameValueCollection
            {
                {"IFSPID", ifspID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return _communicationManager.Delete<Response<bool>>(param, apiUrl);
        }

        /// <summary>
        /// Get IFSP members list
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
   
        public Response<IFSPTeamMemberModel> GetIFSPMembers(long contactId)
        {
            const string apiUrl = BaseRoute + "GetIFSPMembers";
            var param = new NameValueCollection { { "contactId", contactId.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<IFSPTeamMemberModel>>(param, apiUrl);
        }

    
        public Response<IFSPParentGuardianModel> GetIFSPParentGuardians(long contactId)
        {
            const string apiUrl = BaseRoute + "GetIFSPParentGuardians";
            var param = new NameValueCollection { { "contactId", contactId.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<IFSPParentGuardianModel>>(param, apiUrl);
        }

    }
}
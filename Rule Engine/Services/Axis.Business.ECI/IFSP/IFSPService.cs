using System;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Configuration;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.ECI
{
    public class IFSPService : IIFSPService
    {
        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "ifsp/";

        /// <summary>
        /// Constructor
        /// </summary>
        public IFSPService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Gets the list of IFSP
        /// </summary>
        /// <returns></returns>
        public Response<IFSPDetailModel> GetIFSPList(long contactId)
        {
            const string apiUrl = BaseRoute + "GetIFSPList";
            var requestId = new NameValueCollection { { "contactId", contactId.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<IFSPDetailModel>>(requestId, apiUrl);
        }

        public Response<IFSPDetailModel> GetIFSP(long ifspID)
        {
            var apiUrl = BaseRoute + "GetIFSP";
            var param = new NameValueCollection();
            param.Add("ifspID", ifspID.ToString());

            return _communicationManager.Get<Response<IFSPDetailModel>>(param, apiUrl);
        }

        /// <summary>
        /// Adds IFSP
        /// </summary>
        /// <param name="ifspDetail"></param>
        /// <returns></returns>
        public Response<IFSPDetailModel> AddIFSP(IFSPDetailModel ifspDetail)
        {
            const string apiUrl = BaseRoute + "AddIFSP";
            return _communicationManager.Post<IFSPDetailModel, Response<IFSPDetailModel>>(ifspDetail, apiUrl);
        }

        /// <summary>
        /// Updates IFSP
        /// </summary>
        /// <param name="ifspDetail"></param>
        /// <returns></returns>
        public Response<IFSPDetailModel> UpdateIFSP(IFSPDetailModel ifspDetail)
        {
            const string apiUrl = BaseRoute + "UpdateIFSP";
            return _communicationManager.Put<IFSPDetailModel, Response<IFSPDetailModel>>(ifspDetail, apiUrl);
        }

        public Response<bool> RemoveIFSP(long ifspID, DateTime modifiedOn)
        {
            var apiUrl = BaseRoute + "RemoveIFSP";
            var param = new NameValueCollection
            {
                {"ifspID", ifspID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };

            return _communicationManager.Delete<Response<bool>>(param, apiUrl);
        }

        /// <summary>
        /// Gets IFSP members list
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public Response<IFSPTeamMemberModel> GetIFSPMembers(long contactId)
        {
            const string apiUrl = BaseRoute + "GetIFSPMembers";
            var requestId = new NameValueCollection { { "contactId", contactId.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<IFSPTeamMemberModel>>(requestId, apiUrl);
        }

        public Response<IFSPParentGuardianModel> GetIFSPParentGuardians(long contactId)
        {
            const string apiUrl = BaseRoute + "GetIFSPParentGuardians";
            var requestId = new NameValueCollection { { "contactId", contactId.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<IFSPParentGuardianModel>>(requestId, apiUrl);
        }
    }
}
using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Security;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Service.ECI
{
    public class ECIAdditionalDemographicService : IECIAdditionalDemographicService
    {
           private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "eciAdditionalDemographics/";

        public ECIAdditionalDemographicService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        public Response<ECIAdditionalDemographicsModel> GetAdditionalDemographic(long contactId)
        {
            const string apiUrl = BaseRoute + "GetAdditionalDemographic";
            var requestXMLValueNvc = new NameValueCollection { { "contactId", contactId.ToString(CultureInfo.InvariantCulture) } };

            return _communicationManager.Get<Response<ECIAdditionalDemographicsModel>>(requestXMLValueNvc, apiUrl);
        }

        public Response<ECIAdditionalDemographicsModel> AddAdditionalDemographic(ECIAdditionalDemographicsModel additional)
        {
            const string apiUrl = BaseRoute + "addAdditionalDemographic";
            return _communicationManager.Post<ECIAdditionalDemographicsModel, Response<ECIAdditionalDemographicsModel>>(additional, apiUrl);
        }

        public Response<ECIAdditionalDemographicsModel> UpdateAdditionalDemographic(ECIAdditionalDemographicsModel additional)
        {
            const string apiUrl = BaseRoute + "updateAdditionalDemographic";
            return _communicationManager.Post<ECIAdditionalDemographicsModel, Response<ECIAdditionalDemographicsModel>>(additional, apiUrl);
        }

        public Response<ECIAdditionalDemographicsModel> DeleteAdditionalDemographic(long id, DateTime modifiedOn)
        {
            throw new NotImplementedException();
        }
    }
}

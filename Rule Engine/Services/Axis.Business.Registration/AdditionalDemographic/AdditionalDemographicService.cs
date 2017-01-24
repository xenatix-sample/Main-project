using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Security;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Service.Registration
{
    public class AdditionalDemographicService : IAdditionalDemographicService
    {
        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "additionalDemographic/";

        public AdditionalDemographicService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        public Response<AdditionalDemographicsModel> GetAdditionalDemographic(long contactId)
        {
            const string apiUrl = BaseRoute + "GetAdditionalDemographic";
            var requestXMLValueNvc = new NameValueCollection { { "contactId", contactId.ToString(CultureInfo.InvariantCulture) } };

            return _communicationManager.Get<Response<AdditionalDemographicsModel>>(requestXMLValueNvc, apiUrl);
        }

        public Response<AdditionalDemographicsModel> AddAdditionalDemographic(AdditionalDemographicsModel additional)
        {
            const string apiUrl = BaseRoute + "addAdditionalDemographic";
            return _communicationManager.Post<AdditionalDemographicsModel, Response<AdditionalDemographicsModel>>(additional, apiUrl);
        }

        public Response<AdditionalDemographicsModel> UpdateAdditionalDemographic(AdditionalDemographicsModel additional)
        {
            const string apiUrl = BaseRoute + "updateAdditionalDemographic";
            return _communicationManager.Post<AdditionalDemographicsModel, Response<AdditionalDemographicsModel>>(additional, apiUrl);
        }

        public Response<AdditionalDemographicsModel> DeleteAdditionalDemographic(long id, DateTime modifiedOn)
        {
            throw new NotImplementedException();
        }
    }
}

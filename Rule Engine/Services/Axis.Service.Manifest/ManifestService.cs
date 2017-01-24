using Axis.Configuration;
using Axis.Model.Cache;
using Axis.Model.Common;
using Axis.Security;

namespace Axis.Service.Manifest
{
    public class ManifestService : IManifestService
    {
        private readonly CommunicationManager communicationManager;
        private const string baseRoute = "manifest/";

        public ManifestService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        public Response<ManifestModel> GetFilesToCache()
        {
            var apiUrl = baseRoute + "GetFilesToCache";
            return communicationManager.Get<Response<ManifestModel>>(apiUrl);
        }

    }
}

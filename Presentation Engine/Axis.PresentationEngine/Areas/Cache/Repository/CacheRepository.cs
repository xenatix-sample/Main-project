using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Axis.Model.Common;
using Axis.Model.Cache;
using System.Collections.Specialized;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Service;

namespace Axis.PresentationEngine.Areas.Cache.Repository
{
    public class CacheRepository : ICacheRepository
    {
        private readonly CommunicationManager communicationManager;
        private const string baseRoute = "manifest/";

        #region Constructors

        public CacheRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        #endregion

        #region Methods

        public Response<ManifestModel> GetFilesToCache()
        {
            var apiUrl = baseRoute + "/GetFilesToCache";

            return communicationManager.Get<Response<ManifestModel>>(apiUrl);
        }

        #endregion
    }
}
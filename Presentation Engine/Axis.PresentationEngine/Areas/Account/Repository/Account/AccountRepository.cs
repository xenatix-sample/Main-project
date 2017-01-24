using Axis.Model.Account;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;

namespace Axis.PresentationEngine.Areas.Account.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly CommunicationManager communicationManager;
        private const string baseRoute = "account/";

        public AccountRepository()
        {
            communicationManager = new CommunicationManager();
        }

        public AuthenticationModel Authenticate(UserViewModel user)
        {
            var apiUrl = baseRoute + "/authenticate";
            return communicationManager.Post<UserViewModel, AuthenticationModel>(user, apiUrl);
        }

        public Response<NavigationModel> GetNavigationItems()
        {
            CommunicationManager commManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
            var apiUrl = baseRoute + "/GetNavigationItems";
            var response = commManager.Get<Response<NavigationModel>>(apiUrl);

            return response;
        }
    }
}
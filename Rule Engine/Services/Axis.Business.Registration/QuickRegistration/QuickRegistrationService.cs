using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Service.Registration
{
    public class QuickRegistrationService : IQuickRegistrationService
    {
        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "quickRegistration/";

        public QuickRegistrationService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        public QuickRegistrationService(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }
       
        public Response<QuickRegistrationModel> GetQuickRegistration()
        {
            const string apiUrl = BaseRoute + "GetQuickRegistration";
            return _communicationManager.Get<Response<QuickRegistrationModel>>(apiUrl);
        }


        public Response<QuickRegistrationModel> Add(QuickRegistrationModel patient)
        {
            const string apiUrl = BaseRoute + "Add";
            return _communicationManager.Post<QuickRegistrationModel, Response<QuickRegistrationModel>>(patient, apiUrl);
        }




        public Response<QuickRegistrationModel> GetMrnMpi()
        {
            const string apiUrl = BaseRoute + "GetMrnMpi";
            return _communicationManager.Get<Response<QuickRegistrationModel>>(apiUrl);
        }
    }
}

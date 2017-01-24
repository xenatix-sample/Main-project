using System;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Model.ECI.EligibilityDetermination;
using Axis.Plugins.ECI.Models.EligibilityDetermination;
using Axis.Plugins.ECI.Translator;
using Axis.Service;
using Axis.Constant;


namespace Axis.Plugins.ECI.Repository.EligibilityDetermination
{
    public class EligibilityDeterminationRepository : IEligibilityDeterminationRepository
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "eligibilitydetermination/";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EligibilityDeterminationRepository"/> class.
        /// </summary>
        public EligibilityDeterminationRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        #endregion

        #region Public Methods

       
        public Response<EligibilityDeterminationViewModel> GetEligibilityDetermination(long contactID)
        {
            string apiUrl = BaseRoute + "GetEligibilityDetermination";
            var param = new NameValueCollection {{"contactID", contactID.ToString()}};

            var response = _communicationManager.Get<Response<EligibilityDeterminationModel>>(param, apiUrl);
            return response.ToModel();
        }

       
        public Response<EligibilityDeterminationViewModel> GetEligibility(long eligibilityID)
        {
            string apiUrl = BaseRoute + "GetEligibility";
            var param = new NameValueCollection { { "eligibilityID", eligibilityID.ToString() } };

            var response = _communicationManager.Get<Response<EligibilityDeterminationModel>>(param, apiUrl);
            return response.ToModel();
        }

      
        public Response<EligibilityTeamMemberModel> GetContactEligibilityMembers(long contactID)
        {
            string apiUrl = BaseRoute + "GetContactEligibilityMembers";
            var param = new NameValueCollection { { "contactID", contactID.ToString() } };

            var response = _communicationManager.Get<Response<EligibilityTeamMemberModel>>(param, apiUrl);
            return response;
        }

   
        public Response<EligibilityDeterminationViewModel> GetEligibilityNote(long eligibilityID)
        {
            string apiUrl = BaseRoute + "GetEligibilityNote";
            var param = new NameValueCollection { { "eligibilityID", eligibilityID.ToString() } };

            var response = _communicationManager.Get<Response<EligibilityDeterminationModel>>(param, apiUrl);
            return response.ToModel();
        }

       
        public Response<EligibilityDeterminationViewModel> AddEligibility(EligibilityDeterminationViewModel eligibilityDetermination)
        {
            string apiUrl = BaseRoute + "AddEligibility";
            var response = _communicationManager.Post<EligibilityDeterminationModel, Response<EligibilityDeterminationModel>>(eligibilityDetermination.ToModel(), apiUrl);
            return response.ToModel();
        }

        
        public Response<EligibilityDeterminationViewModel> UpdateEligibility(EligibilityDeterminationViewModel eligibilityDetermination)
        {
            string apiUrl = BaseRoute + "UpdateEligibility";
            var response = _communicationManager.Put<EligibilityDeterminationModel, Response<EligibilityDeterminationModel>>(eligibilityDetermination.ToModel(), apiUrl);
            return response.ToModel();
        }

      
        public Response<EligibilityDeterminationViewModel> DeactivateEligibility(long eligibilityID, DateTime modifiedOn)
        {
            string apiUrl = BaseRoute + "DeactivateEligibility";
            var param = new NameValueCollection
            {
                {"eligibilityID", eligibilityID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            var response = _communicationManager.Delete<Response<EligibilityDeterminationModel>>(param, apiUrl);

            return response.ToModel();
        }

   
        public Response<EligibilityDeterminationViewModel> SaveEligibilityNote(EligibilityDeterminationViewModel eligibilityDetermination)
        {
            string apiUrl = BaseRoute + "SaveEligibilityNote";
            var response = _communicationManager.Put<EligibilityDeterminationModel, Response<EligibilityDeterminationModel>>(eligibilityDetermination.ToModel(), apiUrl);
            return response.ToModel();
        }

        #endregion
    }
}

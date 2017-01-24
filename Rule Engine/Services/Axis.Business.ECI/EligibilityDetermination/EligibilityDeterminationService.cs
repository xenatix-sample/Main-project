using System;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Configuration;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Model.ECI.EligibilityDetermination;
using Axis.Security;

namespace Axis.Service.ECI.EligibilityDetermination
{
    public class EligibilityDeterminationService : IEligibilityDeterminationService
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "eligibilityDetermination/";

        #endregion

        #region Constructors

        public EligibilityDeterminationService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        public Response<EligibilityDeterminationModel> GetEligibilityDetermination(long contactID)
        {
            var apiUrl = BaseRoute + "GetEligibilityDetermination";
            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());

            return _communicationManager.Get<Response<EligibilityDeterminationModel>>(param, apiUrl);
        }

        public Response<EligibilityDeterminationModel> GetEligibility(long eligibilityID)
        {
            var apiUrl = BaseRoute + "GetEligibility";
            var param = new NameValueCollection();
            param.Add("eligibilityID", eligibilityID.ToString());

            return _communicationManager.Get<Response<EligibilityDeterminationModel>>(param, apiUrl);
        }

        public Response<EligibilityTeamMemberModel> GetContactEligibilityMembers(long contactID)
        {
            var apiUrl = BaseRoute + "GetContactEligibilityMembers";
            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());

            return _communicationManager.Get<Response<EligibilityTeamMemberModel>>(param, apiUrl);
        }

        public Response<EligibilityDeterminationModel> GetEligibilityNote(long eligibilityID)
        {
            var apiUrl = BaseRoute + "GetEligibilityNote";
            var param = new NameValueCollection();
            param.Add("eligibilityID", eligibilityID.ToString());

            return _communicationManager.Get<Response<EligibilityDeterminationModel>>(param, apiUrl);
        }

        public Response<EligibilityDeterminationModel> AddEligibility(EligibilityDeterminationModel eligibilityDetermination)
        {
            var apiUrl = BaseRoute + "AddEligibility";
            return _communicationManager.Post<EligibilityDeterminationModel, Response<EligibilityDeterminationModel>>(eligibilityDetermination, apiUrl);
        }

        public Response<EligibilityDeterminationModel> UpdateEligibility(EligibilityDeterminationModel eligibilityDetermination)
        {
            var apiUrl = BaseRoute + "UpdateEligibility";
            return _communicationManager.Put<EligibilityDeterminationModel, Response<EligibilityDeterminationModel>>(eligibilityDetermination, apiUrl);
        }

        public Response<EligibilityDeterminationModel> DeactivateEligibility(long eligibilityID, DateTime modifiedOn)
        {
            var apiUrl = BaseRoute + "DeactivateEligibility";
            var param = new NameValueCollection
            {
                {"eligibilityID", eligibilityID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return _communicationManager.Delete<Response<EligibilityDeterminationModel>>(param, apiUrl);
        }

        public Response<EligibilityDeterminationModel> SaveEligibilityNote(EligibilityDeterminationModel eligibilityDetermination)
        {
            var apiUrl = BaseRoute + "SaveEligibilityNote";
            return _communicationManager.Put<EligibilityDeterminationModel, Response<EligibilityDeterminationModel>>(eligibilityDetermination, apiUrl);
        }

        #endregion
    }
}

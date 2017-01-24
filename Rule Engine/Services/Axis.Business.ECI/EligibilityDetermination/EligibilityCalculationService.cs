using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.ECI.EligibilityDetermination;
using Axis.Security;
using System.Collections.Specialized;

namespace Axis.Service.ECI.EligibilityDetermination
{
    public class EligibilityCalculationService : IEligibilityCalculationService
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "eligibilityCalculation/";

        #endregion

        #region Constructors

        public EligibilityCalculationService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        public Response<EligibilityCalculationModel> GetEligibilityCalculations(long eligibilityID)
        {
            var apiUrl = BaseRoute + "GetEligibilityCalculations";
            var param = new NameValueCollection();
            param.Add("eligibilityID", eligibilityID.ToString());

            return _communicationManager.Get<Response<EligibilityCalculationModel>>(param, apiUrl);
        }

        public Response<EligibilityCalculationModel> AddEligibilityCalculations(EligibilityCalculationModel calculation)
        {
            var apiUrl = BaseRoute + "AddEligibilityCalculations";
            return _communicationManager.Post<EligibilityCalculationModel, Response<EligibilityCalculationModel>>(calculation, apiUrl);
        }

        public Response<EligibilityCalculationModel> UpdateEligibilityCalculations(EligibilityCalculationModel calculation)
        {
            var apiUrl = BaseRoute + "UpdateEligibilityCalculations";
            return _communicationManager.Put<EligibilityCalculationModel, Response<EligibilityCalculationModel>>(calculation, apiUrl);
        }

        #endregion
    }
}

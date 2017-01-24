using System.Collections.Specialized;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.ECI.EligibilityDetermination;
using Axis.Plugins.ECI.Models.EligibilityDetermination;
using Axis.Plugins.ECI.Translator.EligibilityDetermination;
using Axis.Service;

namespace Axis.Plugins.ECI.Repository.EligibilityDetermination
{
    public class EligibilityCalculationRepository : IEligibilityCalculationRepository
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "eligibilitycalculation/";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EligibilityCalculationRepository"/> class.
        /// </summary>
        public EligibilityCalculationRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        #endregion

        #region Public Methods

        public Response<EligibilityCalculationViewModel> GetEligibilityCalculations(long eligibilityID)
        {
            string apiUrl = BaseRoute + "GetEligibilityCalculations";
            var param = new NameValueCollection { { "eligibilityID", eligibilityID.ToString() } };

            var response = _communicationManager.Get<Response<EligibilityCalculationModel>>(param, apiUrl);
            return response.ToModel();
        }

        public Response<EligibilityCalculationViewModel> AddEligibilityCalculations(EligibilityCalculationViewModel calculation)
        {
            string apiUrl = BaseRoute + "AddEligibilityCalculations";

            var response = _communicationManager.Post<EligibilityCalculationModel, Response<EligibilityCalculationModel>>(calculation.ToModel(), apiUrl);
            return response.ToModel();
        }

        public Response<EligibilityCalculationViewModel> UpdateEligibilityCalculations(EligibilityCalculationViewModel calculation)
        {
            string apiUrl = BaseRoute + "UpdateEligibilityCalculations";

            var response = _communicationManager.Put<EligibilityCalculationModel, Response<EligibilityCalculationModel>>(calculation.ToModel(), apiUrl);
            return response.ToModel();
        }

        #endregion
    }
}

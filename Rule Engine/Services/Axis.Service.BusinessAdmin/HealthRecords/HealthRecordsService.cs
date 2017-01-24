using Axis.Configuration;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Security;

namespace Axis.Service.BusinessAdmin.HealthRecords
{
    public class HealthRecordsService : IHealthRecordsService
    {
        #region Class Variables        
        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "HealthRecords/";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthRecordsService"/> class.
        /// </summary>
        public HealthRecordsService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the health records.
        /// </summary>
        /// <returns></returns>
        public Response<HealthRecordsModel> GetHealthRecords()
        {
            string apiUrl = BaseRoute + "GetHealthRecords";
            return _communicationManager.Get<Response<HealthRecordsModel>>(apiUrl);
        }

        #endregion
    }
}

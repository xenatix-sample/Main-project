using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.BusinessAdmin.Models;
using Axis.Service;
using Axis.Configuration;
using Axis.Helpers;
using Axis.PresentationEngine.Areas.BusinessAdmin.Translator;
using Axis.Model.BusinessAdmin;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Respository.HealthRecords
{
    public class HealthRecordsRepository : IHealthRecordsRepository
    {
        #region Class Variables        
        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        private const string BaseRoute = "healthrecords/";

        #endregion Class Variables

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthRecordsRepository"/> class.
        /// </summary>
        public HealthRecordsRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthRecordsRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public HealthRecordsRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }
        #endregion

        #region Public Methods        
        /// <summary>
        /// Gets the health records.
        /// </summary>
        /// <returns></returns>
        public Response<HealthRecordsModel> GetHealthRecords()
        {
            var apiUrl = BaseRoute + "GetHealthRecords";

            return _communicationManager.Get<Response<HealthRecordsModel>>(apiUrl);
        }
        #endregion
    }
}
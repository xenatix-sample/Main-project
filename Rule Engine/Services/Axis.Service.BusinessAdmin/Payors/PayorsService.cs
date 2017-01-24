using System;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Configuration;
using Axis.Security;
using Axis.Service.BusinessAdmin.Payors;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.BusinessAdmin.Payors
{
    public class PayorsService : IPayorsService
    {
        #region Class Variables        
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "Payors/";

        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="PayorsService"/> class.
        /// </summary>
        public PayorsService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods       
        /// <summary>
        /// Gets payors.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        public Response<PayorsModel> GetPayors(string searchText)
        {
            if (searchText == null)
            {
                searchText = "";
            }
            const string apiUrl = BaseRoute + "GetPayors";
            var requestXMLValueNvc = new NameValueCollection { { "searchText", searchText } };
            return communicationManager.Get<Response<PayorsModel>>(requestXMLValueNvc, apiUrl);

        }



        /// <summary>
        /// Adds the payor.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        public Response<PayorsModel> AddPayor(PayorsModel payorDetails)
        {
            const string apiUrl = BaseRoute + "AddPayor";
            return communicationManager.Post<PayorsModel, Response<PayorsModel>>(payorDetails, apiUrl);
        }

        /// <summary>
        /// Updates the payor.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        public Response<PayorsModel> UpdatePayor(PayorsModel payorDetails)
        {
            const string apiUrl = BaseRoute + "UpdatePayor";
            return communicationManager.Put<PayorsModel, Response<PayorsModel>>(payorDetails, apiUrl);
        }


        /// <summary>
        /// Gets the payor by identifier.
        /// </summary>
        /// <param name="payorID">The payor identifier.</param>
        /// <returns></returns>
        public Response<PayorsModel> GetPayorByID(int payorID)
        {
            const string apiUrl = BaseRoute + "GetPayorByID";
            var requestXMLValueNvc = new NameValueCollection { { "payorID", payorID.ToString() } };
            return communicationManager.Get<Response<PayorsModel>>(requestXMLValueNvc, apiUrl);
        }

        #endregion
    }
}

using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Configuration;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.BusinessAdmin.PlanAddresses
{
    public class PlanAddressesService : IPlanAddressesService
    {
        #region Class Variables        
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;
        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "PlanAddresses/";

        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="PlanAddressesService"/> class.
        /// </summary>
        public PlanAddressesService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the plan addresses.
        /// </summary>
        /// <param name="payorPlanID">The payor plan identifier.</param>
        /// <returns></returns>
        public Response<PlanAddressesModel> GetPlanAddresses(int payorPlanID)
        {
            const string apiUrl = BaseRoute + "GetPlanAddresses";
            var requestXMLValueNvc = new NameValueCollection { { "payorPlanID", payorPlanID.ToString() } };
            return communicationManager.Get<Response<PlanAddressesModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Gets the plan address.
        /// </summary>
        /// <param name="payorAddressID">The payor address identifier.</param>
        /// <returns></returns>
        public Response<PlanAddressesModel> GetPlanAddress(int payorAddressID)
        {
            const string apiUrl = BaseRoute + "GetPlanAddress";
            var requestXMLValueNvc = new NameValueCollection { { "payorAddressID", payorAddressID.ToString() } };
            return communicationManager.Get<Response<PlanAddressesModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Adds the plan address.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        public Response<PlanAddressesModel> AddPlanAddress(PlanAddressesModel payorDetails)
        {
            const string apiUrl = BaseRoute + "AddPlanAddress";
            return communicationManager.Post<PlanAddressesModel, Response<PlanAddressesModel>>(payorDetails, apiUrl);
        }

        /// <summary>
        /// Updates the plan address.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        public Response<PlanAddressesModel> UpdatePlanAddress(PlanAddressesModel payorDetails)
        {
            const string apiUrl = BaseRoute + "UpdatePlanAddress";
            return communicationManager.Put<PlanAddressesModel, Response<PlanAddressesModel>>(payorDetails, apiUrl);
        }



        #endregion
    }
}

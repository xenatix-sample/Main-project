using System;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlanGroup;
using Axis.Model.Registration;

using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;

using Axis.Model.Common.Lookups.PayorPlan;
using Axis.PresentationEngine.Areas.BusinessAdmin.Models;
using Axis.Model.BusinessAdmin;
using Axis.PresentationEngine.Areas.BusinessAdmin.Translator;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.HealthRecords;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Respository.PlanAddresses
{
    /// <summary>
    /// Plan Addresses Repository
    /// </summary>
    /// <seealso cref="Axis.PresentationEngine.Areas.BusinessAdmin.Respository.PlanAddresses.IPlanAddressesRepository" />
    public class PlanAddressesRepository : IPlanAddressesRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "PlanAddresses/";

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanAddressesRepository"/> class.
        /// </summary>
        public PlanAddressesRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanAddressesRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public PlanAddressesRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }


        /// <summary>
        /// Gets the plan addresses.
        /// </summary>
        /// <param name="payorPlanID">The payor plan identifier.</param>
        /// <returns></returns>
        public Response<PlanAddressesModel> GetPlanAddresses(int payorPlanID)
        {
            string apiUrl = baseRoute + "GetPlanAddresses";
            var parameters = new NameValueCollection  {
                                                          { "payorPlanID", payorPlanID.ToString() }
                                                      };
            return communicationManager.Get<Response<PlanAddressesModel>>(parameters, apiUrl);

        }

        /// <summary>
        /// Gets the plan address.
        /// </summary>
        /// <param name="payorAddressID">The payor address identifier.</param>
        /// <returns></returns>
        public Response<PlanAddressesModel> GetPlanAddress(int payorAddressID)
        {
            string apiUrl = baseRoute + "GetPlanAddress";
            var parameters = new NameValueCollection  {
                                                          { "payorAddressID", payorAddressID.ToString() }
                                                      };
            return communicationManager.Get<Response<PlanAddressesModel>>(parameters, apiUrl);

        }

        /// <summary>
        /// Adds the plan address.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        public Response<PlanAddressesModel> AddPlanAddress(PlanAddressesModel payorDetails)
        {
            string apiUrl = baseRoute + "AddPlanAddress";
            var response = communicationManager.Post<PlanAddressesModel, Response<PlanAddressesModel>>(payorDetails, apiUrl);
            return response;
        }

        /// <summary>
        /// Updates the plan address.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        public Response<PlanAddressesModel> UpdatePlanAddress(PlanAddressesModel payorDetails)
        {
            string apiUrl = baseRoute + "UpdatePlanAddress";
            var response = communicationManager.Put<PlanAddressesModel, Response<PlanAddressesModel>>(payorDetails, apiUrl);
            return response;
        }
    }
}
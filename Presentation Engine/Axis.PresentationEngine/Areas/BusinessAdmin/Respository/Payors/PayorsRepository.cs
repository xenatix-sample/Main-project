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

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Respository.Payors
{
    /// <summary>
    /// Payors Repository
    /// </summary>
    /// <seealso cref="Axis.PresentationEngine.Areas.BusinessAdmin.Respository.Payors.IPayorsRepository" />
    public class PayorsRepository : IPayorsRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "Payors/";

        /// <summary>
        /// Initializes a new instance of the <see cref="PayorsRepository"/> class.
        /// </summary>
        public PayorsRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PayorsRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public PayorsRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets Payors.
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public Response<PayorsModel> GetPayors(string searchText)
        {
            if(searchText == null)
            {
                searchText = "";
            }
            string apiUrl = baseRoute + "GetPayors";
            var parameters = new NameValueCollection { { "searchText", searchText.ToString(CultureInfo.InvariantCulture) } };
            var response = communicationManager.Get<Response<PayorsModel>>(parameters, apiUrl);
            return response;
        }


        /// <summary>
        /// Adds the payor.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        public Response<PayorsModel> AddPayor(PayorsModel payorDetails)
        {
            string apiUrl = baseRoute + "AddPayor";
            var response = communicationManager.Post<PayorsModel, Response<PayorsModel>>(payorDetails, apiUrl);
            return response;
        }





        /// <summary>
        /// Updates the payor.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        public Response<PayorsModel> UpdatePayor(PayorsModel payorDetails)
        {
            string apiUrl = baseRoute + "UpdatePayor";
            var response = communicationManager.Put<PayorsModel, Response<PayorsModel>>(payorDetails, apiUrl);
            return response;
        }




        /// <summary>
        /// Gets the payor by identifier.
        /// </summary>
        /// <param name="payorId">The payor identifier.</param>
        /// <returns></returns>
        public Response<PayorsModel> GetPayorByID(int payorId)
        {
            string apiUrl = baseRoute + "GetPayorByID";
            var parameters = new NameValueCollection  {
                                                          { "payorId", payorId.ToString() }
                                                      };
            return communicationManager.Get<Response<PayorsModel>>(parameters, apiUrl);

        }



    }
}
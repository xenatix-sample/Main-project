using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using Axis.Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Plugins.Registration.Translator;
namespace Axis.Plugins.Registration.Repository
{
    public class SelfPayRepository : ISelfPayRepository
    {
                /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "SelfPay/";

        /// <summary>
        /// Initializes a new instance of the <see cref="SelfPayRepository" /> class.
        /// </summary>
        public SelfPayRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelfPayRepository" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public SelfPayRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the self pay list.
        /// </summary>
        /// <param name="selfPayID">The self pay identifier.</param>
        /// <returns></returns>
      
        public Response<SelfPayViewModel> GetSelfPayDetails(long selfPayID)
        {
            const string apiUrl = baseRoute + "GetSelfPayDetails";
            var param = new NameValueCollection { { "selfPayID", selfPayID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<SelfPayModel>>(param, apiUrl).ToViewModel();
        }

        /// <summary>
        /// Adds the self pay
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        
        public Response<SelfPayViewModel> AddSelfPay(SelfPayViewModel selfPay)
        {
            string apiUrl = baseRoute + "AddSelfPay";
            var response = communicationManager.Post<SelfPayModel, Response<SelfPayModel>>(selfPay.ToModel(), apiUrl);
            return response.ToViewModel();
        }


        /// <summary>
        /// Adds the self pay header
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
     
        public Response<SelfPayViewModel> AddSelfPayHeader(SelfPayViewModel selfPay)
        {
            string apiUrl = baseRoute + "AddSelfPayHeader";
            var response = communicationManager.Post<SelfPayModel, Response<SelfPayModel>>(selfPay.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the self pay.
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
      
        public Response<SelfPayViewModel> UpdateSelfPay(SelfPayViewModel selfPay)
        {
            string apiUrl = baseRoute + "UpdateSelfPay";
            var response = communicationManager.Put<SelfPayModel, Response<SelfPayModel>>(selfPay.ToModel(), apiUrl);
            return response.ToViewModel();
        }


        /// <summary>
        /// delete the self pay.
        /// </summary>
        /// <param name="selfPayID">The self pay identifier.</param>
        /// <param name="modifiedOn">The modified on</param>
        /// <returns></returns>
       
        public Response<SelfPayViewModel> DeleteSelfPay(long selfPayID, DateTime modifiedOn)
        {
            string apiUrl = baseRoute;
            var param = new NameValueCollection
            {
                {"selfPayID", selfPayID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
                
            };
            return communicationManager.Delete<Response<SelfPayViewModel>>(param, apiUrl);
        }
    }
}

using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Security;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Service.Registration
{
    public class SelfPayService : ISelfPayService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "SelfPay/";

        /// <summary>
        /// Initializes a new instance of the <see cref="SelfPayService" /> class.
        /// </summary>
        public SelfPayService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }


        /// <summary>
        /// Gets the self pay list.
        /// </summary>
        /// <param name="selfPayID">The self pay identifier.</param>
        /// <returns></returns>
        public Response<SelfPayModel> GetSelfPayDetails(long selfPayID)
        {
            const string apiUrl = BaseRoute + "GetSelfPayDetails";
            var requestParams = new NameValueCollection { { "selfPayID", selfPayID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<SelfPayModel>>(requestParams, apiUrl);
        }

        /// <summary>
        /// Adds the self pay
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        public Response<SelfPayModel> AddSelfPay(SelfPayModel selfPay)
        {
            const string apiUrl = BaseRoute + "AddSelfPay";
            return communicationManager.Post<SelfPayModel, Response<SelfPayModel>>(selfPay, apiUrl);
        }

        /// <summary>
        /// Adds the self pay header
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        public Response<SelfPayModel> AddSelfPayHeader(SelfPayModel selfPay)
        {
            const string apiUrl = BaseRoute + "AddSelfPayHeader";
            return communicationManager.Post<SelfPayModel, Response<SelfPayModel>>(selfPay, apiUrl);
        }

        /// <summary>
        /// update the self pay header
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        public Response<SelfPayModel> UpdateSelfPay(SelfPayModel selfPay)
        {
            const string apiUrl = BaseRoute + "UpdateSelfPay";
            return communicationManager.Put<SelfPayModel, Response<SelfPayModel>>(selfPay, apiUrl);
        }

        /// <summary>
        /// delete the self pay.
        /// </summary>
        /// <param name="selfPayID">The self pay identifier.</param>
        /// <param name="modifiedOn">The modified on</param>
        /// <returns></returns>
        public Response<SelfPayModel> DeleteSelfPay(long selfPayID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteSelfPay";
            var requestParams = new NameValueCollection
            {
                { "selfPayID", selfPayID.ToString(CultureInfo.InvariantCulture) },
                  { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
               
            };
            return communicationManager.Delete<Response<SelfPayModel>>(requestParams, apiUrl);
        }
    }
}

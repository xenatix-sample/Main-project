using System;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Configuration;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration
{
    public class IntakeFormsService : IIntakeFormsService
    {
        private readonly CommunicationManager communicationManager;
        private const string BaseRoute = "IntakeForms/";

        /// <summary>
        /// Initializes a new instance of the <see cref="IntakeFormsService"/> class.
        /// </summary>
        public IntakeFormsService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }


        /// <summary>
        /// Gets the Forms.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
        public Response<FormsModel> GetIntakeForm(long contactFormsID)
        {
            const string apiUrl = BaseRoute + "GetIntakeForm";
            var requestParams = new NameValueCollection { { "contactFormsID", contactFormsID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<FormsModel>>(requestParams, apiUrl);
        }

        /// <summary>
        /// Gets the intake forms by contact id.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
        public Response<FormsModel> GetIntakeFormsByContactID(long contactID)
        {
            const string apiUrl = BaseRoute + "GetIntakeFormsByContactID";
            var requestParams = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<FormsModel>>(requestParams, apiUrl);
        }

        /// <summary>
        /// Adds the Forms.
        /// </summary>
        /// <param name="FormsModel">The Forms model.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
        public Response<FormsModel> AddIntakeForms(FormsModel FormsModel)
        {
            const string apiUrl = BaseRoute + "AddIntakeForms";
            return communicationManager.Post<FormsModel, Response<FormsModel>>(FormsModel, apiUrl);
        }

        /// <summary>
        /// Updates the Forms.
        /// </summary>
        /// <param name="FormsModel">The Forms model.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
        public Response<FormsModel> UpdateIntakeForms(FormsModel FormsModel)
        {
            const string apiUrl = BaseRoute + "UpdateIntakeForms";
            return communicationManager.Put<FormsModel, Response<FormsModel>>(FormsModel, apiUrl);
        }

        /// <summary>
        /// Deletes the Forms.
        /// </summary>
        /// <param name="contactFormsID">The contact Forms identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
        public Response<FormsModel> DeleteIntakeForms(long contactFormsID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteIntakeForms";
            var requestParams = new NameValueCollection
            {
                { "contactFormsID", contactFormsID.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return communicationManager.Delete<Response<FormsModel>>(requestParams, apiUrl);
        }
    }
}

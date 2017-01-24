using System;
using Axis.Model.Common;
using Axis.Service;
using Axis.Configuration;
using Axis.Helpers;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Plugins.Registration.Translator;
using Axis.Plugins.Registration.Models;
using Axis.Model.Registration;

namespace Axis.Plugins.Registration.Repository
{
    public class IntakeFormsRepository : IIntakeFormsRepository
    {
        readonly CommunicationManager communicationManager;

        const string baseRoute = "IntakeForms/";

        /// <summary>
        /// Initializes a new instance of the <see cref="FormsRepository"/> class.
        /// </summary>
        public IntakeFormsRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }



        /// <summary>
        /// Gets the Forms.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns>Response&lt;FormsViewModel&gt;.</returns>
        public Response<FormsViewModel> GetIntakeForm(long contactFormsID)
        {
            const string apiUrl = baseRoute + "GetIntakeForm";
            var param = new NameValueCollection { { "contactFormsID", contactFormsID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<FormsModel>>(param, apiUrl).ToViewModel();
        }

        /// <summary>
        /// Gets the intake forms by contact id.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns>Response&lt;FormsViewModel&gt;.</returns>
        public Response<FormsViewModel> GetIntakeFormsByContactID(long contactID)
        {
            const string apiUrl = baseRoute + "GetIntakeFormsByContactID";
            var param = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<FormsModel>>(param, apiUrl).ToViewModel();
        }

        /// <summary>
        /// Adds the Forms.
        /// </summary>
        /// <param name="FormsModel">The Forms model.</param>
        /// <returns>Response&lt;FormsViewModel&gt;.</returns>
        public Response<FormsViewModel> AddIntakeForms(FormsViewModel FormsModel)
        {
            string apiUrl = baseRoute + "AddIntakeForms";
            var response = communicationManager.Post<FormsModel, Response<FormsModel>>(FormsModel.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the Forms.
        /// </summary>
        /// <param name="FormsModel">The Forms model.</param>
        /// <returns>Response&lt;FormsViewModel&gt;.</returns>
        public Response<FormsViewModel> UpdateIntakeForms(FormsViewModel FormsModel)
        {
            string apiUrl = baseRoute + "UpdateIntakeForms";
            var response = communicationManager.Put<FormsModel, Response<FormsModel>>(FormsModel.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the Forms.
        /// </summary>
        /// <param name="contactFormsID">The contact Forms identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns>Response&lt;FormsViewModel&gt;.</returns>
        public Response<FormsViewModel> DeleteIntakeForms(long contactFormsID, DateTime modifiedOn)
        {
            string apiUrl = baseRoute + "DeleteIntakeForms";
            var param = new NameValueCollection
            {
                {"ContactFormsID", contactFormsID.ToString()},
                {"ModifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return communicationManager.Delete<Response<FormsViewModel>>(param, apiUrl);
        }
    }
}

using System;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Clinical.Allergy;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.Allergy;
using Axis.Plugins.Clinical.Translator;
using Axis.Service;

namespace Axis.Plugins.Clinical.Repository.Allergy
{
    public class AllergyRepository : IAllergyRepository
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "Allergy/";

        #endregion

        #region Constructors

        public AllergyRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }
        #endregion

        #region Public Methods

        public Response<ContactAllergyViewModel> GetAllergyBundle(long contactID, Int16 allergyTypeID)
        {
            string apiUrl = BaseRoute + "GetAllergyBundle";
            var param = new NameValueCollection();
            param.Add("contactId", contactID.ToString());
            param.Add("allergyTypeID", allergyTypeID.ToString());

            var response = _communicationManager.Get<Response<ContactAllergyModel>>(param, apiUrl);
            return response.ToModel();
        }

        public Response<ContactAllergyViewModel> GetAllergy(long contactAllergyID)
        {
            string apiUrl = BaseRoute + "GetAllergy";
            var param = new NameValueCollection();
            param.Add("contactAllergyID", contactAllergyID.ToString());

            var response = _communicationManager.Get<Response<ContactAllergyModel>>(param, apiUrl);
            return response.ToModel();
        }

        public Response<ContactAllergyDetailsViewModel> GetAllergyDetails(long contactAllergyID, Int16 allergyTypeID)
        {
            string apiUrl = BaseRoute + "GetAllergyDetails";
            var param = new NameValueCollection();
            param.Add("contactAllergyID", contactAllergyID.ToString());
            param.Add("allergyTypeID", allergyTypeID.ToString());

            var response = _communicationManager.Get<Response<ContactAllergyDetailsModel>>(param, apiUrl);
            return response.ToModel();
        }

        public Response<ContactAllergyHeaderViewModel> GetTopAllergies(long contactID)
        {
            string apiUrl = BaseRoute + "GetTopAllergies";
            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());
            
            var response = _communicationManager.Get<Response<ContactAllergyHeaderModel>>(param, apiUrl);
            return response.ToModel();
        }

        public Response<ContactAllergyViewModel> DeleteAllergy(long contactAllergyID, DateTime modifiedOn)
        {
            string apiUrl = BaseRoute + "DeleteAllergy";
            var param = new NameValueCollection
            {
                {"contactAllergyID", contactAllergyID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };

            var response = _communicationManager.Delete<Response<ContactAllergyModel>>(param, apiUrl);
            return response.ToModel();
        }

        public Response<ContactAllergyDetailsViewModel> DeleteAllergyDetail(long contactAllergyDetailID, string reasonForDeletion, DateTime modifiedOn)
        {
            string apiUrl = BaseRoute + "DeleteAllergyDetail";
            var param = new NameValueCollection
            {
                {"contactAllergyDetailID", contactAllergyDetailID.ToString()},
                {"reasonForDeletion", reasonForDeletion},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };

            var response = _communicationManager.Delete<Response<ContactAllergyDetailsModel>>(param, apiUrl);
            return response.ToModel();
        }

        public Response<ContactAllergyViewModel> AddAllergy(ContactAllergyViewModel allergy)
        {
            string apiUrl = BaseRoute + "AddAllergy";

            var response = _communicationManager.Post<ContactAllergyModel, Response<ContactAllergyModel>>(allergy.ToModel(), apiUrl);
            return response.ToModel();
        }

        public Response<ContactAllergyDetailsViewModel> AddAllergyDetail(ContactAllergyDetailsViewModel allergy)
        {
            string apiUrl = BaseRoute + "AddAllergyDetail";

            var response = _communicationManager.Post<ContactAllergyDetailsModel, Response<ContactAllergyDetailsModel>>(allergy.ToModel(), apiUrl);
            return response.ToModel();
        }

        public Response<ContactAllergyViewModel> UpdateAllergy(ContactAllergyViewModel allergy)
        {
            string apiUrl = BaseRoute + "UpdateAllergy";

            var response = _communicationManager.Put<ContactAllergyModel, Response<ContactAllergyModel>>(allergy.ToModel(), apiUrl);
            return response.ToModel();
        }

        public Response<ContactAllergyDetailsViewModel> UpdateAllergyDetail(ContactAllergyDetailsViewModel allergy)
        {
            string apiUrl = BaseRoute + "UpdateAllergyDetail";

            var response = _communicationManager.Put<ContactAllergyDetailsModel, Response<ContactAllergyDetailsModel>>(allergy.ToModel(), apiUrl);
            return response.ToModel();
        }

        #endregion
    }
}

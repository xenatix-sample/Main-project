using System;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Configuration;
using Axis.Model.Clinical.Allergy;
using Axis.Model.Common;
using Axis.Security;

namespace Axis.Service.Clinical.Allergy
{
    public class AllergyService : IAllergyService
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "allergy/";

        #endregion

        #region Constructors

        public AllergyService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        public Response<ContactAllergyModel> GetAllergyBundle(long contactID, Int16 allergyTypeID)
        {
            var apiUrl = BaseRoute + "GetAllergyBundle";
            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());
            param.Add("allergyTypeID", allergyTypeID.ToString());

            return _communicationManager.Get<Response<ContactAllergyModel>>(param, apiUrl);
        }

        public Response<ContactAllergyModel> GetAllergy(long contactAllergyID)
        {
            var apiUrl = BaseRoute + "GetAllergy";
            var param = new NameValueCollection();
            param.Add("contactAllergyID", contactAllergyID.ToString());

            return _communicationManager.Get<Response<ContactAllergyModel>>(param, apiUrl);
        }

        public Response<ContactAllergyDetailsModel> GetAllergyDetails(long contactAllergyID, Int16 allergyTypeID)
        {
            var apiUrl = BaseRoute + "GetAllergyDetails";
            var param = new NameValueCollection();
            param.Add("contactAllergyID", contactAllergyID.ToString());
            param.Add("allergyTypeID", allergyTypeID.ToString());

            return _communicationManager.Get<Response<ContactAllergyDetailsModel>>(param, apiUrl);
        }

        public Response<ContactAllergyHeaderModel> GetTopAllergies(long contactID)
        {
            var apiUrl = BaseRoute + "GetTopAllergies";
            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());

            return _communicationManager.Get<Response<ContactAllergyHeaderModel>>(param, apiUrl);
        }

        public Response<ContactAllergyModel> DeleteAllergy(long contactAllergyID, DateTime modifiedOn)
        {
            var apiUrl = BaseRoute + "DeleteAllergy";
            var param = new NameValueCollection
            {
                {"contactAllergyID", contactAllergyID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };

            return _communicationManager.Delete<Response<ContactAllergyModel>>(param, apiUrl);
        }

        public Response<ContactAllergyDetailsModel> DeleteAllergyDetail(long contactAllergyDetailID, string reasonForDeletion, DateTime modifiedOn)
        {
            var apiUrl = BaseRoute + "DeleteAllergyDetail";
            var param = new NameValueCollection
            {
                {"contactAllergyDetailID", contactAllergyDetailID.ToString()},
                {"reasonForDeletion", reasonForDeletion},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };

            return _communicationManager.Delete<Response<ContactAllergyDetailsModel>>(param, apiUrl);
        }

        public Response<ContactAllergyModel> AddAllergy(ContactAllergyModel allergy)
        {
            var apiUrl = BaseRoute + "AddAllergy";
            return _communicationManager.Post<ContactAllergyModel, Response<ContactAllergyModel>>(allergy, apiUrl);
        }

        public Response<ContactAllergyDetailsModel> AddAllergyDetail(ContactAllergyDetailsModel allergy)
        {
            var apiUrl = BaseRoute + "AddAllergyDetail";
            return _communicationManager.Post<ContactAllergyDetailsModel, Response<ContactAllergyDetailsModel>>(allergy, apiUrl);
        }

        public Response<ContactAllergyModel> UpdateAllergy(ContactAllergyModel allergy)
        {
            var apiUrl = BaseRoute + "UpdateAllergy";
            return _communicationManager.Put<ContactAllergyModel, Response<ContactAllergyModel>>(allergy, apiUrl);
        }

        public Response<ContactAllergyDetailsModel> UpdateAllergyDetail(ContactAllergyDetailsModel allergy)
        {
            var apiUrl = BaseRoute + "UpdateAllergyDetail";
            return _communicationManager.Put<ContactAllergyDetailsModel, Response<ContactAllergyDetailsModel>>(allergy, apiUrl);
        }

        #endregion
    }
}

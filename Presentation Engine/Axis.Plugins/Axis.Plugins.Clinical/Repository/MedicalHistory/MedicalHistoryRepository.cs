using System;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Clinical.MedicalHistory;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.MedicalHistory;
using Axis.Plugins.Clinical.Translator;
using Axis.Service;


namespace Axis.Plugins.Clinical.Repository.MedicalHistory
{
    public class MedicalHistoryRepository : IMedicalHistoryRepository
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "MedicalHistory/";

        #endregion

        #region Constructors

        public MedicalHistoryRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }
        #endregion

        #region Public Methods

        public Response<MedicalHistoryViewModel> GetMedicalHistoryBundle(long contactID)
        {
            string apiUrl = BaseRoute + "GetMedicalHistoryBundle";
            var param = new NameValueCollection();
            param.Add("contactId", contactID.ToString());

            var response = _communicationManager.Get<Response<MedicalHistoryModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        public Response<MedicalHistoryViewModel> GetMedicalHistoryConditionDetails(long medicalHistoryID)
        {
            string apiUrl = BaseRoute + "GetMedicalHistoryConditionDetails";
            var param = new NameValueCollection();
            param.Add("medicalHistoryID", medicalHistoryID.ToString());

            var response = _communicationManager.Get<Response<MedicalHistoryModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        public Response<MedicalHistoryViewModel> GetAllMedicalConditions(long medicalHistoryID)
        {
            string apiUrl = BaseRoute + "GetAllMedicalConditions";
            var param = new NameValueCollection();
            param.Add("medicalHistoryID", medicalHistoryID.ToString());

            var response = _communicationManager.Get<Response<MedicalHistoryModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        public Response<MedicalHistoryViewModel> DeleteMedicalHistory(long medicalHistoryID, DateTime modifiedOn)
        {
            string apiUrl = BaseRoute + "DeleteMedicalHistory";
            var param = new NameValueCollection
            {
                {"medicalHistoryID", medicalHistoryID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };

            var response = _communicationManager.Delete<Response<MedicalHistoryModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        public Response<MedicalHistoryViewModel> AddMedicalHistory(MedicalHistoryViewModel medicalHistory)
        {
            string apiUrl = BaseRoute + "AddMedicalHistory";

            var response = _communicationManager.Post<MedicalHistoryModel, Response<MedicalHistoryModel>>(medicalHistory.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        public Response<MedicalHistoryViewModel> UpdateMedicalHistory(MedicalHistoryViewModel medicalHistory)
        {
            string apiUrl = BaseRoute + "UpdateMedicalHistory";

            var response = _communicationManager.Post<MedicalHistoryModel, Response<MedicalHistoryModel>>(medicalHistory.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        public Response<MedicalHistoryViewModel> SaveMedicalHistoryDetails(MedicalHistoryViewModel medicalHistory)
        {
            string apiUrl = BaseRoute + "SaveMedicalHistoryDetails";

            var response = _communicationManager.Post<MedicalHistoryModel, Response<MedicalHistoryModel>>(medicalHistory.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        public Response<MedicalHistoryViewModel> GetMedicalHistory(long medicalHistoryID)
        {
            string apiUrl = BaseRoute + "GetMedicalHistory";
            var param = new NameValueCollection();
            param.Add("medicalHistoryID", medicalHistoryID.ToString());
            var response = _communicationManager.Get<Response<MedicalHistoryModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        #endregion       
    }
}

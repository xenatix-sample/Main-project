using System;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Configuration;
using Axis.Model.Clinical.MedicalHistory;
using Axis.Model.Common;
using Axis.Security;

namespace Axis.Service.Clinical.MedicalHistory
{
    public class MedicalHistoryService : IMedicalHistoryService
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "medicalhistory/";

        #endregion

        #region Constructors

        public MedicalHistoryService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        public Response<MedicalHistoryModel> GetMedicalHistoryBundle(long contactID)
        {
            var apiUrl = BaseRoute + "GetMedicalHistoryBundle";
            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());

            return _communicationManager.Get<Response<MedicalHistoryModel>>(param, apiUrl);
        }

        public Response<MedicalHistoryModel> GetMedicalHistoryConditionDetails(long medicalHistoryID)
        {
            var apiUrl = BaseRoute + "GetMedicalHistoryConditionDetails";
            var param = new NameValueCollection();
            param.Add("medicalHistoryID", medicalHistoryID.ToString());

            return _communicationManager.Get<Response<MedicalHistoryModel>>(param, apiUrl);
        }

        public Response<MedicalHistoryModel> GetAllMedicalConditions(long medicalHistoryID)
        {
            var apiUrl = BaseRoute + "GetAllMedicalConditions";
            var param = new NameValueCollection();
            param.Add("medicalHistoryID", medicalHistoryID.ToString());

            return _communicationManager.Get<Response<MedicalHistoryModel>>(param, apiUrl);
        }

        public Response<MedicalHistoryModel> DeleteMedicalHistory(long medicalHistoryID, DateTime modifiedOn)
        {
            var apiUrl = BaseRoute + "DeleteMedicalHistory";
            var param = new NameValueCollection { { "medicalHistoryID", medicalHistoryID.ToString() }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };

            return _communicationManager.Delete<Response<MedicalHistoryModel>>(param, apiUrl);
        }

        public Response<MedicalHistoryModel> AddMedicalHistory(MedicalHistoryModel medicalHistory)
        {
            var apiUrl = BaseRoute + "AddMedicalHistory";
            return _communicationManager.Post<MedicalHistoryModel, Response<MedicalHistoryModel>>(medicalHistory, apiUrl);
        }

        public Response<MedicalHistoryModel> UpdateMedicalHistory(MedicalHistoryModel medicalHistory)
        {
            var apiUrl = BaseRoute + "UpdateMedicalHistory";
            return _communicationManager.Post<MedicalHistoryModel, Response<MedicalHistoryModel>>(medicalHistory, apiUrl);
        }

        public Response<MedicalHistoryModel> SaveMedicalHistoryDetails(MedicalHistoryModel medicalHistory)
        {
            var apiUrl = BaseRoute + "SaveMedicalHistoryDetails";
            return _communicationManager.Post<MedicalHistoryModel, Response<MedicalHistoryModel>>(medicalHistory, apiUrl);
        }

        public Response<MedicalHistoryModel> GetMedicalHistory(long medicalHistoryID)
        {
            var apiUrl = BaseRoute + "GetMedicalHistory";
            var param = new NameValueCollection();
            param.Add("medicalHistoryID", medicalHistoryID.ToString());
            return _communicationManager.Get<Response<MedicalHistoryModel>>(param, apiUrl);
        }

        #endregion

    }
}

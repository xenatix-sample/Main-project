using System;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Configuration;
using Axis.Model.Clinical.PresentIllness;
using Axis.Model.Common;
using Axis.Security;

namespace Axis.Service.Clinical.PresentIllness
{
    public class PresentIllnessService : IPresentIllnessService
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "presentIllness/";

        #endregion

        #region Constructors

        public PresentIllnessService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        public Response<PresentIllnessModel> GetHPIBundle(long contactID)
        {
            var apiUrl = BaseRoute + "GetHPIBundle";
            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());

            return _communicationManager.Get<Response<PresentIllnessModel>>(param, apiUrl);
        }

        public Response<PresentIllnessModel> GetHPI(long HPIID)
        {
            var apiUrl = BaseRoute + "GetHPI";
            var param = new NameValueCollection();
            param.Add("HPIID", HPIID.ToString());

            return _communicationManager.Get<Response<PresentIllnessModel>>(param, apiUrl);
        }

        public Response<PresentIllnessDetailModel> GetHPIDetail(long HPIID)
        {
            var apiUrl = BaseRoute + "GetHPIDetail";
            var param = new NameValueCollection();
            param.Add("HPIID", HPIID.ToString());

            return _communicationManager.Get<Response<PresentIllnessDetailModel>>(param, apiUrl);
        }

        public Response<PresentIllnessModel> DeleteHPI(long HPIID, DateTime modifiedOn)
        {
            var apiUrl = BaseRoute + "DeleteHPI";
            var param = new NameValueCollection();
            param.Add("HPIID", HPIID.ToString());

            return _communicationManager.Delete<Response<PresentIllnessModel>>(param, apiUrl);
        }

        public Response<PresentIllnessDetailModel> DeleteHPIDetail(long HPIDetailID, DateTime modifiedOn)
        {
            var apiUrl = BaseRoute + "DeleteHPIDetail";
            var param = new NameValueCollection
            {
                {"HPIDetailID", HPIDetailID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };

            return _communicationManager.Delete<Response<PresentIllnessDetailModel>>(param, apiUrl);
        }

        public Response<PresentIllnessModel> AddHPI(PresentIllnessModel HPI)
        {
            var apiUrl = BaseRoute + "AddHPI";
            return _communicationManager.Post<PresentIllnessModel, Response<PresentIllnessModel>>(HPI, apiUrl);
        }

        public Response<PresentIllnessDetailModel> AddHPIDetail(PresentIllnessDetailModel HPI)
        {
            var apiUrl = BaseRoute + "AddHPIDetail";
            return _communicationManager.Post<PresentIllnessDetailModel, Response<PresentIllnessDetailModel>>(HPI, apiUrl);
        }

        public Response<PresentIllnessModel> UpdateHPI(PresentIllnessModel HPI)
        {
            var apiUrl = BaseRoute + "UpdateHPI";
            return _communicationManager.Put<PresentIllnessModel, Response<PresentIllnessModel>>(HPI, apiUrl);
        }

        public Response<PresentIllnessDetailModel> UpdateHPIDetail(PresentIllnessDetailModel HPI)
        {
            var apiUrl = BaseRoute + "UpdateHPIDetail";
            return _communicationManager.Put<PresentIllnessDetailModel, Response<PresentIllnessDetailModel>>(HPI, apiUrl);
        }

        #endregion

    }

}

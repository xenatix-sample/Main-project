using System;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Clinical.PresentIllness;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.PresentIllness;
using Axis.Plugins.Clinical.Translator;
using Axis.Service;


namespace Axis.Plugins.Clinical.Repository.PresentIllness
{
    public class PresentIllnessRepository : IPresentIllnessRepository
    {
         #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "PresentIllness/";
        private string p;

        #endregion

         #region Constructors

        public PresentIllnessRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public PresentIllnessRepository(string p)
        {
            // TODO: Complete member initialization
            this.p = p;
        }
        #endregion

         #region Public Methods

        public Response<PresentIllnessViewModel> GetHPIBundle(long contactID)
        {
            string apiUrl = BaseRoute + "GetHPIBundle";
            var param = new NameValueCollection();
            param.Add("contactId", contactID.ToString());
           
            var response = _communicationManager.Get<Response<PresentIllnessModel>>(param, apiUrl);
            return response.ToModel();

         }

        public Response<PresentIllnessViewModel> GetHPI(long HPIID)
        {
            string apiUrl = BaseRoute + "GetHPI";
            var param = new NameValueCollection();
            param.Add("HPIID", HPIID.ToString());

            var response = _communicationManager.Get<Response<PresentIllnessModel>>(param, apiUrl);
            return response.ToModel();
        }

       
      
          public Response<PresentIllnessViewModel> DeleteHPI(long HPIID, DateTime modifiedOn)
        {
            string apiUrl = BaseRoute + "DeleteHPI";
            var param = new NameValueCollection
            {
                {"HPIID", HPIID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };

            var response = _communicationManager.Delete<Response<PresentIllnessModel>>(param, apiUrl);
            return response.ToModel();
        }
                
      
        public Response<PresentIllnessViewModel> AddHPI(PresentIllnessViewModel hpi)
        {
            string apiUrl = BaseRoute + "AddHPI";

            var response = _communicationManager.Post<PresentIllnessModel, Response<PresentIllnessModel>>(hpi.ToModel(), apiUrl);
            return response.ToModel();
        }
                
       
        public Response<PresentIllnessViewModel> UpdateHPI(PresentIllnessViewModel hpi)
        {
            string apiUrl = BaseRoute + "UpdateHPI";

            var response = _communicationManager.Put<PresentIllnessModel, Response<PresentIllnessModel>>(hpi.ToModel(), apiUrl);
            return response.ToModel();
        }

         public Response<PresentIllnessDetailViewModel> GetHPIDetails(long hpiID)
        {
            string apiUrl = BaseRoute + "GetHPIDetail";
            var param = new NameValueCollection();
            param.Add("HPIID", hpiID.ToString());

            var response = _communicationManager.Get<Response<PresentIllnessDetailModel>>(param, apiUrl);
            return response.ToModel();
        }

        public Response<PresentIllnessDetailViewModel> DeleteHPIDetail(long HPIDetailID, DateTime modifiedOn)
        {
            string apiUrl = BaseRoute + "DeleteHPIDetail";
            var param = new NameValueCollection
            {
                {"HPIDetailID", HPIDetailID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };

            var response = _communicationManager.Delete<Response<PresentIllnessDetailModel>>(param, apiUrl);
            return response.ToModel();
        }

        public Response<PresentIllnessDetailViewModel> AddHPIDetail(PresentIllnessDetailViewModel hpi)
        {
            string apiUrl = BaseRoute + "AddHPIDetail";

            var response = _communicationManager.Post<PresentIllnessDetailModel, Response<PresentIllnessDetailModel>>(hpi.ToModel(), apiUrl);
            return response.ToModel();
        }


        public Response<PresentIllnessDetailViewModel> UpdateHPIDetail(PresentIllnessDetailViewModel hpi)
        {
            string apiUrl = BaseRoute + "UpdateHPIDetail";

            var response = _communicationManager.Put<PresentIllnessDetailModel, Response<PresentIllnessDetailModel>>(hpi.ToModel(), apiUrl);
            return response.ToModel();
        }
        
         #endregion   
    }
}


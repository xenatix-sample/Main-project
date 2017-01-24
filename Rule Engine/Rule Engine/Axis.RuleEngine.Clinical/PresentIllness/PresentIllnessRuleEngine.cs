using System;
using Axis.Model.Clinical.PresentIllness;
using Axis.Model.Common;
using Axis.Service.Clinical.PresentIllness;

namespace Axis.RuleEngine.Clinical.PresentIllness
{
    public class PresentIllnessRuleEngine : IPresentIllnessRuleEngine
    {
        #region Class Variables

        private readonly IPresentIllnessService _PresentIllnessService;

        #endregion

        #region Constructors

        public PresentIllnessRuleEngine(IPresentIllnessService PresentIllnessService)
        {
            _PresentIllnessService = PresentIllnessService;
        }

        #endregion

        #region Public Methods

        public Response<PresentIllnessModel> GetHPIBundle(long contactID)
        {
            return _PresentIllnessService.GetHPIBundle(contactID);
        }

        public Response<PresentIllnessModel> GetHPI(long HPIID)
        {
            return _PresentIllnessService.GetHPI(HPIID);
        }

        public Response<PresentIllnessDetailModel> GetHPIDetail(long HPIID)
        {
            return _PresentIllnessService.GetHPIDetail(HPIID);
        }

        public Response<PresentIllnessModel> DeleteHPI(long HPIID, DateTime modifiedOn)
        {
            return _PresentIllnessService.DeleteHPI(HPIID, modifiedOn);
        }

        public Response<PresentIllnessDetailModel> DeleteHPIDetail(long HPIDetailID, DateTime modifiedOn)
        {
            return _PresentIllnessService.DeleteHPIDetail(HPIDetailID, modifiedOn);
        }

        public Response<PresentIllnessModel> AddHPI(PresentIllnessModel HPI)
        {
            return _PresentIllnessService.AddHPI(HPI);
        }

        public Response<PresentIllnessDetailModel> AddHPIDetail(PresentIllnessDetailModel HPI)
        {
            return _PresentIllnessService.AddHPIDetail(HPI);
        }

        public Response<PresentIllnessModel> UpdateHPI(PresentIllnessModel HPI)
        {
            return _PresentIllnessService.UpdateHPI(HPI); 
        }

        public Response<PresentIllnessDetailModel> UpdateHPIDetail(PresentIllnessDetailModel HPI)
        {
            return _PresentIllnessService.UpdateHPIDetail(HPI);
        }
        #endregion

    }    
}

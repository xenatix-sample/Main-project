using System;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.PresentIllness;
using Axis.Plugins.Clinical.Repository.PresentIllness;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Clinical.ApiControllers
{
    public class PresentIllnessController : BaseApiController
    {
        #region Class Variables

        private readonly IPresentIllnessRepository _PresentIllnessRepository;

        #endregion

        #region Constructors

        public PresentIllnessController(IPresentIllnessRepository PresentIllnessRepository)
        {
            this._PresentIllnessRepository = PresentIllnessRepository;
        }

        #endregion

        #region Json Results

        /// <summary>
        /// Load the PresentIllness details for the contact
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns></returns>

        [HttpGet]
        public Response<PresentIllnessViewModel> GetHPIBundle(long contactID)
        {
            return _PresentIllnessRepository.GetHPIBundle(contactID);
        }

        [HttpGet]
        public Response<PresentIllnessDetailViewModel> GetHPIDetails(long hpiID)
        {
            return _PresentIllnessRepository.GetHPIDetails(hpiID);
        }
        /// <summary>
        /// Load a single HPI record
        /// </summary>
        /// 
        /// <param name="HPIID"></param>
        /// <returns></returns>
        
        [HttpGet]
        public Response<PresentIllnessViewModel> GetHPI(long HPIID)
        {
            return _PresentIllnessRepository.GetHPI(HPIID);
        }

        /// <summary>
        /// Delete the single HPI record
        /// </summary>
        /// <param name="HPIID"></param>
        /// <returns></returns>
        
        [HttpDelete]
        public Response<PresentIllnessViewModel> DeleteHPI(long HPIID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _PresentIllnessRepository.DeleteHPI(HPIID, modifiedOn);
        }

        [HttpPost]
        public Response<PresentIllnessViewModel> AddHPI(PresentIllnessViewModel HPI)
        {
            HPI.TakenTime = HPI.TakenTime.ToUniversalTime();
            return _PresentIllnessRepository.AddHPI(HPI);
        }

        [HttpPut]
        public Response<PresentIllnessViewModel> UpdateHPI(PresentIllnessViewModel HPI)
        {
            HPI.TakenTime = HPI.TakenTime.ToUniversalTime();
            return _PresentIllnessRepository.UpdateHPI(HPI);
        }

        [HttpPost]
        public Response<PresentIllnessDetailViewModel> AddHPIDetail(PresentIllnessDetailViewModel hpiDetail)
        {
            return _PresentIllnessRepository.AddHPIDetail(hpiDetail);
        }

        [HttpDelete]
        public Response<PresentIllnessDetailViewModel> DeleteHPIDetail(long HPIDetailID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _PresentIllnessRepository.DeleteHPIDetail(HPIDetailID, modifiedOn);
        }

        [HttpPut]
        public Response<PresentIllnessDetailViewModel> UpdateHPIDetail(PresentIllnessDetailViewModel HPI)
        {
            return _PresentIllnessRepository.UpdateHPIDetail(HPI);
        }

        #endregion
        
    }
}

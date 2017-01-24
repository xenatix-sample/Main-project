using System;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.Allergy;
using Axis.Plugins.Clinical.Repository.Allergy;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Clinical.ApiControllers
{
    public class AllergyController : BaseApiController
    {
        #region Class Variables

        private readonly IAllergyRepository _allergyRepository;

        #endregion

        #region Constructors

        public AllergyController(IAllergyRepository allergyRepository)
        {
            this._allergyRepository = allergyRepository;
        }

        #endregion

        #region Json Results

        /// <summary>
        /// Load the allergy details for the contact
        /// </summary>
        /// <param name="contactID"></param>
        /// <param name="allergyTypeID"></param>
        /// <returns></returns>
        [HttpGet]
        public Response<ContactAllergyViewModel> GetAllergyBundle(long contactID, Int16 allergyTypeID)
        {
            return _allergyRepository.GetAllergyBundle(contactID, allergyTypeID);
        }

        /// <summary>
        /// Load a single allergy record
        /// </summary>
        /// <param name="contactAllergyID"></param>
        /// <returns></returns>
        [HttpGet]
        public Response<ContactAllergyViewModel> GetAllergy(long contactAllergyID)
        {
            return _allergyRepository.GetAllergy(contactAllergyID);
        }

        /// <summary>
        /// Load a single allergy record's details
        /// </summary>
        /// <param name="contactAllergyID"></param>
        /// <param name="allergyTypeID"></param>
        /// <returns></returns>
        [HttpGet]
        public Response<ContactAllergyDetailsViewModel> GetAllergyDetails(long contactAllergyID, Int16 allergyTypeID)
        {
            return _allergyRepository.GetAllergyDetails(contactAllergyID, allergyTypeID);
        }

        /// <summary>
        /// Load a single allergy record's details
        /// </summary>
        /// <param name="contactAllergyID"></param>
        /// <param name="allergyTypeID"></param>
        /// <returns></returns>
        /// Uncomment this when clinical is added to the product
        //[HttpGet]
        //public Response<ContactAllergyHeaderViewModel> GetTopAllergies(long contactID)
        //{
        //    return _allergyRepository.GetTopAllergies(contactID);
        //}

        /// <summary>
        /// Deactive the single allergy record
        /// </summary>
        /// <param name="contactAllergyID"></param>
        /// <returns></returns>
        [HttpDelete]
        public Response<ContactAllergyViewModel> DeleteAllergy(long contactAllergyID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _allergyRepository.DeleteAllergy(contactAllergyID, modifiedOn);
        }

        /// <summary>
        /// Delete a single allergy record's details
        /// </summary>
        /// <param name="contactAllergyDetailID"></param>
        /// <returns></returns>
        [HttpDelete]
        public Response<ContactAllergyDetailsViewModel> DeleteAllergyDetail(long contactAllergyDetailID, string reasonForDeletion, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _allergyRepository.DeleteAllergyDetail(contactAllergyDetailID, reasonForDeletion, modifiedOn);
        }

        [HttpPost]
        public Response<ContactAllergyViewModel> AddAllergy(ContactAllergyViewModel allergy)
        {
            allergy.TakenTime = allergy.TakenTime.ToUniversalTime();
            return _allergyRepository.AddAllergy(allergy);
        }

        [HttpPost]
        public Response<ContactAllergyDetailsViewModel> AddAllergyDetail(ContactAllergyDetailsViewModel allergy)
        {
            return _allergyRepository.AddAllergyDetail(allergy);
        }

        [HttpPut]
        public Response<ContactAllergyViewModel> UpdateAllergy(ContactAllergyViewModel allergy)
        {
            allergy.TakenTime = allergy.TakenTime.ToUniversalTime();
            return _allergyRepository.UpdateAllergy(allergy);
        }
        
        [HttpPut]
        public Response<ContactAllergyDetailsViewModel> UpdateAllergyDetail(ContactAllergyDetailsViewModel allergy)
        {
            return _allergyRepository.UpdateAllergyDetail(allergy);
        }

        #endregion
    }
}

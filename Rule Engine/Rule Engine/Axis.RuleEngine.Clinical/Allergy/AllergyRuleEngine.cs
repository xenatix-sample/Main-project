using System;
using Axis.Model.Clinical.Allergy;
using Axis.Model.Common;
using Axis.Service.Clinical.Allergy;

namespace Axis.RuleEngine.Clinical.Allergy
{
    public class AllergyRuleEngine : IAllergyRuleEngine
    {
        #region Class Variables

        private readonly IAllergyService _allergyService;

        #endregion

        #region Constructors

        public AllergyRuleEngine(IAllergyService allergyService)
        {
            _allergyService = allergyService;
        }

        #endregion

        #region Public Methods

        public Response<ContactAllergyModel> GetAllergyBundle(long contactID, Int16 allergyTypeID)
        {
            return _allergyService.GetAllergyBundle(contactID, allergyTypeID);
        }

        public Response<ContactAllergyModel> GetAllergy(long contactAllergyID)
        {
            return _allergyService.GetAllergy(contactAllergyID);
        }

        public Response<ContactAllergyDetailsModel> GetAllergyDetails(long contactAllergyID, Int16 allergyTypeID)
        {
            return _allergyService.GetAllergyDetails(contactAllergyID, allergyTypeID);
        }

        public Response<ContactAllergyHeaderModel> GetTopAllergies(long contactID)
        {
            return _allergyService.GetTopAllergies(contactID);
        }

        public Response<ContactAllergyModel> DeleteAllergy(long contactAllergyID, DateTime modifiedOn)
        {
            return _allergyService.DeleteAllergy(contactAllergyID, modifiedOn);
        }

        public Response<ContactAllergyDetailsModel> DeleteAllergyDetail(long contactAllergyDetailID, string reasonForDeletion, DateTime modifiedOn)
        {
            return _allergyService.DeleteAllergyDetail(contactAllergyDetailID, reasonForDeletion, modifiedOn);
        }

        public Response<ContactAllergyModel> AddAllergy(ContactAllergyModel allergy)
        {
            return _allergyService.AddAllergy(allergy);
        }

        public Response<ContactAllergyDetailsModel> AddAllergyDetail(ContactAllergyDetailsModel allergy)
        {
            return _allergyService.AddAllergyDetail(allergy);
        }

        public Response<ContactAllergyModel> UpdateAllergy(ContactAllergyModel allergy)
        {
            return _allergyService.UpdateAllergy(allergy);
        }

        public Response<ContactAllergyDetailsModel> UpdateAllergyDetail(ContactAllergyDetailsModel allergy)
        {
            return _allergyService.UpdateAllergyDetail(allergy);
        }

        #endregion
    }
}

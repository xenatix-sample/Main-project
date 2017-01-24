using System;
using Axis.Model.Clinical.Allergy;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.Allergy;

namespace Axis.Plugins.Clinical.Repository.Allergy
{
    public interface IAllergyRepository
    {
        Response<ContactAllergyViewModel> GetAllergyBundle(long contactID, Int16 allergyTypeID);
        Response<ContactAllergyViewModel> GetAllergy(long contactAllergyID);
        Response<ContactAllergyDetailsViewModel> GetAllergyDetails(long contactAllergyID, Int16 allergyTypeID);
        Response<ContactAllergyHeaderViewModel> GetTopAllergies(long contactID);
        Response<ContactAllergyViewModel> DeleteAllergy(long contactAllergyID, DateTime modifiedOn);
        Response<ContactAllergyDetailsViewModel> DeleteAllergyDetail(long contactAllergyDetailID, string reasonForDeletion, DateTime modifiedOn);
        Response<ContactAllergyViewModel> AddAllergy(ContactAllergyViewModel allergy);
        Response<ContactAllergyDetailsViewModel> AddAllergyDetail(ContactAllergyDetailsViewModel allergy);
        Response<ContactAllergyViewModel> UpdateAllergy(ContactAllergyViewModel allergy);
        Response<ContactAllergyDetailsViewModel> UpdateAllergyDetail(ContactAllergyDetailsViewModel allergy);
    }
}

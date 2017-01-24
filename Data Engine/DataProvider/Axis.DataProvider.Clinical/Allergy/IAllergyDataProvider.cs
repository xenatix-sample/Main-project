using System;
using Axis.Model.Clinical.Allergy;
using Axis.Model.Common;

namespace Axis.DataProvider.Clinical.Allergy
{
    public interface IAllergyDataProvider
    {
        Response<ContactAllergyModel> GetAllergyBundle(long contactID, Int16 allergyTypeID);
        Response<ContactAllergyModel> GetAllergy(long contactAllergyID);
        Response<ContactAllergyDetailsModel> GetAllergyDetails(long contactAllergyID, Int16 allergyTypeID);
        Response<ContactAllergyHeaderModel> GetTopAllergies(long contactID);
        Response<ContactAllergyModel> DeleteAllergy(long contactAllergyID, DateTime modifiedOn);
        Response<ContactAllergyDetailsModel> DeleteAllergyDetail(long contactAllergyDetailID, string reasonForDeletion, DateTime modifiedOn);
        Response<ContactAllergyModel> AddAllergy(ContactAllergyModel allergy);
        Response<ContactAllergyDetailsModel> AddAllergyDetail(ContactAllergyDetailsModel allergy);
        Response<ContactAllergyModel> UpdateAllergy(ContactAllergyModel allergy);
        Response<ContactAllergyDetailsModel> UpdateAllergyDetail(ContactAllergyDetailsModel allergy);
    }
}

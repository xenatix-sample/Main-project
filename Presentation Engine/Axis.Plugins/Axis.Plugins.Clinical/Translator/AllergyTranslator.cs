using System.Collections.Generic;
using Axis.Model.Clinical.Allergy;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.Allergy;

namespace Axis.Plugins.Clinical.Translator
{
    public static class AllergyTranslator
    {
        public static ContactAllergyViewModel ToViewModel(this ContactAllergyModel entity)
        {
            if (entity == null)
                return null;

            var model = new ContactAllergyViewModel
            {
                EncounterID = entity.EncounterID,
                ContactID = entity.ContactID,
                ContactAllergyID = entity.ContactAllergyID,
                AllergyTypeID = entity.AllergyTypeID,
                NoKnownAllergy = entity.NoKnownAllergy,
                TakenBy = entity.TakenBy,
                TakenTime = entity.TakenTime,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static ContactAllergyDetailsViewModel ToViewModel(this ContactAllergyDetailsModel entity)
        {
            if (entity == null)
                return null;

            var model = new ContactAllergyDetailsViewModel
            {
                ContactAllergyDetailID = entity.ContactAllergyDetailID,
                ContactAllergyID = entity.ContactAllergyID,
                AllergyID = entity.AllergyID,                
                SeverityID = entity.SeverityID,
                Symptoms = entity.Symptoms,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static ContactAllergyHeaderViewModel ToViewModel(this ContactAllergyHeaderModel entity)
        {
            if (entity == null)
                return null;

            var model = new ContactAllergyHeaderViewModel
            {
                ContactAllergyDetailID = entity.ContactAllergyDetailID,
                ContactAllergyID = entity.ContactAllergyID,
                AllergyID = entity.AllergyID,
                AllergyName = entity.AllergyName,
                AllergySeverityID = entity.AllergySeverityID,
                AllergySeverity = entity.AllergySeverity,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static ContactAllergyModel ToModel(this ContactAllergyViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new ContactAllergyModel
            {
                EncounterID = entity.EncounterID,
                ContactID = entity.ContactID,
                ContactAllergyID = entity.ContactAllergyID,
                AllergyTypeID = entity.AllergyTypeID,
                NoKnownAllergy = entity.NoKnownAllergy,
                TakenBy = entity.TakenBy,
                TakenTime = entity.TakenTime,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static ContactAllergyDetailsModel ToModel(this ContactAllergyDetailsViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new ContactAllergyDetailsModel
            {
                ContactAllergyDetailID = entity.ContactAllergyDetailID,
                ContactAllergyID = entity.ContactAllergyID,
                AllergyID = entity.AllergyID,
                SeverityID = entity.SeverityID,
                Symptoms = entity.Symptoms,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static Response<ContactAllergyViewModel> ToModel(this Response<ContactAllergyModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<ContactAllergyViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ContactAllergyModel allergyModel)
                {
                    var transformedModel = allergyModel.ToViewModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
            {
                dataItems = null;
            }

            var model = new Response<ContactAllergyViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult,
                ID = entity.ID
            };

            return model;
        }

        public static Response<ContactAllergyDetailsViewModel> ToModel(this Response<ContactAllergyDetailsModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<ContactAllergyDetailsViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ContactAllergyDetailsModel allergyModel)
                {
                    var transformedModel = allergyModel.ToViewModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
            {
                dataItems = null;
            }

            var model = new Response<ContactAllergyDetailsViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult,
                ID = entity.ID
            };

            return model;
        }

        public static Response<ContactAllergyHeaderViewModel> ToModel(this Response<ContactAllergyHeaderModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<ContactAllergyHeaderViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ContactAllergyHeaderModel allergyModel)
                {
                    var transformedModel = allergyModel.ToViewModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
            {
                dataItems = null;
            }

            var model = new Response<ContactAllergyHeaderViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult,
                ID = entity.ID
            };

            return model;
        }
    }
}

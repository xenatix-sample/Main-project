using Axis.Model.Common;
using Axis.Model.Consents;
using Axis.PresentationEngine.Areas.Consents.Models;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.Consents.Translator
{
    /// <summary>
    /// Consents Translator
    /// </summary>
    public static class ConsentsTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ConsentsViewModel ToViewModel(this ConsentsModel entity)
        {
            if (entity == null)
                return null;

            var model = new ConsentsViewModel
            {
                AssessmentID = entity.AssessmentID,
                ConsentName = entity.ConsentName,
                ContactConsentID = entity.ContactConsentID,
                DateSigned = entity.DateSigned,
                ExpirationDate = entity.ExpirationDate,
                ExpirationReason = entity.ExpirationReason,
                ExpirationReasonID = entity.ExpirationReasonID,
                ResponseID = entity.ResponseID,
                SignatureStatus = entity.SignatureStatus,
                SignatureStatusID = entity.SignatureStatusID,
                AssessmentSectionID = entity.AssessmentSectionID,
                EffectiveDate = entity.EffectiveDate,
                ModifiedOn = entity.ModifiedOn,
                ContactID = entity.ContactID,
                ExpiredResponseID = entity.ExpiredResponseID,
                ExpiredBy = entity.ExpiredBy
            };

            return model;
        }



        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ConsentsViewModel> ToViewModel(this Response<ConsentsModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ConsentsViewModel>();
            var admissionViewModal = new List<ConsentsViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ConsentsModel admission)
                {
                    var transformedModel = admission.ToViewModel();
                    admissionViewModal.Add(transformedModel);
                });

                model.DataItems = admissionViewModal;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ConsentsModel ToModel(this ConsentsViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ConsentsModel
            {
                AssessmentID = model.AssessmentID,
                ConsentName = model.ConsentName,
                ContactConsentID = model.ContactConsentID,
                DateSigned = model.DateSigned,
                ExpirationDate = model.ExpirationDate,
                ExpirationReason = model.ExpirationReason,
                ExpirationReasonID = model.ExpirationReasonID,
                ResponseID = model.ResponseID,
                SignatureStatus = model.SignatureStatus,
                SignatureStatusID = model.SignatureStatusID,
                AssessmentSectionID = model.AssessmentSectionID,
                EffectiveDate = model.EffectiveDate,
                ModifiedOn = model.ModifiedOn,
                ContactID = model.ContactID,
                ExpiredResponseID = model.ExpiredResponseID,
                ExpiredBy = model.ExpiredBy
            };

            return entity;
        }
    }
}
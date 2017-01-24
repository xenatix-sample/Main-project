using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.Registration.Translator
{
    /// <summary>
    /// Admission translator
    /// </summary>
    public static class AdmissionTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static AdmissionViewModal ToViewModel(this AdmissionModal entity)
        {
            if (entity == null)
                return null;

            var model = new AdmissionViewModal
            {
                ContactAdmissionID = entity.ContactAdmissionID,
                ContactID = entity.ContactID,
                Comments = entity.Comments,
                EffectiveDate = entity.EffectiveDate,
                IsDocumentationComplete = entity.IsDocumentationComplete,
                UserID = entity.UserID,
                CompanyID = entity.CompanyID,
                DivisionID = entity.DivisionID,
                ProgramID = entity.ProgramID,
                ProgramUnitID = entity.ProgramUnitID,
                IsDischarged=entity.IsDischarged,
                IsCompanyActiveForProgramUnit = entity.IsCompanyActiveForProgramUnit,
                IsProgramUnitActiveForCompany = entity.IsProgramUnitActiveForCompany,
                DischargeDate = entity.DischargeDate,
                DataKey=entity.DataKey,
                IsCompanyActive=entity.IsCompanyActive,
                ContactDischargeNoteID=entity.ContactDischargeNoteID,
                SignedByEntityID = entity.SignedByEntityID,
                SignedByEntityName = entity.SignedByEntityName,
                DateSigned = entity.DateSigned,
                ModifiedOn = entity.ModifiedOn,
                DOB = entity.DOB,
                AdmissionReasonID=entity.AdmissionReasonID
                
            };

            return model;
        }



        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<AdmissionViewModal> ToViewModel(this Response<AdmissionModal> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<AdmissionViewModal>();
            var admissionViewModal = new List<AdmissionViewModal>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(AdmissionModal admission)
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
        public static AdmissionModal ToModel(this AdmissionViewModal model)
        {
            if (model == null)
                return null;

            var entity = new AdmissionModal
            {
                ContactAdmissionID = model.ContactAdmissionID,
                ContactID = model.ContactID,
                Comments = model.Comments,
                EffectiveDate = model.EffectiveDate,
                IsDocumentationComplete = model.IsDocumentationComplete,
                UserID = model.UserID,
                CompanyID = model.CompanyID,
                DivisionID = model.DivisionID,
                ProgramID = model.ProgramID,
                ProgramUnitID = model.ProgramUnitID,
                IsDischarged = model.IsDischarged,
                IsCompanyActiveForProgramUnit = model.IsCompanyActiveForProgramUnit,
                IsProgramUnitActiveForCompany = model.IsProgramUnitActiveForCompany,
                DischargeDate = model.DischargeDate,
                DataKey=model.DataKey,
                IsCompanyActive = model.IsCompanyActive,
                ContactDischargeNoteID = model.ContactDischargeNoteID,
                SignedByEntityID = model.SignedByEntityID,
                SignedByEntityName = model.SignedByEntityName,
                DateSigned = model.DateSigned,
                ModifiedOn = model.ModifiedOn,
                AdmissionReasonID = model.AdmissionReasonID

            };

            return entity;
        }
    }
}

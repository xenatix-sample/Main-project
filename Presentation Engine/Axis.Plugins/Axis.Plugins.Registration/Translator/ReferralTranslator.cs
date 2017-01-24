using System.Collections.Generic;
using System.Linq;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;

namespace Axis.Plugins.Registration.Translator
{
    /// <summary>
    /// Converts Model to ViewModel and vice versa
    /// </summary>
    public static class ReferralTranslator
    {
        /// <summary>
        /// Convert ReferralModel Model to ReferralViewModel ViewModel
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static ReferralViewModel ToViewModel(this ReferralModel entity)
        {
            if (entity == null)
                return null;
            var model = new ReferralViewModel
            {
                ReferralID = entity.ReferralID,
                ReferralName = entity.ReferralName,
                ReferralOrganization = entity.ReferralOrganization,
                ReferralCategoryID = entity.ReferralCategoryID,
                ReferralSourceID = entity.ReferralSourceID,
                ReferralOriginID = entity.ReferralOriginID,
                ReferralProgramID = entity.ReferralProgramID,
                ReferralClosureReasonID = entity.ReferralClosureReasonID,
                ReferralConcern = entity.ReferralConcern,
                ContactName = entity.ContactName,
                Organization = entity.Organization,
                ReferredDate = entity.ReferredDate,
                ReferralContactID = entity.ReferralContactID,
                ContactID = entity.ContactID,
                ProgramName = entity.ProgramName,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// Convert ReferralModel Model to ReferralViewModel ViewModel for response
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Response<ReferralViewModel> ToViewModel(this Response<ReferralModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ReferralViewModel>();
            if (entity.DataItems != null)
                model.DataItems = entity.DataItems.Select(x => x.ToViewModel()).ToList();

            return model;
        }

        /// <summary>
        /// Convert ReferralViewModel ViewModel to ReferralModel Model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ReferralModel ToModel(this ReferralViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ReferralModel
            {
                ReferralID = model.ReferralID,
                ReferralName = model.ReferralName,
                ReferralOrganization = model.ReferralOrganization,
                ReferralCategoryID = model.ReferralCategoryID,
                ReferralSourceID = model.ReferralSourceID,
                ReferralOriginID = model.ReferralOriginID,
                ReferralProgramID = model.ReferralProgramID,
                ReferralClosureReasonID = model.ReferralClosureReasonID,
                ReferralConcern = model.ReferralConcern,
                ContactName = model.ContactName,
                Organization = model.Organization,
                ReferredDate = model.ReferredDate,
                ReferralContactID = model.ReferralContactID,
                ContactID = model.ContactID,
                ProgramName = model.ProgramName,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }
    }
}

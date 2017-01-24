using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.Plugins.Registration.Models;
using System.Linq;

namespace Axis.Plugins.Registration.Translator
{
    /// <summary>
    /// ReferralSearch Translator
    /// </summary>
    public static class ReferralSearchTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ReferralSearchViewModel ToViewModel(this ReferralSearchModel entity)
        {
            if (entity == null)
                return null;
            var model = new ReferralSearchViewModel
            {
                ReferralHeaderID = entity.ReferralHeaderID,
                MRN = entity.MRN,
                TKIDSID = entity.TKIDSID,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Contact = entity.Contact,
                DOB = entity.DOB,
                ReferralType = entity.ReferralType,
                RequestorName = entity.RequestorName,
                ProgramUnit = entity.ProgramUnit,
                TransferReferralDate = entity.TransferReferralDate,
                ReferralStatus = entity.ReferralStatus,
                ContactID = entity.ContactID,
                HeaderContactID = entity.HeaderContactID,
                RequestorContact  = entity.RequestorContact,
                ModifiedOn = entity.ModifiedOn,
                ForwardedTo = entity.ForwardedTo,
                SubmittedBy = entity.SubmittedBy
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ReferralSearchViewModel> ToViewModel(this Response<ReferralSearchModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ReferralSearchViewModel>();
            if (entity.DataItems != null)
                model.DataItems = entity.DataItems.Select(x => x.ToViewModel()).ToList();

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ReferralSearchModel ToModel(this ReferralSearchViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ReferralSearchModel
            {
                ReferralHeaderID = model.ReferralHeaderID,
                MRN = model.MRN,
                TKIDSID = model.TKIDSID,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Contact = model.Contact,
                DOB = model.DOB,
                ReferralType = model.ReferralType,
                RequestorName = model.RequestorName,
                ProgramUnit = model.ProgramUnit,
                TransferReferralDate = model.TransferReferralDate,
                ReferralStatus = model.ReferralStatus,
                ContactID = model.ContactID,
                HeaderContactID = model.HeaderContactID,
                RequestorContact = model.RequestorContact,
                ModifiedOn = model.ModifiedOn,
                ForwardedTo = model.ForwardedTo,
                SubmittedBy = model.SubmittedBy
            };

            return entity;
        }
    }
}

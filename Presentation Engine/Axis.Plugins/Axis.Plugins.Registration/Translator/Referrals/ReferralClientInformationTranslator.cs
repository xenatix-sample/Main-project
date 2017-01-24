using Axis.Model.Registration.Referrals;
using Axis.Plugins.Registration.Models.Referrals;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator.Referrals
{
    /// <summary>
    /// 
    /// </summary>
    public static class ReferralClientInformationTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ReferralClientInformationViewModel ToViewModel(this ReferralClientInformationModel entity)
        {
            if (entity == null)
                return null;

            var model = new ReferralClientInformationViewModel
            {
                ReferralID = entity.ReferralHeaderID,
                referralClientAdditionalDetails = new ReferralClientAdditionalDetailsViewModel(),
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }
    }
}

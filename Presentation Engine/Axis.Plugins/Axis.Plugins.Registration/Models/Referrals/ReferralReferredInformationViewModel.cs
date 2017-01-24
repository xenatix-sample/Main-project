using Axis.Model.Common;
using Axis.PresentationEngine.Helpers.Model;
using System;

namespace Axis.Plugins.Registration.Models.Referrals
{
    /// <summary>
    /// Referral Referred Information View Model
    /// </summary>
    public class ReferralReferredInformationViewModel : BaseViewModel
    {
        /// <summary>
        ///Gets or sets the ReferredToDetail ID
        /// </summary> 
        public long ReferredToDetailID { get; set; }

        /// <summary>
        /// Gets or sets the ReferralHeader ID
        /// </summary>
        public long ReferralHeaderID { get; set; }     

        /// <summary>
        /// Gets or sets the action taken
        /// </summary>
        public string ActionTaken { get; set; }

        /// <summary>
        /// Gets or sets the Comments
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the UserID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// GEts or sets the Referral information Date time
        /// </summary>
        public DateTime? ReferredDateTime { get; set; }


        /// <summary>
        /// Gets or sets the Programe ID
        /// </summary>
        public long? OrganizationID { get; set; }

        /// <summary>
        /// Gets or sets the Contact No
        /// </summary>
        public int ContactNo { get; set; }
    }
}
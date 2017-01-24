using Axis.PresentationEngine.Helpers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.Registration.Models.Referrals
{
    public class ReferralDemographicsViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the referral identifier.
        /// </summary>
        /// <value>
        /// The referral identifier.
        /// </value>
        public long ReferralID { get; set; }

        /// <summary>
        /// Gets or sets the contact type identifier.
        /// </summary>
        /// <value>
        /// The contact type identifier.
        /// </value>
        public int? ContactTypeID { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the middle.
        /// </summary>
        /// <value>
        /// The middle.
        /// </value>
        public string Middle { get; set; }

        /// <summary>
        /// Gets or sets the suffix identifier.
        /// </summary>
        /// <value>
        /// The suffix identifier.
        /// </value>
        public int? SuffixID { get; set; }

        /// <summary>
        /// Gets or sets the mpi.
        /// </summary>
        /// <value>
        /// The mpi.
        /// </value>
        public string MPI { get; set; }


        /// <summary>
        /// Gets or sets the title identifier.
        /// </summary>
        /// <value>
        /// The title identifier.
        /// </value>
        public int? TitleID { get; set; }
    }
}

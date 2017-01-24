using Axis.Model.Common;
using Axis.Model.Consents;
using Axis.PresentationEngine.Areas.Consents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.PresentationEngine.Areas.Consents.Repository
{

    public interface IConsentsRepository
    {
        /// <summary>
        /// Get Consents
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns></returns>
        Response<ConsentsViewModel> GetConsents(long contactID);

        /// <summary>
        /// Adds the consent.
        /// </summary>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        Response<ConsentsViewModel> AddConsent(ConsentsViewModel consent);

        /// <summary>
        /// Updates the consent.
        /// </summary>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        Response<ConsentsViewModel> UpdateConsent(ConsentsViewModel consent);
    }
}

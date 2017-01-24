using Axis.Model.Common;
using Axis.Model.Consents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Service.Consents
{
    public interface IConsentsService
    {
        /// <summary>
        /// Get Consents
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns></returns>
        Response<ConsentsModel> GetConsents(long contactID);

        /// <summary>
        /// Adds the consent.
        /// </summary>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        Response<ConsentsModel> AddConsent(ConsentsModel consent);

        /// <summary>
        /// Updates the consents.
        /// </summary>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        Response<ConsentsModel> UpdateConsent(ConsentsModel consent);
    }
}

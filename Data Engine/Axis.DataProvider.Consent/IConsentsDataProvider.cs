using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.DataProvider.Common;
using Axis.Model.Common;
using Axis.Model.Consents;

namespace Axis.DataProvider.Consents
{
    public interface IConsentsDataProvider
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
        /// Updates the consent.
        /// </summary>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        Response<ConsentsModel> UpdateConsent(ConsentsModel consent);
    }
}

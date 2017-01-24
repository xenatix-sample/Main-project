using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Service.Registration
{
    public interface IBenefitsAssistanceService
    {
        /// <summary>
        /// Gets the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceID">The benefits assistance identifier.</param>
        /// <returns></returns>
        Response<BenefitsAssistanceModel> GetBenefitsAssistance(long benefitsAssistanceID);

        /// <summary>
        /// Gets the benefits assistance by contact identifier.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<BenefitsAssistanceModel> GetBenefitsAssistanceByContactID(long contactID);

        /// <summary>
        /// Adds the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceModel">The benefits assistance model.</param>
        /// <returns></returns>
        Response<BenefitsAssistanceModel> AddBenefitsAssistance(BenefitsAssistanceModel benefitsAssistanceModel);

        /// <summary>
        /// Updates the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceModel">The benefits assistance model.</param>
        /// <returns></returns>
        Response<BenefitsAssistanceModel> UpdateBenefitsAssistance(BenefitsAssistanceModel benefitsAssistanceModel);

        /// <summary>
        /// Deletes the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceID">The benefits assistance identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        Response<BenefitsAssistanceModel> DeleteBenefitsAssistance(long benefitsAssistanceID, DateTime modifiedOn);
    }
}

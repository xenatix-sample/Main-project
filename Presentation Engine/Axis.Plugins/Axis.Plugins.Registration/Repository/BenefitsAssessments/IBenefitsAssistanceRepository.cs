using Axis.Model.Common;
using Axis.Plugins.Registration.Models;
using System;


namespace Axis.Plugins.Registration.Repository
{
    public interface IBenefitsAssistanceRepository
    {
        /// <summary>
        /// Gets the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceID">The benefits assistance identifier.</param>
        /// <returns></returns>
        Response<BenefitsAssistanceViewModel> GetBenefitsAssistance(long benefitsAssistanceID);

        /// <summary>
        /// Gets the benefits assistance by contact identifier.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<BenefitsAssistanceViewModel> GetBenefitsAssistanceByContactID(long contactID);

        /// <summary>
        /// Adds the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssessmentsModel">The benefits assessments model.</param>
        /// <returns></returns>
        Response<BenefitsAssistanceViewModel> AddBenefitsAssistance(BenefitsAssistanceViewModel benefitsAssessmentsModel);

        /// <summary>
        /// Updates the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssessmentsModel">The benefits assessments model.</param>
        /// <returns></returns>
        Response<BenefitsAssistanceViewModel> UpdateBenefitsAssistance(BenefitsAssistanceViewModel benefitsAssessmentsModel);

        /// <summary>
        /// Deletes the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceID">The benefits assistance identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        Response<BenefitsAssistanceViewModel> DeleteBenefitsAssistance(long benefitsAssistanceID, DateTime modifiedOn);
    }
}

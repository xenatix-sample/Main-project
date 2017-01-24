using System;
using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.DataProvider.Registration
{
    /// <summary>
    ///
    /// </summary>
    public interface IAdditionalDemographicDataProvider
    {
        /// <summary>
        /// Gets the additional demographic.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        Response<AdditionalDemographicsModel> GetAdditionalDemographic(long contactId);

        /// <summary>
        /// Adds the additional demographic.
        /// </summary>
        /// <param name="additional">The additional.</param>
        /// <returns></returns>
        Response<AdditionalDemographicsModel> AddAdditionalDemographic(AdditionalDemographicsModel additional);

        /// <summary>
        /// Updates the additional demographic.
        /// </summary>
        /// <param name="additional">The additional.</param>
        /// <returns></returns>
        Response<AdditionalDemographicsModel> UpdateAdditionalDemographic(AdditionalDemographicsModel additional);

        /// <summary>
        /// Deletes the additional demographic.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<AdditionalDemographicsModel> DeleteAdditionalDemographic(long id, DateTime modifiedOn);
    }
}
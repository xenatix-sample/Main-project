using System;
using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;

namespace Axis.Plugins.Registration.Repository
{
    /// <summary>
    ///
    /// </summary>
    public interface IAdditionalDemographicRepository
    {
        /// <summary>
        /// Gets the additional demographic.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        Response<AdditionalDemographicsViewModel> GetAdditionalDemographic(long contactId);
        Task<Response<AdditionalDemographicsViewModel>> GetAdditionalDemographicAsync(long contactId);

        /// <summary>
        /// Adds the additional demographic.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        Response<AdditionalDemographicsViewModel> AddAdditionalDemographic(AdditionalDemographicsViewModel patient);

        /// <summary>
        /// Updates the additional demographic.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        Response<AdditionalDemographicsViewModel> UpdateAdditionalDemographic(AdditionalDemographicsViewModel patient);

        /// <summary>
        /// Deletes the additional demographic.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void DeleteAdditionalDemographic(long id, DateTime modifiedOn);
    }
}
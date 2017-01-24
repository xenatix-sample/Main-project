using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.ECI
{
    public interface IECIAdditionalDemographicRepository
    {
        /// <summary>
        /// Gets the additional demographic.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        Response<ECIAdditionalDemographicsViewModel> GetAdditionalDemographic(long contactId);
        Task<Response<ECIAdditionalDemographicsViewModel>> GetAdditionalDemographicAsync(long contactId);

        /// <summary>
        /// Adds the additional demographic.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        Response<ECIAdditionalDemographicsViewModel> AddAdditionalDemographic(ECIAdditionalDemographicsViewModel patient);

        /// <summary>
        /// Updates the additional demographic.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        Response<ECIAdditionalDemographicsViewModel> UpdateAdditionalDemographic(ECIAdditionalDemographicsViewModel patient);

        /// <summary>
        /// Deletes the additional demographic.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void DeleteAdditionalDemographic(long id, DateTime modifiedOn);
    }
}

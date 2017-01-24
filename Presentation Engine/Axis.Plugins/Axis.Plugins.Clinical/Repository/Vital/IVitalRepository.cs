using System;
using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.Vital;

namespace Axis.Plugins.Clinical.Repository.Vital
{
    public interface IVitalRepository
    {
        /// <summary>
        /// Gets the contact Vitals.
        /// </summary>
        /// <param name="ContactId">The contact identifier.</param>
        /// <returns></returns>
        Response<VitalViewModel> GetContactVitals(long ContactId);
        Task<Response<VitalViewModel>> GetContactVitalsAsync(long ContactId);

        /// <summary>
        /// Adds the contact Vital.
        /// </summary>
        /// <param name="vital">The Vital.</param>
        /// <returns></returns>
        Response<VitalViewModel> AddVital(VitalViewModel vital);

        /// <summary>
        /// Updates the Vital.
        /// </summary>
        /// <param name="vital">The Vital.</param>
        /// <returns></returns>
        Response<VitalViewModel> UpdateVital(VitalViewModel vital);

        /// <summary>
        /// Deletes the Vital.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<VitalViewModel> DeleteVital(long id, DateTime modifiedOn);
    }
}

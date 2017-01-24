using System;
using Axis.Model.Clinical;
using Axis.Model.Common;
using Axis.Service.Clinical.Vital;

namespace Axis.RuleEngine.Clinical.Vital
{
    public class VitalRuleEngine : IVitalRuleEngine
    {
        /// <summary>
        /// The contact Vital service
        /// </summary>
        private readonly IVitalService vitalService;

        /// <summary>
        /// Initializes a new instance of the <see cref="VitalRuleEngine" /> class.
        /// </summary>
        /// <param name="vitalService">The Vital service.</param>
        public VitalRuleEngine(IVitalService vitalService)
        {
            this.vitalService = vitalService;
        }

        /// <summary>
        /// Gets the contact Vitals.
        /// </summary>
        /// <param name="ContactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<VitalModel> GetContactVitals(long ContactId)
        {
            return vitalService.GetContactVitals(ContactId);
        }

        /// <summary>
        /// Adds the Vital.
        /// </summary>
        /// <param name="vital">The vital.</param>
        /// <returns></returns>
        public Response<VitalModel> AddVital(VitalModel vital)
        {
            return vitalService.AddVital(vital);
        }

        /// <summary>
        /// Updates the Vital.
        /// </summary>
        /// <param name="vital">The vital.</param>
        /// <returns></returns>
        public Response<VitalModel> UpdateVital(VitalModel vital)
        {
            return vitalService.UpdateVital(vital);
        }

        /// <summary>
        /// Deletes the Vital.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<VitalModel> DeleteVital(long id, DateTime modifiedOn)
        {
            return vitalService.DeleteVital(id, modifiedOn);
        }
    }
}

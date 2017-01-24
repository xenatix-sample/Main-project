using Axis.Model.Common;
using Axis.Model.Consents;
using Axis.Service.Consents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.Consents
{
    public class ConsentsRuleEngine : IConsentsRuleEngine
    {
        /// <summary>
        /// The consents service
        /// </summary>
        private IConsentsService _consentsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsentsRuleEngine" /> class.
        /// </summary>
        /// <param name="consents">The consents service.</param>
        public ConsentsRuleEngine(IConsentsService consentsService)
        {
            this._consentsService = consentsService;
        }

        /// <summary>
        /// Gets the consents.
        /// </summary>
        /// <param name="contactID">The consents identifier.</param>
        /// <returns></returns>
        public Response<ConsentsModel> GetConsents(long contactID)
        {
            return _consentsService.GetConsents(contactID);
        }

        /// <summary>
        /// Adds the consent.
        /// </summary>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        public Response<ConsentsModel> AddConsent(ConsentsModel consent)
        {
            return _consentsService.AddConsent(consent);
        }

        /// <summary>
        /// Updates the consents.
        /// </summary>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        public Response<ConsentsModel> UpdateConsent(ConsentsModel consent)
        {
            return _consentsService.UpdateConsent(consent);
        }
    }
}

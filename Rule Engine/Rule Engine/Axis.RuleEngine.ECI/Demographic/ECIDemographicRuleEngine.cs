using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Service.ECI.Demographic;

namespace Axis.RuleEngine.ECI.Demographic
{
    public class ECIDemographicRuleEngine : IECIDemographicRuleEngine
    {
        #region Class Variables

        private readonly IECIDemographicService _eciDemographicService;

        #endregion

        #region Constructors

        public ECIDemographicRuleEngine(IECIDemographicService eciDemographicService)
        {
            _eciDemographicService = eciDemographicService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the contact demographics.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ECIContactDemographicsModel> GetContactDemographics(long contactID)
        {
            return _eciDemographicService.GetContactDemographics(contactID);
        }

        /// <summary>
        /// Adds the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ECIContactDemographicsModel> AddContactDemographics(ECIContactDemographicsModel contact)
        {
            return _eciDemographicService.AddContactDemographics(contact);
        }

        /// <summary>
        /// Updates the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ECIContactDemographicsModel> UpdateContactDemographics(ECIContactDemographicsModel contact)
        {
            return _eciDemographicService.UpdateContactDemographics(contact);
        }

        #endregion
    }
}

using System;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration;
using System.Web.Http;
using System.Web.Http.Cors;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Registration
{
    /// <summary>
    ///
    /// </summary>


    public class ContactPhonesController : BaseApiController
    {
        /// <summary>
        /// The contact phones rule engine
        /// </summary>
        private readonly IContactPhonesRuleEngine contactPhonesRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPhonesController" /> class.
        /// </summary>
        /// <param name="contactPhonesRuleEngine">The contact phones rule engine.</param>
        public ContactPhonesController(IContactPhonesRuleEngine contactPhonesRuleEngine)
        {
            this.contactPhonesRuleEngine = contactPhonesRuleEngine;
        }

        /// <summary>
        /// Gets the contact phones.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>

        [Authorization(Modules = new string[] { Module.General, Module.Registration, Module.ECI, Module.LawLiaison, Module.CrisisLine, Module.Intake, Module.BusinessAdministration }, PermissionKeys = new string[] { ChartPermissionKey.Chart_Chart_Assessment, BusinessAdministrationPermissionKey.BusinessAdministration_ClientMerge_ClientMerge }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetContactPhones(long contactID, int? contactTypeID)
        {
            return new HttpResult<Response<ContactPhoneModel>>(contactPhonesRuleEngine.GetContactPhones(contactID, contactTypeID), Request);
        }

        /// <summary>
        /// Adds the update contact phones.
        /// </summary>
        /// <param name="contactPhoneModel">The contact phone model.</param>
        /// <returns></returns>
        [Authorization(Modules = new string[] { Module.General, Module.Registration, Module.ECI, Module.LawLiaison, Module.CrisisLine }, Permissions = new string[] { Permission.Create, Permission.Update })]
        [HttpPost]
        public IHttpActionResult AddUpdateContactPhones(ContactPhoneModel contactPhoneModel)
        {
            return new HttpResult<Response<ContactPhoneModel>>(contactPhonesRuleEngine.AddUpdateContactPhones(contactPhoneModel), Request);
        }

        /// <summary>
        /// Deletes the contact phones.
        /// </summary>
        /// <param name="contactPhoneModel">The contact phone model.</param>
        /// <returns></returns>
        [Authorization(Modules = new string[] { Module.General, Module.Registration, Module.ECI, Module.LawLiaison, Module.CrisisLine }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteContactPhone(long contactPhoneId, DateTime modifiedOn)
        {
            return new HttpResult<Response<ContactPhoneModel>>(contactPhonesRuleEngine.DeleteContactPhone(contactPhoneId, modifiedOn), Request);
        }
    }
}
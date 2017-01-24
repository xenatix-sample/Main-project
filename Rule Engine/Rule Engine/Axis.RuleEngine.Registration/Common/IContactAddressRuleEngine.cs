using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.Registration.Common
{
    public interface IContactAddressRuleEngine
    {
        /// <summary>
        /// Get Contact Address
        /// </summary>
        /// <param name="contactID"></param>
        /// <param name="contactTypeId"></param>
        /// <returns></returns>
        Response<ContactAddressModel> GetAddresses(long contactID, int contactTypeId);
        
        /// <summary>
        /// Add Update contact Address
        /// </summary>
        /// <param name="contactID"></param>
        /// <param name="contactAddress"></param>
        /// <returns></returns>
        Response<ContactAddressModel> AddUpdateAddress(List<ContactAddressModel> contactAddress);
        
        
        /// <summary>
        /// Delete contact address
        /// </summary>
        /// <param name="contactAddressID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ContactAddressModel> DeleteAddress(long contactAddressID, DateTime modifiedOn);
    }
}

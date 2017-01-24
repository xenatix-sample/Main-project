using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Service.Registration.Common
{
    public interface IContactAddressService
    {
        /// <summary>
        /// Get Contact Address
        /// </summary>
        /// <param name="contactID"></param>
        /// <param name="contactTypeID"></param>
        /// <returns></returns>
        Response<ContactAddressModel> GetAddresses(long contactID, int contactTypeID);

        /// <summary>
        /// Add Update contact Address
        /// </summary>
        /// <param name="addressModel"></param>
        /// <returns></returns>
        Response<ContactAddressModel> AddUpdateAddress(List<ContactAddressModel> addressModel);


        /// <summary>
        /// Delete Address
        /// </summary>
        /// <param name="contactAddressID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ContactAddressModel> DeleteAddress(long contactAddressID, DateTime modifiedOn);
    }
}

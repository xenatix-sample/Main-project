using System;
using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.DataProvider.Registration
{
    public interface IContactAddressDataProvider
    {
        Response<ContactAddressModel> AddAddresses(long contactID, List<ContactAddressModel> contact);
        Response<ContactAddressModel> UpdateAddresses(long contactID, List<ContactAddressModel> contact);
        Response<ContactAddressModel> CopyContactAddresses(long contactID, List<ContactAddressModel> contact);
        Response<ContactAddressModel> GetAddresses(long contactID, int? contactTypeID);
        Response<ContactAddressModel> DeleteAddress(long ContactAddressID, DateTime modifiedOn);
    }
}

using System;
using Axis.Model.Common;
using Axis.Plugins.Registration.Model;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Repository.Common
{
    public interface IContactAddressRepository
    {
        Response<ContactAddressViewModel> GetAddresses(long contactID, int contactTypeId);
        Response<ContactAddressViewModel> AddUpdateAddress(List<ContactAddressViewModel> addressModel);
        Response<ContactAddressViewModel> DeleteAddress(long contactAddressID, DateTime modifiedOn);
    }
}

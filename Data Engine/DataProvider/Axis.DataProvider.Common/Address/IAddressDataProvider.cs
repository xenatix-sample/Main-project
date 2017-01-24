using System.Collections.Generic;
using Axis.Model.Address;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IAddressDataProvider
    {
        Response<AddressModel> GetAddress(int addressID);
        Response<AddressModel> AddAddress(AddressModel address);
        Response<AddressModel> UpdateAddress(AddressModel address);        
        Response<UserAddressModel> GetUserAddresses(int userID);
        Response<UserAddressModel> SaveUserAddresses(int userID, List<UserAddressModel> addresses);
    }
}
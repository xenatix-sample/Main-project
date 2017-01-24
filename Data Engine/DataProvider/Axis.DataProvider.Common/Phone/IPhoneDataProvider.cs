using Axis.Model.Common;
using System.Collections.Generic;
using Axis.Model.Phone;

namespace Axis.DataProvider.Common
{
    public interface IPhoneDataProvider
    {
        Response<PhoneModel> GetPhones(long phoneID);
        Response<PhoneModel> AddPhones(PhoneModel phone);
        Response<PhoneModel> UpdatePhones(PhoneModel phone);
        Response<UserPhoneModel> GetUserPhones(int userID);
        Response<UserPhoneModel> SaveUserPhones(int userID, List<UserPhoneModel> phones);
    }
}
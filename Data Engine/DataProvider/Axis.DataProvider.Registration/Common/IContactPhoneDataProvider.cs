using System;
using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.DataProvider.Registration
{
    public interface IContactPhoneDataProvider
    {
        //Response<ContactPhoneModel> GetPhones(long contactID);
        Response<ContactPhoneModel> GetPhones(long contactID, int? contactTypeID);
        Response<ContactPhoneModel> AddPhones(long contactID, List<ContactPhoneModel> contact);
        Response<ContactPhoneModel> UpdatePhones(long contactID, List<ContactPhoneModel> contact);
        Response<ContactPhoneModel> DeleteContactPhone(long contactPhoneID, DateTime modifiedOn);
    }
}


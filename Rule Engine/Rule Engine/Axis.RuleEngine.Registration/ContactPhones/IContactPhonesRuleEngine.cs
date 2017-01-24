using System;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlanGroup;
using Axis.Model.Registration;
using System.Collections.Generic;

namespace Axis.RuleEngine.Registration
{

    public interface IContactPhonesRuleEngine
    {
        Response<ContactPhoneModel> GetContactPhones(long contactID, int? contactTypeID);
        Response<ContactPhoneModel> AddUpdateContactPhones(ContactPhoneModel contactPhoneModel);
        Response<ContactPhoneModel> DeleteContactPhone(long contactPhoneId, DateTime modifiedOn);
    }
}
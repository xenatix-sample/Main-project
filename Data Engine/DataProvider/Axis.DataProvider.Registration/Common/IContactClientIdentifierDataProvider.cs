using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.DataProvider.Registration.Common
{
    public interface IContactClientIdentifierDataProvider
    {
        Response<ContactClientIdentifierModel> AddUpdateContactClientIdentifiers(long contactID, List<ContactClientIdentifierModel> identifiers);
        Response<ContactClientIdentifierModel> GetContactClientIdentifiers(long contactID);
    }
}

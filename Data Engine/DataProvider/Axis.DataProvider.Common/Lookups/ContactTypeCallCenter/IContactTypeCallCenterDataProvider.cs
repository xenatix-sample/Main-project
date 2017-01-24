using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IContactTypeCallCenterDataProvider
    {
        Response<ContactTypeModel> GetContactTypes();
    }
}

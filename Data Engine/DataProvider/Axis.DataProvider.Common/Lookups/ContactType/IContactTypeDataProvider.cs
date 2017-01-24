using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IContactTypeDataProvider
    {
        Response<ContactTypeModel> GetContactTypes();
    }
}
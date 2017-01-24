using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IRegistrationTypeDataProvider
    {
        Response<RegistrationTypeModel> GetRegistrationType();
    }
}
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IMaritalStatusDataProvider
    {
        Response<MaritalStatusModel> GetMaritalStatuses();
    }
}
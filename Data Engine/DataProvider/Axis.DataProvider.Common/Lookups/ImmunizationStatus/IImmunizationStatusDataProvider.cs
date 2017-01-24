using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IImmunizationStatusDataProvider
    {
        Response<ImmunizationStatusModel> GetImmunizationStatuses();
    }
}

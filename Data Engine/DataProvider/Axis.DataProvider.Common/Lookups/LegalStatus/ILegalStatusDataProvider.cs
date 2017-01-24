using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface ILegalStatusDataProvider
    {
        Response<LegalStatusModel> GetLegalStatuses();
    }
}
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IEducationStatusDataProvider
    {
        Response<EducationStatusModel> GetEducationStatuses();
    }
}
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IEmploymentStatusDataProvider
    {
        Response<EmploymentStatusModel> GetEmploymentStatuses();
    }
}
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IReportDataProvider
    {
        Response<ReportsModel> GetReportsByType(string reportTypeName);
    }
}
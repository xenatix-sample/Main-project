using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// Interface for Finance Frequency Data provider
    /// </summary>
    public interface IFinanceFrequencyDataProvider
    {
        Response<FinanceFrequencyModel> GetFrequencies();
    }
}
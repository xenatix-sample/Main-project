using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface ICitizenshipDataProvider
    {
        Response<CitizenshipModel> GetCitizenships();
    }
}
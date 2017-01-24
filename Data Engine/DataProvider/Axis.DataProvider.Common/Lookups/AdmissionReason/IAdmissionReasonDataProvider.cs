using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IAdmissionReasonDataProvider
    {
        Response<AdmissionReasonModel> GetAdmissionReasonTypes();
    }
}

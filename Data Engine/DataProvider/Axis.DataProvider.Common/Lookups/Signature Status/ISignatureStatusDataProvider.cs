using Axis.Model.Common;
namespace Axis.DataProvider.Common
{
    public interface ISignatureStatusDataProvider
    {
        Response<SignatureStatusModel> GetSignatureStatus();
    }
}

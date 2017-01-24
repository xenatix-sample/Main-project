using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface ICredentialDataProvider
    {
        Response<CredentialModel> GetCredentials();
    }
}

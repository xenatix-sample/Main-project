using Axis.Model.Common;
namespace Axis.DataProvider.Common
{
    public interface IDocumentTypeGroupDataProvider
    {
        Response<DocumentTypeGroupModel> GetDocumentTypeGroup();
    }
}

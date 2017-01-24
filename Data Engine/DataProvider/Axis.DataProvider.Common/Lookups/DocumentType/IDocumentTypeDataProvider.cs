using Axis.Model.Common;
namespace Axis.DataProvider.Common
{
    public interface IDocumentTypeDataProvider
    {
        Response<DocumentTypeModel> GetDocumentType();
    }
}

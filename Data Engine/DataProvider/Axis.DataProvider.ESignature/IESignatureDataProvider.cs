using Axis.Model.Common;
using Axis.Model.ESignature;

namespace Axis.DataProvider.ESignature
{
    public interface IESignatureDataProvider 
    {
        Response<DocumentEntitySignatureModel> SaveDocumentSignature(DocumentEntitySignatureModel documentSignature);
        Response<DocumentEntitySignatureModel> GetDocumentSignatures(long documentId, int documentTypeId);
        Response<EntitySignatureModel> GetEntitySignature(long entityId, int entityTypeId, int? signatureTypeId);
        Response<DocumentEntitySignatureModel> AddDocumentEntitySignatures(DocumentEntitySignatureModel documentEntitySignature);
        Response<EntitySignatureModel> AddSignature(EntitySignatureModel entitySignature);
        Response<EntitySignatureModel> AddEntitySignature(EntitySignatureModel entitySignature);
        Response<EntitySignatureModel> UpdateEntitySignature(EntitySignatureModel entitySignature);
    }
}

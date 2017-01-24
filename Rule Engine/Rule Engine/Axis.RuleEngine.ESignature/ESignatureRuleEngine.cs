using Axis.Model.Common;
using Axis.Model.ESignature;
using Axis.Service.ESignature;

namespace Axis.RuleEngine.ESignature
{
    public class ESignatureRuleEngine : IESignatureRuleEngine
    {
        #region Class Variables

        private IESignatureService _eSignatureService;

        #endregion

        #region Constructors

        public ESignatureRuleEngine(IESignatureService esignatureService)
        {
            this._eSignatureService = esignatureService;
        }

        #endregion

        #region Public Methods

        public Response<DocumentEntitySignatureModel> SaveDocumentSignature(DocumentEntitySignatureModel documentSignature)
        {
            return _eSignatureService.SaveDocumentSignature(documentSignature);
        }

        public Response<DocumentEntitySignatureModel> GetDocumentSignatures(long documentId, int documentTypeId)
        {
            return _eSignatureService.GetDocumentSignatures(documentId, documentTypeId);
        }

        public Response<EntitySignatureModel> GetEntitySignature(long entityId, int entityTypeId, int? signatureTypeId)
        {
            return _eSignatureService.GetEntitySignature(entityId, entityTypeId, signatureTypeId);
        }

        public Response<EntitySignatureModel> AddSignature(EntitySignatureModel entitySignature)
        {
            return _eSignatureService.AddSignature(entitySignature);
        }

        public Response<EntitySignatureModel> AddEntitySignature(EntitySignatureModel entitySignature)
        {
            return _eSignatureService.AddEntitySignature(entitySignature);
        }

        public Response<EntitySignatureModel> UpdateEntitySignature(EntitySignatureModel entitySignature)
        {
            return _eSignatureService.UpdateEntitySignature(entitySignature);
        }


        #endregion
    }
}

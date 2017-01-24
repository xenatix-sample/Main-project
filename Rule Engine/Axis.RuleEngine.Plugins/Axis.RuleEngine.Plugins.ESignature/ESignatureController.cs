using System.Web.Http;
using Axis.Model.Common;
using Axis.Model.ESignature;
using Axis.RuleEngine.ESignature;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;

namespace Axis.RuleEngine.Plugins.ESignature
{
    public class ESignatureController : BaseApiController
    {
        #region Class Variables

        IESignatureRuleEngine _eSignatureRuleEngine = null;

        #endregion

        #region Constructors

        public ESignatureController(IESignatureRuleEngine esignatureRuleEngine)
        {
            this._eSignatureRuleEngine = esignatureRuleEngine;
        }

        #endregion

        #region Public Methods

        [HttpPost]
        public IHttpActionResult SaveDocumentSignature(DocumentEntitySignatureModel documentEntitySignature)
        {
            return new HttpResult<Response<DocumentEntitySignatureModel>>(_eSignatureRuleEngine.SaveDocumentSignature(documentEntitySignature), Request);
        }

        [HttpGet]
        public IHttpActionResult GetDocumentSignatures(long documentId, int documentTypeId)
        {
            return new HttpResult<Response<DocumentEntitySignatureModel>>(_eSignatureRuleEngine.GetDocumentSignatures(documentId, documentTypeId), Request);
        }


        [HttpGet]
        public IHttpActionResult GetEntitySignature(long entityId, int entityTypeId, int? signatureTypeId)
        {
            return new HttpResult<Response<EntitySignatureModel>>(_eSignatureRuleEngine.GetEntitySignature(entityId, entityTypeId, signatureTypeId), Request);
        }

        [HttpPost]
        public IHttpActionResult AddEntitySignature(EntitySignatureModel entitySignature)
        {
            return new HttpResult<Response<EntitySignatureModel>>(_eSignatureRuleEngine.AddEntitySignature(entitySignature), Request);
        }

        [HttpPost]
        public IHttpActionResult UpdateEntitySignature(EntitySignatureModel entitySignature)
        {
            return new HttpResult<Response<EntitySignatureModel>>(_eSignatureRuleEngine.UpdateEntitySignature(entitySignature), Request);
        }

        [HttpPost]
        public IHttpActionResult AddSignature(EntitySignatureModel entitySignature)
        {
            return new HttpResult<Response<EntitySignatureModel>>(_eSignatureRuleEngine.AddSignature(entitySignature), Request);
        }
        #endregion
    }
}

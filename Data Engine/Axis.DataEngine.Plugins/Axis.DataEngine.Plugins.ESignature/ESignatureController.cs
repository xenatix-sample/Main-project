using System.Web.Http;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.ESignature;
using Axis.Model.ESignature;
using Axis.DataEngine.Helpers.Controllers;
using Axis.Model.Common;

namespace Axis.DataEngine.Plugins.ESignature
{
    public class ESignatureController : BaseApiController
    {
        #region Class Variables

        IESignatureDataProvider eSignatureDataProvider = null;

        #endregion

        #region Constructors

        public ESignatureController(IESignatureDataProvider esignatureDataProvider)
        {
            this.eSignatureDataProvider = esignatureDataProvider;
        }

        #endregion

        #region Public Methods
        [HttpPost]
        public IHttpActionResult SaveDocumentSignature(DocumentEntitySignatureModel documentEntitySignature)
        {
            return new HttpResult<Response<DocumentEntitySignatureModel>>(eSignatureDataProvider.SaveDocumentSignature(documentEntitySignature), Request);
        }

        [HttpGet]
        public IHttpActionResult GetDocumentSignatures(long documentId, int documentTypeId)
        {
            return new HttpResult<Response<DocumentEntitySignatureModel>>(eSignatureDataProvider.GetDocumentSignatures(documentId, documentTypeId), Request);
        }

        [HttpGet]
        public IHttpActionResult GetEntitySignature(long entityId, int entityTypeId, int? signatureTypeId)
        {
            return new HttpResult<Response<EntitySignatureModel>>(eSignatureDataProvider.GetEntitySignature(entityId, entityTypeId, signatureTypeId), Request);
        }

        [HttpPost]
        public IHttpActionResult AddEntitySignature(EntitySignatureModel entitySignature)
        {
            return new HttpResult<Response<EntitySignatureModel>>(eSignatureDataProvider.AddEntitySignature(entitySignature), Request);
        }

        [HttpPost]
        public IHttpActionResult UpdateEntitySignature(EntitySignatureModel entitySignature)            
        {
            return new HttpResult<Response<EntitySignatureModel>>(eSignatureDataProvider.UpdateEntitySignature(entitySignature), Request);
        }

        [HttpPost]
        public IHttpActionResult AddSignature(EntitySignatureModel entitySignature)
        {
            return new HttpResult<Response<EntitySignatureModel>>(eSignatureDataProvider.AddSignature(entitySignature), Request);
        }
        #endregion
    }
}

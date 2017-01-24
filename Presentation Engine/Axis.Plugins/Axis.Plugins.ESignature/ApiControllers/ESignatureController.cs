using System.Web.Helpers;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Plugins.ESignature.Repository;
using Axis.Plugins.ESignature.Translator;
using Axis.Plugins.Registration.Models;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.Plugins.ESignature.Models;
using Axis.Model.ESignature;

namespace Axis.Plugins.ESignature.ApiControllers
{
    public class ESignatureController : BaseApiController
    {
        #region Class Variables

        private readonly IESignatureRepository _eSignatureRepository;

        #endregion

        #region Constructors

        public ESignatureController(IESignatureRepository eSignatureRepository)
        {
            _eSignatureRepository = eSignatureRepository;
        }

        public ESignatureController()
        {
        }

        #endregion

        #region Json Results

        /// <summary>
        /// Saves the signature to the database
        /// </summary>
        [HttpPost]
        public Response<Model.ESignature.DocumentEntitySignatureModel> SaveDocumentSignature(DocumentEntitySignatureViewModel documentEntitySignature)
        {
            return _eSignatureRepository.SaveDocumentSignature(documentEntitySignature.ToModel());
        }

        /// <summary>
        /// Retrieves the signature from the database using the provided IDs
        /// </summary>
        [HttpGet]
        public Response<DocumentEntitySignatureViewModel> GetDocumentSignatures(long documentId, int documentTypeId)
        {
            return _eSignatureRepository.GetDocumentSignatures(documentId, documentTypeId).ToViewModel();
        }

        /// <summary>
        /// Get entity signature
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        public Response<EntitySignatureViewModel> GetEntitySignature(long entityId, int entityTypeId, int? signatureTypeId)
        {
            return _eSignatureRepository.GetEntitySignature(entityId, entityTypeId, signatureTypeId).ToViewModel();
        }

        /// <summary>
        /// Add entity signature
        /// </summary>
        /// <param name="entitySignature"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<EntitySignatureModel> AddEntitySignature(EntitySignatureViewModel entitySignature)
        {
            return _eSignatureRepository.AddEntitySignature(entitySignature.ToModel());
        }

        /// <summary>
        /// Update entity signature
        /// </summary>
        /// <param name="entitySignature"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<EntitySignatureModel> UpdateEntitySignature(EntitySignatureViewModel entitySignature) 
        {
            return _eSignatureRepository.UpdateEntitySignature(entitySignature.ToModel());
        }

        /// <summary>
        /// Add signature
        /// </summary>
        /// <param name="entitySignature"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<EntitySignatureModel> AddSignature(EntitySignatureViewModel entitySignature)
        {
            return _eSignatureRepository.AddSignature(entitySignature.ToModel());
        }


        #endregion
    }
}

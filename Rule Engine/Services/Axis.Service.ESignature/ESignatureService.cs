using System.Collections.Specialized;
using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.ESignature;
using Axis.Security;

namespace Axis.Service.ESignature
{
    public class ESignatureService : IESignatureService
    {
        #region Class Variables

        private CommunicationManager _communicationManager;
        private const string baseRoute = "eSignature/";

        #endregion

        #region Constructors

        public ESignatureService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        public Response<DocumentEntitySignatureModel> SaveDocumentSignature(
            DocumentEntitySignatureModel documentSignature)
        {
            var apiUrl = baseRoute + "SaveDocumentSignature";
            return _communicationManager.Post<DocumentEntitySignatureModel, Response<DocumentEntitySignatureModel>>(documentSignature, apiUrl);
        }

        public Response<DocumentEntitySignatureModel> GetDocumentSignatures(long documentId, int documentTypeId)
        {
            var apiUrl = baseRoute + "GetDocumentSignatures";
            var paramCollection = new NameValueCollection();
            paramCollection.Add("documentId", documentId.ToString());
            paramCollection.Add("documentTypeId", documentTypeId.ToString());

            return _communicationManager.Get<Response<DocumentEntitySignatureModel>>(paramCollection, apiUrl);
        }

        public Response<EntitySignatureModel> GetEntitySignature(long entityId, int entityTypeId, int? signatureTypeId)
        {
            var apiUrl = baseRoute + "GetEntitySignature";
            var paramCollection = new NameValueCollection();
            paramCollection.Add("entityId", entityId.ToString());
            paramCollection.Add("entityTypeId", entityTypeId.ToString());
            paramCollection.Add("signatureTypeId", signatureTypeId.ToString());

            return _communicationManager.Get<Response<EntitySignatureModel>>(paramCollection, apiUrl);
        }

        public Response<EntitySignatureModel> AddSignature(EntitySignatureModel entitySignature)
        {
            string apiUrl = baseRoute + "AddSignature";
            var response = _communicationManager.Post<EntitySignatureModel, Response<EntitySignatureModel>>(entitySignature, apiUrl);
            return response;
        }

        public Response<EntitySignatureModel> AddEntitySignature(EntitySignatureModel entitySignature)
        {
            string apiUrl = baseRoute + "AddEntitySignature";
            var response = _communicationManager.Post<EntitySignatureModel, Response<EntitySignatureModel>>(entitySignature, apiUrl);
            return response;
        }

        public Response<EntitySignatureModel> UpdateEntitySignature(EntitySignatureModel entitySignature)
        {
            string apiUrl = baseRoute + "UpdateEntitySignature";
            var response = _communicationManager.Post<EntitySignatureModel, Response<EntitySignatureModel>>(entitySignature, apiUrl);
            return response;
        }

        #endregion

    }
}
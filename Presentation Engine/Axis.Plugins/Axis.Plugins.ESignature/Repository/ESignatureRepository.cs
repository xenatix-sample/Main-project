using System.Collections.Specialized;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.ESignature;
using Axis.Service;

namespace Axis.Plugins.ESignature.Repository
{
    public class ESignatureRepository : IESignatureRepository
    {
        #region Class Variables

        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "eSignature/";

        #endregion Class Variables

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ESignatureRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsentRepository" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ESignatureRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion Constructor

        #region Public Methods

        public Response<DocumentEntitySignatureModel> SaveDocumentSignature(DocumentEntitySignatureModel documentSignature)
        {
            string apiUrl = baseRoute + "SaveDocumentSignature";
            var response = _communicationManager.Post<DocumentEntitySignatureModel, Response<DocumentEntitySignatureModel>>(documentSignature, apiUrl);
            return response;
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

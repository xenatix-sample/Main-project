using System;
using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.Registration.Models
{
    public class DocumentEntitySignatureViewModel : BaseViewModel
    {
        /// <summary>
        /// The primary key of the document being signed
        /// </summary>
        public long DocumentId { get; set; }

        /// <summary>
        /// The type of document being signed (assessment, screening, etc)
        /// </summary>
        public int? DocumentTypeId { get; set; }

        /// <summary>
        /// The primary key of the Entity-Signature table
        /// </summary>
        public long? EntitySignatureId { get; set; }

        /// <summary>
        /// The primary key of the person signing
        /// </summary>
        public long? EntityId { get; set; }

        /// <summary>
        /// The entity name for the person signing
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// The type of person signing (user, staff, external user, etc)
        /// </summary>
        public int? EntityTypeId { get; set; }

        /// <summary>
        /// The binary image data of the signature itself
        /// </summary>
        public string SignatureBlob { get; set; }

        /// <summary>
        /// Gets or sets the credential identifier.
        /// </summary>
        /// <value>
        /// The credential identifier.
        /// </value>
        public long? CredentialID { get; set; }

        /// <summary>
        /// Gets or sets the signature identifier.
        /// </summary>
        /// <value>
        /// The signature identifier.
        /// </value>
        public long? SignatureID { get; set; }
    }
}

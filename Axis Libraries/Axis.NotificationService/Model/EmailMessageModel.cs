using System.Collections.Generic;

namespace Axis.NotificationService.Model
{
    /// <summary>
    ///
    /// </summary>
    public class EmailMessageModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailMessageModel"/> class.
        /// </summary>
        public EmailMessageModel()
        {
            From = new EmailAddressModel();
            To = new List<EmailAddressModel>();
            CC = new List<EmailAddressModel>();
            BCC = new List<EmailAddressModel>();
            Attachments = new List<EmailAttachmentModel>();
        }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is body HTML.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is body HTML; otherwise, <c>false</c>.
        /// </value>
        public bool IsBodyHtml { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public EmailAddressModel From { get; set; }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public List<EmailAddressModel> To { get; set; }

        /// <summary>
        /// Gets or sets the cc.
        /// </summary>
        /// <value>
        /// The cc.
        /// </value>
        public List<EmailAddressModel> CC { get; set; }

        /// <summary>
        /// Gets or sets the BCC.
        /// </summary>
        /// <value>
        /// The BCC.
        /// </value>
        public List<EmailAddressModel> BCC { get; set; }

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        /// <value>
        /// The attachments.
        /// </value>
        public List<EmailAttachmentModel> Attachments { get; set; }
    }
}
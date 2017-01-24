namespace Axis.Model.Common.MessageTemplate
{
    /// <summary>
    /// Represents email template
    /// </summary>
    public class MessageTemplateModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the email template identifier.
        /// </summary>
        /// <value>
        /// The email template identifier.
        /// </value>
        public long MessageTemplateID { get; set; }

        /// <summary>
        /// Gets or sets the template identifier.
        /// </summary>
        /// <value>
        /// The template identifier.
        /// </value>
        public long TemplateID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is HTML body.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is HTML body; otherwise, <c>false</c>.
        /// </value>
        public bool IsHtmlBody { get; set; }

        /// <summary>
        /// Gets or sets the email subject.
        /// </summary>
        /// <value>
        /// The email subject.
        /// </value>
        public string EmailSubject { get; set; }

        /// <summary>
        /// Gets or sets the email body.
        /// </summary>
        /// <value>
        /// The email body.
        /// </value>
        public string MessageBody { get; set; }
    }
}
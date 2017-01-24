using Axis.Model.Common;
using System;

namespace Axis.Model.Account
{
    /// <summary>
    ///
    /// </summary>
    public class ResetPasswordModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the reset password identifier.
        /// </summary>
        /// <value>
        /// The reset password identifier.
        /// </value>
        public long ResetPasswordID { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets the reset link.
        /// </summary>
        /// <value>
        /// The reset link.
        /// </value>
        public Guid ResetIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>
        /// The phone.
        /// </value>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the phone identifier.
        /// </summary>
        /// <value>
        /// The phone identifier.
        /// </value>
        public long PhoneID { get; set; }

        /// <summary>
        /// Gets or sets the otp code.
        /// </summary>
        /// <value>
        /// The otp code.
        /// </value>
        public string OTPCode { get; set; }

        /// <summary>
        /// Gets or sets the security question identifier.
        /// </summary>
        /// <value>
        /// The security question identifier.
        /// </value>
        public long SecurityQuestionID { get; set; }

        /// <summary>
        /// Gets or sets the security answer.
        /// </summary>
        /// <value>
        /// The security answer.
        /// </value>
        public string SecurityAnswer { get; set; }

        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        /// <value>
        /// The new password.
        /// </value>
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the requestor ip address.
        /// </summary>
        /// <value>
        /// The requestor ip address.
        /// </value>
        public string RequestorIPAddress { get; set; }

        /// <summary>
        /// Gets or sets the expires on.
        /// </summary>
        /// <value>
        /// The expires on.
        /// </value>
        public DateTime ExpiresOn { get; set; }

        public bool ADFlag { get; set; }
        public string ADUserPasswordResetMessage { get; set; }
        public bool IsDigitalPassword { get; set; }
    }
}
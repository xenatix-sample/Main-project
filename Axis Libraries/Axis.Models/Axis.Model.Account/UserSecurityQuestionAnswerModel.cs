using Axis.Model.Common;
namespace Axis.Model.Account
{
    /// <summary>
    /// User Security Question Answer Model
    /// </summary>
    public class UserSecurityQuestionAnswerModel : BaseEntity
    {
        /// <summary>
        /// Identify user id
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Identify user security question id
        /// </summary>
        public long UserSecurityQuestionID { get; set; }

        /// <summary>
        /// Identify security question id
        /// </summary>
        public int SecurityQuestionID { get; set; }

        /// <summary>
        /// Security answer
        /// </summary>
        public string SecurityAnswer { get; set; }

        public bool ADFlag { get; set; }
    }
}

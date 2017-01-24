using Axis.PresentationEngine.Helpers.Model;

namespace Axis.PresentationEngine.Areas.Account.Model
{
    /// <summary>
    /// Security question answer view modal
    /// </summary>
    public class UserSecurityQuestionAnswerViewModel : BaseViewModel
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
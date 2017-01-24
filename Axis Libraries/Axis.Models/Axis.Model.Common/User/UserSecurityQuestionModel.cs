using Axis.Model.Common.Lookups.SecurityQuestion;

namespace Axis.Model.Common.User
{
    public class UserSecurityQuestionModel : SecurityQuestionModel
    {
        public int UserID { get; set; }
        public long UserSecurityQuestionID { get; set; }
        public string SecurityAnswer { get; set; }
    }
}

using Axis.Model.Common;
using Axis.Model.Email;
using System.Collections.Generic;

namespace Axis.DataProvider.Common
{
    public interface IEmailDataProvider
    {
        Response<EmailModel> GetEmail(long emailID);
        Response<EmailModel> AddEmail(EmailModel email);
        Response<EmailModel> UpdateEmail(EmailModel email);
        Response<UserEmailModel> GetUserEmails(int userID);
        Response<UserEmailModel> UpdateUserEmails(int userID, List<UserEmailModel> userEmails);
    }
}
using Axis.Model.Email;
using Axis.NotificationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.NotificationService.Email
{
    public interface IEmailService
    {
        bool Send(EmailMessageModel email, out string error);
    }
}

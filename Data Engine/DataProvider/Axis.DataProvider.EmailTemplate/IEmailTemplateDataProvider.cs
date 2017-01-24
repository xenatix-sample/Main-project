using Axis.Model.Common;
using Axis.Model.Common.EmailTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.DataProvider.EmailTemplate
{
    public interface IEmailTemplateDataProvider
    {
        Response<EmailTemplateModel> GetEmailTemplate(int templateID);
    }
}

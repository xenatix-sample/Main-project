using Axis.Model.Common;
using Axis.Model.Common.MessageTemplate;

namespace Axis.DataProvider.MessageTemplate
{
    public interface IMessageTemplateDataProvider
    {
        Response<MessageTemplateModel> GetMessageTemplate(int templateID);
    }
}

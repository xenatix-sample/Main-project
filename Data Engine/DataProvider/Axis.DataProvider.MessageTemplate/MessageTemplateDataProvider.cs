using Axis.Data.Repository;
using Axis.Model.Common;
using Axis.Model.Common.MessageTemplate;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.MessageTemplate
{
    /// <summary>
    ///
    /// </summary>
    public class MessageTemplateDataProvider : IMessageTemplateDataProvider
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTemplateDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public MessageTemplateDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the message template.
        /// </summary>
        /// <param name="templateID">The template identifier.</param>
        /// <returns></returns>
        public Response<MessageTemplateModel> GetMessageTemplate(int templateID)
        {
            SqlParameter templateIDParam = new SqlParameter("TemplateID", templateID);

            List<SqlParameter> procParams = new List<SqlParameter>() { templateIDParam, };

            var repository = unitOfWork.GetRepository<MessageTemplateModel>();
            var results = repository.ExecuteStoredProc("usp_GetMessageTemplate", procParams);

            return results;
        }
    }
}
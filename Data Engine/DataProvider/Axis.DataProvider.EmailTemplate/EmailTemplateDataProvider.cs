using Axis.Data.Repository;
using Axis.Model.Common;
using Axis.Model.Common.EmailTemplate;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.EmailTemplate
{
    /// <summary>
    ///
    /// </summary>
    public class EmailTemplateDataProvider : IEmailTemplateDataProvider
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTemplateDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public EmailTemplateDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the email template.
        /// </summary>
        /// <param name="templateID">The template identifier.</param>
        /// <returns></returns>
        public Response<EmailTemplateModel> GetEmailTemplate(int templateID)
        {
            SqlParameter templateIDParam = new SqlParameter("TemplateID", templateID);

            List<SqlParameter> procParams = new List<SqlParameter>() { templateIDParam, };

            var repository = unitOfWork.GetRepository<EmailTemplateModel>();
            var results = repository.ExecuteStoredProc("usp_GetEmailTemplate", procParams);

            return results;
        }
    }
}
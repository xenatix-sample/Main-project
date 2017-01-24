using System.Collections.Generic;

namespace Axis.Model.Common
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T>
    {
        /// <summary>
        /// Gets or sets the result code.
        /// </summary>
        /// <value>
        /// The result code.
        /// </value>
        public int ResultCode { get; set; }

        /// <summary>
        /// Gets or sets the result message.
        /// </summary>
        /// <value>
        /// The result message.
        /// </value>
        public string ResultMessage { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long ID { get; set; }

        /// <summary>
        /// Gets or sets the additional result.
        /// </summary>
        /// <value>
        /// The additional result.
        /// </value>
        public string AdditionalResult { get; set; }

        /// <summary>
        /// Gets or sets the data items.
        /// </summary>
        /// <value>
        /// The data items.
        /// </value>
        public List<T> DataItems { get; set; }

        /// <summary>
        /// Gets or sets the row affected.
        /// </summary>
        /// <value>
        /// The row affected.
        /// </value>
        public int RowAffected { get; set; }

        /// <summary>
        /// Gets or sets the server validation errors.
        /// </summary>
        /// <value>
        /// The server validation errors.
        /// </value>
        public List<ServerValidationError> ServerValidationErrors { get; set; }

        /// <summary>
        /// Clones the response.
        /// </summary>
        /// <typeparam name="O"></typeparam>
        /// <returns></returns>
        public Response<O> CloneResponse<O>()
        {
            var newResponse = new Response<O>();
            newResponse.ResultCode = this.ResultCode;
            newResponse.ResultMessage = this.ResultMessage;
            newResponse.ID = this.ID;
            newResponse.RowAffected = this.RowAffected;
            newResponse.AdditionalResult = this.AdditionalResult;
            newResponse.ServerValidationErrors = this.ServerValidationErrors;
            return newResponse;
        }

    }
}
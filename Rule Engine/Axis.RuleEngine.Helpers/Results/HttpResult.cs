using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Axis.RuleEngine.Helpers.Results
{
    public class HttpResult<T> : IHttpActionResult
    {
        public T Value { get; set; }
        public HttpRequestMessage Request { get; set; }

        public HttpResult(T value, HttpRequestMessage request)
        {
            Value = value;
            Request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Request.CreateResponse<T>(HttpStatusCode.OK, Value));
        }
    }
}
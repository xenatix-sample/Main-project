using Axis.Configuration;
using Axis.Logging;
using Axis.Model.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Service
{
    /// <summary>
    /// Revamped library to make http calls to rest service.
    /// </summary>
    public class CommunicationManager
    {
        /// <summary>
        /// Http client to communicate with rest service
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Axis.Logging Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Key to identify rest host in application configuration file
        /// </summary>
        private const string BaseUrlKey = "restService";

        /// <summary>
        /// Constructor of CommunicationManager class
        /// </summary>
        public CommunicationManager()
        {
            _logger = new Logger();
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationManager"/> class.
        /// </summary>
        /// <param name="tokenName">Name of the token.</param>
        /// <param name="securityToken">The security token.</param>
        public CommunicationManager(string tokenName, string securityToken)
        {
            _logger = new Logger();
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add(tokenName, securityToken);
        }

        /// <summary>
        /// Gets the base url of rest service. Url is taken from application configuration file
        /// </summary>
        /// <value>
        /// The base URL.
        /// </value>
        public string BaseUrl
        {
            get
            {
                return string.IsNullOrEmpty(UnitTestUrl) ? ApplicationSettings.ApiUrl : UnitTestUrl;
            }
        }

        /// <summary>
        /// Gets or sets the unit test URL.
        /// </summary>
        /// <value>
        /// The unit test URL.
        /// </value>
        public string UnitTestUrl { get; set; }

        /// <summary>
        /// Makes a simple HTTPGET request to rest service with parameters or query string
        /// </summary>
        /// <typeparam name="T1">Type of object in request body</typeparam>
        /// <param name="apiUri">Api Uri to connect to. Typically api/{Controller}/{Action}</param>
        /// <returns></returns>
        public T1 Get<T1>(string apiUri)
        {
            var uri = new Uri(
                String.Format("{0}/{1}", BaseUrl, apiUri)
                );

            var response = _httpClient.GetAsync(uri).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T1>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Makes a HTTPGET request to rest service having parameter id in url
        /// e.g http://{server}:{port}/api/{controller}/{id}
        /// </summary>
        /// <typeparam name="T1">Type of object expected in response</typeparam>
        /// <param name="id">Identifier of requested object</param>
        /// <param name="apiUri">Api Uri to connect to. Typically api/{Controller}/{Action}</param>
        /// <returns></returns>
        public T1 Get<T1>(int id, string apiUri)
        {
            var uri = new Uri(
                String.Format("{0}/{1}/{2}", BaseUrl, apiUri, id)
                );

            var response = _httpClient.GetAsync(uri).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T1>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="apiUri">The API URI.</param>
        /// <returns></returns>
        public T1 Get<T1>(int id, T1 parameters, string apiUri)
        {
            var urlEncodedParams = GetUrlEncodedHttpContent(parameters);
            var uri = new Uri(
                String.Format("{0}/{1}/{2}/{3}", BaseUrl, apiUri, id, urlEncodedParams)
                );

            var response = _httpClient.GetAsync(uri).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T1>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Gets the specified parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <param name="apiUri">The API URI.</param>
        /// <returns></returns>
        public T2 Get<T1, T2>(T1 parameters, string apiUri)
        {
            var urlEncodedParams = GetUrlEncodedHttpContent(parameters);
            var uri = new Uri(
                String.Format("{0}/{1}/{2}", BaseUrl, apiUri, urlEncodedParams)
                );

            var response = _httpClient.GetAsync(uri).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T2>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Gets the specified parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <param name="apiUri">The API URI.</param>
        /// <returns></returns>
        public T1 Get<T1>(NameValueCollection parameters, string apiUri)
        {
            var urlEncodedParams = GetUrlEncodedHttpContent(parameters);
            var uri = new Uri(
                String.Format("{0}/{1}/{2}", BaseUrl, apiUri, urlEncodedParams)
                );

            var response = _httpClient.GetAsync(uri).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T1>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Async wrapper to the Get function above
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <param name="apiUri">The API URI.</param>
        /// <returns></returns>
        public async Task<T1> GetAsync<T1>(NameValueCollection parameters, string apiUri)
        {
            return await Task.Factory.StartNew((() => Get<T1>(parameters, apiUri)), TaskCreationOptions.None);
        }

        /// <summary>
        /// Makes a HTTPGET request to rest service having id and query string in url
        /// </summary>
        /// <typeparam name="T1">Type of object expected in response</typeparam>
        /// <param name="id">Identifier of request object</param>
        /// <param name="parameters">Paramters to be added as query string</param>
        /// <param name="apiUri">Api Uri to connect to. Typically api/{Controller}/{Action}</param>
        /// <returns></returns>
        public T1 Get<T1>(int id, NameValueCollection parameters, string apiUri)
        {
            var urlEncodedParams = GetUrlEncodedHttpContent(parameters);
            var uri = new Uri(
                String.Format("{0}/{1}/{2}/{3}", BaseUrl, apiUri, id, urlEncodedParams)
                );

            var response = _httpClient.GetAsync(uri).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T1>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="apiUri">The API URI.</param>
        /// <returns></returns>
        public T2 Get<T1, T2>(T1 request, NameValueCollection parameters, string apiUri)
        {
            var uri = new Uri(
                String.Format("{0}/{1}/{2}{3}", BaseUrl, apiUri, GetUrlEncodedHttpContent(request), GetUrlEncodedHttpContent(parameters).Replace("?", "&"))
                );

            var response = _httpClient.GetAsync(uri).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T2>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Makes a HTTPPOST request to rest service and doesn't expect content in response
        /// </summary>
        /// <typeparam name="T1">Type of object in request body</typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <param name="apiUri">Api Uri to connect to. Typically api/{Controller}/{Action}</param>
        /// <returns></returns>
        public T1 Post<T1>(NameValueCollection parameters, string apiUri)
        {
            var urlEncodedParams = GetUrlEncodedHttpContent(parameters);
            var uri = new Uri(
                String.Format("{0}/{1}/{2}", BaseUrl, apiUri, urlEncodedParams)
                );

            var response = _httpClient.PostAsync(uri, null).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T1>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Posts the specified request.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="apiUri">The API URI.</param>
        /// <returns></returns>
        public string Post<T1>(T1 request, string apiUri)
        {
            var content = GetJsonHttpContent(request, GetJsonSerializationSettings());

            var uri = new Uri(
                String.Format("{0}/{1}", BaseUrl, apiUri)
                );

            var response = _httpClient.PostAsync(uri, content).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
            return response.Headers.Location != null ? response.Headers.Location.AbsoluteUri : string.Empty; ;
        }

        /// <summary>
        /// Makes a HTTPPOST request to rest service and expencts a non-empty response in return
        /// </summary>
        /// <typeparam name="T1">Type of object in request body</typeparam>
        /// <typeparam name="T2">Type of object in response body</typeparam>
        /// <param name="request">Object to be created on server</param>
        /// <param name="apiUri">Api Uri to connect to. Typically api/{Controller}/{id}</param>
        /// <returns></returns>
        public T2 Post<T1, T2>(T1 request, string apiUri)
        {
            var content = GetJsonHttpContent(request, GetJsonSerializationSettings());

            var uri = new Uri(
                String.Format("{0}/{1}", BaseUrl, apiUri)
                );

            var response = _httpClient.PostAsync(uri, content).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T2>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Posts the specified identifier.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="apiUri">The API URI.</param>
        /// <returns></returns>
        public T2 Post<T1, T2>(int id, T1 request, string apiUri)
        {
            var content = GetJsonHttpContent(request, GetJsonSerializationSettings());

            var uri = new Uri(
                String.Format("{0}/{1}/{2}", BaseUrl, apiUri, id)
                );

            var response = _httpClient.PostAsync(uri, content).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T2>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Posts the specified identifier.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="apiUri">The API URI.</param>
        /// <returns></returns>
        public T2 Post<T1, T2>(int id, T1 request, NameValueCollection parameters, string apiUri)
        {
            var content = GetJsonHttpContent(request, GetJsonSerializationSettings());

            var uri = new Uri(
                String.Format("{0}/{1}/{2}/{3}", BaseUrl, apiUri, id, GetUrlEncodedHttpContent(parameters))
                );

            var response = _httpClient.PostAsync(uri, content).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T2>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Makes a HTTPDELETE request to rest service
        /// </summary>
        /// <param name="id">Id of the object which requires deletion</param>
        /// <param name="apiUri">Api Uri to connect to. Typically api/{Controller}/{id}</param>
        public void Delete(long id, string apiUri)
        {
            var uri = new Uri(
                String.Format("{0}/{1}/{2}", BaseUrl, apiUri, id)
                );

            var response = _httpClient.DeleteAsync(uri).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Makes a HTTPDELETE request to rest service
        /// </summary>
        /// <param name="id">Id of the object which requires deletion</param>
        /// <param name="parameters">Parameters to be appended in url</param>
        /// <param name="apiUri">Api Uri to connect to. Typically api/{Controller}/{id}</param>
        public void Delete(int id, NameValueCollection parameters, string apiUri)
        {
            var uri = new Uri(
                String.Format("{0}/{1}/{2}/{3}", BaseUrl, apiUri, id, GetUrlEncodedHttpContent(parameters))
                );

            var response = _httpClient.DeleteAsync(uri).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Makes a HTTPDELETE request to rest service
        /// </summary>
        /// <param name="parameters">Parameters to be appended in url</param>
        /// <param name="apiUri">Api Uri to to connect to. Typically api/{Controller}/{id}</param>
        public void Delete(NameValueCollection parameters, string apiUri)
        {
            var uri = new Uri(
                String.Format("{0}/{1}/{2}", BaseUrl, apiUri, GetUrlEncodedHttpContent(parameters))
                );

            var response = _httpClient.DeleteAsync(uri).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Makes a HTTPDELETE request to rest service
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <param name="id">Id of the object which requires deletion</param>
        /// <param name="apiUri">Api Uri to connect to. Typically api/{Controller}/{id}</param>
        /// <returns></returns>
        /// <remarks>
        /// Added retrun type on 08/25/2015
        /// </remarks>
        public T1 Delete<T1>(long id, string apiUri)
        {
            var uri = new Uri(
                String.Format("{0}/{1}/{2}", BaseUrl, apiUri, id)
                );

            var response = _httpClient.DeleteAsync(uri).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T1>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Makes a HTTPDELETE request to rest service
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <param name="id">Id of the object which requires deletion</param>
        /// <param name="parameters">Parameters to be appended in url</param>
        /// <param name="apiUri">Api Uri to connect to. Typically api/{Controller}/{id}</param>
        /// <returns></returns>
        /// <remarks>
        /// Added retrun type on 08/25/2015
        /// </remarks>
        public T1 Delete<T1>(int id, NameValueCollection parameters, string apiUri)
        {
            var uri = new Uri(
                String.Format("{0}/{1}/{2}/{3}", BaseUrl, apiUri, id, GetUrlEncodedHttpContent(parameters))
                );

            var response = _httpClient.DeleteAsync(uri).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T1>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Makes a HTTPDELETE request to rest service
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <param name="parameters">Parameters to be appended in url</param>
        /// <param name="apiUri">Api Uri to to connect to. Typically api/{Controller}/{id}</param>
        /// <returns></returns>
        /// <remarks>
        /// Added retrun type on 08/25/2015
        /// </remarks>
        public T1 Delete<T1>(NameValueCollection parameters, string apiUri)
        {
            var uri = new Uri(
                String.Format("{0}/{1}/{2}", BaseUrl, apiUri, GetUrlEncodedHttpContent(parameters))
                );

            var response = _httpClient.DeleteAsync(uri).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T1>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Puts the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="apiUri">The API URI.</param>
        public void Put(NameValueCollection parameters, string apiUri)
        {
            var urlEncodedParams = GetUrlEncodedHttpContent(parameters);
            var uri = new Uri(
                String.Format("{0}/{1}/{2}", BaseUrl, apiUri, urlEncodedParams)
                );

            var response = _httpClient.PutAsync(uri, null).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Makes a HTTPPUT request to rest service
        /// </summary>
        /// <typeparam name="T1">Type of object in request body</typeparam>
        /// <param name="request">Updated object to be serialized</param>
        /// <param name="apiUri">Api Uri to connect to. Typically api/{Controller}/{Action}</param>
        public void Put<T1>(T1 request, string apiUri)
        {
            var content = GetJsonHttpContent(request, GetJsonSerializationSettings());

            var uri = new Uri(
                String.Format("{0}/{1}", BaseUrl, apiUri)
                );

            var response = _httpClient.PutAsync(uri, content).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Puts the specified identifier.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="apiUri">The API URI.</param>
        public void Put<T1>(int id, T1 request, string apiUri)
        {
            var content = GetJsonHttpContent(request, GetJsonSerializationSettings());

            var uri = new Uri(
                String.Format("{0}/{1}/{2}", BaseUrl, apiUri, id)
                );

            var response = _httpClient.PutAsync(uri, content).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Puts the specified identifier.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="apiUri">The API URI.</param>
        public void Put<T1>(int id, T1 request, NameValueCollection parameters, string apiUri)
        {
            var content = GetJsonHttpContent(request, GetJsonSerializationSettings());

            var uri = new Uri(
                String.Format("{0}/{1}/{2}/{3}", BaseUrl, apiUri, id, GetUrlEncodedHttpContent(parameters))
                );

            var response = _httpClient.PutAsync(uri, content).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Makes a HTTPPUT request to rest service and expects a non-empty content in response
        /// </summary>
        /// <typeparam name="T1">Type of object in request body</typeparam>
        /// <typeparam name="T2">Type of object expected in response body</typeparam>
        /// <param name="request">Object to be updated</param>
        /// <param name="apiUri">Api Uri to connect to. Typeically api/{Controller}/{Action}</param>
        /// <returns></returns>
        public T2 Put<T1, T2>(T1 request, string apiUri)
        {
            var content = GetJsonHttpContent(request, GetJsonSerializationSettings());

            var uri = new Uri(
                String.Format("{0}/{1}", BaseUrl, apiUri)
                );

            var response = _httpClient.PutAsync(uri, content).Result;

            HandleException(response);

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T2>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Gets default Json serialization settings
        /// </summary>
        /// <returns></returns>
        private JsonSerializerSettings GetJsonSerializationSettings()
        {
            return new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                //Un-indented keeps payload small
                //Formatting = Formatting.Indented,
            };
        }

        /// <summary>
        /// Serializes request body to Json
        /// </summary>
        /// <typeparam name="T1">Type of request object</typeparam>
        /// <param name="request">Request object to be sent to rest service</param>
        /// <param name="settings">Json serialization settings</param>
        /// <returns></returns>
        private HttpContent GetJsonHttpContent<T1>(T1 request, JsonSerializerSettings settings)
        {
            var jsonContent = JsonConvert.SerializeObject(request, settings);

            var content = new StringContent(jsonContent) as HttpContent;
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return content;
        }

        /// <summary>
        /// Serializes request to application/x-www-form-urlencoded format to be used in HTTPGET request
        /// </summary>
        /// <typeparam name="T1">Type of request object</typeparam>
        /// <param name="input">Request object to be converted to. Nested property types are ignored</param>
        /// <returns></returns>
        private string GetUrlEncodedHttpContent<T1>(T1 input)
        {
            var objType = input.GetType();
            var paramBuilder = new StringBuilder();
            const string paramFormat = "{0}={1}&";
            foreach (var property in objType.GetProperties().Where(property => !property.PropertyType.IsNested))
            {
                var value = property.GetValue(input);
                if (null != value)
                    paramBuilder.Append(string.Format(paramFormat, property.Name, value));
            }

            return paramBuilder.ToString().TrimEnd('&').Insert(0, "?");
        }

        /// <summary>
        /// Serializes parameters to application/x-www-form-urlencoded format to be used in HTTPGET request. Only first value will be used in case of multivalue key
        /// </summary>
        /// <param name="parameters">NameValueCollection of HTTPGET parameters</param>
        /// <returns></returns>
        private string GetUrlEncodedHttpContent(NameValueCollection parameters)
        {
            var paramBuilder = new StringBuilder();
            const string paramFormat = "{0}={1}&";

            foreach (var key in parameters.AllKeys)
            {
                var values = parameters.GetValues(key);
                if (values != null && values.Length > 0)
                    paramBuilder.AppendFormat(paramFormat, key, values[0]);
            }

            return paramBuilder.ToString().TrimEnd('&').Insert(0, "?");
        }

        /// <summary>
        /// Gets the header response.
        /// </summary>
        /// <param name="objHTTPResponse">The object HTTP response.</param>
        /// <returns></returns>
        private string GetHeaderResponse(HttpResponseMessage objHTTPResponse)
        {
            IEnumerable<string> errorLoggedHeader = null;
            var code = string.Empty;
            if (objHTTPResponse.Headers.TryGetValues("ErrorCode", out errorLoggedHeader))
            {
                code = errorLoggedHeader.First();
            }

            if (!string.IsNullOrEmpty(code))
            {
                _logger.Error(string.Format("ErrorCode: {0} - {1}", code, objHTTPResponse.ToString()));
                objHTTPResponse.Headers.Add("ErrorCode", code);
            }

            return code;
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <exception cref="XenatixException"></exception>
        private void HandleException(HttpResponseMessage response)
        {
            var headerResponse = GetHeaderResponse(response);

            if (!string.IsNullOrEmpty(headerResponse))
            {
                throw new XenatixException(headerResponse);
            }
        }

        //Commented out as we are getting some error and not sure if that is because of this code. 
        //~CommunicationManager()
        //{
        //    if (_httpClient != null)
        //    {
        //        _httpClient.Dispose();
        //    }
        //}
    }
}
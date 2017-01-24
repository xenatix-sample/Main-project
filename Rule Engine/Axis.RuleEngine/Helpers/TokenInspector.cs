using System.Diagnostics;
using Axis.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Configuration;
using Axis.Model;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.WebApi;
using Axis.Model.Account;
using Axis.RuleEngine.Account;
using Axis.Security;
using System.Web;
using Microsoft.Owin;
using Axis.RuleEngine.Security;
using Axis.Logging;

namespace Axis.RuleEngine.Service.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenInspector : DelegatingHandler
    {
        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>
        /// Returns <see cref="T:System.Threading.Tasks.Task`1" />. The task object representing the asynchronous operation.
        /// </returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string token_name = ConfigurationManager.AppSettings["Token"].ToString();

            if (request.Headers.Contains(token_name))
            {
                string encryptedToken = request.Headers.GetValues(token_name).First();

                try
                {
                    var token = new AccessTokenModel();

                    var accountService = (IAccountRuleEngine)request.GetDependencyScope().GetService(typeof(IAccountRuleEngine));

                    token = token.Decrypt(encryptedToken);
                    token.Token = encryptedToken;

                    var user = accountService.GetValidUserInfoByToken(token);

                    var requestIPMatchesTokenIP = IsValidServerIP(token, request);

                    if (user == null || !requestIPMatchesTokenIP)
                    {
                        HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "InvalidTokenAccess");
                        return Task.FromResult(reply);
                    }
                    else
                    {
                        WebSecurity.SignIn(user, token);
                    }
                }
                catch (Exception exc)
                {
                    var _logger = new Logger(true);
                    _logger.Error(exc);
                    HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "InvalidToken");
                    return Task.FromResult(reply);
                }
            }
            else
            {
                HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "TokenMissing");
                return Task.FromResult(reply);
            }

            return base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Validates the client ip.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="request">The request.</param>
        /// <returns>True/False</returns>
        private bool IsValidServerIP(AccessTokenModel token, HttpRequestMessage request)
        {
            var IP_Address = request.GetClientIP();

            bool ValidIP = false;
            var serverResourceService = (IAccountRuleEngine)request.GetDependencyScope().GetService(typeof(IAccountRuleEngine));
            var serverResource = serverResourceService.IsValidServerIP(IP_Address);
            //if client IP exist in database then we will get integer > 0.
            if (serverResource.ID > 0)
            {
                ValidIP = true;
            }
            return ValidIP;
        }
    }
}
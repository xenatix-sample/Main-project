using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Axis.Model.Account;

namespace Axis.Security
{
    /// <summary>
    /// 
    /// </summary>
    public static class TokenManager
    {
        /// <summary>
        /// Gets the client ip.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static string GetClientIP(this HttpRequestMessage request)
        {
            return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
        }

        /// <summary>
        /// To the dictionary.
        /// </summary>
        /// <param name="keyValue">The key value.</param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary(this string keyValue)
        {
            return keyValue.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                          .Select(part => part.Split('='))
                          .ToDictionary(split => split[0], split => split[1]);
        }

        /// <summary>
        /// Encrypts the specified access token.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <returns></returns>
        public static string Encrypt(this AccessTokenModel accessToken)
        {
            return Cryptography.Encrypt(accessToken.ToTokenString(), "");
        }

        /// <summary>
        /// To the token string.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <returns></returns>
        public static string ToTokenString(this AccessTokenModel accessToken)
        {
            return String.Format("UserName={0};ClientIP={1};UserId={2};SessionID={3};GeneratedOn={4};ExpirationDate={5}", accessToken.UserName, accessToken.ClientIP, accessToken.UserId, accessToken.SessionID, accessToken.GeneratedOn, accessToken.ExpirationDate);
        }

        /// <summary>
        /// Decrypts the specified access token.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="encryptedToken">The encrypted token.</param>
        /// <returns></returns>
        public static AccessTokenModel Decrypt(this AccessTokenModel accessToken, string encryptedToken)
        {
            string decrypted = Cryptography.Decrypt(encryptedToken, "");

            //Splitting it to dictionary
            Dictionary<string, string> dictionary = decrypted.ToDictionary();
            return new AccessTokenModel()
            {
                UserId = Convert.ToInt32(dictionary["UserId"]),
                UserName = dictionary["UserName"],
                ClientIP = dictionary["ClientIP"],                 
                SessionID = dictionary["SessionID"],
                GeneratedOn = Convert.ToDateTime(dictionary["GeneratedOn"]),
                ExpirationDate = Convert.ToDateTime(dictionary["ExpirationDate"])
            };
        }
    }
}
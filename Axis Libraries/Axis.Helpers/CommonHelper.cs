using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Axis.Helpers
{
    public class CommonHelper
    {
        /// <summary>
        /// Ensures the subscriber email or throw.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public static string EnsureSubscriberEmailOrThrow(string email)
        {
            string output = EnsureNotNull(email);
            output = output.Trim();
            output = EnsureMaximumLength(output, 255);

            if (!IsValidEmail(output))
            {
                throw new Exception("Email is not valid.");
            }

            return output;
        }

        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public static bool IsValidEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            var result = Regex.IsMatch(email, "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase);
            return result;
        }

        /// <summary>
        /// Generate random digit code
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>Result string</returns>
        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            string str = string.Empty;
            for (int i = 0; i < length; i++)
                str = String.Concat(str, random.Next(10).ToString());
            return str;
        }

        /// <summary>
        /// Returns an random interger number within a specified rage
        /// </summary>
        /// <param name="min">Minimum number</param>
        /// <param name="max">Maximum number</param>
        /// <returns>Result</returns>
        public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
        {
            var randomNumberBuffer = new byte[10];
            new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
            return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
        }

        /// <summary>
        /// Ensure that a string doesn't exceed maximum allowed length
        /// </summary>
        /// <param name="str">Input string</param>
        /// <param name="maxLength">Maximum length</param>
        /// <param name="postfix">A string to add to the end if the original string was shorten</param>
        /// <returns>Input string if its lengh is OK; otherwise, truncated input string</returns>
        public static string EnsureMaximumLength(string str, int maxLength, string postfix = null)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            if (str.Length > maxLength)
            {
                var result = str.Substring(0, maxLength);
                if (!String.IsNullOrEmpty(postfix))
                {
                    result += postfix;
                }
                return result;
            }

            return str;
        }

        /// <summary>
        /// Ensures that a string only contains numeric values
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Input string with only numeric values, empty string if input is null/empty</returns>
        public static string EnsureNumericOnly(string str)
        {
            if (String.IsNullOrEmpty(str))
                return string.Empty;

            var result = new StringBuilder();
            foreach (char c in str)
            {
                if (Char.IsDigit(c))
                    result.Append(c);
            }
            return result.ToString();
        }

        /// <summary>
        /// Ensure that a string is not null
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Result</returns>
        public static string EnsureNotNull(string str)
        {
            if (str == null)
                return string.Empty;

            return str;
        }

        /// <summary>
        /// Indicates whether the specified strings are null or empty strings
        /// </summary>
        /// <param name="stringsToValidate">Array of strings to validate</param>
        /// <returns>Boolean</returns>
        public static bool AreNullOrEmpty(params string[] stringsToValidate)
        {
            bool result = false;
            Array.ForEach(stringsToValidate, str =>
            {
                if (string.IsNullOrEmpty(str)) result = true;
            });
            return result;
        }

        /// <summary>
        /// Compare two arrasy
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="a1">Array 1</param>
        /// <param name="a2">Array 2</param>
        /// <returns>Result</returns>
        public static bool ArraysEqual<T>(T[] a1, T[] a2)
        {
            //also see Enumerable.SequenceEqual(a1, a2);
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i])) return false;
            }
            return true;
        }

        private static AspNetHostingPermissionLevel? _trustLevel;
        /// <summary>
        /// Finds the trust level of the running application (http://blogs.msdn.com/dmitryr/archive/2007/01/23/finding-out-the-current-trust-level-in-asp-net.aspx)
        /// </summary>
        /// <returns>The current trust level.</returns>
        public static AspNetHostingPermissionLevel GetTrustLevel()
        {
            if (!_trustLevel.HasValue)
            {
                //set minimum
                _trustLevel = AspNetHostingPermissionLevel.None;

                //determine maximum
                foreach (AspNetHostingPermissionLevel trustLevel in new[] {
                                AspNetHostingPermissionLevel.Unrestricted,
                                AspNetHostingPermissionLevel.High,
                                AspNetHostingPermissionLevel.Medium,
                                AspNetHostingPermissionLevel.Low,
                                AspNetHostingPermissionLevel.Minimal 
                            })
                {
                    try
                    {
                        new AspNetHostingPermission(trustLevel).Demand();
                        _trustLevel = trustLevel;
                        break; //we've set the highest permission we can
                    }
                    catch (System.Security.SecurityException)
                    {
                        continue;
                    }
                }
            }
            return _trustLevel.Value;
        }

        /// <summary>
        /// Convert enum for front-end
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Converted string</returns>
        public static string ConvertEnum(string str)
        {
            string result = string.Empty;
            char[] letters = str.ToCharArray();
            foreach (char c in letters)
                if (c.ToString() != c.ToString().ToLower())
                    result += " " + c.ToString();
                else
                    result += c.ToString();
            return result;
        }

        public static string[] GetPermissionAttribute<T>(string methodName)
        {
            var dnAttribute = typeof(T).GetCustomAttributes(
                typeof(PermissionAttribute), true
            ).FirstOrDefault() as PermissionAttribute;

            var dnAttribute1 = (PermissionAttribute)typeof(T).GetMethod(methodName).GetCustomAttributes(typeof(PermissionAttribute), true)[0];

            if (dnAttribute != null)
            {
                return new[] { dnAttribute.Module, dnAttribute.Feature, dnAttribute1.Action };
            }
            return null;
        }

        /// <summary>
        /// Checks if a string is alphanumeric without spaces and falls within a length range
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static bool IsAlphaNumericWithLengthRange(string str, int minLength, int maxLength)
        {
            string pattern = string.Format("^[a-zA-Z0-9]{0},{1}$", "{" + minLength.ToString(), maxLength.ToString() + "}");

            if (str == null)
                return false;

            return Regex.IsMatch(str, pattern);
        }

        public static bool IsADUserNameValid(string str)
        {
            string pattern = @"[^[\]:;|=+*?<>/\\,]+";

            if (str == null)
                return false;

            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// Checks if a string contains alpha characters with potential spaces and falls within a length range
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static bool IsAlphaAndSpacesOnlyWithLengthRange(string str, int minLength, int maxLength)
        {
            string pattern = string.Format(@"^[a-zA-Z\s-.]{0},{1}$", "{" + minLength.ToString(), maxLength.ToString() + "}");

            if (str == null)
                return false;

            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// This will create a random string of the length passed in. This can be used to create random passwords.
        /// </summary> 
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomString(int length)
        {
            char[] AvailableCharacters = {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 
                'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 
                'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'
              };

            char[] identifier = new char[length];
            byte[] randomData = new byte[length];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomData);
            }

            for (int idx = 0; idx < identifier.Length; idx++)
            {
                int pos = randomData[idx] % AvailableCharacters.Length;
                identifier[idx] = AvailableCharacters[pos];
            }

            return new string(identifier);
        }
    }

    public static class Tools
    {
        private static readonly Type[] WriteTypes = new[] {
            typeof(string), typeof(DateTime), typeof(Enum), 
            typeof(decimal), typeof(Guid),
        };
        public static bool IsSimpleType(this Type type)
        {
            return type.IsPrimitive || WriteTypes.Contains(type);
        }
        public static XElement ToXml(this object input)
        {
            return input.ToXml(null);
        }
        public static XElement ToXml(this object input, string element)
        {
            if (input == null) return null;

            if (string.IsNullOrEmpty(element))
                element = "object";
            element = XmlConvert.EncodeName(element);
            var ret = new XElement(element);

            var type = input.GetType();
            var props = type.GetProperties();

            var elements = from prop in props
                           let name = XmlConvert.EncodeName(prop.Name)
                           let val = prop.GetValue(input, null)
                           let value = prop.PropertyType.IsSimpleType()
                               ? new XElement(name, val)
                               : val.ToXml(name)
                           where value != null
                           select value;

            ret.Add(elements);

            return ret;
        }
    }

    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts an anonymous type to an XElement.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Returns the object as it's XML representation in an XElement.</returns>
        public static XElement ToXml2(this object input)
        {
            return input.ToXml2(null);
        }

        /// <summary>
        /// Converts an anonymous type to an XElement.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="element">The element name.</param>
        /// <returns>Returns the object as it's XML representation in an XElement.</returns>
        public static XElement ToXml2(this object input, string element)
        {
            return _ToXml(input, element);
        }

        private static XElement _ToXml(object input, string element, int? arrayIndex = null, string arrayName = null)
        {
            if (input == null) return null;

            if (String.IsNullOrEmpty(element))
            {
                var name = input.GetType().Name;
                element = name.Contains("AnonymousType")
                    ? "Object"
                    : arrayIndex != null
                        ? arrayName + "_" + arrayIndex
                        : name;
            }

            element = XmlConvert.EncodeName(element);
            var ret = new XElement(element);

            var type = input.GetType();
            var props = type.GetProperties();

            var elements = props.Select(p =>
            {
                var pType = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                var name = XmlConvert.EncodeName(p.Name);
                var val = pType.IsArray ? "array" : p.GetValue(input, null);
                var value = pType.IsEnumerable()
                    ? GetEnumerableElements(p, (IEnumerable)p.GetValue(input, null))
                    : pType.IsSimpleType2() || pType.IsEnum
                        ? new XElement(name, val)
                        : val.ToXml2(name);
                return value;
            })
                .Where(v => v != null);

            ret.Add(elements);

            return ret;
        }

        #region helpers

        private static XElement GetEnumerableElements(PropertyInfo info, IEnumerable input)
        {
            var name = XmlConvert.EncodeName(info.Name);

            var rootElement = new XElement(name);

            int i = 0;
            foreach (var v in input)
            {
                XElement childElement = v.GetType().IsSimpleType2() || v.GetType().IsEnum ? new XElement(name + "_" + i, v) : _ToXml(v, null, i, name);
                rootElement.Add(childElement);
                i++;
            }
            return rootElement;
        }

        private static readonly Type[] WriteTypes = new[] {
            typeof(string), typeof(DateTime), typeof(Enum), 
            typeof(decimal), typeof(Guid),
        };
        public static bool IsSimpleType2(this Type type)
        {
            return type.IsPrimitive || WriteTypes.Contains(type);
        }

        private static readonly Type[] FlatternTypes = new[] {
            typeof(string)
        };
        public static bool IsEnumerable(this Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type) && !FlatternTypes.Contains(type);
        }

        #endregion
    }
}

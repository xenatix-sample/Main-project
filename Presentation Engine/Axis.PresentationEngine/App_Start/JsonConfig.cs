using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Axis.PresentationEngine.App_Start
{
    /// <summary>
    ///
    /// </summary>
    public class JsonConfig
    {
        /// <summary>
        /// Setups this instance.
        /// </summary>
        public static void Setup()
        {
            //find the default JsonVAlueProviderFactory
            JsonValueProviderFactory jsonValueProviderFactory = null;

            foreach (var factory in ValueProviderFactories.Factories)
            {
                if (factory is JsonValueProviderFactory)
                {
                    jsonValueProviderFactory = factory as JsonValueProviderFactory;
                }
            }

            //remove the default JsonVAlueProviderFactory
            if (jsonValueProviderFactory != null) ValueProviderFactories.Factories.Remove(jsonValueProviderFactory);

            //add the custom one
            ValueProviderFactories.Factories.Add(new LargeJsonValueProviderFactory());
        }
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class LargeJsonValueProviderFactory : ValueProviderFactory
    {
        /// <summary>
        /// Adds to backing store.
        /// </summary>
        /// <param name="backingStore">The backing store.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="value">The value.</param>
        private static void AddToBackingStore(Dictionary<string, object> backingStore, string prefix, object value)
        {
            IDictionary<string, object> d = value as IDictionary<string, object>;
            if (d != null)
            {
                foreach (KeyValuePair<string, object> entry in d)
                {
                    AddToBackingStore(backingStore, MakePropertyKey(prefix, entry.Key), entry.Value);
                }
                return;
            }

            IList l = value as IList;
            if (l != null)
            {
                for (int i = 0; i < l.Count; i++)
                {
                    AddToBackingStore(backingStore, MakeArrayKey(prefix, i), l[i]);
                }
                return;
            }

            // primitive
            backingStore[prefix] = value;
        }

        /// <summary>
        /// Gets the deserialized object.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <returns></returns>
        private static object GetDeserializedObject(ControllerContext controllerContext)
        {
            if (!controllerContext.HttpContext.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
            {
                // not JSON request
                return null;
            }

            StreamReader reader = new StreamReader(controllerContext.HttpContext.Request.InputStream);
            string bodyText = reader.ReadToEnd();
            if (String.IsNullOrEmpty(bodyText))
            {
                // no JSON data
                return null;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue; //increase MaxJsonLength.  This could be read in from the web.config if you prefer
            object jsonData = serializer.DeserializeObject(bodyText);
            return jsonData;
        }

        /// <summary>
        /// Returns a value-provider object for the specified controller context.
        /// </summary>
        /// <param name="controllerContext">An object that encapsulates information about the current HTTP request.</param>
        /// <returns>
        /// A value-provider object.
        /// </returns>
        /// <exception cref="ArgumentNullException">controllerContext</exception>
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }

            object jsonData = GetDeserializedObject(controllerContext);
            if (jsonData == null)
            {
                return null;
            }

            Dictionary<string, object> backingStore = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            AddToBackingStore(backingStore, String.Empty, jsonData);
            return new DictionaryValueProvider<object>(backingStore, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Makes the array key.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private static string MakeArrayKey(string prefix, int index)
        {
            return prefix + "[" + index.ToString(CultureInfo.InvariantCulture) + "]";
        }

        /// <summary>
        /// Makes the property key.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        private static string MakePropertyKey(string prefix, string propertyName)
        {
            return (String.IsNullOrEmpty(prefix)) ? propertyName : prefix + "." + propertyName;
        }
    }
}
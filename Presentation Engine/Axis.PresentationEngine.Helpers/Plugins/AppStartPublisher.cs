using System;
using System.Linq;
using Axis.Helpers.Infrastructure;
using Axis.PluginsEngine;

namespace Axis.PresentationEngine.Helpers.Plugins
{
    /// <summary>
    /// App Start configuration publisher
    /// </summary>
    public class AppStartPublisher : IAppStartPublisher
    {
        protected readonly ITypeFinder typeFinder;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="typeFinder"></param>
        public AppStartPublisher(ITypeFinder typeFinder)
        {
            this.typeFinder = typeFinder;
        }

        /// <summary>
        /// Find a plugin descriptor by some type which is located into its assembly
        /// </summary>
        /// <param name="providerType">Provider type</param>
        /// <returns>Plugin descriptor</returns>
        protected virtual PluginDescriptor FindPlugin(Type providerType)
        {
            if (providerType == null)
                throw new ArgumentNullException("providerType");

            return
                PluginManager.ReferencedPlugins.Where(plugin => plugin.ReferencedAssembly != null)
                    .FirstOrDefault(plugin => plugin.ReferencedAssembly.FullName == providerType.Assembly.FullName);
        }
    }
}

using System;
using System.Linq;
using Axis.Helpers.Infrastructure;
using System.Web.Optimization;
using Axis.PresentationEngine.Helpers.Plugins;

namespace Axis.PresentationEngine.Helpers.Bundles
{
    /// <summary>
    /// Bundle publisher
    /// </summary>
    public class BundlePublisher : AppStartPublisher, IBundlePublisher
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="typeFinder"></param>
        public BundlePublisher(ITypeFinder typeFinder) : base(typeFinder)
        {
        }

        /// <summary>
        /// Register bundles
        /// </summary>
        /// <param name="bundles">Bundles</param>
        public virtual void RegisterBundles(BundleCollection bundles)
        {
            var bundleProviderTypes = typeFinder.FindClassesOfType<IBundleProvider>();
            var bundleProviders = (from providerType in bundleProviderTypes
                let plugin = FindPlugin(providerType)
                where plugin != null && plugin.Installed
                orderby plugin.DisplayOrder //the registration plugin must be called first so that other plugins can inject files into registration bundles
                select Activator.CreateInstance(providerType) as IBundleProvider).ToList();
            bundleProviders.ForEach(rp => rp.RegisterBundles(bundles));
        }
    }
}

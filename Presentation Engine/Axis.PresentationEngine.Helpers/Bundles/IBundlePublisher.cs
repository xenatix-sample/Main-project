using System.Web.Optimization;

namespace Axis.PresentationEngine.Helpers.Bundles
{
    /// <summary>
    /// Bundle publisher
    /// </summary>
    public interface IBundlePublisher
    {
        /// <summary>
        /// Register bundles
        /// </summary>
        /// <param name="bundles">Bundles</param>
        void RegisterBundles(BundleCollection bundles);
    }
}

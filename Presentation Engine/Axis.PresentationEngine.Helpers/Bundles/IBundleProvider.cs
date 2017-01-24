using System.Web.Optimization;

namespace Axis.PresentationEngine.Helpers.Bundles
{
    public interface IBundleProvider
    {
        void RegisterBundles(BundleCollection bundles);
    }
}

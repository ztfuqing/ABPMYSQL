using System.Web.Optimization;

namespace Fq.Web.Bundling
{
    public static class CommonBundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(
                new StyleBundle("~/Bundles/Common/css")
                    .IncludeDirectory("~/Common/Styles", "*.css", true)
                    .ForceOrdered()
                );

            bundles.Add(
                new ScriptBundle("~/Bundles/Common/js")
                    .IncludeDirectory("~/Common/Scripts", "*.js", true)
                    .ForceOrdered()
                );
        }
    }
}
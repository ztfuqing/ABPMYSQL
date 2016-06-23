using System.Web.Optimization;
using Fq.Web.Bundling;

namespace Fq.Web.Front.Startup
{
    public static class FrontBundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //LIBRARIES

            AddFrontCssLibs(bundles);
            AddFrontJsLibs(bundles);

            //METRONIC

            AddFrontMetrinicCss(bundles);
            AddFrontMetrinicJs(bundles);

            //APPLICATION

            AddFrontCssAndJs(bundles);
        }


        private static void AddFrontCssLibs(BundleCollection bundles)
        {
            bundles.Add(
                new StyleBundle("~/Bundles/Front/libs/css")
                    .Include(StylePaths.FontAwesome, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.Bootstrap, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.SweetAlert)
                    .Include(StylePaths.Toastr)
                    .Include(StylePaths.Angular_Ui_Grid, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .ForceOrdered()
                );
        }

        private static void AddFrontJsLibs(BundleCollection bundles)
        {
            bundles.Add(
                           new ScriptBundle("~/Bundles/Front/libs/js")
                               .Include(
                                   ScriptPaths.Json2,
                                   ScriptPaths.JQuery,
                                   ScriptPaths.JQuery_Migrate,
                                   ScriptPaths.Bootstrap,
                                   ScriptPaths.Bootstrap_Hover_Dropdown,
                                   ScriptPaths.JQuery_Slimscroll,
                                   ScriptPaths.JQuery_BlockUi,
                                   ScriptPaths.JQuery_Cookie,
                                   ScriptPaths.JQuery_Uniform,
                                   ScriptPaths.SpinJs,
                                   ScriptPaths.SpinJs_JQuery,
                                   ScriptPaths.SweetAlert,
                                   ScriptPaths.Toastr,

                                   ScriptPaths.MomentJs,
                                   ScriptPaths.Bootstrap_Select,
                                   ScriptPaths.Underscore,
                                   ScriptPaths.Angular,
                                   ScriptPaths.Angular_Ui_Utils,
                                   ScriptPaths.Angular_Sanitize,
                                   ScriptPaths.Angular_Ui_Router,
                                   ScriptPaths.Angular_OcLazyLoad,
                                   ScriptPaths.Angular_Ui_Bootstrap_Tpls,
                                   ScriptPaths.Angular_Ui_Grid,
                                   ScriptPaths.Angular_Moment,
                                   ScriptPaths.Abp,
                                   ScriptPaths.Abp_JQuery,
                                   ScriptPaths.Abp_Toastr,
                                   ScriptPaths.Abp_BlockUi,
                                   ScriptPaths.Abp_SpinJs,
                                   ScriptPaths.Abp_SweetAlert,
                                   ScriptPaths.Abp_Angular
                               ).ForceOrdered()
                           );
        }

        private static void AddFrontCssAndJs(BundleCollection bundles)
        {
            bundles.Add(
                new StyleBundle("~/Bundles/Front/css")
                    .IncludeDirectory("~/Front", "*.css", true)
                    .ForceOrdered()
                );

            bundles.Add(
                new ScriptBundle("~/Bundles/Front/js")
                    .IncludeDirectory("~/Front", "*.js", true)
                    .ForceOrdered()
                );
        }

        private static void AddFrontMetrinicCss(BundleCollection bundles)
        {
            bundles.Add(
                new StyleBundle("~/Bundles/Front/metronic/css")
                    .Include("~/metronic/global/css/components.css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include("~/metronic/global/css/plugins.css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include("~/metronic/frontend/css/style.css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include("~/metronic/frontend/css/themes/default.css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .ForceOrdered()
                );
        }

        private static void AddFrontMetrinicJs(BundleCollection bundles)
        {
            bundles.Add(
              new ScriptBundle("~/Bundles/Front/metronic/js")
                  .Include(
                      "~/metronic/global/scripts/app.js",
                      "~/metronic/frontend/scripts/layout.js"
                  ).ForceOrdered()
              );
        }
    }
}
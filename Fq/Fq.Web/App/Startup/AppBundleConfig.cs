using System.Web.Optimization;
using Fq.Web.Bundling;

namespace Fq.Web.App.Startup
{
    public static class AppBundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //LIBRARIES

            AddAppCssLibs(bundles);
            AddAppJsLibs(bundles);

            //METRONIC

            AddAppMetrinicCss(bundles);
            AddAppMetrinicJs(bundles);

            //APPLICATION

            AddApplicationCssAndJs(bundles);
        }


        private static void AddAppCssLibs(BundleCollection bundles)
        {
            bundles.Add(
                new StyleBundle("~/Bundles/App/libs/css")
                    .Include(StylePaths.FontAwesome, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.Simple_Line_Icons, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.Bootstrap, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.JQuery_Uniform, new CssRewriteUrlWithVirtualDirectoryTransform())
                     .Include(StylePaths.Ueditor, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.JsTree, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.SweetAlert)
                    .Include(StylePaths.Toastr)
                    .Include(StylePaths.Angular_Ui_Grid, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.Bootstrap_DateRangePicker)
                    .Include(StylePaths.Bootstrap_Select)
                    .Include(StylePaths.Bootstrap_Switch)
                    .Include(StylePaths.Bootstrap_DateTimePicker)
                    .ForceOrdered()
                );
        }

        private static void AddAppJsLibs(BundleCollection bundles)
        {
            bundles.Add(
                           new ScriptBundle("~/Bundles/App/libs/js")
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
                                   ScriptPaths.JsTree,
                                   ScriptPaths.Bootstrap_Switch,
                                   ScriptPaths.SpinJs,
                                   ScriptPaths.SpinJs_JQuery,
                                   ScriptPaths.SweetAlert,
                                   ScriptPaths.Toastr,
                                   ScriptPaths.MomentJs,
                                   ScriptPaths.SignalR,

                                   ScriptPaths.Bootstrap_DateRangePicker,
                                   ScriptPaths.Bootstrap_DateTimePicker,
                                   ScriptPaths.Bootstrap_Select,
                                   ScriptPaths.Underscore,
                                   ScriptPaths.Angular,
                                   ScriptPaths.Angular_Sanitize,
                                   ScriptPaths.Angular_Ui_Router,
                                   ScriptPaths.Angular_Ui_Utils,
                                   ScriptPaths.Angular_Ui_Bootstrap_Tpls,
                                   ScriptPaths.Angular_Ui_Grid,
                                   ScriptPaths.Angular_OcLazyLoad,
                                   ScriptPaths.Angular_File_Upload,
                                   ScriptPaths.Angular_DateRangePicker,
                                   ScriptPaths.Angular_Moment,
                                   ScriptPaths.Angular_Bootstrap_Switch,
                                   ScriptPaths.UEditor_Cfg,
                                   ScriptPaths.UEditor,
                                   ScriptPaths.UEditor_Lang,
                                   ScriptPaths.Angular_UEditor,
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

        private static void AddApplicationCssAndJs(BundleCollection bundles)
        {
            bundles.Add(
                new StyleBundle("~/Bundles/App/css")
                    .IncludeDirectory("~/App", "*.css", true)
                    .ForceOrdered()
                );

            bundles.Add(
                new ScriptBundle("~/Bundles/App/js")
                    .IncludeDirectory("~/App", "*.js", true)
                    .ForceOrdered()
                );
        }

        private static void AddAppMetrinicCss(BundleCollection bundles)
        {
            bundles.Add(
                new StyleBundle("~/Bundles/App/metronic/css")
                    .Include("~/metronic/global/css/components.css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include("~/metronic/global/css/plugins.css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include("~/metronic/admin/css/layout.css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include("~/metronic/admin/css/themes/darkblue.css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .ForceOrdered()
                );
        }

        private static void AddAppMetrinicJs(BundleCollection bundles)
        {
            bundles.Add(
              new ScriptBundle("~/Bundles/App/metronic/js")
                  .Include(
                      "~/metronic/global/scripts/app.js",
                      "~/metronic/admin/scripts/layout.js"
                  ).ForceOrdered()
              );
        }
    }
}
using System.Web;
using System.Web.Optimization;

namespace CustomerPortal
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Theme/css").Include(
                   "~/Theme/css/bootstrap.min.css",
                    "~/Theme/css/font-awesome.min.css",
                       "~/Theme/css/simplebar.min.css",
                   "~/Theme/css/main.css?v=1.0"));
            bundles.Add(new ScriptBundle("~/bundles/CustomTheme").Include(
                  "~/Theme/scripts/jquery-3.2.1.min.js",
                   "~/Theme/scripts/popper.min.js",
                      "~/Theme/scripts/bootstrap.min.js",
                      "~/Theme/scripts/simplebar.min.js",
                         "~/Theme/scripts/jquery.toast.min.js",
                  "~/Theme/scripts/main.js"));
        }
    }
}

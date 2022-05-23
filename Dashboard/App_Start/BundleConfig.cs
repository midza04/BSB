using System.Web;
using System.Web.Optimization;

namespace Dashboard
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/dashboard").Include(
                "~/Scripts/dashboard/jquery.js",
                "~/Scripts/dashboard/moment.js",
                "~/Scripts/dashboard/collapse.js",
                "~/Scripts/dashboard/transition.js",
                "~/Scripts/dashboard/bootstrap-select.js",
                "~/Scripts/dashboard/bootstrap.js",
                "~/Scripts/dashboard/jquery.dataTables.js",
                "~/Scripts/dashboard/dataTables.bootstrap.js",
                "~/Scripts/dashboard/Chart.js",
                "~/Scripts/dashboard/bootstrap-datetimepicker.js",                
                "~/Scripts/dashboard/fileinput.js",
                "~/Scripts/dashboard/chartData.js",
                
                "~/Scripts/dashboard/main.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/dashboardMain").Include(
                "~/Scripts/jquery.signalR-2.2.1.js",
                "~/signalr/hubs",
                "~/Scripts/dashboard/dashboard.realtime.js"                        
            ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/dashboard").Include(
                    "~/Content/themes/dashboard/font-awesome.css",
                    "~/Content/themes/dashboard/font-awesome.css",
                    "~/Content/themes/dashboard/bootstrap.css",
                    "~/Content/themes/dashboard/dataTables.bootstrap.css",
                    "~/Content/themes/dashboard/bootstrap-social.css",
                    "~/Content/themes/dashboard/bootstrap-select.css",
                    "~/Content/themes/dashboard/fileinput.css",
                    "~/Content/themes/dashboard/bootstrap-datetimepicker.css",
                    "~/Content/themes/dashboard/awesome-bootstrap-checkbox.css",
                    "~/Content/themes/dashboard/style.css"
                ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}
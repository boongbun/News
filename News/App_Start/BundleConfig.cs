using System.Web;
using System.Web.Optimization;

namespace News
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/js/core/jquery.min.js",
                        "~/Content/js/core/popper.min.js",
                        "~/Content/js/core/bootstrap-material-design.min.js",
                        "~/Content/js/plugins/moment.min.js",
                        "~/Content/js/plugins/bootstrap-datetimepicker.js",
                        //slider
                        "~/Content/js/plugins/nouislider.min.js",
                        "~/Content/js/plugins/buttons.js",
                        //Plugin for Sharrre btn
                        "~/Content/js/plugins/jquery.sharrre.js",
                        "~/Content/js/plugins/bootstrap-tagsinput.js",
                        //Select, full documentation here: silviomoreto.github.io/bootstrap-select
                        "~/Content/js/plugins/bootstrap-selectpicker.js",
                        //Fileupload, full documentation here: /*www.jasny.net/bootstrap/javascript/#fileinput*/
                        "~/Content/js/plugins/jasny-bootstrap.min.js",
                        //Small Gallery in Product Page
                        "~/Content/js/plugins/jquery.flexisel.js",
                        "~/Content/demo/modernizr.js",
                        "~/Content/demo/vertical-nav.js",
                        "~/Content/js/plugins/bootstrap-notify.js",
                        "~/Content/js/plugins/chartist.min.js",
                        "~/Content/js/plugins/chartist-plugin-tooltip.js",
                        "~/Content/js/material-kit.min.js",
                        "~/Content/js/common/knockout-3.5.0rc.js",
                        "~/Content/js/common/knockout.mapping-latest.js",
                        "~/Content/js/common/common.js",
                        "~/Scripts/jquery.signalR-2.4.0.min.js"

            ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/material-kit.min.css",
                      "~/Content/css/chartist-plugin-tooltip.css",
                      "~/Content/demo/vertical-nav.css"
                      ));


        }
    }
}

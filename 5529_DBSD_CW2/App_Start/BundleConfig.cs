using System.Web;
using System.Web.Optimization;

namespace _4837_DBSD_CW2
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/Header").Include(
                       "~/Scripts/modernizr-2.6.2.min.js"));

            bundles.Add(new StyleBundle("~/Content/HeaderCss").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.css",
                      "~/Content/chocolat.css",
                      "~/Content/easy-responsive-tabs.css",
                      "~/Content/flexslider.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/style.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datepicker/js").Include(
                "~/Scripts/bootstrap-datepicker.js"));
            //Bootstrap datepicker css
            bundles.Add(new StyleBundle("~/bundles/bootstrap-datepicker/css").Include(
                "~/Content/bootstrap-datepicker.css"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/Bottom").Include(
                       "~/Scripts/jquery-2.1.4.min.js",
                        "~/Scripts/jqBootstrapValidation.js",
                        "~/Scripts/contact_me.js",
                        "~/Scripts/jquery-ui.js",
                        "~/Scripts/jquery.swipebox.min.js",
                        "~/Scripts/move-top.js",
                        "~/Scripts/easing.js",
                        "~/Scripts/jquery.flexslider.js",
                        "~/Scripts/responsiveslides.min.js",
                        "~/Scripts/main.js",
                        "~/Scripts/easy-responsive-tabs.js",
                        "~/Scripts/bootstrap-3.1.1.min.js"));

            bundles.Add(new StyleBundle("~/Content/BottomCss").Include(
                      "~/Content/swipebox.css.css"));
        }
    }
}

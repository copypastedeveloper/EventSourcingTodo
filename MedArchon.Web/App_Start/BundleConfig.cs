using System;
using System.Web.Optimization;

namespace MedArchon.Web.App_Start
{
    public class BundleConfig
    {
        private static void SetupIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new ArgumentNullException("ignoreList");

            ignoreList.Clear();
            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
        }

        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            SetupIgnorePatterns(bundles.IgnoreList);

            bundles.Add(new StyleBundle("~/Content/kendoui/css")
                .Include("~/Content/KendoUI/kendo.common.min.css","~/Content/KendoUI/kendo.default.min.css"));

            Bundle bootstrapBundle = new StyleBundle("~/Content/bootstrap/css/styles");
            bootstrapBundle.Include("~/Content/bootstrap/css/bootstrap.css", "~/Content/bootstrap/css/bootstrap-responsive.css");
            bundles.Add(bootstrapBundle);            

            Bundle flattyBundle = new StyleBundle("~/Content/flatty/stylesheets/styles");
            flattyBundle.Include("~/Content/flatty/stylesheets/dark-theme.css", "~/Content/flatty/stylesheets/theme-colors.css", "~/Content/flatty/stylesheets/plugins/tabdrop/tabdrop.css");
            bundles.Add(flattyBundle);
            
            Bundle cssBundle = new StyleBundle("~/Content/css/styles");
            cssBundle.IncludeDirectory("~/Content/css", "*.css");
            cssBundle.IncludeDirectory("~/Content", "*.css");
            bundles.Add(cssBundle);
            
            Bundle lessBundle = new Bundle("~/Content/less", new LessMinify());
            lessBundle.Include("~/Content/Site.less");
            bundles.Add(lessBundle);

            Bundle scriptBundle = new ScriptBundle("~/bundles/scripts");
            scriptBundle.Include("~/Scripts/jquery-{version}.js");
            scriptBundle.Include("~/Scripts/knockout-{version}.js");
            scriptBundle.Include("~/Scripts/knockout.mapping-latest.js");
            scriptBundle.Include("~/Scripts/knockout.validation.js");
            scriptBundle.Include("~/Scripts/kendo.web.min.js");
            scriptBundle.Include("~/Scripts/knockout-kendo.min.js");
            scriptBundle.Include("~/Scripts/jquery.history.js");
            scriptBundle.Include("~/Scripts/moment.js");
            scriptBundle.Include("~/Scripts/signals.min.js");
            scriptBundle.Include("~/Scripts/crossroads.min.js");
            scriptBundle.Include("~/Content/bootstrap/js/bootstrap.js");
            scriptBundle.Include("~/Scripts/select2.js");
            scriptBundle.Include("~/Content/flatty/javascripts/plugins/mobile_events/jquery.mobile-events.min.js");
            scriptBundle.Include("~/Content/flatty/javascripts/jquery/jquery-migrate.min.js");
            scriptBundle.Include("~/Content/flatty/javascripts/nav.js");
            scriptBundle.Include("~/Content/flatty/javascripts/plugins/tabdrop/bootstrap-tabdrop.js");
            scriptBundle.Include("~/Scripts/toastr.js");
            scriptBundle.IncludeDirectory("~/App/Global", "*.js", true);
            scriptBundle.IncludeDirectory("~/App", "*.js", true);
            bundles.Add(scriptBundle);

			// Uncomment to see release configuration bundling.
			//BundleTable.EnableOptimizations = true;
		}
    }
}